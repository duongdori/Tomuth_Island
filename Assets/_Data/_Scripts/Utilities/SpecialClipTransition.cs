using Animancer;
using UnityEngine;

namespace DR.Utilities
{
    [System.Serializable]
    public class SpecialClipTransition : ClipTransition
    {
        public ParticleSystem vfx;
        public AudioClip soundFX01;
        public AudioClip soundFX02;
        public AudioClip soundFX03;
    }
}