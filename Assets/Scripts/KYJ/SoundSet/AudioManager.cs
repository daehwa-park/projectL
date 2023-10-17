using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        instance = this;
        ContiuneSetting();



        FirstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (FirstPlayInt == 0)
        {
            backgroundFloat = 0.1f;
            soundEffectFloat = 0.1f;
            backgroundSlider.value = backgroundFloat;
            sounfEffectSlider.value = soundEffectFloat;



            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            backgroundSlider.value = backgroundFloat;

            soundEffectFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            sounfEffectSlider.value = soundEffectFloat;

        }
    }


    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";

    private int FirstPlayInt;
    public Slider backgroundSlider, sounfEffectSlider;
    private float backgroundFloat, soundEffectFloat;

    public AudioSource[] BGM;
    public AudioSource[] EFS;

    //public Text BgmValue_Text, EffValue_Text;

    void Start()
    {
        //BGM[] = GetComponent<AudioSource>();
        instance = GetComponent<AudioManager>();// 스크립트 SoundManger의 방식 차용
    }
    void Update()
    {
        //int bgm = (int)backgroundSlider.value;

        //BgmValue_Text.text = ((int)(backgroundSlider.value * 1000)).ToString();
        //EffValue_Text.text = ((int)(sounfEffectSlider.value * 1000)).ToString();
    }
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, sounfEffectSlider.value);

    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {

        for (int i = 0; i < BGM.Length; i++)
        {
            BGM[i].volume = backgroundSlider.value;//0번은 worldmapBGM
        }
        for (int i = 0; i < EFS.Length; i++)
        {
            EFS[i].volume = sounfEffectSlider.value;
        }
    }


    void ContiuneSetting()
    {
        backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
        soundEffectFloat = PlayerPrefs.GetFloat(SoundEffectsPref);

        for (int i = 0; i < BGM.Length; i++)
        {
            BGM[i].volume = backgroundFloat;//0번은 worldmapBGM
        }
        for (int i = 0; i < EFS.Length; i++)
        {
            EFS[i].volume = soundEffectFloat;
        }
    }



    public void PlaySE()
    {
        int i = Random.Range(0, 5);
        EFS[i].Play();
    }

}
