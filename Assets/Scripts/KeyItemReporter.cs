using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemReporter : MonoBehaviour
{
    public int itemID;
    public static bool itemFound;

    private void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().name == "GameLevelMain")
        {
            if (other.CompareTag("Player") && !ToolboxManager.itemIDList.Contains(itemID))
            {
                ToolboxManager.itemIDList.Add(itemID);
                itemFound = true;
                ToolboxManager.RemoveToyPrefabInGameManager();
            }
        }

        
    }
}
