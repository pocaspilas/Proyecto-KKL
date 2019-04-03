using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mati36.SoundManager
{
    [CreateAssetMenu(menuName ="SoundManager/SoundAsset")]
    public class SoundAsset : ScriptableObject
    {
        public int category;
        public AudioClip clip;
        public float vol;
    }
}