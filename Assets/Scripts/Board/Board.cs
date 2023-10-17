using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board
{
    int m_nRow;
    int m_nCol;

    public int maxRow { get { return m_nRow; } }
    public int maxCol { get { return m_nCol; } }

    Cell[,] m_Cells;
    public Cell[,] cells { get { return m_Cells; } }

    Block[,] m_Blocks;
    public Block[,] blocks { get { return m_Blocks; } }

    Transform m_Container;
    GameObject m_CellPrefab;
    GameObject m_BlockPrefab;
    GameObject m_MoonPrefab;
    StageBuilder m_StageBuilder;

    BoardEnumerator m_Enumerator;

    public bool isBoom = false;
    public bool isAlreadyCheck = false;
    public bool isVBoom = false;

    public Board(int nRow, int nCol)
    {
        m_nRow = nRow;
        m_nCol = nCol;

        m_Cells = new Cell[nRow, nCol];
        m_Blocks = new Block[nRow, nCol];

        m_Enumerator = new BoardEnumerator(this);
    }

    internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container, StageBuilder stageBuilder, GameObject moonPrefab)
    {
        m_CellPrefab = cellPrefab;
        m_BlockPrefab = blockPrefab;
        m_Container = container;
        m_StageBuilder = stageBuilder;
        m_MoonPrefab = moonPrefab;

        float initX = CalcInitX(0.5f);
        float initY = CalcInitY(0.5f);
        for (int nRow = 0; nRow < m_nRow; nRow++)
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Cell cell = m_Cells[nRow, nCol]?.InstantiateCellObj(cellPrefab, container);
                cell?.Move(initX + nCol, initY + nRow);

                Block block = m_Blocks[nRow, nCol]?.InstantiateBlockObj(blockPrefab, container);
                block?.Move(initX + nCol, initY + nRow);
            }
    }

    public IEnumerator Evaluate(Returnable<bool> matchResult)
    {
        bool bMatchedBlockFound = UpdateAllBlocksMatchedStatus();

        if(bMatchedBlockFound == false)
        {
            matchResult.value = false;
            yield break;
        }

        for(int nRow = 0; nRow < m_nRow; nRow++)
        {
            for(int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                if(block.match == MatchType.FOUR && isVBoom)
                {
                    block.questType = BlockQuestType.CLEAR_VERT;
                }
                else if(block.match == MatchType.FOUR && !isVBoom)
                {
                    block.questType = BlockQuestType.CLEAR_HORZ;
                }
                else if(block.match >= MatchType.FIVE)
                {
                    block.questType = BlockQuestType.CLEAR_CIRCLE;
                }

                if(block.type < 0)
                {
                    //상하좌우 상자확인
                    if(block.type == BlockType.CHEST)
                    {
                        if(nRow - 1 >= 0)
                        {
                            if(m_Blocks[nRow - 1, nCol].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if(nRow + 1 < m_nRow)
                        {
                            if(m_Blocks[nRow + 1, nCol].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if(nCol - 1 >= 0)
                        {
                            if(m_Blocks[nRow, nCol - 1].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if(nCol + 1 < m_nCol)
                        {
                            if(m_Blocks[nRow, nCol + 1].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }
                    }
                    else if(block.type == BlockType.POSTB)
                    {
                        if (nRow - 1 >= 0)
                        {
                            if (m_Blocks[nRow - 1, nCol].status == BlockStatus.MATCH)
                            {
                                //SpawnBlock(nRow, nCol, m_MoonPrefab);

                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nRow + 1 < m_nRow)
                        {
                            if (m_Blocks[nRow + 1, nCol].status == BlockStatus.MATCH)
                            {
                                //SpawnBlock(nRow, nCol, m_MoonPrefab);

                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol - 1 >= 0)
                        {
                            if (m_Blocks[nRow, nCol - 1].status == BlockStatus.MATCH)
                            {
                                //SpawnBlock(nRow, nCol, m_MoonPrefab);

                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol + 1 < m_nCol)
                        {
                            if (m_Blocks[nRow, nCol + 1].status == BlockStatus.MATCH)
                            {
                                //SpawnBlock(nRow, nCol, m_MoonPrefab);

                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }
                    }
                    
                }

                if (block.status == BlockStatus.MATCH && m_Cells[nRow, nCol].type == CellType.GRASS)
                {
                    m_Cells[nRow, nCol].ChangeCellView(CellType.BASIC);
                    m_Cells[nRow, nCol].type = CellType.BASIC;
                    GameManager.Instance.DecreaseClearNum(7);
                    GameManager.Instance.ChangeText();
                }
            }
        }

        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                block?.DoEvaluation(m_Enumerator, nRow, nCol);

                if (block.isMatchedBlock)
                {
                    if (block.breed == BlockBreed.BREED_0)
                    {
                        GameManager.Instance.DecreaseClearNum(0);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_1)
                    {
                        GameManager.Instance.DecreaseClearNum(1);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_2)
                    {
                        GameManager.Instance.DecreaseClearNum(2);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_3)
                    {
                        GameManager.Instance.DecreaseClearNum(3);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_4)
                    {
                        GameManager.Instance.DecreaseClearNum(4);
                        GameManager.Instance.ChangeText();
                    }

                    block.isMatchedBlock = false;
                }

                if (block.isBoomBlock)
                {
                    KeyValuePair<int, int> kvp;

                    kvp = new KeyValuePair<int, int>(nRow, nCol);

                    GameManager.Instance.MoveList.Add(kvp);

                    if (GameManager.Instance.BlockQuest < block.questType)
                    {
                        GameManager.Instance.BlockQuest = block.questType;
                    }

                    block.isBoomBlock = false;

                }
            }
        }

        List<Block> clearBlocks = new List<Block>();

        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                if (block != null)
                {
                    if (block.status == BlockStatus.CLEAR)
                    {
                        clearBlocks.Add(block);

                        m_Blocks[nRow, nCol] = null;  
                    }
                }
            }
        }

        clearBlocks.ForEach((block) => block.Destroy());
        yield return new WaitForSeconds(0.2f);

        matchResult.value = true;

        yield break;
    }

    public IEnumerator EvaluateSpecialBlock(Returnable<bool> matchResult)
    {
        bool bMatchedBlockFound = UpdateAllBlocksMatchedStatus();

        if (bMatchedBlockFound == false)
        {
            matchResult.value = false;
            yield break;
        }

        BoomActivate();

        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                block.isAlreadyCheckP = false;
            }
        }


        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                if (block.match == MatchType.FOUR && isVBoom)
                {
                    block.questType = BlockQuestType.CLEAR_VERT;
                }
                else if (block.match == MatchType.FOUR && !isVBoom)
                {
                    block.questType = BlockQuestType.CLEAR_HORZ;
                }
                else if (block.match >= MatchType.FIVE)
                {
                    block.questType = BlockQuestType.CLEAR_CIRCLE;
                }

                if (block.type < 0)
                {
                    //상하좌우 상자확인
                    if (block.type == BlockType.CHEST)
                    {
                        if (nRow - 1 >= 0)
                        {
                            if (m_Blocks[nRow - 1, nCol].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nRow + 1 < m_nRow)
                        {
                            if (m_Blocks[nRow + 1, nCol].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol - 1 >= 0)
                        {
                            if (m_Blocks[nRow, nCol - 1].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol + 1 < m_nCol)
                        {
                            if (m_Blocks[nRow, nCol + 1].status == BlockStatus.MATCH && block.status != BlockStatus.CLEAR)
                            {
                                block.status = BlockStatus.CLEAR;
                                GameManager.Instance.DecreaseClearNum(5);
                                GameManager.Instance.ChangeText();
                            }
                        }
                    }
                    else if (block.type == BlockType.POSTB)
                    {
                        if (nRow - 1 >= 0)
                        {
                            if (m_Blocks[nRow - 1, nCol].status == BlockStatus.MATCH)
                            {
                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nRow + 1 < m_nRow)
                        {
                            if (m_Blocks[nRow + 1, nCol].status == BlockStatus.MATCH)
                            {
                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol - 1 >= 0)
                        {
                            if (m_Blocks[nRow, nCol - 1].status == BlockStatus.MATCH)
                            {
                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }

                        if (nCol + 1 < m_nCol)
                        {
                            if (m_Blocks[nRow, nCol + 1].status == BlockStatus.MATCH)
                            {
                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }
                    }

                }

                if (block.status == BlockStatus.MATCH && m_Cells[nRow, nCol].type == CellType.GRASS)
                {
                    m_Cells[nRow, nCol].ChangeCellView(CellType.BASIC);
                    m_Cells[nRow, nCol].type = CellType.BASIC;
                    GameManager.Instance.DecreaseClearNum(7);
                    GameManager.Instance.ChangeText();
                }
            }
        }

        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                block?.DoEvaluation(m_Enumerator, nRow, nCol);

                if (block.isMatchedBlock)
                {
                    if (block.breed == BlockBreed.BREED_0)
                    {
                        GameManager.Instance.DecreaseClearNum(0);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_1)
                    {
                        GameManager.Instance.DecreaseClearNum(1);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_2)
                    {
                        GameManager.Instance.DecreaseClearNum(2);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_3)
                    {
                        GameManager.Instance.DecreaseClearNum(3);
                        GameManager.Instance.ChangeText();
                    }
                    else if (block.breed == BlockBreed.BREED_4)
                    {
                        GameManager.Instance.DecreaseClearNum(4);
                        GameManager.Instance.ChangeText();
                    }

                    block.isMatchedBlock = false;
                }

                if (block.isBoomBlock)
                {
                    KeyValuePair<int, int> kvp;

                    kvp = new KeyValuePair<int, int>(nRow, nCol);

                    GameManager.Instance.MoveList.Add(kvp);
                    if (GameManager.Instance.BlockQuest < block.questType)
                    {
                        GameManager.Instance.BlockQuest = block.questType;
                    }

                    block.isBoomBlock = false;

                }
            }
        }

        List<Block> clearBlocks = new List<Block>();

        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];

                if (block != null)
                {
                    if (block.status == BlockStatus.CLEAR)
                    {
                        clearBlocks.Add(block);

                        m_Blocks[nRow, nCol] = null;
                    }
                }
            }
        }

        clearBlocks.ForEach((block) => block.Destroy());
        yield return new WaitForSeconds(0.2f);

        matchResult.value = true;

        yield break;
    }

    public void BlockClear(int row, int col)
    {
        Block m_block = m_Blocks[row, col];
        switch (m_block.type)
        {
            case BlockType.BASIC:
                m_block.status = BlockStatus.CLEAR;
                break;
            case BlockType.VBOOM:
                for(int i = 0; i<m_nRow;i++)
                {
                    if (m_Cells[i, col].type == CellType.GRASS)
                    {
                        m_Cells[i, col].ChangeCellView(CellType.BASIC);
                        m_Cells[i, col].type = CellType.BASIC;
                        GameManager.Instance.DecreaseClearNum(7);
                        GameManager.Instance.ChangeText();
                    }

                    if (m_Blocks[i, col].type == BlockType.POSTB)
                    {
                        if (!m_Blocks[i, col].isAlreadyCheckP)
                        {
                            m_Blocks[i, col].isAlreadyCheckP = true;
                            GameManager.Instance.DecreaseClearNum(6);
                            GameManager.Instance.ChangeText();
                        }
                    }
                    else if (m_Blocks[i, col].type == BlockType.CHEST && m_Blocks[i, col].status != BlockStatus.CLEAR)
                    {
                        m_Blocks[i, col].status = BlockStatus.CLEAR;
                        GameManager.Instance.DecreaseClearNum(5);
                        GameManager.Instance.ChangeText();
                    }
                    else if (m_Blocks[i, col].type != BlockType.EMPTY)
                    {
                        m_Blocks[i, col].status = BlockStatus.CLEAR;
                        //BlockClear(i, col);
                    }
                }
                break;
            case BlockType.HBOOM:
                for (int i = 0; i < m_nRow; i++)
                { 
                    if (m_Cells[row, i].type == CellType.GRASS)
                    {
                        m_Cells[row, i].ChangeCellView(CellType.BASIC);
                        m_Cells[row, i].type = CellType.BASIC;
                        GameManager.Instance.DecreaseClearNum(7);
                        GameManager.Instance.ChangeText();
                    }

                    if (m_Blocks[row, i].type == BlockType.POSTB)
                    {
                        if (!m_Blocks[row, i].isAlreadyCheckP)
                        {
                            m_Blocks[row, i].isAlreadyCheckP = true;
                            GameManager.Instance.DecreaseClearNum(6);
                            GameManager.Instance.ChangeText();
                        }
                    }
                    else if (m_Blocks[row, i].type == BlockType.CHEST && m_Blocks[row, i].status != BlockStatus.CLEAR)
                    {
                        m_Blocks[row, i].status = BlockStatus.CLEAR;
                        GameManager.Instance.DecreaseClearNum(5);
                        GameManager.Instance.ChangeText();
                    }
                    else if (m_Blocks[row, i].type != BlockType.EMPTY)
                    {
                        m_Blocks[row, i].status = BlockStatus.CLEAR;
                        //BlockClear(row, i);
                    }
                }
                break;
            case BlockType.CBOOM:
                for (int i = row - 1; i <= row + 1; i++)
                {
                    for(int j = col - 1; j<=col + 1;j++) 
                    {
                        if (i < 0 || i >= m_nRow || j < 0 || j >= m_nCol || m_Blocks[i, j].type == BlockType.EMPTY)
                        {
                            continue;
                        }

                        if (m_Cells[i, j].type == CellType.GRASS)
                        {
                            m_Cells[i, j].ChangeCellView(CellType.BASIC);
                            m_Cells[i, j].type = CellType.BASIC;
                            GameManager.Instance.DecreaseClearNum(7);
                            GameManager.Instance.ChangeText();
                        }

                        if (m_Blocks[i, j].type == BlockType.POSTB)
                        {
                            if (!m_Blocks[i, j].isAlreadyCheckP)
                            {
                                m_Blocks[i, j].isAlreadyCheckP = true;
                                GameManager.Instance.DecreaseClearNum(6);
                                GameManager.Instance.ChangeText();
                            }
                        }
                        else if (m_Blocks[i, j].type == BlockType.CHEST && m_Blocks[i, j].status != BlockStatus.CLEAR)
                        {
                            m_Blocks[i, j].status = BlockStatus.CLEAR;
                            GameManager.Instance.DecreaseClearNum(5);
                            GameManager.Instance.ChangeText();
                        }
                        else
                        {
                            m_Blocks[i, j].status = BlockStatus.CLEAR;
                            //BlockClear(i, j);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    public void BoomActivate()
    {
        if (GameManager.Instance.Moveblock.type == BlockType.HBOOM && GameManager.Instance.Moveblock.isCanHboom) //가로 폭탄
        {
            for (int i = 0; i < m_nCol; i++)
            {
                if (m_Cells[GameManager.Instance.MoveRow, i].type == CellType.GRASS)
                {
                    m_Cells[GameManager.Instance.MoveRow, i].ChangeCellView(CellType.BASIC);
                    m_Cells[GameManager.Instance.MoveRow, i].type = CellType.BASIC;
                    GameManager.Instance.DecreaseClearNum(7);
                    GameManager.Instance.ChangeText();
                }

                if (m_Blocks[GameManager.Instance.MoveRow, i].type == BlockType.POSTB)
                {
                    if(!m_Blocks[GameManager.Instance.MoveRow, i].isAlreadyCheckP)
                    {
                        m_Blocks[GameManager.Instance.MoveRow, i].isAlreadyCheckP = true;
                        GameManager.Instance.DecreaseClearNum(6);
                        GameManager.Instance.ChangeText();
                    }
                    
                }
                else if(m_Blocks[GameManager.Instance.MoveRow, i].type == BlockType.CHEST && m_Blocks[GameManager.Instance.MoveRow, i].status != BlockStatus.CLEAR)
                {
                    m_Blocks[GameManager.Instance.MoveRow, i].status = BlockStatus.CLEAR;
                    GameManager.Instance.DecreaseClearNum(5);
                    GameManager.Instance.ChangeText();
                }
                else if (m_Blocks[GameManager.Instance.MoveRow, i].type != BlockType.EMPTY)
                {
                    //m_Blocks[GameManager.Instance.MoveRow, i].status = BlockStatus.CLEAR;
                    BlockClear(GameManager.Instance.MoveRow, i);
                }
            }
            GameManager.Instance.Moveblock.isCanHboom = false;
        }

        if (GameManager.Instance.Moveblock.type == BlockType.VBOOM && GameManager.Instance.Moveblock.isCanVboom) //세로 폭탄
        {
            for (int i = 0; i < m_nRow; i++)
            {
                if (m_Cells[i, GameManager.Instance.MoveCol].type == CellType.GRASS)
                {
                    m_Cells[i, GameManager.Instance.MoveCol].ChangeCellView(CellType.BASIC);
                    m_Cells[i, GameManager.Instance.MoveCol].type = CellType.BASIC;
                    GameManager.Instance.DecreaseClearNum(7);
                    GameManager.Instance.ChangeText();
                }

                if (m_Blocks[i, GameManager.Instance.MoveCol].type == BlockType.POSTB)
                {
                    if (!m_Blocks[i, GameManager.Instance.MoveCol].isAlreadyCheckP)
                    {
                        m_Blocks[i, GameManager.Instance.MoveCol].isAlreadyCheckP = true;
                        GameManager.Instance.DecreaseClearNum(6);
                        GameManager.Instance.ChangeText();
                    }
                }
                else if (m_Blocks[i, GameManager.Instance.MoveCol].type == BlockType.CHEST && m_Blocks[i, GameManager.Instance.MoveCol].status != BlockStatus.CLEAR)
                {
                    m_Blocks[i, GameManager.Instance.MoveCol].status = BlockStatus.CLEAR;
                    GameManager.Instance.DecreaseClearNum(5);
                    GameManager.Instance.ChangeText();
                }
                else if (m_Blocks[i, GameManager.Instance.MoveCol].type != BlockType.EMPTY)
                {
                    //m_Blocks[i, GameManager.Instance.MoveCol].status = BlockStatus.CLEAR;
                    BlockClear(i, GameManager.Instance.MoveCol);
                }
            }
            GameManager.Instance.Moveblock.isCanVboom = false;
        }

        if (GameManager.Instance.Moveblock.type == BlockType.CBOOM && GameManager.Instance.Moveblock.isCanCboom) //원형 폭탄
        {
            for (int i = GameManager.Instance.MoveRow - 1; i <= GameManager.Instance.MoveRow + 1; i++)
            {
                for (int j = GameManager.Instance.MoveCol - 1; j <= GameManager.Instance.MoveCol + 1; j++)
                {
                    if (i < 0 || i >= m_nRow || j < 0 || j >= m_nCol || m_Blocks[i, j].type == BlockType.EMPTY)
                    {
                        continue;
                    }

                    if (m_Cells[i, j].type == CellType.GRASS)
                    {
                        m_Cells[i, j].ChangeCellView(CellType.BASIC);
                        m_Cells[i, j].type = CellType.BASIC;
                        GameManager.Instance.DecreaseClearNum(7);
                        GameManager.Instance.ChangeText();
                    }

                    if (m_Blocks[i, j].type == BlockType.POSTB)
                    {
                        if (!m_Blocks[i, j].isAlreadyCheckP)
                        {
                            m_Blocks[i, j].isAlreadyCheckP = true;
                            GameManager.Instance.DecreaseClearNum(6);
                            GameManager.Instance.ChangeText();
                        }
                    }
                    else if (m_Blocks[i, j].type == BlockType.CHEST && m_Blocks[i, j].status != BlockStatus.CLEAR)
                    {
                        m_Blocks[i, j].status = BlockStatus.CLEAR;
                        GameManager.Instance.DecreaseClearNum(5);
                        GameManager.Instance.ChangeText();
                    }
                    else
                    {
                        BlockClear(i,j);
                    }
                }
            }
            GameManager.Instance.Moveblock.isCanCboom = false;
        }
    }

    public bool UpdateAllBlocksMatchedStatus()
    {
        List<Block> matchedBlockList = new List<Block>();
        int nCount = 0;
        for (int nRow = 0; nRow < m_nRow; nRow++)
        {
            for (int nCol = 0; nCol < m_nCol; nCol++)
            {
                if (EvalBlocksIfMatched(nRow, nCol, matchedBlockList))
                {
                    nCount++;
                }
            }
        }

        return nCount > 0;
    }

    public bool EvalBlocksIfMatched(int nRow, int nCol, List<Block> matchedBlockList)
    {
        bool bFound = false;

        Block baseBlock = m_Blocks[nRow, nCol];
        if (baseBlock == null)
            return false;

        if (baseBlock.match != MatchType.NONE || !baseBlock.IsValidate() || m_Cells[nRow, nCol].IsObstracle())
            return false;


        if (baseBlock.type == BlockType.HBOOM && GameManager.Instance.isSwipe && baseBlock == GameManager.Instance.Moveblock)
        {
            GameManager.Instance.Moveblock.isCanHboom = true;
            GameManager.Instance.isSwipe = false;
            return true;
        }
        else if (baseBlock.type == BlockType.VBOOM && GameManager.Instance.isSwipe && baseBlock == GameManager.Instance.Moveblock)
        {
            GameManager.Instance.Moveblock.isCanVboom = true;
            GameManager.Instance.isSwipe = false;
            return true;
        }
        else if (baseBlock.type == BlockType.CBOOM && GameManager.Instance.isSwipe && baseBlock == GameManager.Instance.Moveblock)
        {
            GameManager.Instance.Moveblock.isCanCboom = true;
            GameManager.Instance.isSwipe = false;
            return true;
        }

        matchedBlockList.Add(baseBlock);

        Block block;

        for (int i = nCol + 1; i < m_nCol; i++)
        {
            block = m_Blocks[nRow, i];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchedBlockList.Add(block);
        }

        for (int i = nCol - 1; i >= 0; i--)
        {
            block = m_Blocks[nRow, i];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchedBlockList.Insert(0, block);
        }

        if (matchedBlockList.Count >= 3)
        {
            SetBlockStatusMatched(matchedBlockList, true);
            bFound = true;
        }

        if (baseBlock.match == MatchType.FOUR) //폭탄 퀘스트 -> 이거있어야되나
        {
            isAlreadyCheck = true;

            isVBoom = true;
        }

        matchedBlockList.Clear();

        matchedBlockList.Add(baseBlock);

        for (int i = nRow + 1; i < m_nRow; i++)
        {
            block = m_Blocks[i, nCol];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchedBlockList.Add(block);
        }

        for (int i = nRow - 1; i >= 0; i--)
        {
            block = m_Blocks[i, nCol];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchedBlockList.Insert(0, block);
        }

        if (matchedBlockList.Count >= 3)
        {
            SetBlockStatusMatched(matchedBlockList, false);
            bFound = true;
        }

        if (baseBlock.match == MatchType.FOUR && !isAlreadyCheck) //폭탄 퀘스트 
        {
            isVBoom = false;
        }

        matchedBlockList.Clear();
        isAlreadyCheck = false;

        return bFound;
    }

    void SetBlockStatusMatched(List<Block> blockList, bool bHorz)
    {
        int nMatchCount = blockList.Count;
        blockList.ForEach(block => block.UpdateBlockStatusMatched((MatchType)nMatchCount));
    }

    public IEnumerator ArrangeBlocksAfterClean(List<KeyValuePair<int, int>> unfilledBlocks, List<Block> movingBlocks)
    {
        SortedList<int, int> emptyBlocks = new SortedList<int, int>();
        List<KeyValuePair<int, int>> emptyRemainBlocks = new List<KeyValuePair<int, int>>();

        for (int nCol = 0; nCol < m_nCol; nCol++)
        {
            emptyBlocks.Clear();

            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                if (CanBlockBeAllocatable(nRow, nCol))
                    emptyBlocks.Add(nRow, nRow);
            }

            if (emptyBlocks.Count == 0)
                continue;

            KeyValuePair<int, int> first = emptyBlocks.First();

            for (int nRow = first.Value + 1; nRow < m_nRow; nRow++)
            {
                Block block = m_Blocks[nRow, nCol];

                if (block == null || m_Cells[nRow, nCol].type == CellType.EMPTY || block.type < 0) 
                    continue;

 
                block.dropDistance = new Vector2(0, nRow - first.Value);    //GameObject 애니메이션 이동
                movingBlocks.Add(block);


                Debug.Assert(m_Cells[first.Value, nCol].IsObstracle() == false, $"{m_Cells[first.Value, nCol]}");
                m_Blocks[first.Value, nCol] = block;       

                m_Blocks[nRow, nCol] = null;

                emptyBlocks.RemoveAt(0);

                emptyBlocks.Add(nRow, nRow);

                first = emptyBlocks.First();
                nRow = first.Value; 
            }
        }

        yield return null;

        if (emptyRemainBlocks.Count > 0)
        {
            unfilledBlocks.AddRange(emptyRemainBlocks);
        }

        yield break;
    }

    public IEnumerator SpawnBlocksAfterClean(List<Block> movingBlocks)
    {
        for (int nCol = 0; nCol < m_nCol; nCol++)
        {
            for (int nRow = 0; nRow < m_nRow; nRow++)
            {
                if (m_Blocks[nRow, nCol] == null)
                {
                    int nTopRow = nRow;
                    int nSpawnBaseY = 0;

                    for (int y = nTopRow; y < m_nRow; y++)
                    {
                        if (m_Blocks[y, nCol] != null || !CanBlockBeAllocatable(y, nCol))
                            continue;

                        Block block = SpawnBlockWithDrop(y, nCol, nSpawnBaseY, nCol);
                        if (block != null)
                            movingBlocks.Add(block);

                        nSpawnBaseY++;
                    }

                    break;
                }
            }
        }

        yield return null;
    }

    Block SpawnBlockWithDrop(int nRow, int nCol, int nSpawnedRow, int nSpawnedCol)
    {
        float fInitX = CalcInitX(Constants.BLOCK_ORG);
        float fInitY = CalcInitY(Constants.BLOCK_ORG) + m_nRow;

        Block block = m_StageBuilder.SpawnBlock().InstantiateBlockObj(m_BlockPrefab, m_Container);
        if (block != null)
        {
            m_Blocks[nRow, nCol] = block;
            block.Move(fInitX + (float)nSpawnedCol, fInitY + (float)(nSpawnedRow));
            block.dropDistance = new Vector2(nSpawnedCol - nCol, m_nRow + (nSpawnedRow - nRow));
        }

        return block;
    }

    public float CalcInitX(float offset = 0)
    {
        return -m_nCol / 2.0f + offset;   
    }

    public float CalcInitY(float offset = 0)
    {
        return -m_nRow / 2.0f + offset;
    }

    public bool CanShuffle(int nRow, int nCol, bool bLoading)
    {
        if (!m_Cells[nRow, nCol].type.IsBlockMovableType() || m_Blocks[nRow,nCol].type <= BlockType.EMPTY)
            return false;

        return true;
    }

    public void ChangeBlock(Block block, BlockBreed notAllowedBreed)
    {
        BlockBreed genBreed;

        while (true)
        {
            genBreed = (BlockBreed)UnityEngine.Random.Range(0, 5); //TODO 스테이지파일에서 Spawn 정책을 이용해야함

            if (notAllowedBreed == genBreed)
                continue;

            break;
        }

        block.breed = genBreed;
    }

    public bool IsSwipeable(int nRow, int nCol)
    {
        return m_Cells[nRow, nCol].type.IsBlockMovableType();
    }

 
    bool CanBlockBeAllocatable(int nRow, int nCol)
    {
        if (!m_Cells[nRow, nCol].type.IsBlockAllocatableType())
            return false;

        return m_Blocks[nRow, nCol] == null;
    }

    public void SpawnBlock(int nRow, int nCol, BlockType blockType)
    {
        Block boomblock;

        float initX = CalcInitX(0.5f);
        float initY = CalcInitY(0.5f);

        boomblock = m_StageBuilder.SpawnBlock(blockType).InstantiateBlockObj(m_BlockPrefab, m_Container);
        
        boomblock?.Move(initX + nCol, initY + nRow);
        m_Blocks[nRow, nCol] = boomblock;
    }

    public void SpawnBlock(int nRow, int nCol, GameObject m_prefab)
    {
        Block boomblock;

        float initX = CalcInitX(0.5f);
        float initY = CalcInitY(0.5f);

        boomblock = m_StageBuilder.SpawnBlock(BlockType.POSTD).InstantiateBlockObj(m_prefab, m_Container);

        boomblock?.Move(initX + nCol, initY + nRow);
    }

    public bool ShuffleEvaluate()
    {
        for(int nRow = 0; nRow < m_nRow; nRow++)
        {
            for(int nCol = 0; nCol < m_nCol; nCol++)
            {
                Block block = m_Blocks[nRow, nCol];
                Block tempBlock;

                if (block.type == BlockType.BASIC)
                {
                    if (nRow - 1 >= 0) //아래
                    {
                        tempBlock = m_Blocks[nRow - 1, nCol];
                        m_Blocks[nRow - 1, nCol] = m_Blocks[nRow, nCol];
                        m_Blocks[nRow, nCol] = tempBlock;

                        if(EvalBlocksIfMatched(nRow, nCol))
                        {
                            tempBlock = m_Blocks[nRow - 1, nCol];
                            m_Blocks[nRow - 1, nCol] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;

                            return false;
                        }
                        else
                        {
                            tempBlock = m_Blocks[nRow - 1, nCol];
                            m_Blocks[nRow - 1, nCol] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;
                        }

                    }

                    if (nRow + 1 < m_nRow) //위
                    {
                        tempBlock = m_Blocks[nRow + 1, nCol];
                        m_Blocks[nRow + 1, nCol] = m_Blocks[nRow, nCol];
                        m_Blocks[nRow, nCol] = tempBlock;

                        if (EvalBlocksIfMatched(nRow, nCol))
                        {
                            tempBlock = m_Blocks[nRow + 1, nCol];
                            m_Blocks[nRow + 1, nCol] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;

                            return false;
                        }
                        else
                        {
                            tempBlock = m_Blocks[nRow + 1, nCol];
                            m_Blocks[nRow + 1, nCol] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;
                        }
                    }

                    if (nCol - 1 >= 0) //왼
                    {
                        tempBlock = m_Blocks[nRow, nCol - 1];
                        m_Blocks[nRow, nCol - 1] = m_Blocks[nRow, nCol];
                        m_Blocks[nRow, nCol] = tempBlock;

                        if (EvalBlocksIfMatched(nRow, nCol))
                        {
                            tempBlock = m_Blocks[nRow, nCol - 1];
                            m_Blocks[nRow, nCol - 1] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;

                            return false;
                        }
                        else
                        {
                            tempBlock = m_Blocks[nRow, nCol - 1];
                            m_Blocks[nRow, nCol - 1] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;
                        }
                    }

                    if (nCol + 1 < m_nCol) //오
                    {
                        tempBlock = m_Blocks[nRow, nCol + 1];
                        m_Blocks[nRow, nCol + 1] = m_Blocks[nRow, nCol];
                        m_Blocks[nRow, nCol] = tempBlock;

                        if (EvalBlocksIfMatched(nRow, nCol))
                        {
                            tempBlock = m_Blocks[nRow, nCol + 1];
                            m_Blocks[nRow, nCol + 1] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;

                            return false;
                        }
                        else
                        {
                            tempBlock = m_Blocks[nRow, nCol + 1];
                            m_Blocks[nRow, nCol + 1] = m_Blocks[nRow, nCol];
                            m_Blocks[nRow, nCol] = tempBlock;
                        }
                    }
                }
                else if(block.type <= BlockType.EMPTY)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    public bool EvalBlocksIfMatched(int nRow, int nCol)
    {
        List<Block> matchList = new List<Block>();

        bool bFound = false;

        Block baseBlock = m_Blocks[nRow, nCol];
        if (baseBlock == null)
            return false;

        if (baseBlock.match != MatchType.NONE || !baseBlock.IsValidate() || m_Cells[nRow, nCol].IsObstracle())
            return false;

        matchList.Add(baseBlock);

        Block block;

        for (int i = nCol + 1; i < m_nCol; i++)
        {
            block = m_Blocks[nRow, i];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchList.Add(block);
        }

        for (int i = nCol - 1; i >= 0; i--)
        {
            block = m_Blocks[nRow, i];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchList.Insert(0, block);
        }

        if (matchList.Count >= 3)
        {
            bFound = true;
        }

        matchList.Clear();

        matchList.Add(baseBlock);

        for (int i = nRow + 1; i < m_nRow; i++)
        {
            block = m_Blocks[i, nCol];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchList.Add(block);
        }

        for (int i = nRow - 1; i >= 0; i--)
        {
            block = m_Blocks[i, nCol];
            if (!block.IsSafeEqual(baseBlock))
                break;

            matchList.Insert(0, block);
        }

        if (matchList.Count >= 3)
        {
            bFound = true;
        }

        matchList.Clear();

        return bFound;
    }
}
