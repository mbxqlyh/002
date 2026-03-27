using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSeletionPanel : MonoBehaviour
{
    private UI ui;
    [SerializeField] private MainCamera main;

    [SerializeField] private GameObject audioSetting;

    [SerializeField] public GameObject levelScene1;
    [SerializeField] public GameObject levelScene2;
    [SerializeField] public GameObject levelScene3;

    private bool isShowSetting;

    void Start()
    {
        ui = GetComponentInParent<UI>();

        SelectLevelScene1(false);
        SelectLevelScene2(false);
        SelectLevelScene3(false);

        levelScene1.GetComponent<Button>().onClick.AddListener(() =>
        {

            main.FollowTarget();
            SelectScene("Game1");
        });

        levelScene2.GetComponent<Button>().onClick.AddListener(() =>
        {
            main.FollowTarget();
            SelectScene("Game2");
        });

        levelScene3.GetComponent<Button>().onClick.AddListener(() =>
        {
            main.FollowTarget();
            SelectScene("Game3");
        });

    }

    private void SelectScene(string sceneName)
    {

        SelectionSceneEvent.Trigger();
        SceneLoadManager.instance.LoadGame1Sence(sceneName);
        gameObject.SetActive(false);

    }

    //有事件调用
    public void OnSettingButtonClick()
    {
        isShowSetting = !isShowSetting;

        audioSetting.SetActive(isShowSetting);
    }

    public void Hide()
    {
        SelectLevelScene1(false);
        SelectLevelScene2(false);
        SelectLevelScene3(false);
    }

    public void SelectLevelScene1(bool isShow) => levelScene1.SetActive(isShow);
    public void SelectLevelScene2(bool isShow) => levelScene2.SetActive(isShow);
    public void SelectLevelScene3(bool isShow) => levelScene3.SetActive(isShow);
}
