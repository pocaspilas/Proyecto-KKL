using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.Sound
{
    static public class SoundManager
    {
        static private AudioSource globalAudioSource;
        static private Pool<AudioSource> audioSourcePool;

        static public AudioSource GlobalAudioSource
        {
            get
            {
                if (globalAudioSource == null)
                {
                    globalAudioSource = new GameObject("GlobalAudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
                    globalAudioSource.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
                    globalAudioSource.playOnAwake = false;
                    globalAudioSource.spatialBlend = 0;
                }
                return globalAudioSource;
            }
        }

        static public void InitializeSources()
        {
            if(GlobalAudioSource == null) { Debug.Log("Can't create GlobalAudioSource"); return; }
        }

        //PLAY

        static public void PlaySound(SoundAsset sound, bool loop)
        {
            GlobalAudioSource.clip = sound.clip;
            GlobalAudioSource.loop = loop;
            GlobalAudioSource.Play();
        }

        static public void PlayOneShotSound(SoundAsset sound)
        {
            GlobalAudioSource.PlayOneShot(sound.clip, sound.vol);
        }
    }
}