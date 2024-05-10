using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageAdaptor : MonoBehaviour
{
    public bool customCont, customImgs;
    public string engLine, mandLine;
    public TextMeshPro textToAdapt;
    public Material engImg, mandImg;
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
    }
}
