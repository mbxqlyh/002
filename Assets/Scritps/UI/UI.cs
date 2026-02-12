using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text scoreText;
    public Player player;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject levleSeletionPanel;
    [SerializeField] private GameObject overPanel;
    [SerializeField] private GameObject startdialoguePanel;
    [SerializeField] private GameObject overdialoguePanel;

    [SerializeField] private Slider slider;
    public bool isLoading;

    public bool islevelShow;
    public bool isStartDialogue;
    private float currentTime = 0;

    void OnEnable()
    {
        SelectionSceneEvent.Register(SwitchScene);
    }

    void Start()
    {

        GameStartEvent.Register(StartLoading);

        SetupHideChideObjecs();

        SetupValue();

        startdialoguePanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            isLoading = true;
            islevelShow = true;
            GameStartEvent.Trigger();
            startdialoguePanel.gameObject.SetActive(false);

        });

        startdialoguePanel.GetComponent<Button>().enabled = false;


        overdialoguePanel.GetComponent<Button>().onClick.AddListener(() =>
        {
            LevelHied();
            LevleSeletionPanel(true);
            overdialoguePanel.gameObject.SetActive(false);

        });

        overdialoguePanel.GetComponent<Button>().enabled = false;

    }

    void Update()
    {
        if (isLoading)
        {
            currentTime += Time.deltaTime;
            slider.value = currentTime;

            if (currentTime >= 4)
            {
                LoadPanel(false);
                LevleSeletionPanel(islevelShow);
                if (isStartDialogue)
                    StartDialogue();

                isStartDialogue = false;
                isLoading = false;
                currentTime = 0;
                slider.value = 0;
            }
        }
    }

    void OnDisable()
    {
        SelectionSceneEvent.UnRegister(SwitchScene);
    }
    
    public void SwitchScene()
    {
        SetupScore(GameModel.Score.ToString(), 0);

        scoreText.gameObject.SetActive(true);

        isLoading = true;
        StartLoading();
        islevelShow = false;
    }



    public void StartDialogue()
    {
        StartDialoguePanel(true);

        StartCoroutine(Dialogue(startdialoguePanel));

    }

    public void OverDialogue()
    {
        OverDialoguePanel(true);
        StartCoroutine(Dialogue(overdialoguePanel));


    }

    IEnumerator Dialogue(GameObject _dialoguePanel)
    {

        _dialoguePanel.transform.Find("Text1").gameObject.SetActive(true);
        _dialoguePanel.transform.Find("Text2").gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);

        _dialoguePanel.transform.Find("Text1").gameObject.SetActive(false);
        _dialoguePanel.transform.Find("Text2").gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        _dialoguePanel.GetComponent<Button>().enabled = true;
        _dialoguePanel.transform.Find("Text2").gameObject.SetActive(true);
        _dialoguePanel.transform.Find("Text3").gameObject.SetActive(true);
        
    }

    public void SetupHideChideObjecs()
    {
        StartPanel(true);
        LoadPanel(false);
        LevleSeletionPanel(false);
        OverPanel(true);
        StartDialoguePanel(false);
        OverDialoguePanel(false);
    }

    public void SetupScore(string _text, int scoreValue)
    {
        GameModel.Score = scoreValue;
        scoreText.text = "积分: " + _text;
    }

    private void SetupValue()
    {
        slider.maxValue = 3;
        slider.value = 0;
    }

    public void StartLoading()
    {

        LoadPanel(true);
        isLoading = true;
    }

    public void GameOver()
    {
        var overp = overPanel.GetComponent<GameOverPanel>();
        var scene = SceneLoadManager.instance;
        var rollingui = transform.Find("LevelSeletionPanel/RollingUI").GetComponent<RollingUI>();

        bool isPass = false;

        if (GameModel.Score < 10)
        {
            isPass = false;
        }

        if (GameModel.Score >= 18 && scene.SceneName("Game3"))
        {
            isPass = true;
        }

        if (GameModel.Score >= 15 && scene.SceneName("Game2"))
        {
            rollingui.LevelShow("OptionGroup/Level3");

            isPass = true;
        }

        if (GameModel.Score >= 10 && scene.SceneName("Game1"))
        {
            rollingui.LevelShow("OptionGroup/Level2");

            isPass = true;
        }

        overp.PassShow(isPass);
        overp.FailShow(!isPass);

    }

    public void LevelHied()
    {
        levleSeletionPanel.GetComponent<LevelSeletionPanel>().Hide();
    }

    void OnDestroy()
    {
        GameStartEvent.UnRegister(StartLoading);
    }

    public void StartPanel(bool isShow) => startPanel.SetActive(isShow);
    public void LoadPanel(bool isShow) => loadPanel.SetActive(isShow);
    public void LevleSeletionPanel(bool isShow) => levleSeletionPanel.SetActive(isShow);
    public void OverPanel(bool isShow) => overPanel.SetActive(isShow);
    public void StartDialoguePanel(bool isShow) => startdialoguePanel.SetActive(isShow);
    public void OverDialoguePanel(bool isShow) => overdialoguePanel.SetActive(isShow);


}
