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
        }
    }
   
}
