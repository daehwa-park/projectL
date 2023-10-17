using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public StageController stageController;

    public int MoveNum = 0;
    public int[] ClearNum = new int[8];
    public int[] ClearNumIndex = new int[6];
    public int MoveRow;
    public int MoveCol;
    public int EvalRow;
    public int EvalCol;
    int IndexNum = 6;

    public GameObject ClearResult;
    public GameObject FailResult;
    public FadeController Fader;
    public GameObject FadeCon;
    public Image background;

    public Block Moveblock;
    public BlockQuestType BlockQuest = BlockQuestType.CLEAR_SIMPLE;
    public MatchType BlockMatch = MatchType.NONE;

    public List<KeyValuePair<int, int>> MoveList = new List<KeyValuePair<int, int>>();

    public bool isSwipe = false;

    [SerializeField] TextMeshProUGUI m_MoveText;
    [SerializeField] TextMeshProUGUI[] m_ClearText;
    [SerializeField] Sprite[] m_ClearSprite;
    [SerializeField] Image[] m_ClearImage;
    [SerializeField] Sprite[] m_Background;


    void Start()
    {
        Instance = this;
        Application.targetFrameRate = 60;

        if (StageManager.Instance == null)
        {
            stageController.InitStage(1);
        }
        else
        {
            stageController.InitStage(StageManager.Instance.stageNum);
        }

        if((StageManager.Instance.stageNum -1) / 5 == 0)
        {
            background.sprite = m_Background[0];
        }
        else if((StageManager.Instance.stageNum - 1) / 5 == 1)
        {
            background.sprite = m_Background[1];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 2)
        {
            background.sprite = m_Background[2];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 3)
        {
            background.sprite = m_Background[3];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 4)
        {
            background.sprite = m_Background[4];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 5)
        {
            background.sprite = m_Background[5];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 6)
        {
            background.sprite = m_Background[6];
        }
        else if ((StageManager.Instance.stageNum - 1) / 5 == 7)
        {
            background.sprite = m_Background[0];
        }

        Fader = FadeCon.GetComponent<FadeController>();

        Fader.FadeOut(0.6f);

        Instance.MoveNum = stageController.m_Stage.m_StageBuilder.m_StageInfo.moveNum;

        for (int i = 0; i < 8;i++) 
        {
            Instance.ClearNum[i] = stageController.m_Stage.m_StageBuilder.m_StageInfo.clearNum[i];
        }

        for(int k = 0; k < 6; k++)
        {
            Instance.ClearNumIndex[k] = stageController.m_Stage.m_StageBuilder.m_StageInfo.clearNumIndex[k];
            if(Instance.ClearNumIndex[k] == -1)
            {
                m_ClearText[k].enabled = false;
                m_ClearImage[k].enabled = false;
                IndexNum--;
            }
        }

        for(int i = 0; i< IndexNum; i++)
        {
            m_ClearImage[i].sprite = m_ClearSprite[ClearNumIndex[i]];
        }

        ChangeText();
    }

    public void DecreaseMoveNum()
    {
        MoveNum--;
        if(MoveNum < 0)
        {
            MoveNum = 0;
        }
    }

    public void DecreaseClearNum(int clearNum)
    {
        ClearNum[clearNum]--;
        if(ClearNum[clearNum] < 0)
        {
            ClearNum[clearNum] = 0;
        }
    }

    public void ChangeText()
    {
        for (int i = 0; i < IndexNum; i++)
        {
            m_ClearText[i].text = "" + GameManager.Instance.ClearNum[GameManager.Instance.ClearNumIndex[i]];

        }
        m_MoveText.text = "" + GameManager.Instance.MoveNum;
    }
}
