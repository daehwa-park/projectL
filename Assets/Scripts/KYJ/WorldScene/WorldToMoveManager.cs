using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;


public class WorldToMoveManager : MonoBehaviour
{
    public static WorldToMoveManager instance;
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

        //DontDestroyOnLoad(this.gameObject);
    }
    //----------------------------------------------------------------



    public StageController stageController;

    public bool Game_Home;
    //----------------------------------------------------------------월드데이터 예시 문구


    //public string fileName = "World1-1";
    //World1.WorldName = "Earth";
    //----------------------------------------------------------------게임 오브젝트 지정
    public GameObject WorldScroll;//세계 선택 스크롤
    public GameObject SeletedPannel;//세계 선택 시 맵 선택바
    public GameObject GamePannel;//게임 시작 직전 창
    public GameObject SetPannel;//세팅 창

    public Button Back_Btn;
    public Button SetPannel_On_Btn;
    public Button SetPannel_Off_Btn;
    //public Button Go_Main_Btn;

    public Button Btn_StartGame;
    public Button Btn_GamePannal_Off;
    //public Button Go_Main_Btn;//월드씬->메인씬 버튼
    //----------------------------------------------------------------
    public GameObject PlantImage;


    public Button[] Plant;

    public Text[] stage_Num;

    public Button[] Btn_GameStage;
    public Sprite[] Plant_Off;

    public Sprite[] Plant_On;
    //----------------------------------------------------------------
    //----------------------------------------------------------------

    int Numindex =6;
    int[] ClearNum = new int[8];
    int[] index = new int[6];
    public Text[] Contents_ClearNum;
    public Image[] Contents_ClearImage;
    public Image[] Contents_ClearNum_frame;
    int stage;
    public Sprite[] Clear_sprite;
    //----------------------------------------------------------------
    public Button Btn_Exit;
    public GameObject Pannal_Exit;
    public Button Btn_Exit_Yes;
    public Button Btn_Exit_No;
    //----------------------------------------------------------------
    public Text StageNum;
    public Text StageName;
    //----------------------------------------------------------------

    public FadeController Fader;
    public GameObject FadeCon;

    //----------------------------------------------------------------
    int SceneName;
    string path;
    //----------------------------------------------------------------

    public static int Stage_Clear_Example;

    //----------------------------------------------------------------
    public Text StageNum_Num;
    public Text Move_Num;

    int stagenum;
    //----------------------------------------------------------------
    string data;
    int stage_pass;// /5 = 1 & 2;

    int stage_pass_pass;// 7%5 = 2;
    //----------------------------------------------------------------
    string[] PlantNamed = new string[8];
    //----------------------------------------------------------------
    int Clicked_Stage_Num;// = new int[5];
    int Clicked_Plant_Num;// = new int[8];
    GameObject clicked_object;
    int IndexNum = 6;
    void Start()
    {
        //initialization();
        //Move_Num_text();
        path = Application.dataPath + "/Resources/Stage";//게임정보pass
        Application.targetFrameRate = 60;

        //DataManager.instance._Load();
        stage = 40;//DataManager.instance.userGameData.stageclear;
        //DataManager.instance._save();
        PlantNamed[0] = "EARTH";
        PlantNamed[1] = "PLANET 325";
        PlantNamed[2] = "PLANET 326";
        PlantNamed[3] = "PLANET 327";
        PlantNamed[4] = "PLANET 328";
        PlantNamed[5] = "PLANET 329";
        PlantNamed[6] = "PLANET 330";
        PlantNamed[7] = "EARTH";
        //----------------------------------------------------------------
        //----------------------------------------------------------------\
        Fader = FadeCon.GetComponent<FadeController>();

        Fader.FadeOut(0.6f);
        //----------------------------------------------------------------


        //Debug.Log("월드 정보 path : " + path);
        //----------------------------------------------------------------


        //----------------------------------------------------------------
        PannelInit();
        Plant_Click_Off();//두개 순서 지키기 1번
        Change_Plant();//두개 순서 지키기 2번

        Pannal_Exit.SetActive(false);
        WorldScroll_On();
        SetPannel_Off();

        if (Game_Home)
        {
            SeletedPannel_On();
            GamePannel_On();
            

            Debug.Log("Game_Home=false");
        }
        else
        {
            SeletedPannel_Off();
            GamePannel_Off(); 

            Debug.Log("Game_Home=true");
        }
        //----------------------------------------------------------------


        //----------------------------------------------------------------

        Btn_StartGame.onClick.AddListener(() => GameStart());
        Btn_GamePannal_Off.onClick.AddListener(() =>
        {
            OnClick_GamePannal_Exit();

        });
        //----------------------------------------------------------------

        

        
        Exit_Pannal();
        
        SetPannel_On_Btn.onClick.AddListener(() => SetPannel_On_Btn_onclick());
        for (int i = 0; i < 8; i++)
        {
            
            int temp = i;
            
            
            Plant[temp].onClick.AddListener(() => {
                Plant_Click(temp);
                openStage();
                GameBtnOn();
            });
        }

    }
    void Exit_Pannal()
    {
        Btn_Exit.onClick.AddListener(() => Btn_click_ExitBtn());
        Btn_Exit_Yes.onClick.AddListener(() => Btn_click_Exit_Yes());
        Btn_Exit_No.onClick.AddListener(() => Btn_click_Exit_No());
    }
    void Btn_click_ExitBtn()
    {
        Pannal_Exit.SetActive(true);
    }
    void Btn_click_Exit_Yes()
    {
        Application.Quit();
    }
    void Btn_click_Exit_No()
    {
        Pannal_Exit.SetActive(false);
    }
    void Update()
    {
        clicked_object = EventSystem.current.currentSelectedGameObject;
        if (WorldScroll.activeSelf == true)//월드스크롤에선 
        {
            BackBtn_Off();//뒤로가기가 없고
            //Go_Main_Btn_On();//메인가기가 되고
        }
        else
        {
            BackBtn_On();
            //Go_Main_Btn_Off();
        }

        if (SetPannel.activeSelf == true)//setpannel.active(true) 인 경우
        {
            SetBtn_Off();
            Back_Btn.interactable = false; // 버튼 클릭을 비활성화
            

            if (WorldScroll.activeSelf == true)
            {
                SetBtn_Off();
            }
        }
        else
        {
            SetBtn_On();
            Back_Btn.interactable = true; // 버튼 클릭을 활성화
            
        }
        
    }

    void PannelInit()
    {
        
       
        int i;
        for (i = 0; i < 8; i++)
        {
            Clicked_Plant_Num = i;

            Plant[i].onClick.AddListener(() =>
            {
                Plant_Btn_Onclick_GoGamePannal();
                AudioManager.instance.PlaySE();
                //Debug.Log("이름 바꾸기 : " + i);
                

            });
        }
        
        
        Btn_GameStage[0].onClick.AddListener(()=> {

            stage_stageNum(Clicked_Plant_Num, 1);
            
            Debug.Log(Clicked_Plant_Num);

            Move_Num_text();
            AudioManager.instance.PlaySE();
            GamePannel_On();
            Change_StageNum_Num();
            
            

        });
        Btn_GameStage[1].onClick.AddListener(()=> {
            stage_stageNum(Clicked_Plant_Num, 2);
            Debug.Log(Clicked_Plant_Num);

            Move_Num_text();
            AudioManager.instance.PlaySE();
            GamePannel_On();
            Change_StageNum_Num();
            
            

            

        });
        Btn_GameStage[2].onClick.AddListener(()=> {

            stage_stageNum(Clicked_Plant_Num, 3);
            Debug.Log(Clicked_Plant_Num);
   
            Move_Num_text();
            AudioManager.instance.PlaySE();
            GamePannel_On();
            Change_StageNum_Num();
            
            


        });
        Btn_GameStage[3].onClick.AddListener(()=> {
            stage_stageNum(Clicked_Plant_Num, 4);
            Debug.Log(Clicked_Plant_Num);
        
            Move_Num_text();
            AudioManager.instance.PlaySE();
            GamePannel_On();
            Change_StageNum_Num();
            
           



        });
        Btn_GameStage[4].onClick.AddListener(() => {
            stage_stageNum(Clicked_Plant_Num, 5);
            Debug.Log(Clicked_Plant_Num);
            
            Move_Num_text();
            AudioManager.instance.PlaySE();
            GamePannel_On();
            Change_StageNum_Num();
            
            

            //Move_Num_text();

        });


        //----------------------------------------------------------------
        



    }
    void GameBtnOn()
    {
        if (Clicked_Plant_Num == stage_pass)
        {
            Debug.Log("Clicked_Plant_Num == stage_pass");
            Plant[Clicked_Plant_Num].GetComponent<Image>().sprite = Plant_On[Clicked_Plant_Num];
            Plant[Clicked_Plant_Num].interactable = true;
          
            for (int j = 0; j < stage_pass_pass + 1; j++)
            {
                Btn_GameStage[j].interactable = true;
            }
        }
        else
        {
            Debug.Log("Clicked_Plant_Num =/= stage_pass");
            for (int j = 0; j < 5; j++)
            {
                Btn_GameStage[j].interactable = true;
            }
        }
    }
    public void GameBtnOff()
    {
        for (int j = 0; j < 5; j++)
        {
            Btn_GameStage[j].interactable = false;
        }
    }
    void Move_Num_text()
    {


        string str_stagenum;
        StageInfo stageInfo;
        if (stagenum < 10)
        {
            str_stagenum = "0" + stagenum.ToString();
            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/stage_00" + str_stagenum);


            stageInfo = JsonUtility.FromJson<StageInfo>(textAsset.text);


            //data = File.ReadAllText(path + "/" + "stage_00" + str_stagenum + ".txt");

            //data = 
        }
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>($"Stage/stage_00" + stagenum);


            stageInfo = JsonUtility.FromJson<StageInfo>(textAsset.text);

            //data = File.ReadAllText(path + "/" + "stage_00" + stagenum + ".txt");
        }
        //Move_Num.text = JsonUtility.FromJson<StageInfo>(data).moveNum.ToString();
        Move_Num.text = stageInfo.moveNum.ToString();
        //-------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------
        
        for (int i = 0; i < 8; i++)
        {
            //ClearNum[i] = JsonUtility.FromJson<StageInfo>(data).clearNum[i];
            ClearNum[i] = stageInfo.clearNum[i];

        }
        //Debug.Log("ClearNum");
        for (int k = 0; k < 6; k++)
        {
            //index[k] = JsonUtility.FromJson<StageInfo>(data).clearNumIndex[k];

            index[k] = stageInfo.clearNumIndex[k];

            //Debug.Log("index 지정"+k);

            if (index[k] == -1)
            {
                Contents_ClearNum[k].enabled = false;
                //Debug.Log("clearnum -> false" +k);

                Contents_ClearImage[k].enabled = false;
                //Debug.Log("clearimage -> false"+k);

                Contents_ClearNum_frame[k].enabled = false;
                Numindex=Numindex-1;
                //Debug.Log("numindex");

            }
        }
        for (int i = 0; i < Numindex; i++)
        {
            Contents_ClearImage[i].sprite = Clear_sprite[index[i]];
            //Debug.Log("이미지변환"+i);
            Contents_ClearNum[i].text = ClearNum[index[i]].ToString();
            //Debug.Log("조건 변환" + i);

        }
        //Debug.Log("함수끝");

    }
    //-------------------------------------------------------------------------------------------

    public void initialization()
    {
     
        for(int i = 0; i<8;i++)
        {
            ClearNum[i] = 0;
        }
        for(int j=0;j<6;j++)
        {
            index[j] = 0;
            Contents_ClearNum[j].enabled = true;
            Contents_ClearImage[j].enabled = true;
            Contents_ClearNum_frame[j].enabled = true;
            Numindex = 6;

            Contents_ClearImage[j].sprite = Clear_sprite[index[0]];
            Contents_ClearNum[j].text = "00";
        }
    }


    void stage_stageNum(int i, int k)
    {
        int n = i;
        int m = k;
        
        //Debug.Log("n = "+n);
        //Debug.Log("m= "+m);
        StageManager.Instance.stageNum= ((n * 5) + m);
        //((n * 5) + m) = StageManager.Instance.stageNum;
        //p = (n * 5) + m;
        //Debug.Log("stageNum((n*5)+m) = "+StageManager.Instance.stageNum);
        stagenum = ((n * 5) + m); //StageManager.Instance.stageNum;

    }
    //----------------------------------------------------------------
    void Change_StageNum_Num()
    {
        StageNum_Num.text = clicked_object.GetComponentInChildren<Text>().text.ToString();
        
    }

    //----------------------------------------------------------------
    void Plant_Click(int k)
    {
        int i;
        PlantImage.GetComponent<Image>().sprite = Plant_On[k];
        StageNum.text = "STAGE " + (k + 1).ToString();
        Clicked_Plant_Num = k;
        StageName.text = PlantNamed[k];
        for (i = 0; i < 5; i++)
        {
            int temp = i;
            stage_Num[temp].text = (k + 1).ToString() + "-" + (i + 1).ToString();
        }  
    }
    //----------------------------------------------------------------
    void Change_Plant()
    {
        
        Debug.Log("stage = "+stage);
        stage_pass = stage / 5;// /5 = 1 & 2;
        Debug.Log("Stage_pass = " + stage_pass);
        stage_pass_pass = stage % 5;// 7%5 = 2;
        Debug.Log("Stage_pass_pass = " + stage_pass_pass);

        int i,j;
        i = stage_pass;
        j = stage_pass_pass;

        
        if(0<=stage && stage<39)
        {
            //Debug.Log("stage가 0~39사이");
            Plant[0].interactable = true;
            Plant[0].GetComponent<Image>().sprite = Plant_On[0];
            Btn_GameStage[0].interactable = true;

            for (i = 1; i < stage_pass+1; i++)//37의 경우 그 아래 plant6-btn5까진 다 열리게 만들고
            {
                Plant[i].GetComponent<Image>().sprite = Plant_On[i];
                Plant[i].interactable = true;
                if (Clicked_Plant_Num == stage_pass)
                {
                    Plant[stage_pass].GetComponent<Image>().sprite = Plant_On[stage_pass];
                    Plant[stage_pass].interactable = true;
                    for (i = 0; i < stage_pass_pass; i++)
                    {
                        Btn_GameStage[i].interactable = true;
                        Btn_GameStage[stage_pass_pass - i].interactable = false;
                    }
                }
                else
                {
                    for (int m = 0; m < 5; m++)
                    {
                        Btn_GameStage[m].interactable = true;
                    }
                }
            }

            
            //if (Clicked_Plant_Num == stage_pass)
            //{
            //    Debug.Log("Clicked_Plant_Num == stage_pass");
            //    Plant[Clicked_Plant_Num].GetComponent<Image>().sprite = Plant_On[Clicked_Plant_Num];
            //    Plant[Clicked_Plant_Num].interactable = true;
            //    for (j = 0; j < 5; j++)
            //    {
            //        Btn_GameStage[j].interactable = true;
            //    }
            //}
            //else
            //{
            //    Debug.Log("Clicked_Plant_Num =/= stage_pass");
            //    for (j = 0; j < stage_pass_pass + 1; j++)
            //    {
            //        Btn_GameStage[j].interactable = true;
            //    }
            //}


            //for (j = 0; j < stage_pass_pass + 1; j++)
            //{
            //    Btn_GameStage[j].interactable = true;
            //}

            //---------------------------------------------------------------위에가 버튼 열리게 하는거
        }

        else
        {
            //Debug.Log("stage가 0~39사이");
            stage = 39;
            for (i = 0; i < 8; i++)
            {
                Plant[i].GetComponent<Image>().sprite = Plant_On[i];
                Plant[i].interactable = true;
            }

            for (j = 0; j < 5; j++)
            {
                Btn_GameStage[j].interactable = true;
            }



        }
        

       //for (i = 1; i < stage_pass; i++) // i=8
       //{
       //    Plant[i].GetComponent<Image>().sprite = Plant_On[i];
       //    Plant[i].interactable = true;
       //
       //    for (j = 0; j < stage_pass_pass; j++)
       //    {
       //        Btn_GameStage[j].interactable = true;
       //    }
       //}
        


    }
    void openStage()
    {
        
        
    }
    void Plant_Click_Off()
    {

        for (int i = 1; i < 8; i++)
        {
            Plant[i].interactable = false; // 버튼 클릭을 비활성화
        }

        for (int j = 0; j < 5; j++)
        {
            Btn_GameStage[j].interactable = false;
        }
        
        
        

    }
    //----------------------------------------------------------------


    //----------------------------------------------------------------
    void MoveScene()
    {
        SceneManager.LoadSceneAsync(SceneName);
    }

    void Scene_initialization()
    {
        Back_Btn.onClick.RemoveAllListeners();
        SetPannel_On_Btn.onClick.RemoveAllListeners();
        SetPannel_Off_Btn.onClick.RemoveAllListeners();
        Btn_StartGame.onClick.RemoveAllListeners();
        Btn_GamePannal_Off.onClick.RemoveAllListeners();

        for (int n = 0; n < 8; n++)
        {
            Plant[n].onClick.RemoveAllListeners();

        }
        WorldScroll_On();
        SetPannel_Off();
        SeletedPannel_Off();
        GamePannel_Off();
        BackBtn_Off();

    }
    void GameStart()
    {

        //Scene_initialization();
        SceneManager.LoadSceneAsync(3);//게임씬으로 이동
        Fader.FadeIn(0.2f, () => { SceneManager.LoadSceneAsync(3); });
    }
    void OnClick_GamePannal_Exit()
    {
        GamePannel_Off();
    }
    //----------------------------------------------------------------
    //----------------------------------------------------------------
    //----------------------------------------------------------------
    void SetPannel_On()
    {
        SetPannel.SetActive(true);
    }
    void SetPannel_Off()
    {
        SetPannel.SetActive(false);
    }
    void WorldScroll_On()
    {
        WorldScroll.SetActive(true);
    }
    void WorldScroll_Off()
    {
        WorldScroll.SetActive(false);
    }
    void SeletedPannel_On()
    {
        SeletedPannel.SetActive(true);
    }
    void SeletedPannel_Off()
    {
        SeletedPannel.SetActive(false);
    }
    void GamePannel_On()
    {
        GamePannel.SetActive(true);
    }
    void GamePannel_Off()
    {
        GamePannel.SetActive(false);
    }
    void SetBtn_On()
    {
        SetPannel_On_Btn.gameObject.SetActive(true);
    }
    void SetBtn_Off()
    {
        SetPannel_On_Btn.gameObject.SetActive(false);
    }
    void BackBtn_On()
    {
        Back_Btn.gameObject.SetActive(true);
    }
    void BackBtn_Off()
    {
        Back_Btn.gameObject.SetActive(false);
    }

    //void Go_Main_Btn_On()
    //{
    //    Go_Main_Btn.gameObject.SetActive(true);
    //}
    //void Go_Main_Btn_Off()
    //{
    //    Go_Main_Btn.gameObject.SetActive(false);
    //}

    //----------------------------------------------------------------


    //----------------------------------------------------------------
    //----------------------------------------------------------------
    //----------------------------------------------------------------
    public void SetPannel_On_Btn_onclick()
    {

        SetPannel_On();
    }
    public void SetPannel_Off_Btn_onclick()
    {
        SetPannel_Off();
    }
    //---------------------------------------------------------------------------------------------------------------------------
    public void Selected_Pannel_Off()
    {
        SeletedPannel_Off();
        WorldScroll_Off();
    }
    public void Selected_Pannel_On()
    {
        SeletedPannel_On();
        //Go_Main_Btn_Off();

    }
    //---------------------------------------------------------------------------------------------------------------------------
    void Plant_Btn_Onclick_GoGamePannal()
    {
        Selected_Pannel_Off();
        SeletedPannel.SetActive(true);
    }
    //----------------------------------------------------------------
    public void WorldSelected_ExitBtn()
    {
        WorldScroll.SetActive(true);
        SetPannel_On_Btn.gameObject.SetActive(true);
        SeletedPannel.SetActive(false);
        GamePannel.SetActive(false);
    }
    //--------------------------------------------------------------------

    //--------------------------------------------------------------------

    //--------------------------------------------------------------------


    //--------------------------------------------------------------------


    public void GameScene_To_Home()
    {

    }

    public void Btn_onclick()
    {
        SetPannel_Off_Btn.onClick.AddListener(() => setExit_Btn_onclick());
    }
    void setExit_Btn_onclick()
    {
        SetPannel.SetActive(false);

        SetPannel_On_Btn.gameObject.SetActive(true);

    }

}
