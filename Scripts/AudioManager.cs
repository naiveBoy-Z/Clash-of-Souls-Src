using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton Implementation
    public static AudioManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    AudioSource[] audioSources;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    public void PlayVictoryAudio()
    {
        audioSources[2].Stop();
        audioSources[0].Play();
    }

    public void PlayDefetAudio()
    {
        audioSources[2].Stop();
        audioSources[1].Play();
    }
}
