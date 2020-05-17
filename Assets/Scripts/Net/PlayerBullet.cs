using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerBullet : MonoBehaviourPun, IPunObservable
{
    public float speed = 20.0f;
    public int attackPower = 2;
    public float lifeTime = 4.0f;

    bool canStrike = true;
    Vector3 pos;

    void Start()
    {
        // 4초 뒤에 자동 삭제
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 마스터 오브젝트이면 이동 공식으로 이동한다.
        if (photonView.IsMine)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            
        }
        // 복제 오브젝트이면 서버에서 전송받은 값으로 이동한다.
        else
        {
            transform.position = pos;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        // 만일, 마스터 오브젝트이면서 부딪힌 대상의 레이어가 "Enemy"라면...
        if (photonView.IsMine && col.gameObject.layer == LayerMask.NameToLayer("Enemy") && canStrike)
        {
            canStrike = false;
            PlayerMove pm = col.transform.GetComponent<PlayerMove>();
            //PhotonView pv = col.transform.GetComponent<PhotonView>();
            
            if (pm)
            {
                //pv.RPC("OnDamaged", RpcTarget.All, attackPower);
                pm.OnDamaged(attackPower, PhotonNetwork.NickName);
            }

            // 마스터와 복제 오브젝트 둘 다 제거한다.
            photonView.RPC("DestroySelf", RpcTarget.AllBuffered);
        }
    }

    // 자기 자신을 제거하는 브로드캐스팅 함수
    [PunRPC]
    void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            pos = (Vector3)stream.ReceiveNext();
        }
    }
}
