using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mati36.Sound;

namespace Mati36.SoundEditor
{
    using AssetModificationProcessor = UnityEditor.AssetModificationProcessor;

    public class AudioAssetModification : AssetModificationProcessor
    {
        static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            var obj = AssetDatabase.LoadMainAssetAtPath(path);
            if (obj.GetType() == typeof(SoundAsset))
                SoundConfig.Current.OnDeleteSoundAsset((SoundAsset)obj);
            else if(obj.GetType() == typeof(AudioClip))
                SoundConfig.Current.OnDeleteClip((AudioClip)obj);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}