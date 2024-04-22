using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShadowing : MonoBehaviour
{
    public Transform mainCamera;
    public Camera selfCam;
    public bool host;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        GameObject camChecker = GameObject.Find("NetworkCam(Clone)");
        if (camChecker != null)
        {
            host = false;
        }else host= true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);

        if(!host)
        {
            selfCam.enabled = true;
        }
        else
        {
            selfCam.enabled = false;
        }
    }
}
