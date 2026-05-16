using UnityEngine;
using UnityEngine.Audio;

namespace Code.Scripts.Audio
{
    [CreateAssetMenu(fileName = "New Audio Data", menuName = "Starter Tools/Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        public AudioMixerGroup audioMixerGroup;
    }
}
