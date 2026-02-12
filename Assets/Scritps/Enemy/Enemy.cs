using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Patrol,
    Die
}

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D coll;
    public EnemyState enemyState;
    public float moveSpeed;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    private Player player;

    public LayerMask groundLayer;

    private bool _movingRight = true;
    private bool isStop;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();

        enemyState = EnemyState.Patrol;

    }

    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Die:
                Die();
                break;
        }

        if (Input.GetKeyDown(KeyCode.J))
            enemyState = EnemyState.Die;
    }

    private void Patrol()
    {
        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);

        if (!isGroundAhead)
        {
            Flip();
        }
        if (!player.isStop)
            rb.velocity = new Vector2((_movingRight ? 1 : -1) * moveSpeed, rb.velocity.y);
    }

    private void Die()
    {
        coll.enabled = false;
        rb.velocity = new Vector2(0, rb.velocity.y - 3);
        Destroy(gameObject, 3);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.enemyClip);
            enemyState = EnemyState.Die;
        }
    }

    private void Flip()
    {
        _movingRight = !_movingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }

    }

    public bool isGroundDetected() => Physics2D.Raycast(groundCheck.transform.position, Vector2.down, groundCheckDistance);
}
