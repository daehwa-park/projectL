using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;
    
    [SerializeField] TextMeshProUGUI m_text;
    [SerializeField] TextMeshProUGUI m_name;
    [SerializeField] GameObject[] m_Char;
    [SerializeField] Sprite[] m_back;

    List<Dictionary<string, object>>[] data_Dialog = new List<Dictionary<string, object>>[10];
    int index = 0;
    public int stage = 0;

    public GameObject FadeCon;
    public FadeController Fader;
    public Button nextDialog;
    public Image background;
    public GameObject ending;

    bool isfirstFade = false;
    bool isEnding = false;
    

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Application.targetFrameRate = 60;

        //stagemanager 계속 살아있는지 확인하고 storynum 변수만들기
        //stage = 2;

        stage = StoryStageController.Instance.StoryStageNum;
        
        Fader.FadeOut(0.6f);

        if (stage == 1 || stage == 8)
        {
            background.sprite = m_back[0];
        }
        else
        {
            background.sprite = m_back[1];
        }

        for (int i = 0; i < 10; i++) //데이터 읽기 -> 그냥 원하는것만 불러와도 될듯
        {
            data_Dialog[i] = CSVReader.Read($"scenario_script_{i}");
        }

        int indexNum = data_Dialog[stage].Count;

        CharChange();
        TextChange();

        nextDialog.onClick.AddListener(() => {
            index++;
            if (index == indexNum)
            {
                if (stage == 8)
                {
                    StoryStageController.Instance.StoryStageNum = 9;
                    Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync("StoryScene"); });
                }
                else if(stage == 9)
                {
                    ending.SetActive(true);
                    isEnding = true;
                }
                else
                {
                    BackToStage();
                }
            }
            else
            {
                if (isEnding)
                {
                    Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync(0); });
                }
            }

            CharChange();
            TextChange();
        });

        //test();
    }

    void MoveLeft(int num)
    {
        
    }

    void MoveRight(int num)
    {

    }

    void ReverseChar()
    {

    }

    void TextChange()
    {
        string data = data_Dialog[stage][index]["talk"].ToString();
        string name = data_Dialog[stage][index]["name"].ToString();

        m_name.text = name; 
        m_text.text = data;
        
    }

    void CharChange()
    {
        string charact = data_Dialog[stage][index]["name"].ToString();
        string rev = data_Dialog[stage][index]["reverse"].ToString();

        if (rev == "0")
        {
            if (charact == "어린왕자")
            {
                m_Char[0].SetActive(true);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "술꾼")
            {
                m_Char[4].transform.localPosition = new Vector3(80, 5, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(true);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "왕")
            {
                m_Char[2].transform.localPosition = new Vector3(80, 0, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(true);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "허풍선이")
            {
                m_Char[3].transform.localPosition = new Vector3(80, 18, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(true);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "사업가")
            {
                m_Char[5].transform.localPosition = new Vector3(80, -20, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(true);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "점등인")
            {
                m_Char[6].transform.localPosition = new Vector3(80, -5, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(true);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "지리학자")
            {
                m_Char[7].transform.localPosition = new Vector3(80, -5, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(true);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "여우")
            {
                m_Char[1].transform.localPosition = new Vector3(80, 20, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(true);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "조종사")
            {
                m_Char[8].transform.localPosition = new Vector3(80, 10, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(true);
                m_Char[9].SetActive(false);
            }
            else
            {
                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
        }
        else
        {
            if (charact == "어린왕자")
            {
                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(true);
            }
            else if (charact == "￠∮§∀")
            {
                m_Char[4].transform.localPosition = new Vector3(-80, 5, 0);
                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(true);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "왕")
            {
                m_Char[2].transform.localPosition = new Vector3(-80, 0, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(true);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "ŁÆŧÐ")
            {
                m_Char[3].transform.localPosition = new Vector3(-80, 18, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(true);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "Θ∬ㆋ€")
            {
                m_Char[5].transform.localPosition = new Vector3(-80, -20, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(true);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "Ω∑ㆍ㏉")
            {
                m_Char[6].transform.localPosition = new Vector3(-80, -5, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(true);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "¿╀Δ↗")
            {
                m_Char[7].transform.localPosition = new Vector3(-80, -5, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(true);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "여우")
            {
                m_Char[1].transform.localPosition = new Vector3(-80, 20, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(true);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
            else if (charact == "조종사")
            {
                m_Char[8].transform.localPosition = new Vector3(-80, 10, 0);

                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(true);
                m_Char[9].SetActive(false);
            }
            else
            {
                m_Char[0].SetActive(false);
                m_Char[1].SetActive(false);
                m_Char[2].SetActive(false);
                m_Char[3].SetActive(false);
                m_Char[4].SetActive(false);
                m_Char[5].SetActive(false);
                m_Char[6].SetActive(false);
                m_Char[7].SetActive(false);
                m_Char[8].SetActive(false);
                m_Char[9].SetActive(false);
            }
        }
    }

    void BackToStage()
    {
        //SceneManager.LoadSceneAsync("SelectWorldScene");
        Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync("SelectWorldScene"); });
    }

    void test()
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < data_Dialog[j].Count; i++)
            {
                print(data_Dialog[j][i]["talk"].ToString());
            }
        }
    }
}
