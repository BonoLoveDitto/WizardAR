using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public void OnClickStart(){
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("ClickStart");
    }

    public override void OnConnectedToMaster() {
        print("Connected");
        SceneManager.LoadScene("05-LoadingLobby");
    }
}
