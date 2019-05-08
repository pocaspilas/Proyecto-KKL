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

        private float currentVol, currentPitch;

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
            _audioSource.spatialize = mode == SoundMode.Mode2D ? false : true;
            _audioSource.clip = clip;
            _audioSource.volume = vol;
            currentVol = vol;
            _audioSource.pitch = pitch;
            currentPitch = pitch;
            _audioSource.loop = loop;
            _audioSource.Play();
            isPaused = false;
        }

        private void Update()
        {
            if (!isPaused && !_audioSource.isPlaying)
                StopSource();
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

        public void StopSource()//vuelve al pool
        {
            StopAllCoroutines();
            e_OnEndSound(this);
        }
        //
        //PITCH
        //
        public void ModifyPitch(float value)
        {
            _audioSource.pitch = currentPitch * value;
        }
        //
        //FADE
        //
        public void FadeIn(float duration)
        {
            StartCoroutine(FadeRoutine(duration, true));
        }
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeRoutine(duration, false));
        }

        private IEnumerator FadeRoutine(float duration, bool fadeIn)
        {
            float t = 0;
            while (t < 1)
            {
                if (!isPaused)
                {
                    _audioSource.volume = fadeIn ? Mathf.Lerp(0f, currentVol, t) : Mathf.Lerp(currentVol, 0f, t);
                    t += Time.deltaTime / duration;
                }
                yield return null;
            }
            _audioSource.volume = fadeIn ? currentVol : 0f;
            if (!fadeIn)
                StopSource();
        }

    }

    public enum SoundMode { Mode2D, Mode3D }
}