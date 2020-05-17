using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public Transform[] spawnPoints;

    private void Awake()
    {
        // 해상도 설정하기
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);
    }

    void Start()
    {
        // RPC 패킷 전송 빈도를 설정하기(default: 10)
        PhotonNetwork.SendRate = 30;

        // OnPhotonSerializeView 함수 호출 빈도를 설정하기(default: 10)
        PhotonNetwork.SerializationRate = 30;

        // 플레이어 생성 함수 호출
        OnJoinPlayer();

        // 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 플레이어 생성하기
    void OnJoinPlayer()
    {
        // 위치 지점 설정하기(랜덤)
        int idx = Random.Range(0, spawnPoints.Length);

        // 플레이어 프리팹 생성하기
        //PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation);
        PhotonNetwork.Instantiate("Player", new Vector3(0, 1.5f, 0), Quaternion.identity);
    }

}
