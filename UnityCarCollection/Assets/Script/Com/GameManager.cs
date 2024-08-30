using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    int PlayerClamp;
    int Count;

    void Awake()
    {
        // 최대 입장 가능 플레이어 수
        PlayerClamp = PhotonNetwork.CurrentRoom.MaxPlayers;

        Count = PhotonNetwork.CurrentRoom.PlayerCount;

        if (PlayerClamp > 5) return;

        CreatePlayer(Count);
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    void CreatePlayer(int playerCount)
    {
        switch (playerCount)
        {
            case 1:
                Vector3 pos = new Vector3 (15f, 0f, 0f);
                Quaternion rot = Quaternion.Euler(0f, -88f, 0f);
                PhotonNetwork.Instantiate("PlayerCar", pos, rot);
                break;

            case 2:
                Vector3 pos1 = new Vector3(15f, 0f, 7f);
                Quaternion rot1 = Quaternion.Euler(0f, -88f, 0f);
                PhotonNetwork.Instantiate("PlayerCar", pos1, rot1);
                break;

            case 3:
                Vector3 pos2 = new Vector3(15f, 0f, -6f);
                Quaternion rot2 = Quaternion.Euler(0f, -88f, 0f);
                PhotonNetwork.Instantiate("PlayerCar", pos2, rot2);
                break;

            case 4:
                Vector3 pos3 = new Vector3(-15f, 0f, 5f);
                Quaternion rot3 = Quaternion.Euler(0f, -88f, 0f);
                PhotonNetwork.Instantiate("PlayerCar", pos3, rot3);
                break;
        }
    }

    void Update()
    {
        
    }
}
