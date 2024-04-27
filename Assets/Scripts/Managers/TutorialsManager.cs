using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TutorialsManager : MonoBehaviour
{
    public static TutorialsManager instance;

    public static bool waitForTutEnd, intro, isTut;
    public static bool cicadaTut, gunTut, planeTut, topTut;
    public GameObject kek, toyBox, tutSonarPrefab;
    public static GameObject givenToy;
    public static Transform player, kekActive;
    public Vector3 tutSonarPoint;
    public static IntroSpawnReporter spawnPointToUse;
    

    [Header("Usage Tutorials")]
    public bool cicadaGoal;
    public bool gunGoal;
    public bool planeGoal;
    public bool topGoal;

    [Header("Dialogues")]
    public LanguageScripts[] introLines;
    public LanguageScripts[] cicadaTutLines;
    public LanguageScripts[] gunTutLines;
    public LanguageScripts[] planeTutLines;
    public LanguageScripts[] topTutLines;


    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            isTut = false;
            if(PlayerPrefs.HasKey("IntroDone"))
            {
                waitForTutEnd= true;
                
            }
            if (PlayerPrefs.HasKey("Cicada"))
            {
                cicadaTut = true;
            }
            if (PlayerPrefs.HasKey("Gun"))
            {
                gunTut = true;
            }
            if (PlayerPrefs.HasKey("Plane"))
            {
                planeTut = true;
            }
            if (PlayerPrefs.HasKey("Top"))
            {
                topTut = true;
            }
        }


    }
  
    public void CheckStoryStartTutorial()
    {
        player = GameObject.Find("Main Camera").transform;
       
        if (PlayerPrefs.HasKey("IntroDone"))
        {
            return;
        }
        else
        {
            IntroTutorial();
        }
    }
    
    public void IntroTutorial()
    {
        intro = true;
        kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
        kekActive.LookAt(player.position);
        StartCoroutine(IntroDialogue(kekActive.GetComponentInChildren<TextMeshPro>()));
    }

    IEnumerator IntroDialogue(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        if (DialogueManager.instance.language==DialogueManager.LangSelect.EN)
        {
            yield return new WaitForSeconds(1.5f);
            dialogueText.text = introLines[0].lines[0];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[1];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[2];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[3];
        }
        else if (DialogueManager.instance.language == DialogueManager.LangSelect.ZH)
        {

        }

        GameObject givenToyBox = Instantiate(toyBox, givenToy.transform.position, Quaternion.identity);
        if (givenToy.name == "NewCicada(Clone)")
        {
            givenToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
        }
        else
        givenToy.GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
        waitForTutEnd = true;

    }

    IEnumerator CicadaTutorial(TextMeshPro dialogueText)
    {
        cicadaTut = true;
        isTut = true;
        dialogueText.text = "Ah HERO\nI see you have found\nthe CICADA";
        yield return new WaitForSeconds(3);
        dialogueText.text = "Swing the tub around\nthe stick to expand a\n sonar domain\nGive it a try !";
        yield return new WaitForSeconds(3);
        StartCoroutine(CicadaUsageTutorial(dialogueText));
       
    }

    IEnumerator CicadaUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        tutSonarPoint = player.parent.parent.position;
        GameObject activeTutSonar=Instantiate(tutSonarPrefab, tutSonarPoint - new Vector3(0, 39, 0), Quaternion.identity);
        activeTutSonar.transform.GetChild(0).GetComponent<TutorialSonarReporter>().enabled = true;
        while(!cicadaGoal)
        {
            dialogueText.text = "The more you swing\nthe bigger it gets !\nTry expanding up to\nthis white line !";
            yield return null;
        }
        cicadaGoal = false;
        activeTutSonar = Instantiate(tutSonarPrefab, tutSonarPoint - new Vector3(0, 19, 0), Quaternion.identity);
        activeTutSonar.transform.GetChild(0).GetComponent<TutorialSonarReporter>().enabled = true;
        while (!cicadaGoal)
        {
            dialogueText.text = "Sweet !\nNow up to this white line !";
            yield return null;
        }
        cicadaGoal = false;
        activeTutSonar = Instantiate(tutSonarPrefab, tutSonarPoint - new Vector3(0, -4, 0), Quaternion.identity);
        activeTutSonar.transform.GetChild(0).GetComponent<TutorialSonarReporter>().enabled = true;
        while (!cicadaGoal)
        {
            dialogueText.text = "Last one !\nThis is as far\nas it can go !";
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = "A true talent !";
        
        yield return new WaitForSeconds(3);

        StartCoroutine(EndTutorial(dialogueText));
    }
    IEnumerator GunTutorial(TextMeshPro dialogueText)
    {
        gunTut = true;
        isTut  = true;
        dialogueText.text = "Ah HERO\nI see you have found\nthe GUN";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It shoots sonar bullets\nwhen you press the\ntrigger";
        yield return new WaitForSeconds(3);
        StartCoroutine(GunUsageTutorial(dialogueText));
    }

    IEnumerator GunUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        foreach(TargetPracticeReporter targets in spawnPointToUse.tutSpots)
        {
            targets.ShowBox();
        }
        while(!gunGoal)
        {
            dialogueText.text = "Shoot away!\nYou have infinite bullets !";
            yield return null;
        }
        gunGoal = false;
        while(!gunGoal)
        {
            dialogueText.text = "Shots can knock things back!\nTry it again !";
            yield return null;
        }
        gunGoal= false;
        while(!gunGoal)
        {
            dialogueText.text = "Last one !\nThis is so fun !";
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = "A born marksman !";

        yield return new WaitForSeconds(3);

        StartCoroutine(EndTutorial(dialogueText));
    }
    IEnumerator PlaneTutorial(TextMeshPro dialogueText)
    {
        planeTut = true;
        isTut=true;
        dialogueText.text = "Ah HERO\nI see you have found\nthe PLANE";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It makes sonar pulses\nas it flies, just put\nyour hand through the\nring to launch it";
        yield return new WaitForSeconds(6);
        dialogueText.text = "It regenerates after \nyou throw it so\ndon't worry about\nrunning out";
        yield return new WaitForSeconds(3);
        StartCoroutine(PlaneUsageTutorial(dialogueText));
    }

    IEnumerator PlaneUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        foreach (TargetPracticeReporter targets in spawnPointToUse.tutSpots)
        {
            targets.ShowBox();
        }
        while (!planeGoal)
        {
            dialogueText.text = "The PLANE can mark things\nmaking them easier\nto see !";
            yield return null;
        }
        planeGoal = false;
        while (!planeGoal)
        {
            dialogueText.text = "Aim it right and you'll\nlight up the\nwhole town !";
            yield return null;
        }
        planeGoal = false;
        while (!planeGoal)
        {
            dialogueText.text = "If the PLANE won't fly,\ntry pressing the Oculus\nbutton to recenter";
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = "Such smooth flicks !";

        yield return new WaitForSeconds(3);

        StartCoroutine(EndTutorial(dialogueText));
    }

    IEnumerator TopTutorial(TextMeshPro dialogueText)
    {
        topTut = true;
        isTut=true;
        dialogueText.text = "Ah HERO\nI see you have found\nthe TOP";
        yield return new WaitForSeconds(3);
        dialogueText.text = "It expands a sonar domain\naway from you when\nit spins, put your hand\nthrough the ring, and pull\nto jump start it";
        yield return new WaitForSeconds(6);
        StartCoroutine(TopUsageTutorial(dialogueText));
    }

    IEnumerator TopUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        spawnPointToUse.tutSpots[0].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = "Throw it to that white circle !\nWhen you see PULL,\npull your hand back !";
            yield return null;
        }
        topGoal = false;
        spawnPointToUse.tutSpots[1].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = "It returns after you\n throw it, and you can\n also grip to recall it !";
            yield return null;
        }
        topGoal = false;
        spawnPointToUse.tutSpots[2].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = "If the TOP won't launch,\ntry pressing the Oculus\nbutton to recenter";
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = "My eyes won't stop\nspinning !";
        yield return new WaitForSeconds(3);
        dialogueText.text = "The TOP is loud and annoying,\nthe guards will try to stop\nit if they see it";
        yield return new WaitForSeconds(3);

        StartCoroutine(EndTutorial(dialogueText));
    }

    IEnumerator EndTutorial(TextMeshPro dialogueText)
    {
        if (DialogueManager.instance.language == DialogueManager.LangSelect.EN)
        {
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[4];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[5];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[6];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[7];
            yield return new WaitForSeconds(3);
            dialogueText.text = introLines[0].lines[8];
        }

        yield return new WaitForSeconds(3);

        Destroy(kekActive.gameObject);

        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;

        if(isTut)
        {
            isTut = false;
        }
    }
    public void CallOutTutorialType()
    {
        if(kekActive==null)
        {
            kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
            kekActive.LookAt(player.position);
        }
        else
        {
            if (PlayerPrefs.GetInt("TutID") == 0 && cicadaTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(CicadaTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 1 && gunTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(GunTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 2 && planeTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(PlaneTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else if (PlayerPrefs.GetInt("TutID") == 3 && topTut == false)
            {
                kekActive.position = spawnPointToUse.transform.position;
                StartCoroutine(TopTutorial(kekActive.GetComponentInChildren<TextMeshPro>()));
            }
            else return;
        }
       
       
    }
    

}
