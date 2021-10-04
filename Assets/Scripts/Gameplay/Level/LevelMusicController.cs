using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMusicController : MonoBehaviour
{
    public LevelPhaseManager phaseManager;
    public AudioSource calmMusic;
    public AudioSource stormyMusic;

    private void Awake()
    {
        phaseManager.BuildPhaseStartEvent.AddListener(OnBuildPhaseStart);
        phaseManager.WeatherPhaseStartEvent.AddListener(OnWeatherPhaseStart);
    }

    private void OnBuildPhaseStart()
    {
        stormyMusic.Stop();
        calmMusic.Play();
    }

    private void OnWeatherPhaseStart()
    {
        calmMusic.Stop();
        stormyMusic.Play();
    }
}
