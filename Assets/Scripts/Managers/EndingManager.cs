using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static DialogueManager;

public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;
    public LanguageScripts linesToUse;
    public LanguageScripts[] storyLines;
    public GameObject fadeToWhite, creditsPanel, 
                      splat, girlSplat, flowerBed,
                      flowerRain, dustCloud, nextIndicator, qqNextIndicator;
    public Transform splatPoint, kekSpawnPoint, kekMovePoint;
    public Camera cam; public Color bgColor;
    public bool gameFinished, isPlayingEnd, changeMapMat;

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
            nextIndicator = kekActive.GetChild(3).Find("NextIndicator").gameObject;
            nextIndicator.SetActive(false);
            StartCoroutine(GoodEndCoroutine(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>(), qq.GetChild(1).Find("Dialog").GetComponent<TextMeshPro>()));

        }
       
    }

    IEnumerator GoodEndCoroutine(TextMeshPro kekLine, TextMeshPro girlLine)
    {
        while(!nextLine)
        {
            kekLine.text = linesToUse.lines[0];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine= false;
        while(!nextLine)
        {
            kekLine.text = linesToUse.lines[1];
            kekActive.LookAt(qq.position);
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine= false;
        while(!nextLine)
        {
            girlLine.text = linesToUse.lines[2];
            kekLine.text = "";
            nextIndicator.SetActive(false);
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while(!nextLine)
        {
            kekLine.text = linesToUse.lines[3];
            girlLine.text = "";
            nextIndicator.SetActive(true);
            qqNextIndicator.SetActive(false);
            yield return null;
        }
        nextLine = false;
        while(!nextLine)
        {
            kekLine.text = linesToUse.lines[4];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        StartCoroutine(MoveKek(kekMovePoint.position));
        kekActive.LookAt(cam.transform.position);
        minigameHeart.SetActive(true);
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[5];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        isMinigame = true;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[6];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        kekLine.text = "";
        AudioManager.instance.HeartMiniStartMusic();
       
        while (isMinigame)
        {
            nextIndicator.SetActive(false);
            qqNextIndicator.SetActive(false);
            if(!HeartPunching.stopTime)
            {
                minigameProgress += Time.deltaTime;
            }
            
            if(minigameProgress>1&&minigameProgress<3)
            {
                girlLine.text = linesToUse.lines[7];
            }
            else if(minigameProgress>3&&minigameProgress<5)
            {
                kekLine.text = linesToUse.lines[8];
                girlLine.text = "";
            }
            else if(minigameProgress>5&&minigameProgress<7)
            {
                girlLine.text = linesToUse.lines[9];
                kekLine.text = "";
            }
            else if(minigameProgress>7&&minigameProgress<9)
            {
                kekLine.text = linesToUse.lines[10];
                girlLine.text = "";
            }
            else if(minigameProgress>9&&minigameProgress<11)
            {
                kekLine.text = linesToUse.lines[11];
            }
            else if (minigameProgress > 11 && minigameProgress < 13)
            {
                girlLine.text = linesToUse.lines[12];
                kekLine.text = "";
            }
            else if (minigameProgress > 13)
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
        StartCoroutine(MoveGirl(flowerBed.transform.position + new Vector3(0, 1, 0)));
        AudioManager.instance.GoodEndMusic();
        while(!nextLine)
        {
            girlLine.text = linesToUse.lines[14];
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while(!nextLine)
        {
            girlLine.text = linesToUse.lines[15];
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine= false;
        girlLine.text = "";
        qqNextIndicator.SetActive(false);
        creditsPanel.SetActive(true);
        yield return new WaitForSeconds(70);
        StartCoroutine(ReturnToStart());
    }

        public void BadEnd()
    {
        if(kekActive==null)
        {
            kekActive = Instantiate(kek, kekSpawnPoint.position, Quaternion.identity).transform;
            kekActive.LookAt(cam.transform);
            kekAnim = kekActive.GetComponentInChildren<Animator>();
            nextIndicator = kekActive.GetChild(3).Find("NextIndicator").gameObject;
            nextIndicator.SetActive(false);
            StartCoroutine(BadEndCoroutine(kekActive.GetChild(3).Find("KekDialog").GetComponent<TextMeshPro>(), qq.GetChild(1).Find("Dialog").GetComponent<TextMeshPro>()));

        }
       
       
    }

    IEnumerator BadEndCoroutine(TextMeshPro kekLine, TextMeshPro girlLine)
    {
        while(!nextLine)
        {
            kekLine.text = linesToUse.lines[16];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine= false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[17];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            girlLine.text = linesToUse.lines[18];
            kekLine.text = "";
            nextIndicator.SetActive(false);
            qqNextIndicator.SetActive(true);
            kekActive.LookAt(qq.transform);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            girlLine.text = linesToUse.lines[19];
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            girlLine.text = linesToUse.lines[20];
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[21];
            girlLine.text = "";
            nextIndicator.SetActive(true);
            qqNextIndicator.SetActive(false);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            girlLine.text = linesToUse.lines[22];
            kekLine.text = "";
            nextIndicator.SetActive(false);
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            girlLine.text = linesToUse.lines[23];
            qqNextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[24];
            girlLine.text = "";
            nextIndicator.SetActive(true);
            qqNextIndicator.SetActive(false);
            kekActive.LookAt(cam.transform);
            yield return null;
        }
        nextLine = false;
        StartCoroutine(MoveKek(splatPoint.position));
        kekActive.LookAt(qq.position);
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[25];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[26];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[27];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        while (!nextLine)
        {
            kekLine.text = linesToUse.lines[28];
            nextIndicator.SetActive(true);
            yield return null;
        }
        nextLine = false;
        gameFinished = true;
        while (!gameFinished)
        {
            yield return null;
        }
        Destroy(kekActive.gameObject);
        splat.SetActive(true);
        yield return new WaitForSeconds(3);
        Destroy(qq.gameObject);
        girlSplat.SetActive(true);
        creditsPanel.SetActive(true);
        yield return new WaitForSeconds(70);
        StartCoroutine(ReturnToStart());
    }

    IEnumerator ExpandNewMat()
    {
        Destroy(kekActive.gameObject);
        Destroy(minigameHeart);
        while (fadeToWhite.transform.localScale.x<30)
        {
            fadeToWhite.transform.localScale += new Vector3(.1f, .1f, .1f);

            if(fadeToWhite.transform.localScale.x>15)
            {
                changeMapMat = true;
                cam.backgroundColor = bgColor;
                flowerBed.SetActive(true);
                dustCloud.SetActive(false);
                flowerRain.SetActive(true);
            }
            yield return null;
        }
        gameFinished = true;
        
    }

    IEnumerator MergeCoroutine()
    {
        while(kekActive.position!=mergePoint.position&&qq.position!=mergePoint.position)
        {
            Vector3 kekDir = mergePoint.position-kekActive.position;
            Vector3 girlDir = mergePoint.position-qq.position;

            kekActive.position += 0.1f*kekDir;
            qq.position += 0.1f*girlDir;
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
            kekActive.position += 0.05f*direction;
        }
        yield return null;
    }

    IEnumerator MoveGirl(Vector3 targetPos)
    {
        while (qq.position != targetPos)
        {
            Vector3 direction = targetPos - qq.position;
            qq.position += 0.02f * direction;
        }
        yield return null;
    }
    IEnumerator ReturnToStart()
    {
        yield return null;
        changeMapMat = false;
        
        GameManager.readyToReboot = false;
        GameManager.instance.SpawnPlayerInRoom();

        AudioManager.instance.audioSource.clip = AudioManager.instance.BGMAudioClips[11];
        AudioManager.instance.audioSource.Play();
    }

    public void GoToNextLine(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 1)
        {
            if (!holding)
            {
                nextLine = true;
                holding = true;
            }
            else
            {
                nextLine = false;
            }

        }

    }

    public void ReleaseButton(InputAction.CallbackContext obj)
    {
        if (obj.ReadValue<float>() == 0)
        {
            holding = false;
        }

    }
}
