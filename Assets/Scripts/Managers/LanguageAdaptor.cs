using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageAdaptor : MonoBehaviour
{
    public bool customCont, customImgs, customColor;
    [TextArea]
    public string engLine;
    [TextArea]
    public string mandLine;

    public TextMeshPro textToAdapt;
    public Material engImg, mandImg, engBlackImg, mandBlackImg;
    public Renderer imgToAdapt;

    // Update is called once per frame
    void Update()
    {

        if(DialogueManager.instance.language==DialogueManager.LangSelect.EN)
        {
            if(customImgs)
            {
                imgToAdapt.material= engImg;

            }
            else
            {
                textToAdapt.font = DialogueManager.instance.engFontAsset;
                if (customCont)
                {
                    textToAdapt.text = engLine;
                }
            }

        }

        else if(DialogueManager.instance.language==DialogueManager.LangSelect.ZH)
        {
            if (customImgs)
            {
                imgToAdapt.material = mandImg;

            }
            else
            {
                textToAdapt.font = DialogueManager.instance.mandFontAsset;
                if (customCont)
                {
                    textToAdapt.text = mandLine;
                }
            }
        }

        if(EndingManager.instance!=null)
        {
            if (EndingManager.instance.changeMapMat)
            {
                if (customColor)
                {
                    textToAdapt.color = Color.black;
                    if (DialogueManager.instance.language == DialogueManager.LangSelect.EN && customImgs)
                    {
                        imgToAdapt.material = engBlackImg;
                    }
                    else if (DialogueManager.instance.language == DialogueManager.LangSelect.ZH && customImgs)
                    {
                        imgToAdapt.material = mandBlackImg;
                    }
                }
            }
        }
        else return;
    }
}
