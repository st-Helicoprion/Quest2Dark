using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCamManager : MonoBehaviour
{
    public GameObject UICamera, selectScreen;
    public GameManager gameManager;
    public NetworkManager networkManager;

    private void Awake()
    {

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            gameManager.enabled = false;
            selectScreen.SetActive(true);
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            gameManager.enabled = true;
            UICamera.SetActive(false);
            selectScreen.SetActive(false);
            networkManager.StartHost();
        }
    }

    public void SelectDebug()
    {
        UICamera.SetActive(false);
        selectScreen.SetActive(false);
        gameManager.enabled = true;
    }

    public void SelectSpectate()
    {
        UICamera.SetActive(false);
        selectScreen.SetActive(false);
        networkManager.StartClient();
    }

}
