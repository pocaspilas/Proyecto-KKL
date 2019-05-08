using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.Sound
{
    [ExecuteInEditMode]
    static public class SoundManager
    {
        const string MANAGER_NAME = "Sound Manager";

        static private AudioSource globalAudioSource;
        static private AudioSourcesPool audioSourcePool;

        const string GLOBALSOURCENAME = "GlobalAudioSource";
        const string POOLNAME = "AudioSrcPool";

        static public AudioSource GlobalAudioSource
        {
            get
            {
                if (globalAudioSource == null)
                {
                    var existingSource = GameObject.Find(GLOBALSOURCENAME);
                    if (existingSource != null)
                        globalAudioSource = existingSource.GetComponent<AudioSource>();
                    else
                    {
                        Debug.Log((MANAGER_NAME.ToUpper() + " // Creating " + GLOBALSOURCENAME + "...").Bold());
                        globalAudioSource = new GameObject(GLOBALSOURCENAME, typeof(AudioSource)).GetComponent<AudioSource>();
                        globalAudioSource.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
                        globalAudioSource.playOnAwake = false;
                        globalAudioSource.spatialBlend = 0;
                    }
                }
                return globalAudioSource;
            }
        }

        static public AudioSourcesPool AudioSrcPool
        {
            get
            {
                if (audioSourcePool == null)
                {
                    var existingPool = GameObject.Find(POOLNAME);
                    if (existingPool != null)
                    {
                        audioSourcePool = existingPool.GetComponent<AudioSourcesPool>();
                        audioSourcePool.Initialize();
                    }
                    else
                    {
                        Debug.Log((MANAGER_NAME.ToUpper() + " // CreatingPool...").Bold());
                        audioSourcePool = new GameObject(POOLNAME, typeof(AudioSourcesPool)).GetComponent<AudioSourcesPool>();
                        audioSourcePool.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
                        audioSourcePool.Initialize();
                    }
                }
                return audioSourcePool;
            }
        }
        /// <summary>
        /// Initialize manually the AudioSources
        /// </summary>
        static public void InitializeSources()
        {
            if (GlobalAudioSource == null) { Debug.Log("Can't create GlobalAudioSource"); return; }
            if (AudioSrcPool == null) { Debug.Log("Can't create AudioSourcesPool"); return; }
        }

        //PLAYBACK
        static public PoolableAudioSource PlaySound(SoundAsset sound)
        {
            var source = AudioSrcPool.GetSource();
            source.PlayAudio(sound.clip, SoundMode.Mode2D, sound.vol, sound.Pitch, sound.loop);
            return source;
        }

        static public void PlayOneShotSound(SoundAsset sound)
        {
            GlobalAudioSource.PlayOneShot(sound.clip, sound.vol);
        }

        static public PoolableAudioSource PlaySoundAt(SoundAsset sound, Vector3 position)
        {
            var source = AudioSrcPool.GetSource();
            source.transform.position = position;
            source.PlayAudio(sound.clip, SoundMode.Mode3D, sound.vol, sound.Pitch, sound.loop);
            return source;
        }

        static public PoolableAudioSource PlaySoundAt(SoundAsset sound, Vector3 position, float overridePitch)
        {
            var source = AudioSrcPool.GetSource();
            source.transform.position = position;
            source.PlayAudio(sound.clip, SoundMode.Mode3D, sound.vol, overridePitch, sound.loop);
            return source;
        }


        static public PoolableAudioSource CrossfadeTo(this PoolableAudioSource from, SoundAsset sound, float crossfadeLength)
        {
            from.FadeOut(crossfadeLength);
            var to = PlaySound(sound);
            to.FadeIn(crossfadeLength);
            return to;
        }

        //SPEED
        static public void ModifySpeed(float speed)
        {
            if (speed == 0)
                GlobalAudioSource.Pause();
            else
            {
                GlobalAudioSource.UnPause();
                GlobalAudioSource.pitch = speed;
            }

            AudioSrcPool.ApplyToActiveSources(
                (src) =>
                {
                    if (speed == 0)
                        src.PauseSource();
                    else
                    {
                        src.UnpauseSource();
                        src.ModifyPitch(speed);
                    }
                }
                );
        }
    }
}