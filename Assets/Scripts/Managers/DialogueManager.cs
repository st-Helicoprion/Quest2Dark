using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public enum LangSelect { EN, ZH }
    public LangSelect language;
    public TMP_FontAsset engFontAsset;
    public TMP_FontAsset mandFontAsset;
    public Transform kekActive; public Animator anim;

    [Header("Story")]
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

            if(PlayerPrefs.HasKey("Story1Seen"))
            {
                story1 = true;
            }
            if (PlayerPrefs.HasKey("Story2Seen"))
            {
                story2 = true;
            }
            if (PlayerPrefs.HasKey("Story3Seen"))
            {
                story3 = true;
            }

        }
    }
   
    public void CheckStoryProgress()
    {
        player = GameObject.Find("Main Camera").transform;
        if (TutorialsManager.kekActive != null)
        {
            kekActive = TutorialsManager.kekActive;
            TextMeshPro dialogueText = kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>();
            anim = kekActive.GetComponentInChildren<Animator>();

            if (story1)
            {
                StartCoroutine(StartSecondStory(dialogueText));
            }
            if (story1 && story2)
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

            TextMeshPro dialogueText = kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>();

            if (story1)
            {
                StartCoroutine(StartSecondStory(dialogueText));
            }
            if (story1 && story2)
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
        Debug.Log("watching story 1");
        isStory = true;
        yield return new WaitForSeconds(1);
        dialogueText.text = storyLines[0].lines[0];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[1];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[2];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[3];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[4];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[5];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[6];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[7];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[8];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[9];
        if(TutorialsManager.spawnPointToUse.giftSpot!=null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[10];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("Story1Seen", 1);
        story1= true;
        isStory = false;
        Destroy(kekActive.gameObject);
        Destroy(TutorialsManager.kekActive.gameObject);
    }

   IEnumerator StartSecondStory(TextMeshPro dialogueText)
    {
        Debug.Log("watching story 2");
        isStory = true;
        yield return new WaitForSeconds(1);
        dialogueText.text = storyLines[0].lines[11];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[12];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[13];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[14];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[15];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[16];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[17];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[18];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[19];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[20];
        if (TutorialsManager.spawnPointToUse.giftSpot != null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[21];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[22];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("Story2Seen", 1);
        story2 = true;
        isStory = false;
        Destroy(kekActive.gameObject);
        Destroy(TutorialsManager.kekActive.gameObject);
    }

    IEnumerator StartThirdStory(TextMeshPro dialogueText)
    {
        Debug.Log("watching story 3");
        isStory = true;
        yield return new WaitForSeconds(1);
        dialogueText.text = storyLines[0].lines[23];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[24];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[25];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[26];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[27];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[28];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[29];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[30];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[31];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[32];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[33];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[34];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[35];
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[36];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[37];
        if (TutorialsManager.spawnPointToUse.giftSpot != null)
        {
            Instantiate(giftBox, TutorialsManager.spawnPointToUse.giftSpot.position, Quaternion.identity);
            StoryItemHider.summonToy = true;
        }
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[38];
        anim.SetTrigger("Greet");
        yield return new WaitForSeconds(3);
        dialogueText.text = storyLines[0].lines[39];
        yield return new WaitForSeconds(3);
        PlayerPrefs.SetInt("Story3Seen", 1);
        story3 = true;
        isStory = false;
        Destroy(kekActive.gameObject);
        Destroy(TutorialsManager.kekActive.gameObject);
    }
}
