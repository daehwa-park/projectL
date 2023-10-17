using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    EMPTY = 0,      //빈공간, 블럭이 위치할 수 없음, 드롭 통과
    BASIC = 1,      //배경있는 기본 형 (No action)
    GRASS = 2,      //두번 매치하면 없어짐
    FIXTURE = 3,    //고정된 장애물. 변화없음
    JELLY = 4,      //젤리 : 블럭 이동 OK, 블럭 CLEAR되면 BASIC, 출력 : CellBg
}

static class CellTypeMethod
{
    public static bool IsBlockAllocatableType(this CellType cellType)
    {
        return !(cellType == CellType.EMPTY);
    }

    public static bool IsBlockMovableType(this CellType cellType)
    {
        return !(cellType == CellType.EMPTY);
    }
}
