using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LanguageSelector : MonoBehaviour
{
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
            ClearSaveBools();
            GameManager.readyToReboot = false;
        }
        else return;
    }

    public void ClearSaveBools()
    {
        TutorialsManager.waitForTutEnd = false;
        GameEndReporter.tutorialDone = false;
        TutorialsManager.cicadaTut = false;
        TutorialsManager.topTut = false;
        TutorialsManager.planeTut= false;
        TutorialsManager.gunTut= false;
    }
}
