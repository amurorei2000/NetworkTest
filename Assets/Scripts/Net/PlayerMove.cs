using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{
    public float moveSpeed = 5;
    public float gravity = -20;
    public float jumpPower = 5;
    public GameObject cam;
    public Slider hpSlider;
    public Text nickName;
    public int CurrentHP
    {
        get { return curHp; }
        set { curHp = value; }
    }

    CharacterController cc;
    float yVelocity = 0;
    int curHp = 0;
    int maxHp = 10;
    Animator anim;

    // 적의 트랜스폼 동기화 처리용 변수
    Vector3 npcPos;
    Quaternion npcRot;
    float moveDir_X = 0;
    float moveDir_Y = 0;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        curHp = maxHp;
        
        anim = GetComponentInChildren<Animator>();

        // 본인일 때에만 카메라를 활성화한다.
        cam.SetActive(photonView.IsMine);

        gameObject.layer = photonView.IsMine ? LayerMask.NameToLayer("Player") : LayerMask.NameToLayer("Enemy");
        nickName.text = photonView.IsMine ? PhotonNetwork.NickName : photonView.Owner.NickName;
        nickName.color = photonView.IsMine ? Color.green : Color.red;
        
    }

    void Update()
    {
        hpSlider.value = (float)curHp / (float)maxHp;
        // 본인일 때에만 사용자 입력 움직임을 실행한다.
        if (photonView.IsMine)
        {
            float h = 0;
            float v = 0;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                h = Input.GetAxisRaw("Horizontal");
                anim.SetFloat("Horizontal", h);
                v = Input.GetAxisRaw("Vertical");
                anim.SetFloat("Vertical", v);
            }
            Vector3 dir = transform.forward * v + transform.right * h;
            dir.Normalize();

            if (cc.collisionFlags == CollisionFlags.Below)
            {
                yVelocity = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }

            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;

            cc.Move(dir * moveSpeed * Time.deltaTime);

            
        }
        // 본인이 아닐 경우에는 보간 처리를 한다.
        else
        {
            cc.transform.position = Vector3.Lerp(cc.transform.position, npcPos, Time.deltaTime * 100);
            cc.transform.rotation = Quaternion.Lerp(cc.transform.rotation, npcRot, Time.deltaTime * 100);
            anim.SetFloat("Horizontal", moveDir_X);
            anim.SetFloat("Vertical", moveDir_Y);
        }
    }

    // 피격 함수
    public void OnDamaged(int damage, string shooterName)
    {
        photonView.RPC("DamageProcess", RpcTarget.AllBuffered, damage);
    }

    // 데미지 처리 및 UI 표시용 브로드캐스팅 함수
    [PunRPC]
    void DamageProcess(int damage)
    {
        curHp = Mathf.Max(curHp - damage, 0);
        hpSlider.value = (float)curHp / (float)maxHp;
    }

    // 패킷 송수신 방식 인터페이스 구현
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 만일, 서버에 전송하는 상황이라면...
        if(stream.IsWriting == true)
        {
            // 캐릭터의 위치와 회전을 전송한다.
            stream.SendNext(cc.transform.position);
            stream.SendNext(cc.transform.rotation);
            stream.SendNext(curHp);
            stream.SendNext(anim.GetFloat("Horizontal"));
            stream.SendNext(anim.GetFloat("Vertical"));
        }
        // 그렇지 않고, 서버로부터 읽어오는 상황이라면...
        else
        {
            // 받은 정보를 Vector3 형태로 캐스팅하여 나의 위치를 갱신한다.
            npcPos = (Vector3)stream.ReceiveNext();
            npcRot = (Quaternion)stream.ReceiveNext();
            curHp = (int)stream.ReceiveNext();
            moveDir_X = (float)stream.ReceiveNext();
            moveDir_Y = (float)stream.ReceiveNext();
        }
    }
}
