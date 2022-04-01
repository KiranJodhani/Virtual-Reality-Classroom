using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerElement : MonoBehaviour
{
    public PhotonView PhotonViewInstance;
    public string PlayerName;
    public OVRPlayerController OVRPlayerControllerInstance;

    

    private void Awake()
    {
       
    }

    void Start()
    {
        SetPlayerName();
    }

    private void Update()
    {
       
    }


    /***********************************/
    /***** PLAYER NAME SYNC STARTS *****/

    public void SetPlayerName()
    {

        PlayerName = "Player_" + PhotonViewInstance.Owner.UserId;
        gameObject.name = PlayerName;
        PhotonViewInstance.Owner.NickName = PlayerName;
        if (PhotonViewInstance.IsMine)
        {
            SetPlayerNameOnNetwork();
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            OVRPlayerControllerInstance.enabled = true;
        }
        else
        {
            transform.GetChild(1).gameObject.SetActive(false);
            
        }
    }

    void SetPlayerNameOnNetwork()
    {
        PhotonViewInstance.RPC("SetPlayerNameOnNetwork_RPC", RpcTarget.Others, PlayerName);
        Invoke("SetPlayerNameOnNetwork", 0.5f);
    }

    [PunRPC]
    public void SetPlayerNameOnNetwork_RPC(string PName)
    {
        gameObject.name = PName;
        PlayerName = PName;
        PhotonViewInstance.Owner.NickName = PName;
    }

    /***** PLAYER NAME SYNC ENDS *****/
    /*********************************/
}
