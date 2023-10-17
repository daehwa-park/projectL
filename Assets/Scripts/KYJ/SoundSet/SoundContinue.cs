using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SoundContinue : MonoBehaviour
{
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private float backgroundFloat, soundEffectFloat;
    public AudioSource BGM;



    private void Awake()
    {
        ContiuneSetting();
    }

    void ContiuneSetting()
    {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        soundEffectFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

        BGM.volume = backgroundFloat;//0¹øÀº worldmapBGM
    }

}
