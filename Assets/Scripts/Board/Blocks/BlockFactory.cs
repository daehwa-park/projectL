using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockFactory
{
    public static Block SpawnBlock(BlockType blockType, BlockBreed blockBreed)
    {
        Block block = new Block(blockType);

        if (blockType == BlockType.BASIC && blockBreed == BlockBreed.BREED_0)
        {
            block.breed = BlockBreed.BREED_0;
        }
        else if (blockType == BlockType.BASIC && blockBreed == BlockBreed.BREED_1)
        {
            block.breed = BlockBreed.BREED_1;

        }
        else if (blockType == BlockType.BASIC && blockBreed == BlockBreed.BREED_2)
        {
            block.breed = BlockBreed.BREED_2;

        }
        else if (blockType == BlockType.BASIC && blockBreed == BlockBreed.BREED_3)
        {
            block.breed = BlockBreed.BREED_3;

        }
        else if (blockType == BlockType.BASIC && blockBreed == BlockBreed.BREED_4)
        {
            block.breed = BlockBreed.BREED_4;

        }
        else if (blockType == BlockType.EMPTY)
        {
            block.breed = BlockBreed.NA;
        }
        else if (blockType == BlockType.HBOOM)
        {
            block.breed = BlockBreed.NA;
            //폭탄생성
        }
        else if (blockType == BlockType.VBOOM)
        {
            block.breed = BlockBreed.NA;
            //폭탄생성
        }
        else if (blockType == BlockType.CBOOM)
        {
            block.breed = BlockBreed.NA;
            //폭탄생성
        }
        else if(blockType == BlockType.CHEST)
        {
            block.breed = BlockBreed.NA;
            //상자 생성
        }
        else if(blockType == BlockType.POSTB)
        {
            block.breed = BlockBreed.NA;

        }

        return block;
    }

    public static Block SpawnBlock(BlockType blockType)
    {
        Block block = new Block(blockType);

        if (blockType == BlockType.BASIC)
            block.breed = (BlockBreed)UnityEngine.Random.Range(0, 5);
        else if (blockType == BlockType.EMPTY)
            block.breed = BlockBreed.NA;
        else if (blockType == BlockType.HBOOM)
        {
            block.breed = BlockBreed.NA;
            //폭탄생성
        }
        else if (blockType == BlockType.VBOOM)
        {
            block.breed = BlockBreed.NA;
        }
        else if (blockType == BlockType.CBOOM)
        {
            block.breed = BlockBreed.NA;
        }
        else
        {
            block.breed = BlockBreed.NA;
        }

        return block;
    }
}

