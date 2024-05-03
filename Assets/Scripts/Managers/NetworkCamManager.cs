using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkCamManager : MonoBehaviour
{
    public static NetworkCamManager instance;
    public GameObject UICamera, selectScreen;
    public NetworkManager networkManager;

    private void Awake()
    {
        
        instance = this;

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            selectScreen.SetActive(true);
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            UICamera.SetActive(false);
            selectScreen.SetActive(false);
            networkManager.StartHost();
        }
    }

    public void SelectDebug()
    {
        UICamera.SetActive(false);
        selectScreen.SetActive(false);
        
    }

    public void SelectSpectate()
    {
        UICamera.SetActive(false);
        selectScreen.SetActive(false);
        networkManager.StartClient();
    }

}
