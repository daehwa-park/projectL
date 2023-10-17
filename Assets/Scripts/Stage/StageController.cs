using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StageController : MonoBehaviour
{
    bool m_bInit;
    public Stage m_Stage;
    InputManager m_InputManager;
    ActionManager m_ActionManager;

    bool m_bTouchDown;          //입력상태 처리 플래그, 유효한 블럭을 클릭한 경우 true
    BlockPos m_BlockDownPos;    //블럭 인덱스 (보드에 저장된 위치)
    Vector3 m_ClickPos;         //DOWN 위치(보드 기준 Local 좌표)

    [SerializeField] Transform m_Container;
    [SerializeField] GameObject m_CellPrefab;
    [SerializeField] GameObject m_BlockPrefab;
    [SerializeField] GameObject m_MoonPrefab;

    void Start()
    {
        
    }

    private void Update()
    {
        if (!m_bInit)
            return;

        OnInputHandler();
    }

    public void InitStage(int stageNum)
    {
        if (m_bInit)
            return;

        m_bInit = true;
        m_InputManager = new InputManager(m_Container);

        BuildStage(stageNum);
    }

    void BuildStage(int stageNum)
    {
        m_Stage = StageBuilder.BuildStage(nStage : stageNum);
        m_ActionManager = new ActionManager(m_Container, m_Stage);

        m_Stage.ComposeStage(m_CellPrefab, m_BlockPrefab, m_Container, m_MoonPrefab);

    }

    void OnInputHandler()
    {
        if (!m_bTouchDown && m_InputManager.isTouchDown)
        {
            Vector2 point = m_InputManager.touch2BoardPosition;

            if (!m_Stage.IsInsideBoard(point))
                return;

            BlockPos blockPos;
            if (m_Stage.IsOnValideBlock(point, out blockPos))
            {
                m_bTouchDown = true;        //클릭 상태 플래그 ON
                m_BlockDownPos = blockPos;  //클릭한 블럭의 위치(row, col) 저장
                m_ClickPos = point;         //클릭한 Local 좌표 저장
            }
        }
        else if (m_bTouchDown && m_InputManager.isTouchUp)
        {
            Vector2 point = m_InputManager.touch2BoardPosition;

            Swipe swipeDir = m_InputManager.EvalSwipeDir(m_ClickPos, point);

            if (swipeDir != Swipe.NA)
                m_ActionManager.DoSwipeAction(m_BlockDownPos.row, m_BlockDownPos.col, swipeDir);

            m_bTouchDown = false;   //클릭 상태 플래그 OFF
        }
    }
}

