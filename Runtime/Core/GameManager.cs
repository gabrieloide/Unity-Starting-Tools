using UnityEngine;
using System;

namespace Code.Scripts.Core
{
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : BaseSingleton<GameManager>
    {
        public static event Action<GameState> OnGameStateChanged;

        [SerializeField] private GameState _currentState = GameState.MainMenu;
        public GameState CurrentState => _currentState;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            UpdateGameState(_currentState);
        }

        public void UpdateGameState(GameState newState)
        {
            _currentState = newState;

            switch (newState)
            {
                case GameState.MainMenu:
                    HandleMainMenu();
                    break;
                case GameState.Playing:
                    HandlePlaying();
                    break;
                case GameState.Paused:
                    HandlePaused();
                    break;
                case GameState.GameOver:
                    HandleGameOver();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }

            OnGameStateChanged?.Invoke(newState);
        }

        private void HandleMainMenu()
        {
            Time.timeScale = 1f;
        }

        private void HandlePlaying()
        {
            Time.timeScale = 1f;
        }

        private void HandlePaused()
        {
            Time.timeScale = 0f;
        }

        private void HandleGameOver()
        {
            Time.timeScale = 1f;
        }
    }
}
