using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCycleManager : MonoBehaviour
{
    public static List<int> itemIDList = new List<int>();
    public static List<GameObject> keyItemsSave = new List<GameObject>(); //stored list of equipments, refreshed every load

    // Start is called before the first frame update
    void Start()
    {
        ClearItemIDList();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearItemIDList()
    {
        itemIDList.Clear();
      
    }

    public static void RemoveToyPrefabInGameManager(int itemID)
    {
        {
            GameManager.instance.keyItem.RemoveAt(itemID);
        }
    }

    public static IEnumerator ResetKeyItemList()
    {
        GameManager.instance.keyItem.Clear();
        yield return null;
        for (int i = 0; i < keyItemsSave.Count; i++)
        {
            GameManager.instance.keyItem.Add(keyItemsSave[i]);
           
        }

    }

    public static void StoreKeyItemList()
    {
        for (int i = 0; i < GameManager.instance.keyItem.Count; i++)
        {
            keyItemsSave.Add(GameManager.instance.keyItem[i]);

        }
    }
}
