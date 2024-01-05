using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxVacancyChecker : MonoBehaviour
{
    public bool isOccupied;
    public string occupantName;

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<KeyItemReporter>(out _))
        {
            isOccupied = true;
            occupantName= other.name;
            ToolboxManager.boxToyList.Add(other.gameObject);

        }

   
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<KeyItemReporter>(out _))
        {
            isOccupied = false;
            occupantName = null;
            ToolboxManager.boxToyList.Remove(other.gameObject);

        }

     
    }
}

