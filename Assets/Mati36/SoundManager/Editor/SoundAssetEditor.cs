using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mati36.Sound;

namespace Mati36.SoundEditor
{
    [CustomEditor(typeof(SoundAsset))]
    public class SoundAssetEditor : Editor
    {

        int catIndex;
        SerializedProperty clipProperty, volProperty, catProperty;

        private void OnEnable()
        {
            clipProperty = serializedObject.FindProperty("clip");
            volProperty = serializedObject.FindProperty("vol");
            catProperty = serializedObject.FindProperty("category");
            catIndex = catProperty.intValue;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            if (catIndex >= SoundConfig.Current.categoryNames.Length)
            { catIndex = 0; SetCategory(catIndex); }
            catIndex = EditorGUILayout.Popup("Category", catIndex, SoundConfig.Current.categoryNames);
            if (EditorGUI.EndChangeCheck())
                SetCategory(catIndex);
            EditorGUILayout.PropertyField(clipProperty);
            EditorGUILayout.PropertyField(volProperty);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void SetCategory(int index)
        {
            catProperty.intValue = index;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}