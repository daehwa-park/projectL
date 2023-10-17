using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;



public class WorldSelectedCanvasManager : MonoBehaviour
{
    //-------------------------------------------------------------------
    /*
    public static WorldSelectedCanvasManager instance;
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

    
    //-------------------------------------------------------------------외부 오브젝트 
    GameObject worldcanvas;
    GameObject selectworld;
    //-------------------------------------------------------------------고정 오브젝트 변수 사항
    Button Back_Btn;
    GameObject GamePannel;
    Button stage_1_btn;
    //-------------------------------------------------------------------수치 변동 변수 사항
    Text worldNum_UP_txt;
    Text worldName_UP_txt;
    Image world_Img;
    Text worldStageNum_txt;
    // Image clearContion_Img;
    // Text clearContion_Text;
    //-------------------------------------------------------------------




    //-------------------------------------------------------------------
    void Start()
    {

        Back_Btn = GameObject.Find("SelectedWorldPanel").transform.Find("Back_Btn").GetComponent<Button>();
        GamePannel = GameObject.Find("BackGround").transform.Find("gameStartPannel").gameObject;

        selectworld = WorldToMoveManager.instance.SeletedPannel;
        worldcanvas = WorldToMoveManager.instance.WorldScroll;

        GamePannel.SetActive(false);
        Back_Btn.gameObject.SetActive(true);



    }
    // Update is called once per frame
    void Update()
    {
        
    }
    //--------------------------------------------------------------------
    public void WorldSelected_ExitBtn()
    {
        //GameObject.Find("WorldCanvas").transform.GetChild(0).gameObject.SetActive(true);
        //GameObject.Find("WorldCanvas").transform.GetChild(1).gameObject.SetActive(true);

        //DestroyImmediate(WorldCanvas, true);
        WorldScroll.SetActive(true);
        WorldToMoveManager.instance.SetPannel_On_Btn.gameObject.SetActive(true);
        SeletedPannel.SetActive(false);
        GamePannel.SetActive(false);
    }
    //--------------------------------------------------------------------
    public void GameStart()
    {
        GamePannel.SetActive(false);
        SceneManager.LoadScene(3);//게임씬으로 이동
    }
    //--------------------------------------------------------------------

    //--------------------------------------------------------------------
    public void gamePannelOn()
    {
        //GameObject.Find("WorldSelectCanvas").transform.Find("gameStartPannel").gameObject.SetActive(false);
        GamePannel.SetActive(true);

        //GameObject.Find("Panel").transform.Find("gameStartPannel").gameObject.SetActive(true);
    }
    public void gamePannelOn_NoBtn()
    {
        GamePannel.SetActive(false);

        //GameObject.Find("Panel").transform.Find("gameStartPannel").gameObject.SetActive(false);
    }
    //--------------------------------------------------------------------
    public void game_Onclicked_1_1_gamePannelOn()
    {
        //GameObject.Find("WorldSelectCanvas").transform.Find("gameStartPannel").gameObject.SetActive(false);
        //GameObject.Find("Panel").transform.Find("gameStartPannel").gameObject.SetActive(true);
        GamePannel.SetActive(true);

    }

    
    */
}
