using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemReporter : MonoBehaviour
{
    public int itemID;
    public ToyToolboxInteractionManager toolboxhelper;

    private void Start()
    {
        toolboxhelper = GetComponent<ToyToolboxInteractionManager>();
    }
    private void Update()
    {
        if (toolboxhelper.isInHand)
        {
            if(itemID==0&&!TutorialsManager.cicadaTut)
            {
                CheckTutorial();
            }
            if (itemID ==1 && !TutorialsManager.gunTut)
            {
                CheckTutorial();
            }
            if (itemID ==2 && !TutorialsManager.planeTut)
            {
                CheckTutorial();
            }
            if (itemID == 3 && !TutorialsManager.topTut)
            {
                CheckTutorial();
            }

        }
        else return;
    }
    void SendTutorialSignal()
    {
        if (itemID == 0)
        {
            PlayerPrefs.SetInt("Cicada", 1);
            PlayerPrefs.SetInt("TutID", itemID);
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 1)
        {
            PlayerPrefs.SetInt("Gun", 1);
            PlayerPrefs.SetInt("TutID", itemID);
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 2)
        {
            PlayerPrefs.SetInt("Plane", 1);
            PlayerPrefs.SetInt("TutID", itemID);
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 3)
        {
            PlayerPrefs.SetInt("Top", 1);
            PlayerPrefs.SetInt("TutID", itemID);
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }

    }

    void CheckTutorial()
    {

        if (itemID == 0 && PlayerPrefs.HasKey("Cicada"))
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 1 && PlayerPrefs.HasKey("Gun"))
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 2 && PlayerPrefs.HasKey("Plane"))
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 3 && PlayerPrefs.HasKey("Top"))
        {
            return;
        }
        else SendTutorialSignal();

    }

  
}
