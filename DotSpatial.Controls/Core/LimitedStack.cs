// -----------------------------------------------------------------------
// <copyright file="Lmi.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Controls.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class LimitedStack<T>
    {
        public readonly int Limit;
        private readonly List<T> _stack;

        public LimitedStack(int limit = 32)
        {
            Limit = limit;
            _stack = new List<T>(limit);
        }

        public void Push(T item)
        {
            if (_stack.Count == Limit) _stack.RemoveAt(0);
            _stack.Add(item);
        }

        public T Peek()
        {
            if (_stack.Count == 0) return default(T);
            return _stack[_stack.Count - 1];
        }

        public T Pop()
        {
            var item = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return item;
        }

        public int Count
        {
            get { return _stack.Count; }
        }
    }
}
