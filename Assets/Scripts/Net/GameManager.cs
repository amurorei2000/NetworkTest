using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager gm;
    public Transform spawnPointsTeamOne;
    public Transform spawnPointsTeamTwo;
    public int selectedTeam = 0;
    public GameObject partyPanel;

    private void Awake()
    {
        // 해상도 설정하기
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);

        // 싱글턴 설정
        if(gm == null)
        {
            gm = this;
        }
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
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // 플레이어 생성하기
    void OnJoinPlayer()
    {
        // 위치 지점 설정하기(랜덤)
        //int idx = Random.Range(0, spawnPoints.Length);
        selectedTeam = PhotonNetwork.PlayerList.Length % 2;
        Vector3 spawnPos = GetSpawnPosition();

        // 플레이어 프리팹 생성하기
        //PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation);
        if (selectedTeam == 0)
        {
            PhotonNetwork.Instantiate("Player", spawnPos, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player1"), spawnPos, Quaternion.identity);
        }
    }

    public Vector3 GetSpawnPosition()
    {
        Vector2 randPos = Random.insideUnitCircle * 5;

        if (selectedTeam == 0)
        {
            return spawnPointsTeamOne.position + new Vector3(randPos.x, 0, randPos.y);
        }
        else
        {
            return spawnPointsTeamTwo.position + new Vector3(randPos.x, 0, randPos.y);
        }
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

            //Cursor.lockState = CursorLockMode.None;
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("연결 종료: " + cause);
        // 로그인 씬으로 전환한다.
        Application.Quit();

        Cursor.lockState = CursorLockMode.None;
    }
}
