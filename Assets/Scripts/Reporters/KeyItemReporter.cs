using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemReporter : MonoBehaviour
{
    public static KeyItemReporter instance;
    public int itemID;
    public static bool itemFound;

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftHand") && !ToolboxManager.itemIDList.Contains(itemID) ||
                other.CompareTag("RightHand") && !ToolboxManager.itemIDList.Contains(itemID) ||
                other.CompareTag("ToySpawn") && !ToolboxManager.itemIDList.Contains(itemID)||
                other.CompareTag("ToyBox") && !ToolboxManager.itemIDList.Contains(itemID) )
        {
            ToolboxManager.itemIDList.Add(itemID);
            itemFound = true;
            ToolboxManager.RemoveToyPrefabInGameManager();
        }
    }*/
    private void OnTriggerStay(Collider other)
    {
        //if(SceneManager.GetActiveScene().name == "GameLevelMain")
        {
            if (other.CompareTag("LeftHand") && !ToolboxManager.itemIDList.Contains(itemID)|| 
                other.CompareTag("RightHand") && !ToolboxManager.itemIDList.Contains(itemID)||
                other.CompareTag("ToySpawn") && !ToolboxManager.itemIDList.Contains(itemID)||
                other.CompareTag("ToyBox") && !ToolboxManager.itemIDList.Contains(itemID) )
            {
                ToolboxManager.itemIDList.Add(itemID);
                itemFound = true;
                //ToolboxManager.RemoveToyPrefabInGameManager();
            }
        }

    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            if(this.GetComponent<Rigidbody>()!=null)
            {
                this.GetComponent<Rigidbody>().isKinematic = false;

            }
            
        }
    }*/

}
