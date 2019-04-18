using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mati36.Sound;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Mati36.Sound
{
    [CreateAssetMenu(menuName = "SoundManager/Config")]
    public class SoundConfig : ScriptableObject
    {
        //
        //EDITOR-ONLY
        //
#if UNITY_EDITOR
        //ASSET
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

        //CATEGORY
        public string[] categoryNames = new string[1] { "Default" };

        //SOUNDCLIPS
        public List<AudioClip> processedClips = new List<AudioClip>();

        /// <summary>
        /// Add the clip to the processed clips list, if it isn't already. Otherwise, returns false.
        /// </summary>
        public bool OnProcessClip(AudioClip clip)
        {
            if (processedClips.Contains(clip)) return false;
            processedClips.Add(clip);
            return true;
        }

        public void OnDeleteClip(AudioClip clip)
        {
            if (processedClips.Contains(clip))
                processedClips.Remove(clip);
        }

        public void OnDeleteSoundAsset(SoundAsset asset)
        {
            if (asset.clip != null && processedClips.Contains(asset.clip))
                processedClips.Remove(asset.clip);
        }

        //REF TO SOUNDASSETS
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
#endif //EDITOR


    }
}