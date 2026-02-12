using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager instance { get; private set; }
    private string currentScene = "";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LoadGame1Sence(string newScene)
    {

        if (!string.IsNullOrEmpty(currentScene))
        {
            SceneManager.UnloadSceneAsync(currentScene);

            // DelayHelper.Call(this, 2, () => ui.GameFail());
        }

        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
        currentScene = newScene;

    }

    public bool SceneName(string _name)
    {
        if (currentScene.Equals(_name)) return true;

        return false;
    }

}
