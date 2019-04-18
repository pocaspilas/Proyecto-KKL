using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Mati36.Sound;

namespace Mati36.SoundEditor
{
    public class AudioAssetImporter : AssetPostprocessor
    {
        void OnPostprocessAudio(AudioClip clip)
        {
            SoundAsset newAsset = ScriptableObject.CreateInstance<SoundAsset>();
            AudioClip clipAsset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(AudioClip)) as AudioClip;
            if (clipAsset == null) { Debug.Log("Clip asset is null, reimport."); return; }
            if (!SoundConfig.Current.OnProcessClip(clipAsset)) return;
            newAsset.clip = clipAsset;
            if (!AssetDatabase.IsValidFolder("Assets/Data"))
                AssetDatabase.CreateFolder("Assets", "Data");
            if (!AssetDatabase.IsValidFolder("Assets/Data/Sounds"))
                AssetDatabase.CreateFolder("Assets/Data", "Sounds");
            if (!AssetDatabase.IsValidFolder("Assets/Data/Sounds/Default"))
                AssetDatabase.CreateFolder("Assets/Data/Sounds", "Default");

            AssetDatabase.CreateAsset(newAsset, "Assets/Data/Sounds/Default/" + clipAsset.name + ".asset");
            AssetDatabase.SaveAssets();
            Debug.Log("Created " + clipAsset.name + " SoundAsset.");
        }
    }
}