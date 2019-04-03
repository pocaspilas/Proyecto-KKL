using Mati36.CustomGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(BuildToolConfig))]
public class BuildToolConfigEditor : Editor
{
    int tabSelected;

    SerializedProperty windowsScenesProp, androidScenesProp, autorunProp, debugProp;

    ReorderableList windowsScenesList, androidScenesList;

    private void OnEnable()
    {
        windowsScenesProp = serializedObject.FindProperty("scenesInBuild_Windows");
        androidScenesProp = serializedObject.FindProperty("scenesInBuild_Android");
        autorunProp = serializedObject.FindProperty("autorun");
        debugProp = serializedObject.FindProperty("developmentMode");

        windowsScenesList = CreateReorderableList(windowsScenesProp);
        androidScenesList = CreateReorderableList(androidScenesProp);
    }

    public override void OnInspectorGUI()
    {
        tabSelected = GUILayout.Toolbar(tabSelected, new string[2] { "Windows", "Android" });
        GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
        if (tabSelected == 0)
            WindowsTab();
        else if (tabSelected == 1)
            AndroidTab();

        GUILayout.Label("Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(autorunProp);
        EditorGUILayout.PropertyField(debugProp);

        HandleEvents();
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void HandleEvents()
    {
        Event e = Event.current;
        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
        if (e.type == EventType.DragPerform)
        {
            foreach (var draggedObj in DragAndDrop.objectReferences)
            {
                if (draggedObj is SceneAsset)
                {
                    if (tabSelected == 0)
                        AddNewSceneTo(draggedObj, windowsScenesProp);
                    else if (tabSelected == 1)
                        AddNewSceneTo(draggedObj, androidScenesProp);
                }
            }
        }
    }

    //
    //LIST MANAGEMENT
    //
    private void AddNewSceneTo(UnityEngine.Object scene, SerializedProperty property)
    {
        property.InsertArrayElementAtIndex(windowsScenesProp.arraySize);
        serializedObject.ApplyModifiedProperties();
        property.GetArrayElementAtIndex(windowsScenesProp.arraySize - 1).objectReferenceValue = scene;
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void RemoveSceneFrom(int sceneIndex, SerializedProperty property)
    {
        if (property.GetArrayElementAtIndex(sceneIndex).objectReferenceValue != null)
            property.DeleteArrayElementAtIndex(sceneIndex);
        property.DeleteArrayElementAtIndex(sceneIndex);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    //
    //GUI
    //
    private void WindowsTab()
    {
        windowsScenesList.DoLayoutList();
    }

    private void AndroidTab()
    {
        androidScenesList.DoLayoutList();
    }

    private ReorderableList CreateReorderableList(SerializedProperty prop)
    {
        ReorderableList reorderableList = new ReorderableList(serializedObject, prop, true, false, true, true);
        
        reorderableList.drawElementCallback +=
            (Rect rect,
         int index,
         bool isActive,
         bool isFocused) =>
            {
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
                    reorderableList.serializedProperty.GetArrayElementAtIndex(index), new GUIContent("Scene " + index)
                    );
            };

        return reorderableList;
    }
}
