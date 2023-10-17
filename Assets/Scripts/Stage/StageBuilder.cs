using System;
using UnityEngine;

public class StageBuilder
{
    public StageInfo m_StageInfo;
    int m_nStage;

    public StageBuilder(int nStage)
    {
        m_nStage = nStage;
    }

    public Stage ComposeStage()
    {
        Debug.Assert(m_nStage > 0, $"Invalide Stage : {m_nStage}");

        m_StageInfo = LoadStage(m_nStage);
        
        Stage stage = new Stage(this, m_StageInfo.row, m_StageInfo.col);

        for (int nRow = 0; nRow < m_StageInfo.row; nRow++)
        {
            for (int nCol = 0; nCol < m_StageInfo.col; nCol++)
            {
                stage.blocks[nRow, nCol] = SpawnBlockForStage(nRow, nCol);
                stage.cells[nRow, nCol] = SpawnCellForStage(nRow, nCol);
            }
        }

        return stage;
    }

    public StageInfo LoadStage(int nStage)
    {
        StageInfo stageInfo = StageReader.LoadStage(nStage);
        if (stageInfo != null)
        {
            Debug.Log(stageInfo.ToString());
        }

        return stageInfo;
    }

    Block SpawnBlockForStage(int nRow, int nCol)
    {
        if (m_StageInfo.GetBlockType(nRow, nCol) == BlockType.EMPTY)
            return SpawnEmptyBlock();
        
        return SpawnBlock(nRow, nCol);
    }

    Cell SpawnCellForStage(int nRow, int nCol)
    {
        Debug.Assert(m_StageInfo != null);
        Debug.Assert(nRow < m_StageInfo.row && nCol < m_StageInfo.col);

        return CellFactory.SpawnCell(m_StageInfo, nRow, nCol);
    }

    public static Stage BuildStage(int nStage)
    {
        StageBuilder stageBuilder = new StageBuilder(nStage);
        Stage stage = stageBuilder.ComposeStage();


        return stage;
    }

    public Block SpawnBlock(int nRow, int nCol)
    {
        int revisedRow = (m_StageInfo.row - 1) - nRow;

        return BlockFactory.SpawnBlock(m_StageInfo.GetBlockType(nRow,nCol), (BlockBreed)m_StageInfo.blocks[revisedRow * m_StageInfo.col + nCol]);
        
    }

    public Block SpawnBlock(BlockType blockType)
    {
        return BlockFactory.SpawnBlock(blockType);

    }

    public Block SpawnBlock()
    {
        return BlockFactory.SpawnBlock(BlockType.BASIC);
    }
        
    public Block SpawnEmptyBlock()
    {
        Block newBlock = BlockFactory.SpawnBlock(BlockType.EMPTY, BlockBreed.NA);

        return newBlock;
    }
}
