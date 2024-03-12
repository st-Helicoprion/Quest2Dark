using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemReporter : MonoBehaviour
{
    public static KeyItemReporter instance;
    public int itemID;
    public static bool itemFound;

  /*  private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") && !ItemCycleManager.itemIDList.Contains(itemID) ||
                other.CompareTag("RightHand") && !ItemCycleManager.itemIDList.Contains(itemID))
        {
            ItemCycleManager.itemIDList.Add(itemID);
            itemFound = true;
            ItemCycleManager.RemoveToyPrefabInGameManager();
        }
    }
 *//*   private void OnTriggerStay(Collider other)
    {
        {
            if (other.CompareTag("LeftHand") && !ItemCycleManager.itemIDList.Contains(itemID) ||
                other.CompareTag("RightHand") && !ItemCycleManager.itemIDList.Contains(itemID) ||
                other.name == "ToySpawn" && !ItemCycleManager.itemIDList.Contains(itemID) ||
                other.name == "PrizeSpawn" && !ItemCycleManager.itemIDList.Contains(itemID))
            {
                ItemCycleManager.itemIDList.Add(itemID);
                itemFound = true;
                ItemCycleManager.RemoveToyPrefabInGameManager();
            }
        }

    }*/



}
