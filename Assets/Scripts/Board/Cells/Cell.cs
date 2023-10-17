using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    protected CellType m_CellType;
    public int m_cDurability;
    public CellType type
    {
        get { return m_CellType; }
        set { m_CellType = value; }
    }

    protected CellBehaviour m_CellBehaviour;
    public CellBehaviour cellBehaviour
    {
        get { return m_CellBehaviour; }
        set
        {
            m_CellBehaviour = value;
            m_CellBehaviour.SetCell(this);
        }
    }

    public Cell(CellType cellType)
    {
        m_CellType = cellType;

        if(cellType == CellType.GRASS)
        {
            m_cDurability = 1;
        }
    }

    public Cell InstantiateCellObj(GameObject cellPrefab, Transform containerObj)
    {
        GameObject newObj = Object.Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newObj.transform.parent = containerObj;

        this.cellBehaviour = newObj.transform.GetComponent<CellBehaviour>();

        return this;
    }

    public void Move(float x, float y)
    {
        cellBehaviour.transform.position = new Vector3(x, y);
    }

    public bool IsObstracle()
    {
        return type == CellType.EMPTY;
    }

    public void ChangeCellView(CellType cellType)
    {
        if(cellType == CellType.BASIC)
        {
            cellBehaviour.ChangeView(CellType.BASIC);
        }
        else if( cellType == CellType.GRASS)
        {
            cellBehaviour.ChangeView(CellType.GRASS);
        }
    }
}


