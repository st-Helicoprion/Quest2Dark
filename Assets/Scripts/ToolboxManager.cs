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
    public static int receivedHandID;
    public Animator anim;

    public static List<int> itemIDList = new List<int>();
    public static List<GameObject> keyItemsSave = new List<GameObject>();

    public List<Vector3> offsetList = new List<Vector3>();
    public static List<GameObject> boxToyList= new List<GameObject>();

    public bool isVisible, isHidden;
    
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

    void SummonToolbox(InputAction.CallbackContext summonInput)
    {
      
        if (summonInput.ReadValue<float>() == 1&&this!=null)
        {
            isVisible = true;
            isHidden = false;
            if (receivedHandID==1)
            {
                Debug.Log("box called to left");
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                }
                this.transform.parent = leftHand;
                this.transform.localPosition = offsetList[0];
                

            }
            else if(receivedHandID==2)
            {
                Debug.Log("box called to right");
                for (int j = 0; j < transform.childCount; j++)
                {
                    transform.GetChild(j).GetComponent<Renderer>().enabled = true;
                }
                this.transform.parent = rightHand;
                this.transform.localPosition = offsetList[1];

            }

            GameManager.BindKeyItemToManagers();
        }
        else return;
    }

    public void HideToolbox(InputAction.CallbackContext summonInput)
    {
        isVisible = false;
        isHidden = true;
     
        if (summonInput.ReadValue<float>()==0&&this != null)
        {
            for(int j = 0; j<transform.childCount; j++)
            {
                transform.GetChild(j).GetComponent<Renderer>().enabled = false;
            }
        }
        else return;
    }

    public IEnumerator InitialHideToolbox()
    {
        isVisible = true;
        isHidden = false;

        yield return new WaitForSeconds(1);

        isVisible= false;
        isHidden= true;
        for (int j = 0; j < transform.childCount; j++)
        {
            transform.GetChild(j).GetComponent<Renderer>().enabled = false;
        }

        Debug.Log("toolbox concealed");
    }

   public static void RemoveToyPrefabInGameManager()
    {

        if (SceneManager.GetActiveScene().name == "GameLevelMain")
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
