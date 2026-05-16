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
        private AudioSource _musicSourceA;
        private AudioSource _musicSourceB;
        private AudioSource _activeMusicSource;
        private Coroutine _fadeCoroutine;

        protected override void Awake()
        {
            base.Awake();
            InitializePool();
        }

        private void InitializePool()
        {
            _musicSourceA = CreateMusicSource("MusicSourceA");
            _musicSourceB = CreateMusicSource("MusicSourceB");
            _activeMusicSource = _musicSourceA;

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateNewSFXSource();
            }
        }

        private AudioSource CreateMusicSource(string name)
        {
            GameObject musicObj = new GameObject(name);
            musicObj.transform.SetParent(transform);
            AudioSource source = musicObj.AddComponent<AudioSource>();
            source.loop = true;
            source.playOnAwake = false;
            return source;
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
                if (!source.isPlaying) return source;
            }
            return CreateNewSFXSource();
        }

        public void PlaySFX(string id)
        {
            if (_database == null) return;
            AudioData data = _database.GetAudioData(id);
            if (data == null || data.clip == null) return;

            AudioSource source = GetAvailableSFXSource();
            source.clip = data.clip;
            source.volume = data.volume;
            source.pitch = data.pitch;
            source.loop = data.loop;
            source.outputAudioMixerGroup = data.audioMixerGroup;
            source.Play();
        }

        public void PlayMusic(string id, bool crossfade = false, float duration = 1.0f)
        {
            if (_database == null) return;
            AudioData data = _database.GetAudioData(id);
            if (data == null || data.clip == null) return;

            if (_activeMusicSource.isPlaying && _activeMusicSource.clip == data.clip) return;

            if (crossfade)
            {
                if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = StartCoroutine(CrossfadeCoroutine(data, duration));
            }
            else
            {
                _activeMusicSource.clip = data.clip;
                _activeMusicSource.volume = data.volume;
                _activeMusicSource.pitch = data.pitch;
                _activeMusicSource.loop = data.loop;
                _activeMusicSource.outputAudioMixerGroup = data.audioMixerGroup;
                _activeMusicSource.Play();
            }
        }

        private global::System.Collections.IEnumerator CrossfadeCoroutine(AudioData data, float duration)
        {
            AudioSource nextSource = (_activeMusicSource == _musicSourceA) ? _musicSourceB : _musicSourceA;
            
            nextSource.clip = data.clip;
            nextSource.volume = 0f;
            nextSource.pitch = data.pitch;
            nextSource.loop = data.loop;
            nextSource.outputAudioMixerGroup = data.audioMixerGroup;
            nextSource.Play();

            float startActiveVol = _activeMusicSource.volume;
            float targetNextVol = data.volume;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                
                _activeMusicSource.volume = Mathf.Lerp(startActiveVol, 0f, t);
                nextSource.volume = Mathf.Lerp(0f, targetNextVol, t);
                yield return null;
            }

            _activeMusicSource.Stop();
            _activeMusicSource.volume = 0f;
            _activeMusicSource.clip = null;
            _activeMusicSource = nextSource;
            _fadeCoroutine = null;
        }

        public void StopMusic()
        {
            if (_activeMusicSource != null) _activeMusicSource.Stop();
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }
        }
    }
}
