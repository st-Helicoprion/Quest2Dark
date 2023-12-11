using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAutoPicker : MonoBehaviour
{
    public int pickedToyID;
    public List<GameObject> equipSpawnPoints = new List<GameObject>();
    public ToolboxVacancyChecker[] vacancyCheckers;
    public bool itemPlaced;

    private void Start()
    {
        vacancyCheckers = FindObjectsOfType<ToolboxVacancyChecker>();
    }

    IEnumerator PickupCoroutine()
    {
        equipSpawnPoints.Clear();
        itemPlaced = false;
        yield return null;
        equipSpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("ToySpawn"));
        Debug.Log("boxes refreshed");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KeyItemReporter keyItem))
        {
            StartCoroutine(PickupCoroutine());
            pickedToyID = keyItem.itemID;

            if (GameManager.instance.playerEquipID.Contains(pickedToyID)||ToolboxManager.itemIDList.Contains(pickedToyID))
            {
                Destroy(other.gameObject);

                for(int j = 0;j<vacancyCheckers.Length;j++)
                {
                    if (!vacancyCheckers[j].isOccupied&&!itemPlaced)
                    {
                        Instantiate(ToolboxManager.keyItemsSave[pickedToyID], vacancyCheckers[j].transform.position, Quaternion.identity);
                        itemPlaced= true;
                        GameManager.BindKeyItemToManagers();
                        Debug.Log("dropped item cleaned");
                    }
                }
            }
            

            
        }
        
    }
}
