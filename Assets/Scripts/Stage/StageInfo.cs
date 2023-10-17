using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageInfo
{
    public int row;
    public int col;

    public int[] cells;
    public int[] blocks;
    public int[] blockTypes;

    public int moveNum;
    public int[] clearNum;
    public int[] clearNumIndex;

    public override string ToString()
    {
        return JsonUtility.ToJson(this);
    }


    public CellType GetCellType(int nRow, int nCol)
    {
        Debug.Assert(cells != null && cells.Length > nRow * col + nCol, $"Invalid Row/Col = {nRow}, {nCol}");


        int revisedRow = (row - 1) - nRow;
        if (cells.Length > revisedRow * col + nCol)
            return (CellType)cells[revisedRow * col + nCol];

        Debug.Assert(false);

        return CellType.EMPTY;
    }

    public BlockType GetBlockType(int nRow, int nCol)
    {
        Debug.Assert(blockTypes != null && blockTypes.Length > nRow * col + nCol, $"Invalid Row/Col = {nRow}, {nCol}");


        int revisedRow = (row - 1) - nRow;
        if (blockTypes.Length > revisedRow * col + nCol)
            return (BlockType)blockTypes[revisedRow * col + nCol];

        Debug.Assert(false);

        return BlockType.EMPTY;
    }

    public bool DoValidation()
    {
        Debug.Assert(cells.Length == row * col);
        Debug.Assert(blocks.Length == row * col);
        Debug.Assert(blockTypes.Length == row * col);


        Debug.Log($"cell length : {cells.Length}, row, col = ({row}, {col})");
        Debug.Log($"block length : {blocks.Length}, row, col = ({row}, {col})");
        Debug.Log($"blockTypes length : {blockTypes.Length}, row, col = ({row}, {col})");


        if (cells.Length != row * col)
            return false;

        if (blocks.Length != row * col)
            return false;

        if (blockTypes.Length != row * col)
            return false;

        return true;
    }
}

