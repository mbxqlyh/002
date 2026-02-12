using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trigger"))
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.propClip);
            GameModel.Score++;
            GameObject.FindWithTag("UI").GetComponent<UI>().SetupScore(GameModel.Score.ToString(), GameModel.Score);
            Destroy(gameObject);
        }
    }

}
