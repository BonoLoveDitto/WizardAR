using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int maxPlayers = 2;
    private int playerCount = 0;
    private bool joinedRoom = false;
    private List<string> nickname = new List<string> {"1","2"};
    private int nicknameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //有沒有連接到伺服器
        if(PhotonNetwork.IsConnected == false){
            SceneManager.LoadScene("01-menu");
        }
        else{
            Debug.Log("join Lobby");
            //PhotonNetwork.JoinLobby();
            QuickMatch();
        }
    }

    // Update is called once per frame
    void Update()
    { 
        //計算房間裡面有幾個人
        if(joinedRoom){
            playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log("room: " + PhotonNetwork.CountOfRooms + " ,room Players: " + playerCount);
            if(playerCount == maxPlayers)
            {
                //將每個player命名
                foreach(Player p in PhotonNetwork.PlayerList){
                    p.NickName = nickname[nicknameCount++];
                    //Debug.Log("i'm " + p.NickName);
                }
                nicknameCount = 0;
                
                //切換到遊戲
                SceneManager.LoadScene("04-Game");
            }
        }
    }

    public override void OnJoinedLobby(){
        Debug.Log("Lobby joined");
    }

    //沒有房間就創建一個
    void CreateRoom() {
        Debug.Log("Create Room");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    //快速加入一個房間
    private void QuickMatch(){
        Debug.Log("QuickMatch");
        PhotonNetwork.JoinRandomRoom();
    }

    //加入房間失敗 -> 創建房間
    public override void OnJoinRandomFailed(short returnCode, string message){
        Debug.Log("Joined Randoim Failed");
        CreateRoom();
    }
    
    //成功加入room
    public override void OnJoinedRoom() {
        Debug.Log("Room Joined");
        joinedRoom = true;
    }

    //當back to menu的button按下去，回到menu
    public void onMouseClick(){
        Debug.Log("On Mouse Click");
        //WaitForDisconnect();
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("01-Menu");
        PhotonNetwork.CurrentRoom.SetCustomProperties(null);
    }
}
