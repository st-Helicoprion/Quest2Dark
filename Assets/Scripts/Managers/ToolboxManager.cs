using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ToolboxManager : MonoBehaviour
{
    public static ToolboxManager instance;

    public Transform mainCamera, toolboxParent, leftHand, rightHand;
    public static int receivedHandID; //inform left or right hand

    public static List<int> itemIDList = new List<int>();
    public static List<GameObject> keyItemsSave = new List<GameObject>(); //stored list of equipments, refreshed every load

    public List<Vector3> offsetList = new List<Vector3>(); 
    public static List<GameObject> boxToyList= new List<GameObject>(); //list of toys in box

    public bool isVisible, isOpen;
    
    public InputActionReference summonToolboxInputLeft;
    public InputActionReference summonToolboxInputRight;
    

    private void OnEnable()
    {
        instance= this;

        summonToolboxInputLeft.action.performed += GetHandIDLeft;
        summonToolboxInputLeft.action.performed += SummonToolbox;
        summonToolboxInputLeft.action.canceled += HideToolbox;

        summonToolboxInputRight.action.performed += GetHandIDRight;
        summonToolboxInputRight.action.performed += SummonToolbox;
        summonToolboxInputRight.action.canceled += HideToolbox;
    }
    // Start is called before the first frame update
    void Start()
    {
        ClearItemIDList();
        toolboxParent = GameObject.Find("ToolboxParent").transform;
        leftHand = GameObject.FindWithTag("LeftHand").transform;
        rightHand = GameObject.FindWithTag("RightHand").transform;
    }

    // Update is called once per frame
    void Update()
    {
        toolboxParent.localRotation = new Quaternion(0, mainCamera.localRotation.y, 0, mainCamera.localRotation.w);
        
    }

    public void ClearItemIDList()
    {
        itemIDList.Clear();
        //boxToyList.Clear();
    }

    public void SummonToolbox(InputAction.CallbackContext summonInput)
    {
        isOpen = true;
        //summons toolbox to side of arm, input is xr primary button
        if (summonInput.ReadValue<float>() == 1&&this!=null)
        {
            isVisible = true;

            if (receivedHandID == 1)
            {
                Debug.Log("box called to left");
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(j).GetComponent<Collider>().enabled = true;
                }
                this.transform.parent = leftHand;
                this.transform.localPosition = offsetList[0];
                this.transform.localRotation = Quaternion.identity;
            }
            else if (receivedHandID == 2)
            {
                Debug.Log("box called to right");
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(j).GetComponent<Collider>().enabled = true;
                }
                this.transform.parent = rightHand;
                this.transform.localPosition = offsetList[1];
                this.transform.localRotation = Quaternion.identity;
            }
        }
        else return;
    }

    public void HideToolbox(InputAction.CallbackContext summonInput)
    {
        isOpen= false;
        isVisible = false;
     
        if (summonInput.ReadValue<float>()==0&&this != null)
        {
            for(int j = 0; j<transform.childCount; j++)
            {
                transform.GetChild(j).GetComponent<Renderer>().enabled = false;
                transform.GetChild(j).GetComponent<Collider>().enabled = false;
            }
        }
        else return;
    }

    public void ReturnSummonToolbox()
    {
        isVisible = true;
        if (receivedHandID == 1)
        {
            Debug.Log("box called to left");
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                transform.GetChild(j).GetComponent<Collider>().enabled = true;
            }
            this.transform.parent = leftHand;
            this.transform.localPosition = offsetList[0];
            this.transform.localRotation = Quaternion.identity;
        }
        else if (receivedHandID == 2)
        {
            Debug.Log("box called to right");
            for (int j = 0; j < transform.childCount; j++)
            {
                transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                transform.GetChild(j).GetComponent<Collider>().enabled = true;
            }
            this.transform.parent = rightHand;
            this.transform.localPosition = offsetList[1];
            this.transform.localRotation = Quaternion.identity;
        }

       
    }


   public static void RemoveToyPrefabInGameManager()
    {

        //if (SceneManager.GetActiveScene().name == "GameLevelMain")
        
        {
            for (int i = 0; i < GameManager.instance.keyItem.Count; i++)
            {
                Debug.Log("item ID is : "+GameManager.instance.keyItem[i].GetComponent<KeyItemReporter>().itemID);
                if (itemIDList.Contains(GameManager.instance.keyItem[i].GetComponent<KeyItemReporter>().itemID))
                {
                    GameManager.instance.keyItem.RemoveAt(i);
                    Debug.Log("removable key item found");
                }
       
            }
        }
    }

    public static IEnumerator ResetKeyItemList()
    {
        GameManager.instance.keyItem.Clear();
        yield return null;
        for(int i = 0; i<keyItemsSave.Count; i++)
        {
            GameManager.instance.keyItem.Add(keyItemsSave[i]);

        }

    }

    public static void StoreKeyItemList()
    {
        for(int i = 0;i<GameManager.instance.keyItem.Count;i++)
        {
            keyItemsSave.Add(GameManager.instance.keyItem[i]);

        }
    }

    public void GetHandIDLeft(InputAction.CallbackContext context) { receivedHandID = 1; }
    public void GetHandIDRight(InputAction.CallbackContext context) { receivedHandID = 2; }
}
