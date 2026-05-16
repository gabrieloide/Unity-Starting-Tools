using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Audio
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Starter Tools/Audio/Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        public List<AudioData> audioDataList = new List<AudioData>();

        public AudioData GetAudioData(string id)
        {
            return audioDataList.Find(x => x.id == id);
        }
    }
}
