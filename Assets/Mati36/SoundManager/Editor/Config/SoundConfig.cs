using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mati36.SoundManager
{
    [CreateAssetMenu(menuName = "SoundManager/Config")]
    public class SoundConfig : ScriptableObject
    {
        static private SoundConfig _current;
        static public SoundConfig Current
        {
            get
            {
                if (_current == null)
                {
                    _current = (SoundConfig)Resources.Load("SoundConfig");
                    if (_current == null)
                    { Debug.LogWarning("SoundConfig not found, creating default config..."); _current = CreateSoundConfigDefault(); }
                }
                return _current;
            }
        }

        private static SoundConfig CreateSoundConfigDefault()
        {
            SoundConfig newConfig = CreateInstance<SoundConfig>();
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                AssetDatabase.CreateFolder("Assets", "Resources");
            AssetDatabase.CreateAsset(newConfig, "Assets/Resources/SoundConfig.asset");
            AssetDatabase.SaveAssets();
            return newConfig;
        }

        public string[] categoryNames = new string[1] { "Default" };

        static public string[] SoundAssetsPaths
        {
            get
            {
                return AssetDatabase.FindAssets("t:SoundAsset").Select(guid => AssetDatabase.GUIDToAssetPath(guid)).ToArray();
            }
        }

        static public SoundAsset[] SoundAssets
        {
            get
            {
                return SoundAssetsPaths.Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SoundAsset)) as SoundAsset).ToArray();
            }
        }
    }
}