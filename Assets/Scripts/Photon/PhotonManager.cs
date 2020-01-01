using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    ManagerHub _managerHub;
    [SerializeField]
    int maxPlayer = 2;

    private void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        //開始連線
        PhotonNetwork.ConnectUsingSettings();
        Application.runInBackground = true;

        //InvokeRepeating("UpdateStatus", 0, 1f);
    }

    #region MonoBehaviourPunCallbacks CallBacks
    // below, we implement some callbacks of PUN
    // you can find PUN's callbacks in the class MonoBehaviourPunCallbacks


    /// <summary>
    /// Called after the connection to the master is established and authenticated
    /// </summary>
    public override void OnConnectedToMaster()
    {
        Debug.Log("連線到Photon伺服器成功");
        Debug.Log("加入隨機房間");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("加入隨機房間失敗");
        Debug.Log("創造一個新房間，人數" + maxPlayer + "人");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = (byte)maxPlayer });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError("<Color=Red>已斷線，原因:</Color>" + cause);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("加入新房間");
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayer)
        {
            //Method1  生出所有的坦克車(2台)
            _managerHub._gameManager.SpawnAllTanks();
            Debug.Log("2人滿");
        }
        else
        {
            Debug.Log("等待玩家加入");
        }
    }

    #endregion

}
