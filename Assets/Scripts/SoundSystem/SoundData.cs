using UnityEngine;
using UnityEngine.Audio;

namespace Runner.SoundSystem
{
    [CreateAssetMenu(fileName = "NewSoundData", menuName = "Scriptable Objects/Sound Data")]
    public class SoundData : ScriptableObject
    {
        public AudioClip[] clips;
        public SoundType soundType = SoundType.TwoDimensional;
        public bool looping = false;

        [Range(0f, 1f)] public float volume = 1f;
        public float[] pitch = new float[] { 1 };

        public AudioMixerGroup mixerGroup;

        // 3d settings

        public float minDistance = 1f;
        public float maxDistance = 30f;
        public enum SoundType
        {
            TwoDimensional,
            ThreeDimensional,
        }
    }
}
