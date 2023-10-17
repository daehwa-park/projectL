using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;

[System.Serializable]
public class UserData
{
    public int First_Login;  
    public int stageclear;// 스테이지 클리어 상태 저장변수
}
public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public UserData userGameData = new UserData();
    public string path;
    public string fileName = "userdata";
    
    private void Awake()//싱글톤 아래와는 다른 방식
    {
        #region
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        #endregion

        instance = this;
        //path = Application.streamingAssetsPath;//Application.dataPath + "/StreamingAssets";

  
        //Debug.Log(path);


        //First_Login_int = PlayerPrefs.GetInt(First_Login);
        //
        //if (First_Login_int == 0)
        //{
        //    stageclear = 0;
        //    PlayerPrefs.SetFloat(stageclearPref, stageclear_Float);
        //    PlayerPrefs.SetInt(First_Login, -1);
        //}
        //else
        //{
        //    stageclear_Float = PlayerPrefs.GetFloat(stageclearPref);
        //
        //}




    }
    //private static readonly string First_Login = "First_Login";
    //private static readonly string stageclearPref = "Stage_Clear";
    //
    //public int First_Login_int;
    ////public int First_Login;
    //public int stageclear;
    //private float stageclear_Float;

   
    void Start()
    {

    }

    void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            _save();
        }
    }

    public void _save()
    {
        string data = JsonUtility.ToJson(userGameData);
        //Debug.Log(path);
        File.WriteAllText(path+"/" + fileName+".json", data);
        //print(path);
        Debug.Log(userGameData.First_Login);
        Debug.Log(userGameData.stageclear);


        //PlayerPrefs.SetFloat(stageclearPref, stageclear);


    }
    public void _Load()
    {
        //stageclear = PlayerPrefs.GetInt(stageclearPref);
       string data = File.ReadAllText(path+"/" + fileName+".json");
       userGameData = JsonUtility.FromJson<UserData>(data);
        Debug.Log(userGameData.First_Login);
        Debug.Log(userGameData.stageclear);

    }

    public void DataClear()
    {
        userGameData = new UserData();
    }

}
