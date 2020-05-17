using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInterfaceTest
{
    /// <summary>
    /// 점프 함수를 구현해봥~
    /// </summary>
    /// <param name="jumpPower">점프력</param>
    /// <param name="maxCount">점프 가능 횟수</param>
    void Jump(float jumpPower, int maxCount);
}
