using System.Collections.Generic;
using Code.Scripts.Core;
using UnityEngine;

namespace Code.Scripts.Audio
{
    public class AudioManager : BaseSingleton<AudioManager>
    {
        [SerializeField] private AudioDatabase _database;
        [SerializeField] private int _initialPoolSize = 10;
        
        private List<AudioSource> _sfxPool = new List<AudioSource>();
        private AudioSource _musicSource;

        protected override void Awake()
        {
            base.Awake();
            InitializePool();
        }

        private void InitializePool()
        {
            GameObject musicObj = new GameObject("MusicSource");
            musicObj.transform.SetParent(transform);
            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.loop = true;

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewSFXSource();
            }
        }

        private AudioSource CreateNewSFXSource()
        {
            GameObject sfxObj = new GameObject("SFXSource");
            sfxObj.transform.SetParent(transform);
            AudioSource source = sfxObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _sfxPool.Add(source);
            return source;
        }

        private AudioSource GetAvailableSFXSource()
        {
            foreach (var source in _sfxPool)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            return CreateNewSFXSource();
        }

        public void PlaySFX(string id)
        {
            if (_database == null)
            {
                Debug.LogError("[AudioManager] AudioDatabase is not assigned!");
                return;
            }

            AudioData data = _database.GetAudioData(id);
            if (data == null || data.clip == null)
            {
                Debug.LogWarning($"[AudioManager] Audio '{id}' not found in database or clip is missing.");
                return;
            }

            AudioSource source = GetAvailableSFXSource();
            source.clip = data.clip;
            source.volume = data.volume;
            source.pitch = data.pitch;
            source.loop = data.loop;
            source.outputAudioMixerGroup = data.audioMixerGroup;
            source.Play();
        }

        public void PlayMusic(string id)
        {
            if (_database == null) return;

            AudioData data = _database.GetAudioData(id);
            if (data == null || data.clip == null) return;

            if (_musicSource.isPlaying && _musicSource.clip == data.clip) return;

            _musicSource.clip = data.clip;
            _musicSource.volume = data.volume;
            _musicSource.pitch = data.pitch;
            _musicSource.loop = data.loop;
            _musicSource.outputAudioMixerGroup = data.audioMixerGroup;
            _musicSource.Play();
        }

        public void StopMusic()
        {
            if (_musicSource != null && _musicSource.isPlaying)
            {
                _musicSource.Stop();
            }
        }
    }
}
