using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    Button m_bt;
    public string SceneName;
    // Start is called before the first frame update
    void Start()
    {
        m_bt = this.transform.GetComponent<Button>();
        m_bt.onClick.AddListener(NextScene);
    }

    void NextScene()
    {
        //WorldToMoveManager.instance.World_0Btn_To_GamePlayScene();
        //WorldToMoveManager.instance.SeletedPannel.SetActive(true);


        SceneManager.LoadScene(SceneName);



        //WorldToMoveManager.instance.

        //WorldToMoveManager.instance.SeletedPannel.SetActive(true);
        //WorldToMoveManager.instance.SetPannel_On_Btn.gameObject.SetActive(false);
        //WorldToMoveManager.instance.WorldScroll.SetActive(false);

    }

}
