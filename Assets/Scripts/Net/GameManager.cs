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

    void Update()
    {
        // ESC 키를 누르면 연결 종료
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.Disconnect();
        }
        // 키보드의 L 키를 누르면 방 나가기 및 로비로 변경
        else if(Input.GetKeyDown(KeyCode.L))
        {
            // 만일, 방장이라면 방장 권한을 다른 사람에게 넘긴다.
            if(PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerListOthers[0]);
            }

            // 방을 떠난다.
            PhotonNetwork.LeaveRoom();

            // 로비 씬으로 전환한다.
            PhotonNetwork.LoadLevel("LobbyScene");

            Cursor.lockState = CursorLockMode.None;

        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결 종료: " + cause);
        // 로그인 씬으로 전환한다.
        PhotonNetwork.LoadLevel("LoginScene");

        Cursor.lockState = CursorLockMode.None;
    }
}
