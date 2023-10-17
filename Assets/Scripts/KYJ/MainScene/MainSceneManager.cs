using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//여기가 영상에서의 select 함수//이거 나중에 지워라.

public class MainSceneManager : MonoBehaviour
{
    public GameObject exitmessageBox;//게임을 끌때 뜨는 창
    public Button exitYes;//게임을 끝때 Yes버튼
    public Button exitNo;//게임을 끝때 No버튼
    public bool saveFile;//세이브파일이 존재하는지 확인
    public Text Start_Login_Btn_Text;
    public Button Start_Login_Btn;
    public Button Setting_Btn;
    public Button Btn_exit;
    public GameObject SettingPannel;
    public Button setting_out_Btn;
    public static MainSceneManager instance;


    public FadeController Fader;
    public GameObject FadeCon;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        Fader = FadeCon.GetComponent<FadeController>();

        Fader.FadeOut(0.6f);

        SettingPannel.SetActive(false);
        //if (File.Exists(DataManager.instance.path + "/" + DataManager.instance.fileName + ".json")) // 데이터가 있는 경우
        //{
        //     Debug.Log(DataManager.instance.path + "/" + DataManager.instance.fileName);
        //     saveFile = true;            // 해당 슬롯 번호의 bool배열 true로 변환
        //                                 //DataManager.instance.nowSlot = i;	// 선택한 슬롯 번호 저장
        //     DataManager.instance._Load();   // 해당 슬롯 데이터 불러옴
        //                                     //Start_Login_Btn_Text.GetComponent<Text>().text = "게임시작하기";
        //     Debug.Log("버튼 게임시작하기로 바꿨다.");
        //     if (DataManager.instance.userGameData.First_Login == 0)
        //     {
        //         //saveFile = false;//flase
        //         Debug.Log("버튼 로그인 만들기 바꿨다.");
        //         Start_Login_Btn.onClick.AddListener(() => {
        //             //GoGameFirst();
        //             //targetFrameRate();
        //             DataManager.instance.userGameData.First_Login = -1;
        //             DataManager.instance._save();
        //             GoGameFirst();
        //         });
        //     }
        //     else
        //     {
        //         Start_Login_Btn.onClick.AddListener(() =>
        //         {
        //             DataManager.instance.userGameData.First_Login = -1;
        //             DataManager.instance._save();
        //
        //             GoGame();
        //             targetFrameRate();
        //         });
        //     }
        // }
        //else
        //{
        //    saveFile = false;//flase
        //    Debug.Log("버튼 로그인 만들기 바꿨다.");
        //    Start_Login_Btn.onClick.AddListener(() => {
        //        //GoGameFirst();
        //        //targetFrameRate();
        //        DataManager.instance.userGameData.First_Login = -1;
        //        DataManager.instance._save();
        //        GoGameFirst();
        //    });
        //}
        //if(DataManager.instance.userGameData.First_Login == -1)
        //{
        //    saveFile = false;//flase
        //    Start_Login_Btn.onClick.AddListener(() =>
        //    {
        //        GoGame();
        //        targetFrameRate();
        //    });
        //}
        //
        //else    // 데이터가 없는 경우
        //{
        //    saveFile = false;//flase
        //    Debug.Log("버튼 로그인 만들기 바꿨다.");
        //    Start_Login_Btn.onClick.AddListener(() => {
        //        //GoGameFirst();
        //        //targetFrameRate();
        //        Creat();
        //    });
        //
        //}



        Start_Login_Btn.onClick.AddListener(() => {
                        //GoGameFirst();
                        //targetFrameRate();
                        //DataManager.instance.userGameData.First_Login = -1;
                        //DataManager.instance._save();
                        GoGameFirst();
                    });




















            Setting_Btn.onClick.AddListener(() => Setting_btn());
        Btn_exit.onClick.AddListener(() => ExitBtn());

        exitmessageBox.gameObject.SetActive(false);
        SettingPannel.SetActive(false);


    }


    // Update is called once per frame
    void Update()
    {
        

    }


    void targetFrameRate()
    {
        Application.targetFrameRate = 60;
    }



    //-----로그인 창으로 넘어가기 명령어.
    public void Creat()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    //--------------세이브파일 bool 값이 false라 뜨면 DataManager에 만든 class형식으로 userData에 NewPlayer값을 저장 후, 이를 디버그로 출력후 씬 매니저로 통하여 행성선택창(3)으로 넘어간다.
    public void GoGame()
    {
        if (saveFile==false)
        {
            //DataManager.instance.userGameData.ID = LoginManager.instance.NewPlayerName.text;
            //DataManager.instance.userGameData.passward = LoginManager.instance.NewPlayerPassward.text;


            //DataManager.instance._save();//복구
        }
        else
        {
            
        }
       
        //Debug.Log(DataManager.instance.userGameData.stageClear);
        //Debug.Log(DataManager.instance.path);

        SceneManager.LoadSceneAsync(2);
    }

    public void GoGameFirst()
    {
        if (saveFile == false)
        {
            //DataManager.instance.userGameData.ID = LoginManager.instance.NewPlayerName.text;
            //DataManager.instance.userGameData.passward = LoginManager.instance.NewPlayerPassward.text;


            //DataManager.instance._save(); //복구
        }
        else
        {

        }

        //Debug.Log(DataManager.instance.userGameData.stageClear);
        //Debug.Log(DataManager.instance.path);
        //SceneManager.LoadSceneAsync(1);

        SceneManager.LoadSceneAsync("StoryScene");
    }


    //-----------------------------------------------
    //게임 창 종료 시 뜨는 캔버스 및 버튼 함수들
    public void ExitBtn()
    {
        Debug.Log("게임이 종료되었습니다.");
        //Application.Quit();

        exitmessageBox.SetActive(true);
        //Instantiate(exitmessageBox);
    }
    public void ExitMessageYes()
    {
        Application.Quit();
    }
    public void ExitMessageNo()
    {
        exitmessageBox.SetActive(false);
    }
    //게임 창 종료 시 뜨는 캔버스 및 버튼 함수들
    //-----------------------------------------------

    public void Setting_btn()
    {
        SettingPannel.SetActive(true);
    }
    //----------------------------------------------------------------------------------------
    public void Setting_Out_Btn()
    {
        SettingPannel.SetActive(false);
    }
}
