using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sample.Systems
{
    public static class EventDispatcher
    {
        private static readonly Dictionary<Type, List<object>> _handlers = new();

        public static void Raise<TEvent>(TEvent @event)
        {
            var eventType = typeof(TEvent);
            EnsureHandlersCreated(eventType);

            foreach (var handler in _handlers[eventType])
            {
                try
                {
                    ((Action<TEvent>) handler).Invoke(@event);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                }
            }
        }

        public static void Subscribe<TEvent>(Action<TEvent> handler)
        {
            var eventType = typeof(TEvent);
            EnsureHandlersCreated(eventType);
            _handlers[eventType].Add(handler);
        }

        public static void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            var eventType = typeof(TEvent);
            _handlers[eventType].Remove(handler);
        }

        private static void EnsureHandlersCreated(Type eventType)
        {
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers.Add(eventType, new List<object>());
            }
        }
    }
}
