using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    public AudioClip jumpClip;
    public AudioClip propClip;
    public AudioClip enemyClip;

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

        bgmSlider.value = 0.7f;
        sfxSlider.value = 0.7f;

    }

    void Start()
    {
        BgmVolume();
        SfxVolume();
    }

    public void BgmVolume()
    {
        bgmSource.volume = bgmSlider.value;

    }

    public void SfxVolume()
    {
        sfxSource.volume = sfxSlider.value;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
