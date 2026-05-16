using System;
using Code.Scripts.Core;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Code.Scripts.Input
{
    public class InputReader : BaseSingleton<InputReader>
    {
#if ENABLE_INPUT_SYSTEM
        private InputSystem_Actions _inputActions;

        public static event Action<Vector2> OnMove;
        public static event Action OnJump;
        public static event Action OnInteract;

        protected override void Awake()
        {
            base.Awake();
            
            _inputActions = new InputSystem_Actions();
            
            _inputActions.Player.Move.performed += ctx => OnMove?.Invoke(ctx.ReadValue<Vector2>());
            _inputActions.Player.Move.canceled += ctx => OnMove?.Invoke(Vector2.zero);
            
            _inputActions.Player.Jump.performed += ctx => OnJump?.Invoke();
            _inputActions.Player.Interact.performed += ctx => OnInteract?.Invoke();
        }

        private void OnEnable()
        {
            if (_inputActions != null)
                _inputActions.Enable();
        }

        private void OnDisable()
        {
            if (_inputActions != null)
                _inputActions.Disable();
        }
#else
        private void Awake()
        {
            Debug.LogError("[InputReader] New Input System is not enabled in Player Settings.");
        }
#endif
    }
}
