using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionUImanager : MonoBehaviour
{
    public Renderer[] elements;
    public Transform player;
    public ToyToolboxInteractionManager interactionManager;
    public bool released;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Main Camera").transform;
        

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);

        if(!interactionManager.isInHand)
        {
            if(NewToolboxManager.isOpen)
            {
                elements[0].enabled = true;

                if (interactionManager.isInBox||!released)
                {
                    elements[1].enabled = false;
                }

                else { elements[1].enabled = true; }
            }
            else
            {
                elements[0].enabled = false;
                elements[1].enabled = false;
               

            }


        }
        else
        {
            released = false;
            elements[0].enabled = false;
            elements[1].enabled = false;
        
        }

        if (released)
        {
           
            if(interactionManager.isInBox)
            {
                if(NewToolboxManager.isOpen)
                {
                    elements[0].enabled = true;
                    elements[1].enabled = false;
                   
                }
                else
                {
                    elements[0].enabled = false;
                    elements[1].enabled = false;
                    
                }
            }
            else if(interactionManager.isInHand)
            {
                elements[0].enabled = false;
                elements[1].enabled = false;
            }
            else
            {
                elements[0].enabled = true;
                elements[1].enabled = true;
              
            }
        }

        if(!PlayerPrefs.HasKey("IntroDone"))
        {
            elements[0].enabled = false;
            elements[1].enabled = false;
        }
      
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name=="LogoToyBox")
        {
            elements[0].enabled = false;
            elements[1].enabled = false;
           

            released = false;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "LogoToyBox")
        {
            released = true;
        }
    }
}
