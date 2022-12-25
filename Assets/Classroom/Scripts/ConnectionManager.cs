using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public byte maxPlayersPerRoom = 4;

    public GameObject SpawnPoints_Professor;
    public GameObject SpawnPoints_Student;

    public GameObject CameraTemp;

    void Start()
    {
        
    }

 

    /************************************ PHOTON STARTS ************************************/
    /***************************************************************************************/


    public void ConnectToPhoton()
    {
        GameManagerMultiplayer.Instance.StatusText.text = "Connecting to the server..";
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ReConnect()
    {
        ConnectToPhoton();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
        JoinRoom();
    }

    public void JoinRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameManagerMultiplayer.Instance.StatusText.text = "Joining room..";
            PhotonNetwork.JoinRandomRoom();
        }
    }



    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }




    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        if (cause == DisconnectCause.ExceptionOnConnect)
        {
            Debug.Log("Please check your internet connection!");
        }
        if (cause == DisconnectCause.ServerTimeout || cause == DisconnectCause.DisconnectByServerReasonUnknown)
        {
            ConnectToPhoton();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed() was called by PUN. Failed to create room... trying again");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom Player Counter : " + PhotonNetwork.CurrentRoom.PlayerCount);
        
        if(PhotonNetwork.IsMasterClient)
        {
                PhotonNetwork.Instantiate("OVRPlayer_Professor", SpawnPoints_Professor.transform.localPosition,
                                                                 SpawnPoints_Professor.transform.localRotation);
        }
        else
        {
            Vector3 StudentPos = new Vector3(Random.Range(-3, 3), 1.2f, Random.Range(-1.5f, 3.5f));
            PhotonNetwork.Instantiate("OVRPlayer_Student", StudentPos, SpawnPoints_Student.transform.localRotation);
        }
        CameraTemp.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom() " + newPlayer.NickName);
        
        //if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        //    waitingText.SetActive(false);
    }

    public override void OnLeftRoom()
    {

    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        GameObject NewMasterClient_Player = GameObject.Find("Player_" + newMasterClient.UserId);
        if (newMasterClient.IsMasterClient)
        {

        }
    }

    /************************************* PHOTON ENDS *************************************/
    /***************************************************************************************/
}
