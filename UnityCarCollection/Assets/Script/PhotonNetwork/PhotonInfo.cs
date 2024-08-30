using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonInfo : MonoBehaviourPunCallbacks
{
    public string Version = "Car1.0";
    public InputField userID;
    public Text userId;

    void Awake()
    {
        PhotonNetwork.GameVersion = Version;
        // 게임 버전 설정

        PhotonNetwork.AutomaticallySyncScene = true;
        // 자동으로 씬 동기화 처리 하도록 설정 한다.

        if (!PhotonNetwork.IsConnected)
        {// 게임도중 로비로 가면 재접속 불가
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속");
        PhotonNetwork.JoinLobby();
        userId.text = GetUserID();
    }

    public override void OnJoinedLobby()  // 포톤네트워크 로비로 들어오는 로직
    {
        Debug.Log("로비 입장");
        //PhotonNetwork.JoinRandomRoom();
        // 아무 방이나 접속
    }

    string GetUserID()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if (string.IsNullOrEmpty(userId))
        {
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        }
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    //public override void OnJoinRandomRoomFailed(short returnCode, string message)
    //{// 방 참가 실패시 랜덤룸 생성
    //    Debug.Log("랜덤룸 참가 실패");
    //    PhotonNetwork.CreateRoom("CarRoom", new RoomOptions { MaxPlayers = 4 });
    //    // 최대 인원 수 4명인 방 생성
    //}

    public override void OnCreatedRoom()
    {// 포톤 네트워크 상에서 방이 생성이 되었는지 알려줌
        Debug.Log("방 생성 성공 !!");
    }

    public override void OnJoinedRoom()
    {// 생성된 방에 들어갔는지 알려줌
        Debug.Log("방 참가 성공");
        StartCoroutine(CarMineScene());
    }

    IEnumerator CarMineScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("F1TrackDisplayScene");
        yield return ao;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {// 포톤 네트워크 상에서 생성된 방이 없을 때 알려줌
        Debug.Log("오류 코드" + returnCode.ToString());
        
        Debug.Log("방 생성 실패 : " + message);
        // 방 생성 실패 이유를 알려줌
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnGUI()
    {// 포톤 네트워크에서 클라이언트 상태 정보를 알려준다.
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
