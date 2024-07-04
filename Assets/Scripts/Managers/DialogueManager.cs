using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.XR.Interaction.Toolkit;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public enum LangSelect { EN, ZH }
    public LangSelect language;
    public TMP_FontAsset engFontAsset;
    public TMP_FontAsset mandFontAsset;
    public Transform kekActive; public Animator anim;

    [Header("Story")]
    public LanguageScripts linesToUse;
    public static Transform player;
    public static bool isStory;
    public bool story1;
    public bool story2;
    public bool story3;
    public LanguageScripts[] storyLines;
    public GameObject giftBox, nextIndicator;

    public InputActionReference uiPressLeft, uiPressRight;
    bool holding, nextLine;

    private void OnEnable()
    {
        uiPressLeft.action.performed += GoToNextLine;
        uiPressLeft.action.canceled += ReleaseButton;
        uiPressRight.action.performed += GoToNextLine;
        uiPressRight.action.canceled += ReleaseButton;
    }
    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance= this;
            DontDestroyOnLoad(gameObject);

           /* if(PlayerPrefs.GetInt("Story1Seen") == 1)
            {
                story1 = true;
            }
            if (PlayerPrefs.GetInt("Story2Seen") == 1)
            {
                story2 = true;
            }
            if (PlayerPrefs.GetInt("Story3Seen") == 1)
            {
                story3 = true;
            }*/

        }
    }

    public void CheckSelectedLang()
    {
        if(language==LangSelect.EN)
        {
            linesToUse = storyLines[0];
        }
        else if(language==LangSelect.ZH)
        {
            linesToUse = storyLines[1];
        }
    }
    public void CheckStoryProgress()
    {
        StoryPortalReporter.storyStandby = false;
        player = GameObject.Find("Main Camera").transform;
        CheckSelectedLang();
        if (TutorialsManager.kekActive != null)
        {
            kekActive = TutorialsManager.kekActive;
            TextMeshPro dialogueText = kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>();
            nextIndicator = kekActive.GetChild(3).Find("NextIndicator").gameObject;
            nextIndicator.SetActive(false);
            anim = kekActive.GetComponentInChildren<Animator>();

            if (story1)
            {
                StartCoroutine(StartSecondStory(dialogueText));
            }
            else if (story1 && story2)
            {
                StartCoroutine(StartThirdStory(dialogueText));
            }
            else
            {
                StartCoroutine(StartFirstStory(dialogueText));
            }
        }
        else
        { 
            TutorialsManager.kekActive= Instantiate(TutorialsManager.instance.kek, TutorialsManager.spawnPointToUse.transform.position, Quaternion.identity).transform;
            kekActive = TutorialsManager.kekActive;
            anim = kekActive.GetComponentInChildren<Animator>();
            
            kekActive.LookAt(player.position);

            AudioManager.instance.KekThemeMusic();

            TextMeshPro dialogueText = kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>();
            nextIndicator = kekActive.GetChild(3).Find("NextIndicator").gameObject;
            nextIndicator.SetActive(false);

            if (story1)
            {
                StartCoroutine(StartSecondStory(dialogueText));
            }
            else if (story1 && story2)
            {
                StartCoroutine(StartThirdStory(dialogueText));
            }
            else
            {
                StartCoroutine(StartFirstStory(dialogueText));
            }
        }


    }

    IEnumerator StartFirstStory(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        Debug.Log("watching story 1");
        isStory = true;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Greet");
        nextIndicator.SetActive(true);
        while(!nextLine)
        {
            dialogueText.text = linesToUse.lines[0];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[1];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[2];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[3];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[4];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[5];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[6];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[7];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[8];
            yield return null;
        }
        nextLine= false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[9];
            yield return null;
        }
        nextLine = false;
        anim.SetTrigger("Greet");
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[10];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[11];
            yield return null;
        }
        nextLine = false;
        dialogueText.text = linesToUse.lines[12];
        nextIndicator.SetActive(false);
       /* yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[10];
        anim.SetTrigger("Greet");*/
        if(TutorialsManager.spawnPointToUse.giftSpot!=null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
/*        yield return new WaitForSeconds(5);
        if(!TutorialsManager.isTut)
        {
            Destroy(kekActive.gameObject);
            Destroy(TutorialsManager.kekActive.gameObject);
            player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
            player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;
        }*/
        //PlayerPrefs.SetInt("Story1Seen", 1);
        story1= true;
        isStory = false;
       
    }

   IEnumerator StartSecondStory(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        Debug.Log("watching story 2");
        isStory = true;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Greet");
        nextIndicator.SetActive(true);
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[13];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[14];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[15];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[16];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[17];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[18];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[19];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[20];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[21];
            yield return null;
        }
        nextLine = false;
        anim.SetTrigger("Greet");
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[22];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[23];
            yield return null;
        }
        nextLine = false;
        dialogueText.text = linesToUse.lines[24];
        nextIndicator.SetActive(false);
        if (TutorialsManager.spawnPointToUse.giftSpot != null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
       /* if(!TutorialsManager.isTut)
        {
            yield return new WaitForSeconds(5);
            dialogueText.text = linesToUse.lines[22];
            anim.SetTrigger("Greet");
            yield return new WaitForSeconds(5);
            Destroy(kekActive.gameObject);
            Destroy(TutorialsManager.kekActive.gameObject);
            player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
            player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;
        }*/
        //PlayerPrefs.SetInt("Story2Seen", 1);
        story2 = true;
        isStory = false;
        
    }

    IEnumerator StartThirdStory(TextMeshPro dialogueText)
    {
        player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = false;
        player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 0;
        Debug.Log("watching story 3");
        isStory = true;
        yield return new WaitForSeconds(1);
        anim.SetTrigger("Greet");
        nextIndicator.SetActive(true);
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[25];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[26];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[27];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[28];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[29];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[30];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[31];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[32];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[33];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[34];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[35];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[36];
            yield return null;
        }
        nextLine = false;
        anim.SetTrigger("Greet");
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[37];
            yield return null;
        }
        nextLine = false;
        anim.SetTrigger("Greet");
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[38];
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            dialogueText.text = linesToUse.lines[39];
            yield return null;
        }
        nextLine = false;
        dialogueText.text = linesToUse.lines[40];
        nextIndicator.SetActive(false);
        if (TutorialsManager.spawnPointToUse.giftSpot != null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
      /*  if(!TutorialsManager.isTut)
        {
            yield return new WaitForSeconds(5);
            Destroy(kekActive.gameObject);
            Destroy(TutorialsManager.kekActive.gameObject);
            player.parent.parent.GetComponentInChildren<PlayerMoveFeedback>().enabled = true;
            player.parent.parent.GetComponentInChildren<ContinuousMoveProviderBase>().moveSpeed = 1.5f;
        }*/
        //PlayerPrefs.SetInt("Story3Seen", 1);
        story3 = true;
        isStory = false;
        
    }

    public void GoToNextLine(InputAction.CallbackContext obj)
    {
        if(obj.ReadValue<float>()==1)
        {
            if(!holding)
            {
                nextLine = true;
                holding = true;
            }
            else
            {
                nextLine= false;
            }
           
        }
       
    }

    public void ReleaseButton(InputAction.CallbackContext obj)
    {
        if(obj.ReadValue<float>()==0)
        {
            holding = false;
        }
        
    }
}
