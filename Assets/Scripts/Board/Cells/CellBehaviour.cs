using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour
{
    Cell m_Cell;
    SpriteRenderer m_SpriteRenderer;
    [SerializeField] CellConfig m_CellConfig;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        UpdateView(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCell(Cell cell)
    {
        m_Cell = cell;
    }

    public void UpdateView(bool bValueChanged)
    {
        if (m_Cell.type == CellType.EMPTY)
        {
            m_SpriteRenderer.sprite = null;
        }
        else if(m_Cell.type == CellType.BASIC)
        {
            m_SpriteRenderer.sprite = m_CellConfig.CellSprites[0];
        }
        else if(m_Cell.type == CellType.GRASS)
        {
            m_SpriteRenderer.sprite = m_CellConfig.CellSprites[1];
        }
    }

    public void ChangeView(CellType cellType)
    {
        if(cellType == CellType.BASIC)
        {
            m_SpriteRenderer.sprite = m_CellConfig.CellSprites[0];
        }
        else if(cellType == CellType.GRASS)
        {
            m_SpriteRenderer.sprite = m_CellConfig.CellSprites[1];
        }
    }
}
