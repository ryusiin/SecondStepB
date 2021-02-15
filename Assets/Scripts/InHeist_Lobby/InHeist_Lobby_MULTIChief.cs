using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InHeist_Lobby_MULTIChief : MonoBehaviourPunCallbacks
{
    // : Chief
    private InHeist_Lobby_UIChief UIChief;

    // : Function
    private Manager_Photon PHOTONManager;
    private Manager_Dim DIMManager;

    // : Init
    public void Init()
    {
        // :: UIChief
        this.UIChief = GameObject.FindObjectOfType<InHeist_Lobby_UIChief>();

        // :: Function
        this.PHOTONManager = GameObject.FindObjectOfType<Manager_Photon>();
        this.DIMManager = new Manager_Dim(this.UIChief.GetImage_Dim());

        // :: Complete
        Dictator.Debug_Init(this.ToString());
    }

    // : Load Level
    public void MoveScene_InHeist()
    {
        if (PhotonNetwork.IsConnectedAndReady)
            this.photonView.RPC("LoadLevel_InHeist", RpcTarget.All);
        else
            this.LoadLevel_InHeist();
    }

    [PunRPC]
    private void LoadLevel_InHeist()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.DIMManager.Show(true);
            this.DIMManager.Fade(1f, 1f);
            return;
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount < 1)
            return;

        // :: Status
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        // :: Fade and Load Level
        this.DIMManager.Show(true);
        this.DIMManager.Fade(1f, 1f, () => {
            this.PHOTONManager.LoadComplete(() => {
                this.PHOTONManager.RPC_Init_InHeist();
            });
            PhotonNetwork.LoadLevel("InHeist");
        });
    }
}
