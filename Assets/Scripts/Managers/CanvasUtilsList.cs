using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasUtilsList : MonoBehaviour
{
    public TextMeshProUGUI text;

    [Header("Modes")]
    public bool isBaby;
    public Color toggleGray;
    public Image leftImg, rightImg;
    public Sprite babyModeMap, regularMap;
    public GameObject babyWalls;
    public SpriteRenderer minimapPic;


    private void Start()
    {
        isBaby = true;
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LabyrinthGameScene")
        {
            text.text = "Time : "+GameManager.instance.gameTimeLimit.ToString();
        }
            
    }
    public void SelectEnglish()
    {
        DialogueManager.instance.language = DialogueManager.LangSelect.EN;
    }

    public void SelectMandarin()
    {
        DialogueManager.instance.language = DialogueManager.LangSelect.ZH;
    }

    public void ManualRestartGame()
    {
        if (GameManager.readyToReboot)
        {
            GameManager.readyToReboot = false;
            GameManager.instance.SpawnPlayerInRoom();
        }
        else return;
    }

    public void ModeToggle()
    {
        if(isBaby)
        {
            AsianMode();
            isBaby = false;
        }
        else
        {
            BabyMode();
            isBaby=true;
        }
    }
    public void BabyMode()
    {
        
            babyWalls.SetActive(true);
            minimapPic.sprite = babyModeMap;
            leftImg.color = Color.black;
            rightImg.color = toggleGray;
    }

    public void AsianMode()
    {
        babyWalls.SetActive(false);
        minimapPic.sprite = regularMap;
        rightImg.color = Color.black;
        leftImg.color = toggleGray;
    }

}
