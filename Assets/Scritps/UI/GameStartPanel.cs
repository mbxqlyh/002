using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : MonoBehaviour
{
    void Start()
    {
        transform.Find("BtnStart").GetComponent<Button>().onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            GameStartEvent.Trigger();
            GetComponentInParent<UI>().isStartDialogue = true;

            // GetComponentInParent<UI>().StartDialogue();
        });
    }

}
