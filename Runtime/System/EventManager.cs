using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.System
{
    public static class EventManager
    {
        private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        public static void AddListener<T>(Action<T> listener)
        {
            Type eventType = typeof(T);

            if (!_events.ContainsKey(eventType))
            {
                _events[eventType] = listener;
            }
            else
            {
                _events[eventType] = Delegate.Combine(_events[eventType], listener);
            }
        }

        public static void RemoveListener<T>(Action<T> listener)
        {
            Type eventType = typeof(T);

            if (_events.ContainsKey(eventType))
            {
                var currentDel = _events[eventType];
                currentDel = Delegate.Remove(currentDel, listener);

                if (currentDel == null)
                {
                    _events.Remove(eventType);
                }
                else
                {
                    _events[eventType] = currentDel;
                }
            }
        }

        public static void TriggerEvent<T>(T eventData)
        {
            Type eventType = typeof(T);

            if (_events.TryGetValue(eventType, out Delegate currentDel))
            {
                if (currentDel is Action<T> action)
                {
                    action.Invoke(eventData);
                }
            }
        }
        
        public static void ClearAllEvents()
        {
            _events.Clear();
        }
    }
}
