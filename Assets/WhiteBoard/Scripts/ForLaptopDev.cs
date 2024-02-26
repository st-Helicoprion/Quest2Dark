using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ForLaptopDev : MonoBehaviour
{
    public InputActionAsset Controls;
    private InputAction look;
    public Transform mainCamera, playerBody;
    public float rotSpeed;
    // public GameObject VRController;
    public Vector2 turn;
    float XRot =0;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;        

         var PlayerControls = Controls.FindActionMap("Player");
         look = PlayerControls.FindAction("Look");

         look.Enable();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Transform>();
        // VRController.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    //    Vector2 lookDir = look.ReadValue<Vector2>();

    //    Debug.Log("MouseX:"+lookDir.x+", MouseY:"+lookDir.y);

    //   mainCamera.Rotate(new Vector3(-lookDir.y, lookDir.x, 0)*rotSpeed,Space.Self);

       Vector2 lookDir = look.ReadValue<Vector2>();
    
        turn.x = lookDir.x*rotSpeed*Time.deltaTime;
        turn.y = lookDir.y*rotSpeed*Time.deltaTime;
        XRot-=turn.y;
       mainCamera.localRotation = Quaternion.Euler(XRot,0,0);
      
       XRot=Mathf.Clamp(XRot,-90,90);
       playerBody = this.transform;
       playerBody.Rotate(Vector3.up*turn.x);
       
      

    }

}
