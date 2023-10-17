using System.Collections.Generic;
using UnityEngine;

public class BoardShuffler
{
    Board m_Board;
    bool m_bLoadingMode;

    SortedList<int, KeyValuePair<Block, Vector2Int>> m_OrgBlocks = new SortedList<int, KeyValuePair<Block, Vector2Int>>();
    IEnumerator<KeyValuePair<int, KeyValuePair<Block, Vector2Int>>> m_it;
    Queue<KeyValuePair<Block, Vector2Int>> m_UnusedBlocks = new Queue<KeyValuePair<Block, Vector2Int>>();
    bool m_bListComplete;

    public BoardShuffler(Board board, bool bLoadingMode)
    {
        m_Board = board;
        m_bLoadingMode = bLoadingMode;
    }

    public void Shuffle(bool bAnimation = false)
    {
        PrepareDuplicationDatas();


        PrepareShuffleBlocks();

        RunShuffle(bAnimation);
    }

    KeyValuePair<Block, Vector2Int> NextBlock(bool bUseQueue)
    {
        if (bUseQueue && m_UnusedBlocks.Count > 0)
            return m_UnusedBlocks.Dequeue();

        if (!m_bListComplete && m_it.MoveNext())
            return m_it.Current.Value;

        m_bListComplete = true;

        return new KeyValuePair<Block, Vector2Int>(null, Vector2Int.zero);
    }

    void PrepareDuplicationDatas()
    {
        for (int nRow = 0; nRow < m_Board.maxRow; nRow++)
            for (int nCol = 0; nCol < m_Board.maxCol; nCol++)
            {
                Block block = m_Board.blocks[nRow, nCol];

                if (block == null)
                    continue;

                if (m_Board.CanShuffle(nRow, nCol, m_bLoadingMode))
                    block.ResetDuplicationInfo();
                else
                {
                    block.horzDuplicate = 1;
                    block.vertDuplicate = 1;

                    if (nCol > 0 && !m_Board.CanShuffle(nRow, nCol - 1, m_bLoadingMode) && m_Board.blocks[nRow, nCol - 1].IsSafeEqual(block))
                    {
                        block.horzDuplicate = 2;
                        m_Board.blocks[nRow, nCol - 1].horzDuplicate = 2;
                    }

                    if (nRow > 0 && !m_Board.CanShuffle(nRow - 1, nCol, m_bLoadingMode) && m_Board.blocks[nRow - 1, nCol].IsSafeEqual(block))
                    {
                        block.vertDuplicate = 2;
                        m_Board.blocks[nRow - 1, nCol].vertDuplicate = 2;
                    }
                }
            }
    }

    void PrepareShuffleBlocks()
    {
        for (int nRow = 0; nRow < m_Board.maxRow; nRow++)
            for (int nCol = 0; nCol < m_Board.maxCol; nCol++)
            {
                if (!m_Board.CanShuffle(nRow, nCol, m_bLoadingMode))
                    continue;

                while (true)
                {
                    int nRandom = UnityEngine.Random.Range(0, 10000);
                    if (m_OrgBlocks.ContainsKey(nRandom))
                        continue;

                    m_OrgBlocks.Add(nRandom, new KeyValuePair<Block, Vector2Int>(m_Board.blocks[nRow, nCol], new Vector2Int(nCol, nRow)));
                    break;
                }
            }

        m_it = m_OrgBlocks.GetEnumerator();
    }


    void RunShuffle(bool bAnimation) // 전체 블럭 섞기
    {
        for (int nRow = 0; nRow < m_Board.maxRow; nRow++)
        {
            for (int nCol = 0; nCol < m_Board.maxCol; nCol++)
            {
                if (!m_Board.CanShuffle(nRow, nCol, m_bLoadingMode))
                    continue;

                m_Board.blocks[nRow, nCol] = GetShuffledBlock(nRow, nCol);
            }
        } 
    } 

    Block GetShuffledBlock(int nRow, int nCol)
    {
        BlockBreed prevBreed = BlockBreed.NA;   
        Block firstBlock = null;                

        bool bUseQueue = true;  
        while (true)
        {
            KeyValuePair<Block, Vector2Int> blockInfo = NextBlock(bUseQueue);
            Block block = blockInfo.Key;

            if (block == null)
            {
                blockInfo = NextBlock(true);
                block = blockInfo.Key;
            }

            Debug.Assert(block != null, $"block can't be null : queue  count -> {m_UnusedBlocks.Count}");

            if (prevBreed == BlockBreed.NA) 
                prevBreed = block.breed;

            if (m_bListComplete)
            {
                if (firstBlock == null)
                {
                    firstBlock = block;  
                }
                else if (System.Object.ReferenceEquals(firstBlock, block))
                {
                    m_Board.ChangeBlock(block, prevBreed);
                }
            }

            Vector2Int vtDup = CalcDuplications(nRow, nCol, block);

            if (vtDup.x > 2 || vtDup.y > 2)
            {
                m_UnusedBlocks.Enqueue(blockInfo);
                bUseQueue = m_bListComplete || !bUseQueue;

                continue;
            }

            block.vertDuplicate = vtDup.y;
            block.horzDuplicate = vtDup.x;
            if (block.blockObj != null)
            {
                float initX = m_Board.CalcInitX(Constants.BLOCK_ORG);
                float initY = m_Board.CalcInitY(Constants.BLOCK_ORG);
                block.Move(initX + nCol, initY + nRow);
            }

            return block;
        }
    }

    Vector2Int CalcDuplications(int nRow, int nCol, Block block)
    {
        int colDup = 1, rowDup = 1;

        if (nCol > 0 && m_Board.blocks[nRow, nCol - 1].IsSafeEqual(block))
            colDup += m_Board.blocks[nRow, nCol - 1].horzDuplicate;

        if (nRow > 0 && m_Board.blocks[nRow - 1, nCol].IsSafeEqual(block))
            rowDup += m_Board.blocks[nRow - 1, nCol].vertDuplicate;

        if (nCol < m_Board.maxCol - 1 && m_Board.blocks[nRow, nCol + 1].IsSafeEqual(block))
        {
            Block rightBlock = m_Board.blocks[nRow, nCol + 1];
            colDup += rightBlock.horzDuplicate;

            if (rightBlock.horzDuplicate == 1)
                rightBlock.horzDuplicate = 2;
        }

        if (nRow < m_Board.maxRow - 1 && m_Board.blocks[nRow + 1, nCol].IsSafeEqual(block))
        {
            Block upperBlock = m_Board.blocks[nRow + 1, nCol];
            rowDup += upperBlock.vertDuplicate;

            if (upperBlock.vertDuplicate == 1)
                upperBlock.vertDuplicate = 2;
        }

        return new Vector2Int(colDup, rowDup);
    }
}

