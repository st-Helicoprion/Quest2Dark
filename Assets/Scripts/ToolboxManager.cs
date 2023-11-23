using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ToolboxManager : MonoBehaviour
{
    public Transform mainCamera, toolboxParent;
    public Animator anim;
    public static List<int> itemIDList = new List<int>();
    public static List<GameObject> keyItemsSave = new List<GameObject>();

    public InputActionReference summonToolboxInputLeft;
    public InputActionReference summonToolboxInputRight;

    private void OnEnable()
    {
        summonToolboxInputLeft.action.performed += SummonToolbox;
        summonToolboxInputLeft.action.canceled += HideToolbox;

        summonToolboxInputRight.action.performed += SummonToolbox;
        summonToolboxInputRight.action.canceled += HideToolbox;
    }
    // Start is called before the first frame update
    void Start()
    {
        ClearItemIDList();
        toolboxParent = this.transform.parent;
   
    }

    // Update is called once per frame
    void Update()
    {
        toolboxParent.localRotation = new Quaternion(0, mainCamera.localRotation.y, 0, mainCamera.localRotation.w);
        
    }

    public void ClearItemIDList()
    {
        itemIDList.Clear();
    }

    void SummonToolbox(InputAction.CallbackContext summonInput)
    {
        if (summonInput.ReadValue<float>() == 1&&this!=null)
        {
            this.transform.gameObject.SetActive(true);

        }
        else return;
    }

    void HideToolbox(InputAction.CallbackContext summonInput)
    {
        if (summonInput.ReadValue<float>() == 0&&this != null)
        {
            this.transform.gameObject.SetActive(false);

        }
        else return;
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
        yield return null;

    }

    public static void StoreKeyItemList()
    {
        for(int i = 0;i<GameManager.instance.keyItem.Count;i++)
        {
            keyItemsSave.Add(GameManager.instance.keyItem[i]);

        }
    }

   


}
