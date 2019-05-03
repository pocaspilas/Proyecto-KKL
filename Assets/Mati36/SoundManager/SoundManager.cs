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
        static private Pool<PoolableAudioSource> audioSourcePool;
        const int DEFAULT_POOL_SIZE = 10;
        const string GLOBALSOURCENAME = "GlobalAudioSource";

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
                        Debug.Log((MANAGER_NAME.ToUpper() + " // " + GLOBALSOURCENAME + "...").Bold());
                        globalAudioSource = new GameObject(GLOBALSOURCENAME, typeof(AudioSource)).GetComponent<AudioSource>();
                        globalAudioSource.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
                        globalAudioSource.playOnAwake = false;
                        globalAudioSource.spatialBlend = 0;
                    }
                }
                return globalAudioSource;
            }
        }

        static public Pool<PoolableAudioSource> AudioSourcesPool
        {
            get
            {
                if (audioSourcePool == null)
                {
                    Debug.Log((MANAGER_NAME.ToUpper() + " // CreatingPool...").Bold());
                    audioSourcePool = new Pool<PoolableAudioSource>(DEFAULT_POOL_SIZE, CreatePoolableSource, (source) => source.gameObject.SetActive(true), (source) => source.gameObject.SetActive(false));
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
            if (AudioSourcesPool == null) { Debug.Log("Can't create AudioSourcesPool"); return; }
        }

        static private PoolableAudioSource CreatePoolableSource()
        {
            PoolableAudioSource source = new GameObject("PooledAudioSource", typeof(PoolableAudioSource)).GetComponent<PoolableAudioSource>();
            source.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            source.Initialize();
            source.e_OnEndSound += ReturnSourceToPool;
            return source;
        }

        //POOL
        private static void ReturnSourceToPool(PoolableAudioSource source)
        {
            AudioSourcesPool.Return(source);
        }


        //PLAYBACK
        static public PoolableAudioSource PlaySound(SoundAsset sound)
        {
            var source = AudioSourcesPool.Get();
            source.PlayAudio(sound.clip, SoundMode.Mode2D, sound.vol, sound.Pitch, sound.loop);
            return source;
        }

        static public void PlayOneShotSound(SoundAsset sound)
        {
            GlobalAudioSource.PlayOneShot(sound.clip, sound.vol);
        }

        static public PoolableAudioSource PlaySoundAt(SoundAsset sound, Vector3 position)
        {
            var source = AudioSourcesPool.Get();
            source.transform.position = position;
            source.PlayAudio(sound.clip, SoundMode.Mode3D, sound.vol, sound.Pitch, sound.loop);
            return source;
        }

        static public PoolableAudioSource PlaySoundAt(SoundAsset sound, Vector3 position, float overridePitch)
        {
            var source = AudioSourcesPool.Get();
            source.transform.position = position;
            source.PlayAudio(sound.clip, SoundMode.Mode3D, sound.vol, overridePitch, sound.loop);
            return source;
        }

        static public void ModifySpeed(float speed)
        {
            if (speed == 0)
                GlobalAudioSource.Pause();
            else
            {
                GlobalAudioSource.UnPause();
                GlobalAudioSource.pitch = speed;
            }

            foreach(var source in AudioSourcesPool)
            {
                if (speed == 0)
                    source.PauseSource();
                else
                {
                    source.UnpauseSource();
                    source.ModifyPitch(speed);
                }
            }
        }
    }
}