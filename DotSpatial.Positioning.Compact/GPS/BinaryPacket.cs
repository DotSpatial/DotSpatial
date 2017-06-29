using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning.Gps
{
    /// <summary>
    /// Represents a base class for designing GPS packets which store binary data.
    /// </summary>
    public abstract class BinaryPacket : Packet, IList<byte>
    {
        private List<byte> _Data;

        /// <summary>
        /// Creates a BinaryPacket
        /// </summary>
        protected BinaryPacket()
            : this(32)
        { }
        /// <summary>
        /// BinaryPacket with the specified capacity
        /// </summary>
        /// <param name="capacity"></param>
        protected BinaryPacket(int capacity)
        {
            _Data = new List<byte>(capacity);
        }

        /// <summary>
        /// Creates a BinaryPacket from an IEnumerable
        /// </summary>
        /// <param name="bytes"></param>
        protected BinaryPacket(IEnumerable<byte> bytes)
        {
            _Data = new List<byte>(bytes);
        }

        #region IList<byte> Members

        /// <summary>
        /// Returns the index of the specified byte
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(byte item)
        {
            return _Data.IndexOf(item);
        }

        /// <summary>
        /// Inserts a byte at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, byte item)
        {
            _Data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _Data.RemoveAt(index);
        }

        public byte this[int index]
        {
            get
            {
                return _Data[index];
            }
            set
            {
                _Data[index] = value;
            }
        }

        #endregion

        #region ICollection<byte> Members

        public void Add(byte item)
        {
            _Data.Add(item);
        }

        public void Clear()
        {
            _Data.Clear();
        }

        public bool Contains(byte item)
        {
            return _Data.Contains(item);
        }

        public void CopyTo(byte[] array, int arrayIndex)
        {
            _Data.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _Data.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(byte item)
        {
            return _Data.Remove(item);
        }

        #endregion

        #region IEnumerable<byte> Members

        public IEnumerator<byte> GetEnumerator()
        {
            return _Data.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Data.GetEnumerator();
        }

        #endregion
    }
}
