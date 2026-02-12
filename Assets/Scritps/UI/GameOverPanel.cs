
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    private UI ui;
    [SerializeField] private GameObject pass;
    [SerializeField] private GameObject fail;

    void Start()
    {

        ui = GetComponentInParent<UI>();

        pass.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            PassShow(false);
            ui.LevelHied();
            ui.LevleSeletionPanel(true);

            if (SceneLoadManager.instance.SceneName("Game3"))
            {
                ui.OverDialogue();
            }

        });

        fail.GetComponentInChildren<Button>().onClick.AddListener(() =>
       {
           FailShow(false);
           ui.LevelHied();
           ui.LevleSeletionPanel(true);
       });

        pass.SetActive(false);
        fail.SetActive(false);

    }

    public void PassShow(bool isShow)
    {
        pass.SetActive(isShow);
    }

    public void FailShow(bool isShow)
    {
        fail.SetActive(isShow);
    }

}
