using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block
{
    public BlockStatus status;
    public BlockQuestType questType;

    public MatchType match = MatchType.NONE;

    public short matchCount;

    BlockType m_BlockType;

    public bool isMatchedBlock = false;

    public bool isBoomBlock = false;

    //public bool isHboomActivate = false;
    //public bool isVboomActivate = false;
    //public bool isCboomActivate = false;

    public bool isCanHboom = false;
    public bool isCanVboom = false;
    public bool isCanCboom = false;

    public bool isAlreadyCheckP = false;

    public BlockType type
    {
        get { return m_BlockType; }
        set 
        { 
            m_BlockType = value;
            m_BlockBehaviour?.UpdateView(true);
        }
    }

    protected BlockBreed m_Breed;   
    public BlockBreed breed
    {
        get { return m_Breed; }
        set
        {
            m_Breed = value;
            m_BlockBehaviour?.UpdateView(true);
        }
    }

    protected BlockBehaviour m_BlockBehaviour;
    public BlockBehaviour blockBehaviour
    {
        get { return m_BlockBehaviour; }
        set
        {
            m_BlockBehaviour = value;
            m_BlockBehaviour.SetBlock(this);
        }
    }

    public Transform blockObj { get { return m_BlockBehaviour?.transform; } }

    Vector2Int m_vtDuplicate;       // 블럭 젠, Shuffle시에 중복검사에 사용. stage file에서 생성시 (-1, -1)
    //중복 검사시 사용 
    public int horzDuplicate
    {
        get { return m_vtDuplicate.x; }
        set { m_vtDuplicate.x = value; }
    }

    //중복 검사시 사용 
    public int vertDuplicate
    {
        get { return m_vtDuplicate.y; }
        set { m_vtDuplicate.y = value; }
    }

    int m_nDurability;        
    public virtual int durability
    {
        get { return m_nDurability; }
        set { m_nDurability = value; }
    }

    protected BlockActionBehaviour m_BlockActionBehaviour;

    public bool isMoving
    {
        get
        {
            return blockObj != null && m_BlockActionBehaviour.isMoving;
        }
    }

    public Vector2 dropDistance
    {
        set
        {
            m_BlockActionBehaviour?.MoveDrop(value);
        }
    }


    public Block(BlockType blockType)
    {
        m_BlockType = blockType;

        status = BlockStatus.NORMAL;
        questType = BlockQuestType.CLEAR_SIMPLE;
        match = MatchType.NONE;
        m_Breed = BlockBreed.NA;

        if (m_BlockType == BlockType.BASIC)
        {
            m_nDurability = 1;
        }
        else if(m_BlockType == BlockType.TWICE)
        {
            m_nDurability = 2;
        }
        else
        {
            m_nDurability = 1;
        }
    }

    internal Block InstantiateBlockObj(GameObject blockPrefab, Transform containerObj)
    {
        //유효하지 않은 블럭인 경우, Block GameObject를 생성하지 않는다.
        if (IsValidate() == false)
            return null;

        GameObject newObj = Object.Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        newObj.transform.parent = containerObj;

        this.blockBehaviour = newObj.transform.GetComponent<BlockBehaviour>();
        m_BlockActionBehaviour = newObj.transform.GetComponent<BlockActionBehaviour>();

        return this;
    }

    public bool DoEvaluation(BoardEnumerator boardEnumerator, int nRow, int nCol)
    {

        Debug.Assert(boardEnumerator != null, $"({nRow},{nCol})");

        if (!IsEvaluatable())
            return false;

        if (status == BlockStatus.MATCH)
        {
            if (questType == BlockQuestType.CLEAR_SIMPLE || boardEnumerator.IsCageTypeCell(nRow, nCol)) 
            {
                Debug.Assert(m_nDurability > 0, $"durability is zero : {m_nDurability}");

                durability--;
            }
            else if(questType == BlockQuestType.CLEAR_HORZ)
            {
                //폭탄
                isBoomBlock = true;
                durability--;
            }
            else if(questType == BlockQuestType.CLEAR_VERT)
            {
                isBoomBlock = true;
                durability--;
            }
            else if(questType == BlockQuestType.CLEAR_CIRCLE)
            {
                isBoomBlock = true;
                durability--;
            }

            if (m_nDurability == 0)
            {
                status = BlockStatus.CLEAR;
                isMatchedBlock = true;
                return false;
            }
        }
        else if(status == BlockStatus.CLEAR)
        {
            isMatchedBlock = true;
            return false;
        }

        if (m_nDurability == 0)
        {
            status = BlockStatus.CLEAR;
            isMatchedBlock = true;
            return false;
        }

        status = BlockStatus.NORMAL;
        match = MatchType.NONE;
        matchCount = 0;

        return false;
    }

    public void UpdateBlockStatusMatched(MatchType matchType, bool bAccumulate = true)
    {
        this.status = BlockStatus.MATCH;

        if (match == MatchType.NONE)
        {
            this.match = matchType;
        }
        else
        {
            this.match = bAccumulate ? match.Add(matchType) : matchType; //match + matchType
        }

        matchCount = (short)matchType;
    }

    internal void Move(float x, float y)
    {
        blockBehaviour.transform.position = new Vector3(x, y);
    }

    public void MoveTo(Vector3 to, float duration)
    {
        m_BlockBehaviour.StartCoroutine(Action2D.MoveTo(blockObj, to, duration));
    }

    public virtual void Destroy()
    {
        Debug.Assert(blockObj != null, $"{match}");
        blockBehaviour.DoActionClear();
    }

    public bool IsValidate()
    {
        return type != BlockType.EMPTY;
    }

    public void ResetDuplicationInfo()
    {
        m_vtDuplicate.x = 0;
        m_vtDuplicate.y = 0;
    }

    public bool IsEqual(Block target)
    {
        if (IsMatchableBlock() && this.breed == target.breed && this.breed != BlockBreed.NA)
            return true;

        return false;
    }

    public bool IsMatchableBlock()
    {
        if(type <= BlockType.EMPTY)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsSwipeable(Block baseBlock)
    {
        return true;
    }

    public bool IsEvaluatable()
    {
        if (!IsMatchableBlock()) // status == BlockStatus.CLEAR 뺐음
            return false;

        return true;
    }
}
