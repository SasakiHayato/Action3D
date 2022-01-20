# if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldManager))]
[CanEditMultipleObjects]
public class FieldManagerEditor : Editor
{
    bool _fieldManageInspector = true;
    bool _fieldManageEditorInspector = false;

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("FieldManager"))
        {
            if (!_fieldManageInspector)
            {
                _fieldManageInspector = true;
                _fieldManageEditorInspector = false;
            }
        }

        if (GUILayout.Button("FieldDataEditor"))
        {
            if (!_fieldManageEditorInspector)
            {
                _fieldManageEditorInspector = true;
                _fieldManageInspector = false;
            }
        }
        EditorGUILayout.EndHorizontal();
        ShowInspector();
    }

    void ShowInspector()
    {
        if (_fieldManageInspector)
        {
            base.OnInspectorGUI();
        }
        else if (_fieldManageEditorInspector)
        {
            OnGUIFieldData();
        }
    }

    void OnGUIFieldData()
    {
        FieldManager fieldManager = (FieldManager)target;
        EditorGUILayout.BeginVertical();
        fieldManager.FieldData.DebugBool = EditorGUILayout.Toggle($"DebugBool", fieldManager.FieldData.DebugBool);
        fieldManager.FieldData.DebugLevel = EditorGUILayout.IntField("DebugLevel", fieldManager.FieldData.DebugLevel);
        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Apply"))
        {
            fieldManager.FieldData.SetDebugLevel = fieldManager.FieldData.DebugLevel;
            Debug.Log("Apply");
        }
    }
}
#endif