using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static DialogueManager;
using static Unity.VisualScripting.Icons;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;
    public LanguageScripts linesToUse;
    public LanguageScripts[] storyLines;
    public GameObject fadeToWhite, creditsPanel, 
                      splat, girlSplat, flowerBed,
                      flowerRain, dustCloud;
    public Transform splatPoint, kekSpawnPoint, kekMovePoint;
    public Camera cam; public Color bgColor;
    public bool gameFinished, isPlayingEnd;
    public LineRenderer snipeLine;

    [Header("Minigame")]
    public bool isMinigame;
    public float minigameProgress;
    public GameObject minigameHeart;
    public GameObject kek;
    public Transform kekActive;
    public Transform qq;
    public Transform mergePoint;
    public Animator kekAnim;
    public Animator girlAnim;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    { 
        //girlAnim = qq.GetComponentInChildren<Animator>();

        AudioManager.instance.EndMapMusic();
    }

    private void Update()
    {
        if(EndRoomReporter.startTheEnd)
        {
            isPlayingEnd= true;
            EndRoomReporter.startTheEnd= false;

            if(GameManager.instance.gameForceEnd)
            {
                BadEnd();
            }
            else
            {
                GoodEnd();
            }
        }

        CheckLanguage();
    }
    public void GoodEnd()
    {
        if (kekActive == null)
        {
            kekActive = Instantiate(kek, kekSpawnPoint.position, Quaternion.identity).transform;
            kekActive.LookAt(cam.transform);
            kekAnim = kekActive.GetComponentInChildren<Animator>();
            StartCoroutine(GoodEndCoroutine(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>(), qq.GetChild(3).Find("Dialog").GetComponent<TextMeshPro>()));

        }
       
    }

    IEnumerator GoodEndCoroutine(TextMeshPro kekLine, TextMeshPro girlLine)
    {
        kekLine.text = linesToUse.lines[0];
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[1];
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[2];
        kekLine.text = "";
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[3];
        girlLine.text = "";
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[4];
        StartCoroutine(MoveKek(kekMovePoint.position));
        kekActive.LookAt(qq.position);
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[5];
        minigameHeart.SetActive(true);
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[6];
        isMinigame = true;
        yield return new WaitForSeconds(3);
        kekLine.text = "";
        AudioManager.instance.HeartMiniStartMusic();
        while (isMinigame)
        {
            if(!HeartPunching.stopTime)
            {
                minigameProgress += Time.deltaTime;
            }
            
            if(minigameProgress>3&&minigameProgress<6)
            {
                girlLine.text = linesToUse.lines[7];
            }
            else if(minigameProgress>6&&minigameProgress<9)
            {
                kekLine.text = linesToUse.lines[8];
                girlLine.text = "";
            }
            else if(minigameProgress>9&&minigameProgress<12)
            {
                girlLine.text = linesToUse.lines[9];
                kekLine.text = "";
            }
            else if(minigameProgress>12&&minigameProgress<15)
            {
                kekLine.text = linesToUse.lines[10];
                girlLine.text = "";
            }
            else if(minigameProgress>15&&minigameProgress<18)
            {
                kekLine.text = linesToUse.lines[11];
            }
            else if (minigameProgress > 18 && minigameProgress < 21)
            {
                girlLine.text = linesToUse.lines[12];
                kekLine.text = "";
            }
            else if (minigameProgress > 21)
            {
                kekLine.text = linesToUse.lines[13];
                girlLine.text = "";
                yield return new WaitForSeconds(3);
                isMinigame = false;
            }

            if(!AudioManager.instance.audioSource.isPlaying)
            {
                AudioManager.instance.HeartMiniLoopMusic();
            }
            yield return null;
        }
        
        kekLine.text = "";
        StartCoroutine(MergeCoroutine());
        
        while (!gameFinished)
        {
            yield return null;
        }
        girlLine.text = linesToUse.lines[14];
        flowerBed.SetActive(true);
        AudioManager.instance.GoodEndMusic();
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[15];
        yield return new WaitForSeconds(3);
        Destroy(qq.gameObject);
        creditsPanel.SetActive(true);
    }

        public void BadEnd()
    {
        if(kekActive==null)
        {
            kekActive = Instantiate(kek, kekSpawnPoint.position, Quaternion.identity).transform;
            kekActive.LookAt(cam.transform);
            kekAnim = kekActive.GetComponentInChildren<Animator>();
            StartCoroutine(BadEndCoroutine(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>(), qq.GetChild(3).Find("Dialog").GetComponent<TextMeshPro>()));

        }
       
       
    }

    IEnumerator BadEndCoroutine(TextMeshPro kekLine, TextMeshPro girlLine)
    {
        kekLine.text = linesToUse.lines[16];
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[17];
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[18];
        kekLine.text = "";
        kekActive.LookAt(qq.transform);
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[19];
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[20];
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[21];
        girlLine.text = "";
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[22];
        kekLine.text = "";
        yield return new WaitForSeconds(3);
        girlLine.text = linesToUse.lines[23];
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[24];
        girlLine.text = "";
        kekActive.LookAt(cam.transform);
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[25];
        StartCoroutine(MoveKek(splatPoint.position));
        kekActive.LookAt(qq.position);
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[26];
        yield return new WaitForSeconds(3);
        kekLine.text = linesToUse.lines[27];
        StartCoroutine(ShootSnipeLine());
        while (!gameFinished)
        {
            yield return null;
        }
        Destroy(kekActive.gameObject);
        splat.SetActive(true);
        yield return new WaitForSeconds(3);
        girlSplat.SetActive(true);
        Destroy(qq.gameObject);
        yield return new WaitForSeconds(3);
        creditsPanel.SetActive(true);
    }

    IEnumerator ExpandNewMat()
    {
        cam.backgroundColor = bgColor;
        Destroy(kekActive.gameObject);
        while (fadeToWhite.transform.localScale.x<50)
        {
            fadeToWhite.transform.localScale += new Vector3(.5f, .5f, .5f);
            yield return null;
        }
        gameFinished = true;
    }

    IEnumerator ShootSnipeLine()
    {
        while(snipeLine.GetPosition(1)!=splatPoint.position)
        {
            Vector3 trackingPos = splatPoint.position-snipeLine.GetPosition(1);
            snipeLine.SetPosition(1, trackingPos);
            yield return new WaitForSeconds(0.05f);
        }
        gameFinished = true;
        snipeLine.gameObject.SetActive(false);
    }

    IEnumerator MergeCoroutine()
    {
        while(kekActive.position!=mergePoint.position&&qq.position!=mergePoint.position)
        {
            Vector3 kekDir = mergePoint.position-kekActive.position;
            Vector3 girlDir = mergePoint.position-qq.position;

            kekActive.position += kekDir;
            qq.position += girlDir;
            yield return null;
        }
        StartCoroutine(ExpandNewMat());
    }
    public void CheckLanguage()
    {
        if (DialogueManager.instance.language == LangSelect.EN)
        {
            linesToUse = storyLines[0];
        }
        else if (DialogueManager.instance.language == LangSelect.ZH)
        {
            linesToUse = storyLines[1];
        }
    }

    IEnumerator MoveKek(Vector3 targetPos)
    {
        while(kekActive.position!=targetPos)
        {
            Vector3 direction = targetPos- kekActive.position;
            kekActive.position += direction;
        }
        yield return null;
    }
}
