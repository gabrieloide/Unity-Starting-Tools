using System.Collections;
using Code.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace Code.Scripts.System
{
    public class SceneLoader : BaseSingleton<SceneLoader>
    {
        [Header("UI References")]
        [SerializeField] private GameObject _loadingScreen;
        [SerializeField] private Image _progressBar;
        
        public static event Action<string> OnSceneLoadStart;
        public static event Action<string> OnSceneLoadComplete;

        protected override void Awake()
        {
            base.Awake();
            if (_loadingScreen != null)
            {
                _loadingScreen.SetActive(false);
            }
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(LoadSceneAsync(SceneUtility.GetScenePathByBuildIndex(sceneIndex)));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            OnSceneLoadStart?.Invoke(sceneName);

            if (_loadingScreen != null)
            {
                _loadingScreen.SetActive(true);
            }
            if (_progressBar != null)
            {
                _progressBar.fillAmount = 0f;
            }

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                if (_progressBar != null)
                {
                    _progressBar.fillAmount = progress;
                }
                yield return null;
            }

            if (_loadingScreen != null)
            {
                _loadingScreen.SetActive(false);
            }

            OnSceneLoadComplete?.Invoke(sceneName);
        }
    }
}
