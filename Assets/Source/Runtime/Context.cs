using System;
using System.Collections.Generic;

namespace Sample
{
    public static class Context
    {
        private static readonly Dictionary<Type, object> _context = new();

        public static T Get<T>() where T : class
        {
            return (T) _context[typeof(T)];
        }

        public static void Set<T>(T element) where T : class
        {
            _context[typeof(T)] = element;
        }

        public static void Clear()
        {
            _context.Clear();
        }
    }
}
