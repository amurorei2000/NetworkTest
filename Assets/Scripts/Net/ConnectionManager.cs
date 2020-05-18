using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public InputField userID;
    public string gameVersion = "1";


    private void Awake()
    {
        // 해상도 변경하기
        Screen.SetResolution(320, 200, FullScreenMode.Windowed);
    }

    // 서버 연결 함수
    public void Connect()
    {
        // 게임 버전 설정하기
        PhotonNetwork.GameVersion = gameVersion;

        // 마스터 클라이언트의 씬 셋팅에 다른 클라이언트를 동기화
        PhotonNetwork.AutomaticallySyncScene = true;

        // 서버에 연결할 아이디 입력
        PhotonNetwork.NickName = userID.text;

        // 환경 설정 파일에 있는 설정대로 서버 연결하기
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
#if CONNECT_TEST
        Debug.Log("네임 서버 접속 완료, 마스터 서버에 연결 요청~");
#endif
    }

    public override void OnConnectedToMaster()
    {
#if CONNECT_TEST
        Debug.Log("마스터 서버 접속 완료, 로비에 연결 요청~");
#endif
        // 로비 접속 요청(디폴트)
        PhotonNetwork.JoinLobby(TypedLobby.Default);

        // 로비 접속 요청(특정 이름)
        //PhotonNetwork.JoinLobby(new TypedLobby("WS_Develop", LobbyType.Default));
    }

    public override void OnJoinedLobby()
    {
#if CONNECT_TEST
        Debug.Log("로비 접속 완료, 로비 씬 로드 하기");
#endif

        // 씬 동기화하여 열기
        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
