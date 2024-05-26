using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static DialogueManager;

public class TutorialsManager : MonoBehaviour
{
    public static TutorialsManager instance;

    public static bool waitForTutEnd, intro, isTut;
    public static bool cicadaTut, gunTut, planeTut, topTut;
    public GameObject kek, toyBox, tutSonarPrefab, video;
    public static GameObject givenToy;
    public static Transform player, kekActive;
    public Vector3 tutSonarPoint;
    public static IntroSpawnReporter spawnPointToUse;
    public Animator anim;
    public DebugHelper controlsMap;

    [Header("Usage Tutorials")]
    public bool cicadaGoal;
    public bool gunGoal;
    public bool planeGoal;
    public bool topGoal;
    public GameObject cicadaVid;
    public GameObject planeVid;
    public GameObject topVid;
    //public GameObject gunVid;

    [Header("Dialogues")]
    public LanguageScripts linesToUse;
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
            //Hide data saving code for demo
            /*if(PlayerPrefs.GetInt("IntroDone")==1)
            {
                waitForTutEnd= true;
                
            }
            if (PlayerPrefs.GetInt("Cicada") == 1)
            {
                cicadaTut = true;
            }
            if (PlayerPrefs.GetInt("Gun") == 1)
            {
                gunTut = true;
            }
            if (PlayerPrefs.GetInt("Plane") == 1)
            {
                planeTut = true;
            }
            if (PlayerPrefs.GetInt("Top") == 1)
            {
                topTut = true;
            }*/
        }


    }

    public void CheckStoryStartTutorial()
    {
        player = GameObject.Find("Main Camera").transform;

        /*if (PlayerPrefs.GetInt("IntroDone") == 1 ||isTut)
        {
            return;
        }
        else*/
        /* {*/
        if (!isTut)
        {
            IntroTutorial();

        }
        else return;
        /*}*/
    }
    
    public void IntroTutorial()
    {
        intro = true;
        isTut = true;
        kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
        anim = kekActive.GetComponentInChildren<Animator>();
        kekActive.LookAt(player.position);

        AudioManager.instance.KekThemeMusic();

        if(DialogueManager.instance.language==LangSelect.EN)
        {
            linesToUse = introLines[0];
        }
        else if(DialogueManager.instance.language == LangSelect.ZH)
        {
            linesToUse = introLines[1];
        }

        StartCoroutine(IntroDialogue(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>()));
    }

    IEnumerator IntroDialogue(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        yield return new WaitForSeconds(1.5f);
        dialogueText.text = linesToUse.lines[0];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[1];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[2];
        yield return new WaitForSeconds(3);
        //anim.SetTrigger("Poop");
        dialogueText.text = linesToUse.lines[3];
        intro = false;
        GameObject givenToyBox = Instantiate(toyBox, givenToy.transform.position, Quaternion.identity);
        if (givenToy.name == "NewCicada(Clone)"||givenToy.name=="NewGun(Clone)")
        {
            givenToy.transform.GetChild(0).GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
        }
        else
        givenToy.GetComponent<ToyToolboxInteractionManager>().ShowEquipVisuals();
    }

    IEnumerator CicadaTutorial(TextMeshPro dialogueText)
    {
        if(anim!=null)
        {
            cicadaTut = true;
            isTut = true;
            dialogueText.text = linesToUse.lines[0];
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            dialogueText.text = linesToUse.lines[1];
            video = Instantiate(cicadaVid, spawnPointToUse.videoPos.position, Quaternion.identity);
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            StartCoroutine(CicadaUsageTutorial(dialogueText));
        }
       
       
    }

    IEnumerator CicadaUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        PlayerMoveFeedback.moving = false;

        tutSonarPoint = player.parent.parent.position;
        GameObject activeTutSonar=Instantiate(tutSonarPrefab, tutSonarPoint - new Vector3(0, 29, 0), Quaternion.identity);
        activeTutSonar.transform.GetChild(0).GetComponent<TutorialSonarReporter>().enabled = true;
        while(!cicadaGoal)
        {
            dialogueText.text = linesToUse.lines[2];
            yield return null;
        }
        cicadaGoal = false;
        activeTutSonar = Instantiate(tutSonarPrefab, tutSonarPoint - new Vector3(0, 9, 0), Quaternion.identity);
        activeTutSonar.transform.GetChild(0).GetComponent<TutorialSonarReporter>().enabled = true;
        while (!cicadaGoal)
        {
            dialogueText.text = linesToUse.lines[3];
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = linesToUse.lines[4];
        Destroy(video);
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        //PlayerPrefs.SetInt("Cicada", 1);
        waitForTutEnd = true;
        if(!GameEndReporter.tutorialDone)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }
       /* if (PlayerPrefs.GetInt("IntroDone")!=1)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }*/
    }
    IEnumerator GunTutorial(TextMeshPro dialogueText)
    {
        if(anim!=null)
        {
            gunTut = true;
            isTut = true;
            dialogueText.text = linesToUse.lines[0];
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            dialogueText.text = linesToUse.lines[1];
            //video = Instantiate(gunVid, spawnPointToUse.videoPos.position, Quaternion.identity);
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            StartCoroutine(GunUsageTutorial(dialogueText));
        }
        
    }

    IEnumerator GunUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        PlayerMoveFeedback.moving = false;
        foreach (TargetPracticeReporter targets in spawnPointToUse.tutSpots)
        {
            targets.ShowBox();
        }
        while(!gunGoal)
        {
            dialogueText.text = linesToUse.lines[2];
            yield return null;
        }
        gunGoal = false;
        while(!gunGoal)
        {
            dialogueText.text = linesToUse.lines[3];
            yield return null;
        }
        gunGoal= false;
        while(!gunGoal)
        {
            dialogueText.text = linesToUse.lines[4];
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = linesToUse.lines[5];
        Destroy(video);
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
       // PlayerPrefs.SetInt("Gun", 1);
        waitForTutEnd = true;
        if (!GameEndReporter.tutorialDone)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }
        /*if (PlayerPrefs.GetInt("IntroDone")!=1)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }*/
    }
    IEnumerator PlaneTutorial(TextMeshPro dialogueText)
    {
       if(anim!=null)
        {
            planeTut = true;
            isTut = true;
            dialogueText.text = linesToUse.lines[0];
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            dialogueText.text = linesToUse.lines[1];
            video = Instantiate(planeVid, spawnPointToUse.videoPos.position, Quaternion.identity);
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(6);
            dialogueText.text = linesToUse.lines[2];
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(3);
            StartCoroutine(PlaneUsageTutorial(dialogueText));
        }
  
    }

    IEnumerator PlaneUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        PlayerMoveFeedback.moving = false;

        spawnPointToUse.tutSpots[0].ShowBox();
        while (!planeGoal)
        {
            dialogueText.text = linesToUse.lines[3];
            yield return null;
        }
        planeGoal = false;
        spawnPointToUse.tutSpots[1].ShowBox();
        while (!planeGoal)
        {
            dialogueText.text = linesToUse.lines[4];
            yield return null;
        }
        planeGoal = false;
        spawnPointToUse.tutSpots[2].ShowBox();
        while (!planeGoal)
        {
            dialogueText.text = linesToUse.lines[5];
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = linesToUse.lines[6];
        Destroy(video);
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        //PlayerPrefs.SetInt("Plane", 1);
        waitForTutEnd = true;
        if (!GameEndReporter.tutorialDone)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }
        /* if (PlayerPrefs.GetInt("IntroDone") != 1)
         {
             StartCoroutine(EndTutorial(dialogueText));
         }
         else
         {
             StartCoroutine(SendOffText(dialogueText));
         }*/
    }

    IEnumerator TopTutorial(TextMeshPro dialogueText)
    {
        if(anim!=null)
        {
            topTut = true;
            isTut=true;
            dialogueText.text = linesToUse.lines[0];
            yield return new WaitForSeconds(3);
            dialogueText.text = linesToUse.lines[1];
            video = Instantiate(topVid, spawnPointToUse.videoPos.position, Quaternion.identity);
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(6);
            StartCoroutine(TopUsageTutorial(dialogueText));

        }
    }

    IEnumerator TopUsageTutorial(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        PlayerMoveFeedback.moving = false;

        spawnPointToUse.tutSpots[0].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = linesToUse.lines[2];
            yield return null;
        }
        topGoal = false;
        spawnPointToUse.tutSpots[1].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = linesToUse.lines[3];
            yield return null;
        }
        topGoal = false;
        spawnPointToUse.tutSpots[2].ShowSonar();
        while (!topGoal)
        {
            dialogueText.text = linesToUse.lines[4];
            yield return null;
        }
        yield return new WaitForSeconds(1);
        dialogueText.text = linesToUse.lines[5];
        Destroy(video);
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[6];
        yield return new WaitForSeconds(3);
        waitForTutEnd = true;
        //PlayerPrefs.SetInt("Top", 1);
        if (!GameEndReporter.tutorialDone)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }
