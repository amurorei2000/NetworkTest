using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerRotate : MonoBehaviourPun, IPunObservable
{
    public float rotSpeed = 300.0f;

    float mx = 0;

    // 적의 회전 동기화 처리를 위한 변수
    Quaternion npcRot;

    void Update()
    {
        if (photonView.IsMine)
        {
            mx += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            transform.eulerAngles = new Vector3(0, mx, 0);
        }
        else
        {
            transform.rotation = npcRot;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            npcRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
