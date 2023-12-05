using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxVacancyChecker : MonoBehaviour
{
    public bool isOccupied;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<KeyItemReporter>(out _))
        {
            isOccupied = true;
            ToolboxManager.boxToyList.Add(other.gameObject);

        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<KeyItemReporter>(out _))
        {
            isOccupied = false;
            ToolboxManager.boxToyList.Remove(other.gameObject);

        }
    }
}

