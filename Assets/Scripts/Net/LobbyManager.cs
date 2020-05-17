using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomName;
    public InputField maxUsers;
    public Button btn_Join;
    public GameObject content;
    public GameObject roomField;

    // 방 정보 저장용 딕셔너리
    Dictionary<string, RoomInfo> cachedRoom = new Dictionary<string, RoomInfo>();

    public override void OnEnable()
    {
        // 콜백 인터페이스 실행
        base.OnEnable();

        // 해상도 조정
        Screen.SetResolution(960, 640, FullScreenMode.Windowed);
    }

    void Start()
    {
        // 방 참가 인원 설정하기
        maxUsers.text = "4";
    }

    // 방 생성 함수
    public void CreateRoom()
    {
#if CONNECT_TEST
        Debug.Log("방 생성 요청!");
#endif

        // 방 생성 옵션 설정하기
        RoomOptions ro = new RoomOptions
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = byte.Parse(maxUsers.text)
        };

        // 동일한 옵션의 방이 있으면 조인하고, 없으면 방을 생성한다.
        PhotonNetwork.JoinOrCreateRoom(roomName.text, ro, TypedLobby.Default);
    }

    // 방 들어가기 함수
    public void JoinRoom()
    {
#if CONNECT_TEST
        Debug.Log("방에 들어간당~");
#endif
        PhotonNetwork.JoinRoom(roomName.text);
    }

    // 방에 입장할 때의 콜백 함수
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("NetworkScene");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    // 방 목록을 갱신할 때 호출되는 콜백 함수
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
#if CONNECT_TEST
        Debug.Log("방 정보 가져오기~");
#endif

        // 기존 방 목록을 지운다.
        RemoveRoomList();

        // 현재 방 목록을 캐싱한다.
        UpdateRoomListCache(roomList);

        // 캐싱된 방 목록을 UI에 추가한다.
        RemakeCachedRoomList();
    }

    // 방 목록 삭제 함수
    void RemoveRoomList()
    {
        foreach (Transform cont in content.transform)
        {
            Destroy(cont.gameObject);
        }
    }

    // 방 목록 캐싱 함수
    void UpdateRoomListCache(List<RoomInfo> roomList)
    {
        foreach (RoomInfo ri in roomList)
        {
            // 만일, 같은 방 이름이 존재하면 정보를 갱신
            if (cachedRoom.ContainsKey(ri.Name))
            {
                // 만일, 제거 대상이면 방 제거
                if (ri.RemovedFromList)
                {
                    cachedRoom.Remove(ri.Name);
#if CONNECT_TEST
                    Debug.Log("방 제거 완료!");
#endif
                    continue;
                }

                // 새로 갱신
                cachedRoom[ri.Name] = ri;
            }
            // 그렇지 않고, 같은 방 이름이 존재하지 않으면 새로 저장
            else
            {
                cachedRoom.Add(ri.Name, ri);
#if CONNECT_TEST
                Debug.Log("방 정보 저장 완료!");
#endif
            }
        }
    }

    // 방 정보를 UI에 출력하는 함수
    void RemakeCachedRoomList()
    {
        if (cachedRoom.Count == 0)
        {
            return;
        }

        foreach (RoomInfo ri in cachedRoom.Values)
        {
            // 만일, 오픈 방이고 보이면서, 제거 대상이 아니라면...
            if (ri.IsOpen && ri.IsVisible && ri.RemovedFromList == false)
            {
                // 방 프리팹 생성
                GameObject go = Instantiate(roomField);
                go.transform.SetParent(content.transform);

                // 방 이름 쓰기
                InputField inputF = go.transform.GetComponent<InputField>();
                inputF.text = ri.Name;

                // 방 목록을 클릭했을 때의 이벤트 바인딩

                // 1. 이벤트 트리거 생성
                EventTrigger myTrigger = go.AddComponent<EventTrigger>();

                // 2. 이벤트 트리거 엔트리 생성
                EventTrigger.Entry entry_select = new EventTrigger.Entry();

                // 3. 이벤트 트리거 엔트리의 이벤트 속성 설정
                entry_select.eventID = EventTriggerType.Select;

                // 4. 이벤트 트리거 엔트리 델리게이트에 함수 바인딩하기(람다식)
                entry_select.callback.AddListener((data) => { OnSelectRoom((BaseEventData)data); });

                // 5. 엔트리 델리게이트를 이벤트 트리거에 바인딩하기
                myTrigger.triggers.Add(entry_select);
            }
        }
    }

    void OnSelectRoom(BaseEventData arg)
    {
        // Join 버튼을 활성화한다.
        btn_Join.interactable = true;
        // 선택한 방의 이름을 roomName 항목에 쓴다.
        roomName.text = arg.selectedObject.GetComponent<InputField>().text;
    }

    public override void OnConnectedToMaster()
    {
        // 로비로 접속
        //PhotonNetwork.JoinLobby(new TypedLobby("WS_Develop", LobbyType.Default));
        PhotonNetwork.JoinLobby(TypedLobby.Default);
#if CONNECT_TEST
        Debug.Log("마스터 서버 접속 완료, 네임 서버에 연결 요청~");
#endif
    }
}
