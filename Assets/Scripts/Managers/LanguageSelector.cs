using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
