using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using Photon.Pun;
using Photon.Realtime;

public class GameManagerMultiplayer : MonoBehaviour
{
    public static GameManagerMultiplayer Instance;


    public ConnectionManager ConnectionManagerInstance;
    public TextMeshProUGUI StatusText;


    private void Awake()
    {
        Instance = this;
    }


 
    void Start()
    {
        StartCoroutine(CheckInternetConnection());
    }

  
    IEnumerator CheckInternetConnection()
    {
        StatusText.text = "Checking internet connectivity";
        UnityWebRequest request = new UnityWebRequest("https://google.com");
        yield return request.SendWebRequest();
        if (request.error != null)
        {
            Debug.Log("Error");
            StatusText.text = "It seems you are offline. Please check your network connectivity";
            yield return new WaitForSeconds(2);
            StartCoroutine(CheckInternetConnection());
        }
        else
        {
            Debug.Log("Success");
            ConnectionManagerInstance.ConnectToPhoton();
        }
    }   
}
