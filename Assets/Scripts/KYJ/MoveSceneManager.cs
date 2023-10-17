using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
public class MoveSceneManager : MonoBehaviour
{
    public GameObject Setting_ExitBtn;
    public Canvas SettingCanvas;
    public GameObject settingBtn;

    void Start()
    {
        //SettingCanvas.gameObject.SetActive(false);
        settingBtn = GetComponent<GameObject>();
    }


    void Update()
    {

    }

    //---------------------------------------------------------------------------------------
    public void LoginBtn()
    {
        SceneManager.LoadScene("Login");
    }
    //---------------------------------------------------------------------------------------
    public void StartBtn()
    {
        SceneManager.LoadScene("SelectWorldScene");
    }
    //---------------------------------------------------------------------------------------
    public void PreferenceBtn()
    {
        //SceneManager.LoadScene("PreferenceScene");
    }
    //---------------------------------------------------------------------------------------
    public void ExitBtn()
    {
        Application.Quit();
    }
    //---------------------------------------------------------------------------------------
    public void StageToMain()
    {
        SceneManager.LoadScene("SelectWorldScene");
    }
    //---------------------------------------------------------------------------------------
    public void MoveToSetting()
    {
        //SettingCanvas.gameObject.SetActive(true);
        //Instantiate<Canvas>(SettingCanvas);
        SceneManager.LoadScene(2);

        //settingBtn.gameObject.SetActive(false);

    }
    //---------------------------------------------------------------------------------------
    public void SettingToOut()
    {
        //SettingCanvas.gameObject.SetActive(false);
        //Destroy(SettingCanvas);
        settingBtn.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------

}
