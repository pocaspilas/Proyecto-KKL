using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mati36.Sound
{
    [CreateAssetMenu(menuName ="SoundManager/SoundAsset"), ]
    public class SoundAsset : ScriptableObject
    {
        public int category;
        public AudioClip clip;
        [Header("Sound Parameters")]
        public bool loop;
        public float vol = 0.5f;
        [RangeMinMax(0f,1f), SerializeField]
        private Vector2 pitchRange = new Vector2(1f, 1f);

        public float Pitch { get { return Random.Range(pitchRange.x, pitchRange.y); } }
    }
}