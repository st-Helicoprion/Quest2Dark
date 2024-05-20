using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItemReporter : MonoBehaviour
{
    public int itemID;
    public ToyToolboxInteractionManager toolboxhelper;
    public static int tutID;

    private void Start()
    {
        toolboxhelper = GetComponent<ToyToolboxInteractionManager>();
    }
    private void Update()
    {
        if (toolboxhelper.isInHand)
        {
            if (itemID == 0 && !TutorialsManager.cicadaTut)
            {
                CheckTutorial();
            }
            else if (itemID == 1 && !TutorialsManager.gunTut)
            {
                CheckTutorial();
            }
            else if (itemID == 2 && !TutorialsManager.planeTut)
            {
                CheckTutorial();
            }
            else if (itemID == 3 && !TutorialsManager.topTut)
            {
                CheckTutorial();
            }
            else return;

        }
        else return;
    }
    void SendTutorialSignal()
    {
       /* if (itemID == 0)
        {

            //PlayerPrefs.SetInt("TutID", itemID);
            tutID = itemID;
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 1)
        {
            
            //PlayerPrefs.SetInt("TutID", itemID);
            tutID=itemID;
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 2)
        {

            //PlayerPrefs.SetInt("TutID", itemID);
            tutID = itemID;
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        }
        else if (itemID == 3)
        {*/
            //PlayerPrefs.SetInt("TutID", itemID);
            tutID = itemID;
            TutorialsManager.givenToy = this.gameObject;
            TutorialsManager.instance.CallOutTutorialType();
        /*}*/

    }

    void CheckTutorial()
    {

       /* if (itemID == 0 && PlayerPrefs.GetInt("Cicada") == 1)
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 1 && PlayerPrefs.GetInt("Gun") == 1)
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 2 && PlayerPrefs.GetInt("Plane") == 1)
        {
            return;
        }
        else SendTutorialSignal();

        if (itemID == 3 && PlayerPrefs.GetInt("Top") == 1)
        {
            return;
        }
        else*/ SendTutorialSignal();

    }

  
}
