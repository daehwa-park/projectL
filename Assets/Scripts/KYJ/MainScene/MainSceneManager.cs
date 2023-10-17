using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

//���Ⱑ ���󿡼��� select �Լ�//�̰� ���߿� ������.

public class MainSceneManager : MonoBehaviour
{
    public GameObject exitmessageBox;//������ ���� �ߴ� â
    public Button exitYes;//������ ���� Yes��ư
    public Button exitNo;//������ ���� No��ư
    public bool saveFile;//���̺������� �����ϴ��� Ȯ��
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
        //if (File.Exists(DataManager.instance.path + "/" + DataManager.instance.fileName + ".json")) // �����Ͱ� �ִ� ���
        //{
        //     Debug.Log(DataManager.instance.path + "/" + DataManager.instance.fileName);
        //     saveFile = true;            // �ش� ���� ��ȣ�� bool�迭 true�� ��ȯ
        //                                 //DataManager.instance.nowSlot = i;	// ������ ���� ��ȣ ����
        //     DataManager.instance._Load();   // �ش� ���� ������ �ҷ���
        //                                     //Start_Login_Btn_Text.GetComponent<Text>().text = "���ӽ����ϱ�";
        //     Debug.Log("��ư ���ӽ����ϱ�� �ٲ��.");
        //     if (DataManager.instance.userGameData.First_Login == 0)
        //     {
        //         //saveFile = false;//flase
        //         Debug.Log("��ư �α��� ����� �ٲ��.");
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
        //    Debug.Log("��ư �α��� ����� �ٲ��.");
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
        //else    // �����Ͱ� ���� ���
        //{
        //    saveFile = false;//flase
        //    Debug.Log("��ư �α��� ����� �ٲ��.");
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



    //-----�α��� â���� �Ѿ�� ��ɾ�.
    public void Creat()
    {
        SceneManager.LoadSceneAsync(1);
    }
    
    //--------------���̺����� bool ���� false�� �߸� DataManager�� ���� class�������� userData�� NewPlayer���� ���� ��, �̸� ����׷� ����� �� �Ŵ����� ���Ͽ� �༺����â(3)���� �Ѿ��.
    public void GoGame()
    {
        if (saveFile==false)
        {
            //DataManager.instance.userGameData.ID = LoginManager.instance.NewPlayerName.text;
            //DataManager.instance.userGameData.passward = LoginManager.instance.NewPlayerPassward.text;


            //DataManager.instance._save();//����
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


            //DataManager.instance._save(); //����
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
    //���� â ���� �� �ߴ� ĵ���� �� ��ư �Լ���
    public void ExitBtn()
    {
        Debug.Log("������ ����Ǿ����ϴ�.");
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
    //���� â ���� �� �ߴ� ĵ���� �� ��ư �Լ���
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
