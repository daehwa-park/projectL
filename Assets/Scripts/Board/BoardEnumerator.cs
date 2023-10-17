using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoardEnumerator
{
    Board m_Board;

    public BoardEnumerator(Board board)
    {
        this.m_Board = board;
    }

    public bool IsCageTypeCell(int nRow, int nCol)
    {
        return false;
    }
}
