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
        // ���� ���� ����

        PhotonNetwork.AutomaticallySyncScene = true;
        // �ڵ����� �� ����ȭ ó�� �ϵ��� ���� �Ѵ�.

        if (!PhotonNetwork.IsConnected)
        {// ���ӵ��� �κ�� ���� ������ �Ұ�
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("������ ���� ����");
        PhotonNetwork.JoinLobby();
        userId.text = GetUserID();
    }

    public override void OnJoinedLobby()  // �����Ʈ��ũ �κ�� ������ ����
    {
        Debug.Log("�κ� ����");
        //PhotonNetwork.JoinRandomRoom();
        // �ƹ� ���̳� ����
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
    //{// �� ���� ���н� ������ ����
    //    Debug.Log("������ ���� ����");
    //    PhotonNetwork.CreateRoom("CarRoom", new RoomOptions { MaxPlayers = 4 });
    //    // �ִ� �ο� �� 4���� �� ����
    //}

    public override void OnCreatedRoom()
    {// ���� ��Ʈ��ũ �󿡼� ���� ������ �Ǿ����� �˷���
        Debug.Log("�� ���� ���� !!");
    }

    public override void OnJoinedRoom()
    {// ������ �濡 ������ �˷���
        Debug.Log("�� ���� ����");
        StartCoroutine(CarMineScene());
    }

    IEnumerator CarMineScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("F1TrackDisplayScene");
        yield return ao;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {// ���� ��Ʈ��ũ �󿡼� ������ ���� ���� �� �˷���
        Debug.Log("���� �ڵ�" + returnCode.ToString());
        
        Debug.Log("�� ���� ���� : " + message);
        // �� ���� ���� ������ �˷���
    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRandomRoom();
    }

    private void OnGUI()
    {// ���� ��Ʈ��ũ���� Ŭ���̾�Ʈ ���� ������ �˷��ش�.
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }
}
