using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.Sound
{
    public class AudioSourcesPool : MonoBehaviour
    {
        private Pool<PoolableAudioSource> _internalPool;

        const int DEFAULT_POOL_SIZE = 10;

        public void Initialize()
        {
            if (_internalPool == null)
                _internalPool = new Pool<PoolableAudioSource>(DEFAULT_POOL_SIZE, CreatePoolableSource, (source) => source.gameObject.SetActive(true), (source) => source.gameObject.SetActive(false));
        }

        private PoolableAudioSource CreatePoolableSource()
        {
            PoolableAudioSource source = new GameObject("PooledAudioSource", typeof(PoolableAudioSource)).GetComponent<PoolableAudioSource>();
            source.transform.parent = transform;
            source.gameObject.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
            source.Initialize();
            source.e_OnEndSound += ReturnSourceToPool;
            return source;
        }

        private void ReturnSourceToPool(PoolableAudioSource source)
        {
            _internalPool.Return(source);
        }

        public PoolableAudioSource GetSource()
        {
            return _internalPool.Get();
        }

        public void ApplyToActiveSources(Action<PoolableAudioSource> actionToApply)
        {
            foreach (var src in _internalPool)
                actionToApply(src);
        }
    }
}