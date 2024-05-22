using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(WeakStateMaterialChanger))]
[CanEditMultipleObjects]
public class WeakStateMatEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var matChanger = (WeakStateMaterialChanger)target;

        matChanger.outlined = EditorGUILayout.Toggle("outlined", matChanger.outlined);

        using (var group1 = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(matChanger.outlined)))
        {
            if(group1.visible==true)
            {
                EditorGUI.indentLevel++;
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.outlinedRend)),new GUIContent("outlineRend"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.weakOutlines)), new GUIContent("weakOutlines"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.normalOutlines)), new GUIContent("normalOutlines"));
                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
        }

        matChanger.pTrailed = EditorGUILayout.Toggle("pTrailed", matChanger.pTrailed);

        using (var group2 = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(matChanger.pTrailed)))
        {
            if (group2.visible == true)
            {
                EditorGUI.indentLevel++;
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.pSystem)), new GUIContent("pSystem"));
                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
        }

        matChanger.standard = EditorGUILayout.Toggle("standard", matChanger.standard);

        using (var group3 = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(matChanger.standard)))
        {
            if (group3.visible == true)
            {
                EditorGUI.indentLevel++;
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.rend)), new GUIContent("rend"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.weakStateMat)), new GUIContent("weakStateMat"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(matChanger.normalStateMat)), new GUIContent("normalStateMat"));
                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel--;
            }
        }
    }
}
