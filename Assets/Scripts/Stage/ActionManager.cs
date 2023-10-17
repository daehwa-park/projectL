using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionManager 
{
    Transform m_Container;          //컨테이저 (Board GameObject)
    Stage m_Stage;                  
    MonoBehaviour m_MonoBehaviour;//코루틴 호출시 필요한 MonoBehaviour

    bool m_bRunning;                //액션 실행 상태 : 실행중인 경우 true

    public ActionManager(Transform container, Stage stage)
    {
        m_Container = container;
        m_Stage = stage;

        m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return m_MonoBehaviour.StartCoroutine(routine);
    }

    public void DoSwipeAction(int nRow, int nCol, Swipe swipeDir)
    {
        Debug.Assert(nRow >= 0 && nRow < m_Stage.maxRow && nCol >= 0 && nCol < m_Stage.maxCol);

        if (m_Stage.IsValideSwipe(nRow, nCol, swipeDir))
        {
            StartCoroutine(CoDoSwipeAction(nRow, nCol, swipeDir));
        }
    }

    IEnumerator CoDoSwipeAction(int nRow, int nCol, Swipe swipeDir)
    {
        if (!m_bRunning) 
        {
            m_bRunning = true;   

            //SoundManager.instance.PlayOneShot(Clip.Chomp);

            
            Returnable<bool> bSwipedBlock = new Returnable<bool>(false);
            yield return m_Stage.CoDoSwipeAction(nRow, nCol, swipeDir, bSwipedBlock);
            GameManager.Instance.isSwipe = true;
            GameManager.Instance.EvalRow = nRow;
            GameManager.Instance.EvalCol = nCol;

            if (swipeDir == Swipe.RIGHT)
            {
                GameManager.Instance.MoveRow = nRow;
                GameManager.Instance.MoveCol = nCol + 1;
            }
            else if (swipeDir == Swipe.UP)
            {
                GameManager.Instance.MoveRow = nRow + 1;
                GameManager.Instance.MoveCol = nCol;
            }
            else if (swipeDir == Swipe.LEFT)
            {
                GameManager.Instance.MoveRow = nRow;
                GameManager.Instance.MoveCol = nCol - 1;
            }
            else if (swipeDir == Swipe.DOWN)
            {
                GameManager.Instance.MoveRow = nRow - 1;
                GameManager.Instance.MoveCol = nCol;
            }

            GameManager.Instance.Moveblock = m_Stage.board.blocks[GameManager.Instance.MoveRow, GameManager.Instance.MoveCol];

            if (bSwipedBlock.value)
            {
                Returnable<bool> bMatchBlock = new Returnable<bool>(false);
                yield return EvaluateBoard(bMatchBlock);
                

                if (!bMatchBlock.value)
                {
                    yield return m_Stage.CoDoSwipeAction(nRow, nCol, swipeDir, bSwipedBlock);
                }
                else
                {
                    //이동 횟수 감소
                    GameManager.Instance.DecreaseMoveNum();
                    GameManager.Instance.ChangeText();
                }
            }
            GameManager.Instance.isSwipe = false;
            m_bRunning = false; 
        }
        yield break;
    }
    bool isClearCondition()
    {
        if(GameManager.Instance.ClearNum[0] <= 0 && GameManager.Instance.ClearNum[1] <= 0 && GameManager.Instance.ClearNum[2] <= 0 && GameManager.Instance.ClearNum[3] <= 0 && GameManager.Instance.ClearNum[4] <= 0 && GameManager.Instance.ClearNum[5] <= 0 && GameManager.Instance.ClearNum[6] <= 0 && GameManager.Instance.ClearNum[7] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator EvaluateBoard(Returnable<bool> matchResult)
    {
        while (true)    
        {
            Returnable<bool> bBlockMatched = new Returnable<bool>(false);
            yield return StartCoroutine(m_Stage.Evaluate(bBlockMatched));

            if (bBlockMatched.value)
            {
                matchResult.value = true;

                SoundManager.instance.PlayOneShot(Clip.BlockClear);

                //폭탄 생성
                if (GameManager.Instance.MoveList.Count != 0)
                {
                    int RandomBlock = UnityEngine.Random.Range(0, GameManager.Instance.MoveList.Count);
                    if (GameManager.Instance.BlockQuest == BlockQuestType.CLEAR_HORZ)
                    {
                        m_Stage.board.SpawnBlock(GameManager.Instance.MoveList[RandomBlock].Key, GameManager.Instance.MoveList[RandomBlock].Value, BlockType.HBOOM);
                    }
                    else if(GameManager.Instance.BlockQuest == BlockQuestType.CLEAR_VERT)
                    {
                        m_Stage.board.SpawnBlock(GameManager.Instance.MoveList[RandomBlock].Key, GameManager.Instance.MoveList[RandomBlock].Value, BlockType.VBOOM);
                    }
                    else if(GameManager.Instance.BlockQuest == BlockQuestType.CLEAR_CIRCLE)
                    {
                        m_Stage.board.SpawnBlock(GameManager.Instance.MoveList[RandomBlock].Key, GameManager.Instance.MoveList[RandomBlock].Value, BlockType.CBOOM);
                    }

                    GameManager.Instance.MoveList.Clear();
                    GameManager.Instance.BlockQuest = BlockQuestType.CLEAR_SIMPLE;
                }
                GameManager.Instance.isSwipe = false;

                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(m_Stage.PostprocessAfterEvaluate());

                if (m_Stage.board.ShuffleEvaluate()) //셔플 판정
                {
                    BoardShuffler shuffler = new BoardShuffler(m_Stage.board, true);
                    shuffler.Shuffle();  // 셔플
                }
            }
            else
                break;  
        }

        if (isClearCondition())
        {
            yield return new WaitForSeconds(0.2f);

            if (StageManager.Instance.stageNum % 5 == 0)
            {
                StoryStageController.Instance.StoryStageNum = StageManager.Instance.stageNum / 5;

                //if (StageManager.Instance.stageNum == (DataManager.instance.userGameData.stageClear + 1))
                //{
                //    DataManager.instance.userGameData.stageClear += 1;
                //    DataManager.instance._save();
                //}

                GameManager.Instance.Fader.FadeIn(0.6f, () => { SceneManager.LoadSceneAsync("StoryScene"); });
                //SceneManager.LoadSceneAsync("StoryScene");
            }
            else
            {
                GameManager.Instance.ClearResult.SetActive(true);
            }
        }
        else if (GameManager.Instance.MoveNum <= 1)
        {
            yield return new WaitForSeconds(0.2f);

            GameManager.Instance.FailResult.SetActive(true);
        }
        

        yield break;
    }

}


