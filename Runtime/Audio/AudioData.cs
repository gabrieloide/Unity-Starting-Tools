using UnityEngine;
using UnityEngine.Audio;

namespace Code.Scripts.Audio
{
    [global::System.Serializable]
    public class AudioData
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        public AudioMixerGroup audioMixerGroup;
    }
}
