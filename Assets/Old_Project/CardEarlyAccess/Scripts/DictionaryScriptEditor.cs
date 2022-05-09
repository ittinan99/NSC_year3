#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardDic))]
public class DictionaryScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (((CardDic)target).modifyValues)
        {
            if (GUILayout.Button("Save changes"))
            {
                ((CardDic)target).DeserializeDictionary();
            }

        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Print Dictionary"))
        {
            ((CardDic)target).PrintDictionary();
        }
    }
}
#endif
