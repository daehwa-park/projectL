using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;

public class PlayScene_SetBtn : MonoBehaviour
{
    public GameObject SetPannel;
    public Button Btn_Seton;
    public Button Btn_Setoff;
    public Button Btn_Home;
    public Button Btn_Continue;
    public Button Btn_Restart;
    //-----------------------------------------------------------------------------------------

    public Button Btn_1;
    public Button Btn_ClearNextStage;
    public Button Btn_3;
    public Button Btn_FailRetry;
    //-----------------------------------------------------------------------------------------

    public GameObject Pannal_ClearResult;
    public GameObject Pannal_FailResult;
    public GameObject Fade;
    //-----------------------------------------------------------------------------------------
    int a;
    //-----------------------------------------------------------------------------------------

    bool IsPause;
    // Start is called before the first frame update
    void Start()
    {
        
        Pannal_ClearResult.SetActive(false);
        Pannal_FailResult.SetActive(false);
        Fade.SetActive(true);
        //DataManager.instance._Load(); 

        //SetPannel = GetComponent<GameObject>(); 
        SetPannel.SetActive(false);
        IsPause = false;
        //-----------------------------------------------------------------------------------------

        Btn_Seton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            //AudioManager.instance.SaveSoundSettings();
            //DataManager.instance._save();
            IsPause = true;
            Btn_onclick_seton();
        });
        //-----------------------------------------------------------------------------------------
        Btn_Setoff.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            SetOut_Btn();
        });
        //-----------------------------------------------------------------------------------------
        Btn_Home.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            ExitBtn();
        });
        //-----------------------------------------------------------------------------------------
        Btn_Continue.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            SetOut_Btn();
        });
        //-----------------------------------------------------------------------------------------
        Btn_Restart.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            Restart();
        });
        Btn_1.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            ExitBtn();
        });
        //Btn_ClearNextStage = GetComponent<Button>();
        Btn_ClearNextStage.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            //AudioManager.instance.SaveSoundSettings();
            
            NextStage();
        });
        Btn_3.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            AudioManager.instance.SaveSoundSettings();
            IsPause = false;
            ExitBtn();
        });
        Btn_FailRetry.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySE();
            //AudioManager.instance.SaveSoundSettings();
            //IsPause = false;
            Restart();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPause==true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        pannal_on();
        

    }
    void pannal_on()
    {
        if(Pannal_ClearResult.activeSelf==true)
        {
            IsPause = true;
        }
        if(Pannal_FailResult.activeSelf==true)
        {
            IsPause = true;
        }
    }
    void Btn_onclick_seton()
    {
        //DataManager.instance._save();
        SetPannel.SetActive(true);
    }
    
    void ExitBtn()
    {
        //DataManager.instance._save();
        GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(2); });
        WorldToMoveManager.instance.Game_Home = false;

    }
    void NextStage()
    {
        if (1<= StageManager.Instance.stageNum && StageManager.Instance.stageNum <= 40)
        {
            Debug.Log("0~39사이");

            //if (StageManager.Instance.stageNum == (DataManager.instance.userGameData.stageClear + 1))
            //{
            //    a = StageManager.Instance.stageNum;
            //    Debug.Log("if stageNum=stageclear");
            //    DataManager.instance.userGameData.stageClear += 1;
            //    DataManager.instance._save();


            //    Debug.Log("StageNum : " + StageManager.Instance.stageNum);

            //    StageManager.Instance.stageNum += 1;
            //    Debug.Log("StageNum+1 : "+ StageManager.Instance.stageNum);
            //    //GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(3); });
            //    SceneManager.LoadSceneAsync(3);
            //    Debug.Log("씬이동");
            //    //DataManager.instance._save();
            //}
            //else
            //{
            
            StageManager.Instance.stageNum += 1;
            //DataManager.instance.userGameData.stageclear += 1;
            //DataManager.instance._save();
            //GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(3); });
            SceneManager.LoadSceneAsync(3);

            //}

        }
        else
        {
            //DataManager.instance._save();
            Debug.Log("39넘어서 스토리씬 넘어가기");
            GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(4); });
        }
    }
    void Restart()
    {
        //DataManager.instance._save();
        //GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(3); });
        SceneManager.LoadSceneAsync(3);
    }
    void SetOut_Btn()
    {
        //DataManager.instance._save();
        SetPannel.SetActive(false);
    }



}
