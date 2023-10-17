using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Stage
{
    public int maxRow { get { return m_Board.maxRow; } }
    public int maxCol { get { return m_Board.maxCol; } }

   Board m_Board;
    public Board board { get { return m_Board; } }

    public StageBuilder m_StageBuilder;

    public Block[,] blocks { get { return m_Board.blocks; } }
    public Cell[,] cells { get { return m_Board.cells; } }

    public Stage(StageBuilder stageBuilder, int nRow, int nCol)
    {
        m_StageBuilder = stageBuilder;

        m_Board = new Board(nRow, nCol);
    }

    internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container, GameObject moonPrefab)
    {
        m_Board.ComposeStage(cellPrefab, blockPrefab, container, m_StageBuilder, moonPrefab);
    }

    public IEnumerator CoDoSwipeAction(int nRow, int nCol, Swipe swipeDir, Returnable<bool> actionResult)
    {
        actionResult.value = false; //코루틴 리턴값 RESET

        int nSwipeRow = nRow, nSwipeCol = nCol;
        nSwipeRow += swipeDir.GetTargetRow(); //Right : +1, LEFT : -1
        nSwipeCol += swipeDir.GetTargetCol(); //UP : +1, DOWN : -1

        Debug.Assert(nRow != nSwipeRow || nCol != nSwipeCol, "Invalid Swipe : ({nSwipeRow}, {nSwipeCol})");
        Debug.Assert(nSwipeRow >= 0 && nSwipeRow < maxRow && nSwipeCol >= 0 && nSwipeCol < maxCol, $"Swipe 타겟 블럭 인덱스 오류 = ({nSwipeRow}, {nSwipeCol}) ");

        if (m_Board.IsSwipeable(nSwipeRow, nSwipeCol))
        {
            Block targetBlock = blocks[nSwipeRow, nSwipeCol];
            Block baseBlock = blocks[nRow, nCol];
            Debug.Assert(baseBlock != null && targetBlock != null);

            Vector3 basePos = baseBlock.blockObj.transform.position;
            Vector3 targetPos = targetBlock.blockObj.transform.position;

            if (targetBlock.IsSwipeable(baseBlock))
            {
                baseBlock.MoveTo(targetPos, Constants.SWIPE_DURATION);
                targetBlock.MoveTo(basePos, Constants.SWIPE_DURATION);

                yield return new WaitForSeconds(Constants.SWIPE_DURATION);

                blocks[nRow, nCol] = targetBlock;
                blocks[nSwipeRow, nSwipeCol] = baseBlock;

                actionResult.value = true;
            }
        }

        yield break;
    }

    public IEnumerator Evaluate(Returnable<bool> matchResult)
    {
        if(GameManager.Instance.Moveblock.type >= BlockType.VBOOM)
        {
            yield return m_Board.EvaluateSpecialBlock(matchResult);
        }
        else
        {
            yield return m_Board.Evaluate(matchResult);
        }
    }

    public IEnumerator PostprocessAfterEvaluate()
    {
        List<KeyValuePair<int, int>> unfilledBlocks = new List<KeyValuePair<int, int>>();
        List<Block> movingBlocks = new List<Block>();

        yield return m_Board.ArrangeBlocksAfterClean(unfilledBlocks, movingBlocks);

        yield return m_Board.SpawnBlocksAfterClean(movingBlocks);

        yield return WaitForDropping(movingBlocks);
    }

    public IEnumerator WaitForDropping(List<Block> movingBlocks)
    {
        WaitForSeconds waitForSecond = new WaitForSeconds(0.05f); 

        while (true)
        {
            bool bContinue = false;

            for (int i = 0; i < movingBlocks.Count; i++)
            {
                if (movingBlocks[i].isMoving)
                {
                    bContinue = true;
                    break;
                }
            }

            if (!bContinue)
                break;

            yield return waitForSecond;
        }

        movingBlocks.Clear();
        yield break;
    }

    #region Simple Methods

    public bool IsInsideBoard(Vector2 ptOrg)
    {
        Vector2 point = new Vector2(ptOrg.x + (maxCol / 2.0f), ptOrg.y + (maxRow / 2.0f));

        if (point.y < 0 || point.x < 0 || point.y > maxRow || point.x > maxCol)
            return false;

        return true;
    }

    public bool IsOnValideBlock(Vector2 point, out BlockPos blockPos)
    {
        Vector2 pos = new Vector2(point.x + (maxCol/ 2.0f), point.y + (maxRow / 2.0f));
        int nRow = (int)pos.y;
        int nCol = (int)pos.x;

        blockPos = new BlockPos(nRow, nCol);

        return board.IsSwipeable(nRow, nCol);
    }


    public bool IsValideSwipe(int nRow, int nCol, Swipe swipeDir)
    {
        switch (swipeDir)
        {
            case Swipe.DOWN:
                if (nRow > 0)
                {
                    if (blocks[nRow - 1, nCol].type < 0 || blocks[nRow, nCol].type < 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                    return false;
            case Swipe.UP:
                if (nRow < maxRow - 1)
                {
                    if (blocks[nRow + 1, nCol].type < 0 || blocks[nRow, nCol].type < 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                    return false;
            case Swipe.LEFT:
                if (nCol > 0)
                {
                    if (blocks[nRow, nCol - 1].type < 0 || blocks[nRow, nCol].type < 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                    return false;
            case Swipe.RIGHT:
                if (nCol < maxCol - 1)
                {
                    if (blocks[nRow, nCol + 1].type < 0 || blocks[nRow, nCol].type < 0)
                    {
                        return false;
                    }
                    return true;
                }
                else
                    return false;
            default:
                return false;
        }
    }
    #endregion

    public void PrintAll()
    {
        System.Text.StringBuilder strCells = new System.Text.StringBuilder();
        System.Text.StringBuilder strBlocks = new System.Text.StringBuilder();

        for (int nRow = maxRow -1; nRow >=0; nRow--)
        {
            for (int nCol = 0; nCol < maxCol; nCol++)
            {
                strCells.Append($"{cells[nRow, nCol].type}, ");
                strBlocks.Append($"{blocks[nRow, nCol].breed}, ");
            }

            strCells.Append("\n");
            strBlocks.Append("\n");
        }

        Debug.Log(strCells.ToString());
        Debug.Log(strBlocks.ToString());
    }
}
