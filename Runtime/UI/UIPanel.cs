using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [Header("Panel Settings")]
        public string panelId;
        public bool blockRaycastsWhileOpen = true;

        public UnityEvent onOpen;
        public UnityEvent onClose;

        private CanvasGroup _canvasGroup;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (string.IsNullOrEmpty(panelId))
            {
                panelId = gameObject.name;
            }
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = blockRaycastsWhileOpen;
            onOpen?.Invoke();
        }

        public virtual void Close()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            onClose?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
