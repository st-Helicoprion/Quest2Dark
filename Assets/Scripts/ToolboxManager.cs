using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ToolboxManager : MonoBehaviour
{
    public Transform mainCamera, toolbox, toolboxParent;
    public Animator anim;
    public static List<int> itemIDList = new List<int>();
    public static List<GameObject> keyItemsSave = new List<GameObject>();
 

    // Start is called before the first frame update
    void Start()
    {
        ClearItemIDList();
        toolboxParent = this.transform.parent;
        toolbox = this.transform;
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
