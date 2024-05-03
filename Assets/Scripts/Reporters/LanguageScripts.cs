using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LanguageScripts : ScriptableObject
{
    public string language;
    [TextArea]
    public string[] lines;
}
