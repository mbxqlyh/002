using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Die
}

public class Player : MonoBehaviour
{

    [SerializeField] private UI ui;
    private PlayerState playerState;
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Settings")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public PhysicsMaterial2D groundMaterial;
    public PhysicsMaterial2D airMaterial;

    private bool facingRight = true;
    private bool isGrounded;

    public bool canControl;

    private bool isOver;

    private Vector3 spawnPoint;
    public bool isStop;

    private static readonly int IsIdle = Animator.StringToHash("isIdle");
    private static readonly int IsMove = Animator.StringToHash("isMove");
    private static readonly int IsGround = Animator.StringToHash("isGround");

    void OnEnable()
    {

        SelectionSceneEvent.Register(ChangeScene);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();


        playerState = PlayerState.Idle;
        rb.gravityScale = 0;

        spawnPoint = transform.position;
    }

    void Update()
    {
        HandleInput();
        UpdateState();
        UpdatePhysicsMaterial();
        UpdateAnimator();
    }

    void OnDisable()
    {
        SelectionSceneEvent.UnRegister(ChangeScene);
    }

    private void HandleInput()
    {

        if (!canControl) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.jumpClip);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    public void ChangeScene()
    {
        canControl = true;
        isStop = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
        rb.gravityScale = 1;
        Spawnwz();
    }

    private void UpdateState()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        if (playerState != PlayerState.Die)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.1f)
                playerState = PlayerState.Move;
            else if (isGrounded)
                playerState = PlayerState.Idle;
        }

        switch (playerState)
        {
            case PlayerState.Idle:
                HandleIdle();
                break;
            case PlayerState.Move:
                HandleMove();
                break;
            case PlayerState.Die:
                HandleDie();
                break;
        }
    }

    private void HandleIdle()
    {
        anim.SetBool(IsIdle, true);
        anim.SetBool(IsMove, false);
    }

    private void HandleMove()
    {
        anim.SetBool(IsIdle, false);
        anim.SetBool(IsMove, true);

        if (rb.velocity.x != 0)
        {
            bool shouldFaceRight = rb.velocity.x > 0;
            if (facingRight != shouldFaceRight)
            {
                facingRight = shouldFaceRight;
                transform.rotation = facingRight ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void HandleDie()
    {

        playerState = PlayerState.Idle;
        canControl = false;

        ui.SetupScore(GameModel.Score.ToString(), 0);

        anim.SetBool(IsIdle, false);
        anim.SetBool(IsMove, false);

        rb.velocity = new Vector2(0, rb.velocity.y - 3);
        GetComponent<CapsuleCollider2D>().enabled = false;
        rb.gravityScale = 0;

        if (isOver)
        {
            DelayHelper.Call(this, 1f, ui.GameOver);
            isOver = false;
        }

    }


    public void Spawnwz()
    {
        gameObject.SetActive(true);
        transform.position = spawnPoint;
    }

    private void UpdatePhysicsMaterial()
    {
        rb.sharedMaterial = isGrounded ? groundMaterial : airMaterial;
    }

    private void UpdateAnimator()
    {
        anim.SetBool(IsGround, !isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null && playerState != PlayerState.Die)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.enemyClip);
            isOver = true;
            // HandleDie();
            playerState = PlayerState.Die;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerState == PlayerState.Die) return;

        switch (collision.tag)
        {
            case "wall":
                isOver = true;
                playerState = PlayerState.Die;
                break;
            case "Props":
                HandleProps(collision.gameObject);
                break;
        }
    }

    private void HandleProps(GameObject prop)
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.propClip);
        isStop = true;
        transform.position = spawnPoint;
        rb.gravityScale = 0;

        ui.GameOver();

        Destroy(prop);
    }
}

public static class GameModel
{
    public static int Score;
}

public static class DelayHelper
{
    public static IEnumerator Run(float delay, Action calback)
    {
        yield return new WaitForSeconds(delay);

        calback();
    }

    public static void Call(MonoBehaviour host, float delay, Action calback)
    {
        host.StartCoroutine(Run(delay, calback));
    }

    public static IEnumerator Dia(Action text1, float time, Action text2)
    {
        text1();
        yield return new WaitForSeconds(time);
        text2();
    }

    public static void StartDia(MonoBehaviour host, Action _text1, float _time, Action _text2)
    {
        host.StartCoroutine(Dia(_text1, _time, _text2));
    }
}
