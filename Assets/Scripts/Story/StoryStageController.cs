using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStageController : MonoBehaviour
{
    public static StoryStageController Instance;

    public int StoryStageNum = 0;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
