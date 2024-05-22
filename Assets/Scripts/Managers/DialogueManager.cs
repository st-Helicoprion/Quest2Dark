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
    public GameObject giftBox;

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
        dialogueText.text = linesToUse.lines[0];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[1];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[2];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[3];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[4];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[5];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[6];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[7];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[8];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[9];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[10];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text= linesToUse.lines[11];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[12];
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
        dialogueText.text = linesToUse.lines[13];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[14];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[15];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[16];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[17];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[18];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[19];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[20];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[21];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[22];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[23];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[24];
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
        dialogueText.text = linesToUse.lines[25];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[26];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[27];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[28];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[29];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[30];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[31];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[32];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[33];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[34];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[35];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[36];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[37];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[38];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[39];
        yield return new WaitForSeconds(5);
        dialogueText.text = linesToUse.lines[40];
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
}
