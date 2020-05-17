using UnityEngine;

public class InharitTest : MonoBehaviour, IInterfaceTest
{
    #region 자식만 접근 가능한 변수
    protected float m_Speed = 15;
    protected float jump = 10;
    protected int jumpCount = 0;
    protected int maxJump = 1;
    #endregion

    void Update()
    {
        Move(m_Speed);
        Jump(jump, maxJump);
    }

    public virtual void Move(float speed)
    {
        float v = Input.GetAxisRaw("Vertical");
        Vector3 dir = new Vector3(0, 0, v);
        transform.position += dir * speed * Time.deltaTime;
        print("부모의 이동 함수를 사용합니다.");
    }

    public void Jump(float jumpPower, int maxCount)
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (jumpCount < maxCount)
            {
                Rigidbody rb = GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }
}
