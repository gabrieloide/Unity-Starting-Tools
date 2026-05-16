using System.Collections.Generic;
using Code.Scripts.Core;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class PanelManager : BaseSingleton<PanelManager>
    {
        [Tooltip("The panel to open when the game starts (optional)")]
        [SerializeField] private UIPanel _initialPanel;

        private Dictionary<string, UIPanel> _panelsMap = new Dictionary<string, UIPanel>();
        private Stack<UIPanel> _panelHistory = new Stack<UIPanel>();
        
        public UIPanel CurrentPanel => _panelHistory.Count > 0 ? _panelHistory.Peek() : null;

        protected override void Awake()
        {
            base.Awake();
            RegisterAllPanels();
        }

        private void Start()
        {
            if (_initialPanel != null)
            {
                OpenPanel(_initialPanel.panelId);
            }
        }

        private void RegisterAllPanels()
        {
            UIPanel[] panels = GetComponentsInChildren<UIPanel>(true);
            foreach (var panel in panels)
            {
                if (!_panelsMap.ContainsKey(panel.panelId))
                {
                    _panelsMap.Add(panel.panelId, panel);
                    panel.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogWarning($"[PanelManager] Duplicate panel ID found: {panel.panelId}");
                }
            }
        }

        public void OpenPanel(string panelId)
        {
            if (_panelsMap.TryGetValue(panelId, out UIPanel nextPanel))
            {
                if (CurrentPanel != null)
                {
                    CurrentPanel.Close();
                }

                _panelHistory.Push(nextPanel);
                nextPanel.Open();
            }
            else
            {
                Debug.LogError($"[PanelManager] Panel with ID '{panelId}' not found!");
            }
        }

        public void CloseCurrentPanel()
        {
            if (_panelHistory.Count > 0)
            {
                UIPanel current = _panelHistory.Pop();
                current.Close();

                if (_panelHistory.Count > 0)
                {
                    UIPanel previous = _panelHistory.Peek();
                    previous.Open();
                }
            }
        }

        public void CloseAllPanels()
        {
            foreach (var panel in _panelsMap.Values)
            {
                panel.Close();
            }
            _panelHistory.Clear();
        }

        public bool TryGetPanel(string id, out UIPanel panel)
        {
            return _panelsMap.TryGetValue(id, out panel);
        }
    }
}
