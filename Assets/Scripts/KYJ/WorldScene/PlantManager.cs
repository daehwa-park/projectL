using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlantManager : MonoBehaviour
{
    public GameObject PlantImage;

    public Sprite [] Plant_off;
    public Sprite [] Plant_on;
    public Button[] Plant;

    public Text[] stage_Num;


    int stage;

    

    // Start is called before the first frame update
    void Start()
    {
        stage = 40;//DataManager.instance.userGameData.stageClear;
        Debug.Log(stage);
        //PlantImage = GameObject.Find("PlantImage");
        //addlistener();
        Plant_Off();//두개 순서 지키기 1번
        Change_Plant();//두개 순서 지키기 2번

        for (int i = 0; i < 8; i++)
        {
            Debug.Log("asdf");
            int temp = i;
            int temp2 = i;

            Plant[temp].onClick.AddListener(() => Plant_Click(temp));
        }

        //PlantImage.GetComponent<Image>().sprite = Plant_on[3];

    }
    void Update()
    {
        
    }
    public void Plant_Click(int i)
    {
        Debug.Log("asdf6");
        PlantImage.GetComponent<Image>().sprite = Plant_on[i];
    }
    public void stage_Num_chage(int j)
    {
        for (int i = 1; i < 6; i++)
        {
            stage_Num[i].text = j+"-"+i;
            Debug.Log("asdf3");
        }
    }
    /*
    public void addlistener()
    {
        for(int i=0;i<8;i++)
        {
            Plant[i].onClick.AddListener(() => Plant_Click(i));
        }
    }
    public void Plant_Click(int j)
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[j];
    }*/

    /*
    public void Plant_Click_0()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[0];
        stage_Num_chage(1);
    }
    public void Plant_Click_1()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[1];
        stage_Num_chage(2);
    }
    public void Plant_Click_2()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[2];
        stage_Num_chage(3);
    }
    public void Plant_Click_3()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[3];
    }
    public void Plant_Click_4()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[4];
    }
    public void Plant_Click_5()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[5];
    }
    public void Plant_Click_6()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[6];
    }
    public void Plant_Click_7()
    {
        PlantImage.GetComponent<Image>().sprite = Plant_on[7];
    }
    */
    // Update is called once per frame


    public void Change_Plant()
    {
        for (int i = 0; i < 8; i++)
        {
            //lant[i] = GetComponent<Sprite>();
        }
        if (stage >= 0)//행성 0번
        {
            Plant[0].GetComponent<Image>().sprite = Plant_on[0];
            Debug.Log("0번 바뀜");
            Plant[0].interactable = true;
            if (stage >= 5)//행성 1번
            {
                Plant[1].GetComponent<Image>().sprite = Plant_on[1];
                Debug.Log("1번 바뀜");
                Plant[1].interactable = true;
                if (stage >= 10)//행성 2번
                {
                    Plant[2].GetComponent<Image>().sprite = Plant_on[2];
                    Debug.Log("2번 바뀜");
                    Plant[2].interactable = true;
                    if (stage >= 15)//행성 3번
                    {
                        Plant[3].GetComponent<Image>().sprite = Plant_on[3];
                        Debug.Log("3번 바뀜");
                        Plant[3].interactable = true;
                        if (stage >= 20)//행성 4번
                        {
                            Plant[4].GetComponent<Image>().sprite = Plant_on[4];
                            Debug.Log("4번 바뀜");
                            Plant[4].interactable = true;
                            if (stage >= 25)//행성 5번
                            {
                                Plant[5].GetComponent<Image>().sprite = Plant_on[5];
                                Debug.Log("5번 바뀜");
                                Plant[5].interactable = true;
                                if (stage >= 30)//행성 6번
                                {
                                    Plant[6].GetComponent<Image>().sprite = Plant_on[6];
                                    Debug.Log("6번 바뀜");
                                    Plant[6].interactable = true;
                                    if (stage >= 35)//행성 7번
                                    {
                                        Plant[7].GetComponent<Image>().sprite = Plant_on[7];
                                        Debug.Log("7번 바뀜");
                                        Plant[7].interactable = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    void Plant_Off()
    {
        for (int i=0;i<8;i++)
        {
            Plant[i].interactable = false; // 버튼 클릭을 비활성화
        }
    }





}
