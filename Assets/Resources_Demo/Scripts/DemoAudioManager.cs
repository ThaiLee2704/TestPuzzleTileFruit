using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoAudioManager : Singleton<DemoAudioManager>
{
    [SerializeField] private AudioSource clickedSource;
    [SerializeField] private AudioSource match3Source;

    private void OnEnable()
    {
        DemoLevelManager.OnClicked += PlayClickedSound;
        DemoTrayManager.OnMatch3 += PlayMatch3Sound;
    }

    private void OnDisable()
    {
        DemoLevelManager.OnClicked -= PlayClickedSound;
        DemoTrayManager.OnMatch3 -= PlayMatch3Sound;
    }

    private void PlayClickedSound()
    {
        clickedSource.Play();
    }

    private void PlayMatch3Sound()
    {
        match3Source.Play();
    }
}
