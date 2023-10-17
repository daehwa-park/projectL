using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BlockBehaviour : MonoBehaviour
{
    Block m_Block;
    public SpriteRenderer m_SpriteRenderer;
    [SerializeField] public BlockConfig m_BlockConfig;

    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        UpdateView(false);
    }

    internal void SetBlock(Block block)
    {
        m_Block = block;
    }

    public void UpdateView(bool bValueChanged)
    {
        if (m_Block.type == BlockType.POSTB)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[9];
        }
        if (m_Block.type == BlockType.CHEST)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[8];
        }
        else if (m_Block.type == BlockType.EMPTY)
        {
            m_SpriteRenderer.sprite = null;
        }
        else if(m_Block.type == BlockType.BASIC)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[(int)m_Block.breed];
        }
        else if(m_Block.type == BlockType.TWICE)
        {
            // 스프라이트 추가
        }
        else if(m_Block.type == BlockType.HBOOM)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[5];
        }
        else if(m_Block.type == BlockType.VBOOM)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[6];
        }
        else if(m_Block.type == BlockType.CBOOM)
        {
            m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[7];
        }
    }


    public void DoActionClear()
    {
        StartCoroutine(CoStartSimpleExplosion(true));
    }

    IEnumerator CoStartSimpleExplosion(bool bDestroy = true)
    {
        yield return Action2D.Scale(transform, Constants.BLOCK_DESTROY_SCALE, 4f);


        GameObject explosionObj = m_BlockConfig.GetExplosionObject(m_Block);
        //ParticleSystem.MainModule newModule = explosionObj.GetComponent<ParticleSystem>().main;
        //newModule.startColor = m_BlockConfig.GetBlockColor(m_Block.breed);

        explosionObj.SetActive(true);
        explosionObj.transform.position = this.transform.position;

        yield return new WaitForSeconds(0.1f);

        if (bDestroy)
            Destroy(gameObject);
        else
        {
            Debug.Assert(false, "Unknown Action : GameObject No Destory After Particle");
        }
    }

    public IEnumerator ScaleUP()
    {
        transform.localScale = new Vector3(0.3f, 0.3f, transform.localScale.z);

        yield return Action2D.Scale(transform, Constants.BLOCK_CREATION_SCALE, 4f);
    }

}
