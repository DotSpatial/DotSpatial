using System.Collections.Generic;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a stack with a limit. If the limit is reached, for each item added at the end one item is removed at the beginning of the stack.
    /// </summary>
    /// <typeparam name="T">Type of the contained objects.</typeparam>
    internal class LimitedStack<T>
    {
        #region Fields
        private readonly List<T> _stack;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedStack{T}"/> class.
        /// </summary>
        /// <param name="limit">Number of items that should be maximally allowed in the stack.</param>
        public LimitedStack(int limit = 32)
        {
            Limit = limit;
            _stack = new List<T>(limit);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of contained items.
        /// </summary>
        public int Count => _stack.Count;

        /// <summary>
        /// Gets the number of items that may maximally be contained in the stack.
        /// </summary>
        public int Limit { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Peeks into the stack.
        /// </summary>
        /// <returns>The last item.</returns>
        public T Peek()
        {
            if (_stack.Count == 0) return default(T);
            return _stack[_stack.Count - 1];
        }

        /// <summary>
        /// Returns the last item, removing it from the stack.
        /// </summary>
        /// <returns>The last item.</returns>
        public T Pop()
        {
            var item = _stack[_stack.Count - 1];
            _stack.RemoveAt(_stack.Count - 1);
            return item;
        }

        /// <summary>
        /// Adds the given item to the end of the stack. If the limit is reached the first item gets removed.
        /// </summary>
        /// <param name="item">Item that gets added.</param>
        public void Push(T item)
        {
            if (_stack.Count == Limit) _stack.RemoveAt(0);
            _stack.Add(item);
        }

        #endregion
    }
}