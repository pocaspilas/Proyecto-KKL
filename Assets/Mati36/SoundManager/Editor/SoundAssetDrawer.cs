using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mati36.Sound;
using System;
using System.Linq;

namespace Mati36.SoundEditor
{
    [CustomPropertyDrawer(typeof(SoundAsset))]
    public class SoundAssetDrawer : PropertyDrawer
    {
        int catIndex;
        public SerializedProperty serializedProp;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            serializedProp = property;

            if (property.isInstantiatedPrefab && property.prefabOverride)
                EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);
            else
                EditorGUI.PrefixLabel(position, label);
            Rect popupRect = position;
            popupRect.x += EditorGUIUtility.labelWidth;
            popupRect.width -= EditorGUIUtility.labelWidth;
            popupRect.height = EditorGUIUtility.singleLineHeight;



            GUIContent content = new GUIContent(property.objectReferenceValue != null ? property.objectReferenceValue.name : "None");
            if (EditorGUI.DropdownButton(popupRect, content, FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                int itemCount = 0;
                for (int i = 0; i < SoundConfig.Current.categoryNames.Length; i++)
                {
                    foreach (var asset in SoundConfig.SoundAssets.Where(asset => asset.category == i))
                    { AddMenuItem(menu, SoundConfig.Current.categoryNames[i] + "/" + asset.name, asset); itemCount++; }
                }
                if (itemCount == 0)
                    AddMenuItem(menu, "No SoundAssets found", null);
                menu.DropDown(popupRect);
            }
        }

        private void AddMenuItem(GenericMenu menu, string menuPath, SoundAsset asset)
        {
            menu.AddItem(new GUIContent(menuPath), false, OnItemSelected, asset);
        }

        private void OnItemSelected(object itemSelected)
        {
            serializedProp.objectReferenceValue = (UnityEngine.Object)itemSelected ?? null;
            serializedProp.serializedObject.ApplyModifiedProperties();
        }
    }
}