using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxVacancyChecker : MonoBehaviour
{
    public bool isOccupied;
    public string occupantName;

    private void OnTriggerStay(Collider other)
    {
      
        if (other.TryGetComponent<ToyToolboxInteractionManager>(out _))
        {
           
                isOccupied = true;
                occupantName= other.name;
                
           
        }


   
       
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.TryGetComponent<ToyToolboxInteractionManager>(out _))
        {
              isOccupied = false;
              occupantName = null;
              
        }


    }
}