/*        if (PlayerPrefs.GetInt("IntroDone") != 1)
        {
            StartCoroutine(EndTutorial(dialogueText));
        }
        else
        {
            StartCoroutine(SendOffText(dialogueText));
        }*/
    }

    IEnumerator EndTutorial(TextMeshPro dialogueText)
    {

        if (DialogueManager.instance.language == LangSelect.EN)
        {
            linesToUse = introLines[0];
        }
        else if (DialogueManager.instance.language == LangSelect.ZH)
        {
            linesToUse = introLines[1];
        }

        if(!GameEndReporter.callTower)
        {
            GameEndReporter.callTower = true;
        }
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[4];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[5];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[6];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[7];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[8];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[9];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[10];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[11];
        ActivateControlsUI();
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);

        GameEndReporter.tutorialDone = true;
        //PlayerPrefs.SetInt("IntroDone", 1);
        GameManager.readyToReboot = true;
        Destroy(kekActive.gameObject);

        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;

        if(isTut)
        {
            isTut = false;
            Debug.Log("tutorial ended, spawn countdown begins");
        }
    }

    IEnumerator SendOffText(TextMeshPro dialogueText)
    {
        if (DialogueManager.instance.language == LangSelect.EN)
        {
            linesToUse = introLines[0];
        }
        else if (DialogueManager.instance.language == LangSelect.ZH)
        {
            linesToUse = introLines[1];
        }
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[12];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[13];
        yield return new WaitForSeconds(3);
        dialogueText.text = linesToUse.lines[14];
        yield return new WaitForSeconds(3);
        Destroy(kekActive.gameObject);

        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;

        if (isTut)
        {
            isTut = false;
        }

    }
    public void CallOutTutorialType()
    {
        if(kekActive==null)
        {
            kekActive = Instantiate(kek, spawnPointToUse.transform.position, Quaternion.identity).transform;
            anim = kekActive.GetComponentInChildren<Animator>();
            kekActive.LookAt(player.position);

            AudioManager.instance.KekThemeMusic();
        }
        else
        {

            anim = kekActive.GetComponentInChildren<Animator>();

            if (KeyItemReporter.tutID == 0&&!cicadaTut)
            {
                kekActive.position = spawnPointToUse.transform.position;
                if(DialogueManager.instance.language==LangSelect.EN)
                {
                    linesToUse = cicadaTutLines[0];
                }
                else if(DialogueManager.instance.language==LangSelect.ZH)
                {
                    linesToUse = cicadaTutLines[1];
                }
                StartCoroutine(CicadaTutorial(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>()));

            }
            else if (KeyItemReporter.tutID == 1&&!gunTut)
            {
                kekActive.position = spawnPointToUse.transform.position;
                if (DialogueManager.instance.language == LangSelect.EN)
                {
                    linesToUse = gunTutLines[0];
                }
                else if (DialogueManager.instance.language == LangSelect.ZH)
                {
                    linesToUse = gunTutLines[1];
                }
                StartCoroutine(GunTutorial(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>()));
            }
            else if (KeyItemReporter.tutID == 2&&!planeTut)
            {
                kekActive.position = spawnPointToUse.transform.position;
                if (DialogueManager.instance.language == LangSelect.EN)
                {
                    linesToUse = planeTutLines[0];
                }
                else if (DialogueManager.instance.language == LangSelect.ZH)
                {
                    linesToUse = planeTutLines[1];
                }
                StartCoroutine(PlaneTutorial(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>()));
            }
            else if (KeyItemReporter.tutID == 3&&!topTut)
            {
                kekActive.position = spawnPointToUse.transform.position;
                if (DialogueManager.instance.language == LangSelect.EN)
                {
                    linesToUse = topTutLines[0];
                }
                else if (DialogueManager.instance.language == LangSelect.ZH)
                {
                    linesToUse = topTutLines[1];
                }
                StartCoroutine(TopTutorial(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>()));
            }
            else return;

            /*if (PlayerPrefs.GetInt("TutID") == 0 && cicadaTut == false)
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
            else return;*/
        }
       
       
    }

    public void ActivateControlsUI()
    {
        controlsMap.enabled = true;
        controlsMap.extraObjects[0].SetActive(true);
    }

    public void DeactivateControlsUI()
    {
        controlsMap.extraObjects[0].SetActive(false);
    }


}
