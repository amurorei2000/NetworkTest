using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTest : InharitTest
{
    float mc_Speed = 2;

    void Update()
    {
        Move(m_Speed);
        Jump(jump, 2);
    }

    public override void Move(float speed)
    {
        base.Move(speed);

        float h = Input.GetAxis("Horizontal");
        Vector3 dir = new Vector3(h, 0, 0);
        transform.position += dir * mc_Speed * Time.deltaTime;
        print("자식의 이동 함수를 사용합니다.");
    }
}
