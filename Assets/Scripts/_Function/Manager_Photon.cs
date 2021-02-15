using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // :: Photon Components for Unity
using Photon.Realtime; // :: Photon Realtime Library

public class Manager_Photon : MonoBehaviourPunCallbacks
{
    // : Function
    private Manager_PhotonRPC_InHeist RPCManager;

    // : Initialise
    public void Init()
    {
        // :: Function
        this.RPCManager = this.GetComponent<Manager_PhotonRPC_InHeist>();

        // :: Status
        PhotonNetwork.AutomaticallySyncScene = true;

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Set
    public void Set_GameVersion()
    {
        PhotonNetwork.GameVersion = Application.version;
    }
    public void Set_NickName()
    {
        PhotonNetwork.NickName = Dictator.Nickname;
    }

    // : Load
    public void Load_Level(EnumAll.eScene eScene)
    {
        switch(eScene)
        {
            case EnumAll.eScene.IN_HEIST:
                this.RPC_Load_InHeist();
                break;
        }
    }
    public void LoadComplete(System.Action action)
    {
        this.StartCoroutine(this.LoadCompleteImplement(action));
    }
    private IEnumerator LoadCompleteImplement(System.Action action)
    {
        while (PhotonNetwork.LevelLoadingProgress < 1)
            yield return null;
        action?.Invoke();
    }

    // : Try
    public void Try_Connect()
    {
        this.Set_GameVersion();
        this.Set_NickName();
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Try_JoinRoom()
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRandomRoom();
    }

    // : Override
    public System.Action Callback_Connected;
    public override void OnConnectedToMaster() // ::: Connect : Success
    {
        base.OnConnectedToMaster();
        Dictator.Debug_All(this.ToString(), "Connect : Success");

        // :: Callback
        this.Callback_Connected?.Invoke();
    }
    public System.Action Callback_FailedToConnect;
    public override void OnDisconnected(DisconnectCause cause) // ::: Connect : Failed
    {
        base.OnDisconnected(cause);
        Dictator.Debug_All(this.ToString(), "Connect : Failed / " + cause.ToString());

        // :: Callback
        this.Callback_FailedToConnect?.Invoke();
    }
    public System.Action Callback_Joined;
    public override void OnJoinedRoom() // ::: Join Random Room : Success
    {
        base.OnJoinedRoom();
        string clientType = this.Is_MasterClient() ? "Master" : "Guest";
        Dictator.Debug_All(this.ToString(), "Join : Success / " + clientType + " / " + PhotonNetwork.CurrentRoom.Name);

        // :: Callback
        this.Callback_Joined?.Invoke();
    }
    public override void OnJoinRandomFailed(short returnCode, string message) // ::: Join Random Room : Fail
    {
        base.OnJoinRandomFailed(returnCode, message);
        // :: Create
        PhotonNetwork.CreateRoom(Dictator.Nickname, new RoomOptions { MaxPlayers = 2 });
    }

    // : Is
    public bool Is_MasterClient()
    {
        return PhotonNetwork.IsMasterClient;
    }

    // : Get
    public int Get_PlayerCount()
    {
        int count = PhotonNetwork.CurrentRoom.PlayerCount;
        //Dictator.Debug_All(this.ToString(), "Player Count : " + count);
        return count;
    }

    // : RPC
    public void RPC_Load_InHeist()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("LoadLevel_InHeist", RpcTarget.All);
        else
            this.RPCManager.LoadLevel_InHeist();
    }
    public void RPC_Set_NewCharacter(EnumAll.eCharacter eCharacter, EnumAll.eTeam eTeam, Vector3 tileCoordinate, string guid)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("Set_NewCharacter", RpcTarget.All, eCharacter, eTeam, tileCoordinate, guid);
        else
            this.RPCManager.Set_NewCharacter(eCharacter, eTeam, tileCoordinate, guid);
    }
    public void RPC_Set_Character(string guid, Vector3 tileCoordinate)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("Set_Character", RpcTarget.All, guid, tileCoordinate);
        else
            this.RPCManager.Set_Character(guid, tileCoordinate);
    }
    public void RPC_Kill_Character(string guid)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("Kill_Character", RpcTarget.All, guid);
        else
            this.RPCManager.Kill_Character(guid);
    }
    public void RPC_Do_AutoBattle()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("Do_AutoBattle", RpcTarget.MasterClient);
        else
            this.RPCManager.Do_AutoBattle();
    }
    public void RPC_Win(EnumAll.eTeam eTeam)
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("Win", RpcTarget.All, eTeam);
        else
            this.RPCManager.Win(eTeam);
    }
    public void RPC_Init_InHeist()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.RPCManager.photonView.RPC("RPCInit_InHeist", RpcTarget.All);
        else
            this.RPCInit_InHeist();
    }
    [PunRPC]
    private void RPCInit_InHeist()
    {
        InHeist_Ruler ruler = GameObject.FindObjectOfType<InHeist_Ruler>();
        ruler.Init();
        RPCManager.Init_InHeist();
    }
}