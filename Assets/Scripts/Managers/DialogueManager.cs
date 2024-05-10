using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public enum LangSelect { EN, ZH }
    public LangSelect language;
    public TMP_FontAsset engFontAsset;
    public TMP_FontAsset mandFontAsset;

    [Header("Story")]
    public static Transform player;
    public bool story1;
    public bool story2;
    public bool story3;
    public LanguageScripts[] storyLines;

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

        if(story1)
        {
            StartCoroutine(StartSecondStory());
        }
        if(story1&&story2)
        {
            StartCoroutine(StartThirdStory());
        }
        else
        {
            StartCoroutine(StartFirstStory());
        }
    }

    IEnumerator StartFirstStory()
    {
        Debug.Log("watching story 1");
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("Story1Seen", 1);
        story1= true;
    }

   IEnumerator StartSecondStory()
    {
        Debug.Log("watching story 2");
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("Story2Seen", 1);
        story2 = true;
    }

    IEnumerator StartThirdStory()
    {
        Debug.Log("watching story 3");
        yield return new WaitForSeconds(1);
        PlayerPrefs.SetInt("Story3Seen", 1);
        story3 = true;
    }
}
