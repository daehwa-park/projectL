using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class LoginManager : MonoBehaviour
{
    //private static Button btn;
    // Start is called before the first frame update

    public Button creatBtn;
    
    public static LoginManager instance;

    void Awake()//싱글톤 아래와는 다른 방식
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(instance.gameObject);
        }
    }

    

    void Start()
    {

    }

    public void Creat_ID_Gogame()
    {
        //creatBtn.gameObject.GetComponent<Button>();
        //DataManager.instance.userGameData.stageclear = 0;
        //DataManager.instance.userGameData.First_Login = -1;
        //DataManager.instance._save();
        creatBtn.onClick.AddListener(() => {
            MainSceneManager.instance.GoGameFirst();
        });
    }


}

