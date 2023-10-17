using UnityEngine;

public static class StageReader
{

    public static StageInfo LoadStage(int nStage)
    {
        Debug.Log($"Load Stage : Stage/{GetFileName(nStage)}");

        TextAsset textAsset = Resources.Load<TextAsset>($"Stage/{GetFileName(nStage)}");
        if (textAsset != null)
        {
            StageInfo stageInfo = JsonUtility.FromJson<StageInfo>(textAsset.text);

            Debug.Assert(stageInfo.DoValidation());

            return stageInfo;
        }

        return null;
    }


    static string GetFileName(int nStage)
    {
        return string.Format("stage_{0:D4}", nStage);
    }
}

