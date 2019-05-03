using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.Sound
{
    public class PoolableAudioSource : MonoBehaviour
    {
        private bool isPaused;
        private AudioSource _audioSource;

        private float currentPitch;

        public event Action<PoolableAudioSource> e_OnEndSound = delegate { };

        public void Initialize()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.rolloffMode = AudioRolloffMode.Logarithmic;
            _audioSource.minDistance = 5;
            _audioSource.maxDistance = 30;
        }

        public void PlayAudio(AudioClip clip, SoundMode mode, float vol, float pitch, bool loop = false)
        {
            _audioSource.spatialBlend = mode == SoundMode.Mode2D ? 0 : 1;
            _audioSource.clip = clip;
            _audioSource.volume = vol;
            _audioSource.pitch = pitch;
            currentPitch = pitch;
            _audioSource.loop = loop;
            _audioSource.Play();
            isPaused = false;
        }

        private void Update()
        {
            if (!isPaused && !_audioSource.isPlaying)
                e_OnEndSound(this);
        }
        //
        //PLAYBACK
        //
        public void PauseSource()
        {
            _audioSource.Pause();
            isPaused = true;
        }

        public void UnpauseSource()
        {
            _audioSource.UnPause();
            isPaused = false;
        }

        public void StopSource()
        {
            e_OnEndSound(this);
        }
        //
        //PITCH
        //
        public void ModifyPitch(float value)
        {
            _audioSource.pitch = currentPitch * value;
        }
    }

    public enum SoundMode { Mode2D, Mode3D }
}