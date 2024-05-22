using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CanvasUtilsList : MonoBehaviour
{
    public TextMeshProUGUI text;
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LabyrinthGameScene")
        {
            text.text = GameManager.instance.gameTimeLimit.ToString();
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
            GameManager.instance.SpawnPlayerInRoom();
            GameManager.readyToReboot = false;
        }
        else return;
    }

}
