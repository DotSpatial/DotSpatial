// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:57:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaEntry
    /// </summary>
    public class HfaEntry
    {
        #region Private Variables

        private long _childPos;
        private List<HfaEntry> _children;
        private byte[] _data;
        private long _dataPos;
        private long _dataSize;
        private long _filePos;
        private HfaInfo _hfa;
        private bool _isDirty;
        private char[] _name;
        private HfaEntry _next;
        private long _nextPos;
        private HfaEntry _parent;
        private HfaEntry _prev;
        private HfaType _type;
        private char[] _typeName;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaEntry from the file
        /// </summary>
        public HfaEntry(HfaInfo hfaIn, long nPos, HfaEntry parent, HfaEntry prev)
        {
            // Initialize fields
            _name = new char[64];
            _typeName = new char[32];
            _hfa = hfaIn;
            _filePos = nPos;
            _parent = parent;
            _prev = prev;
            _hfa.Fp.Seek(nPos, SeekOrigin.Begin);
            _children = new List<HfaEntry>();
            // Read entry information from the file

            int[] anEntryNums = new int[6];
            byte[] entryBytes = new byte[24];
            _hfa.Fp.Read(entryBytes, 0, 24);
            Buffer.BlockCopy(entryBytes, 0, anEntryNums, 0, 24);

            // Undecipherable Code since no implementation of the method HFAStandard exists
            // for (i = 0; i < 6; i++)
            //    HFAStandard(4, anEntryNumns + i);

            NextPos = anEntryNums[0];
            ChildPos = anEntryNums[3];
            DataPos = anEntryNums[4];
            DataSize = anEntryNums[5];

            // Read the name
            byte[] nameBytes = new byte[64];
            _hfa.Fp.Read(nameBytes, 0, 64);

            _name = Encoding.Default.GetChars(nameBytes);

            // Read the type
            byte[] typeBytes = new byte[32];
            _hfa.Fp.Read(typeBytes, 0, 32);
            _typeName = Encoding.Default.GetChars(typeBytes);
        }

        /// <summary>
        /// Create a new instance of the entry with the intention that it would
        /// be written to disk later
        /// </summary>
        /// <param name="hfaIn"></param>
        /// <param name="name"></param>
        /// <param name="typename"></param>
        /// <param name="parent"></param>
        public HfaEntry(HfaInfo hfaIn, string name, string typename, HfaEntry parent)
        {
            // Initialize
            _hfa = hfaIn;
            _parent = parent;
            Name = name;
            TypeName = typename;
            _children = new List<HfaEntry>();

            // Update the previous or parent node to refer to this one
            if (parent == null)
            {
                // do nothing
            }
            else if (parent.Child == null)
            {
                parent.Child = this;
                parent.IsDirty = true;
            }
            else
            {
                HfaEntry prev = parent.Children[parent.Children.Count - 1];
                prev.Next = this;
                prev.IsDirty = true;
            }
            IsDirty = true;
        }

        /// <summary>
        /// Creates a new instance of HfaEntry
        /// </summary>
        public HfaEntry()
        {
            _name = new char[64];
            _typeName = new char[32];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the data bytes for this element from the file
        /// </summary>
        public void LoadData()
        {
            if (_data != null || _dataSize == 0) return;

            _hfa.Fp.Seek(_dataPos, SeekOrigin.Begin);
            _hfa.Fp.Read(_data, 0, (int)_dataSize);
            _type = _hfa.Dictionary[TypeName];
        }

        /// <summary>
        /// Parses a name which may have an unwanted : or multiple sub-tree
        /// names separated with periods.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetFirstChildName(string name)
        {
            int pos = name.Length;
            if (name.Contains(":")) pos = name.IndexOf(':');
            if (name.Contains("."))
            {
                int tempPos = name.IndexOf('.');
                if (tempPos < pos) pos = tempPos;
            }
            return name.Substring(0, pos);
        }

        /// <summary>
        /// If this is null, then there is no further subtree
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetSubtreeName(string name)
        {
            if (name.Contains("."))
            {
                int start = name.IndexOf('.') + 1;
                return name.Substring(start, name.Length - start);
            }
            return null;
        }

        /// <summary>
        /// This parses a complete "path" separated by periods in order
        /// to search for a specific child node.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HfaEntry GetNamedChild(string name)
        {
            string firstName = GetFirstChildName(name);
            string subTree = GetSubtreeName(name);
            foreach (HfaEntry child in Children)
            {
                if (child.Name != firstName) continue;
                return subTree != null ? child.GetNamedChild(subTree) : child;
            }
            return null;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating if this is changed
        /// </summary>
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        /// <summary>
        /// Gets or sets the long integer file position
        /// </summary>
        public long FilePos
        {
            get { return _filePos; }
            set { _filePos = value; }
        }

        /// <summary>
        /// Gets or sets the HFA Info
        /// </summary>
        public HfaInfo Hfa
        {
            get { return _hfa; }
            set { _hfa = value; }
        }

        /// <summary>
        /// Gets or sets the hfa parent
        /// </summary>
        public HfaEntry Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the previous entry
        /// </summary>
        public HfaEntry Prev
        {
            get { return _prev; }
            set { _prev = value; }
        }

        /// <summary>
        /// Gets or sets the long integer position of the next entry in the file
        /// </summary>
        public long NextPos
        {
            get { return _nextPos; }
            set { _nextPos = value; }
        }

        /// <summary>
        /// Gets or sets the HfaEntry that is the next entry
        /// </summary>
        public HfaEntry Next
        {
            get { return _next; }
            set { _next = value; }
        }

        /// <summary>
        /// Gets or sets the long position of the child
        /// </summary>
        public long ChildPos
        {
            get { return _childPos; }
            set { _childPos = value; }
        }

        /// <summary>
        /// Gets or sets the 64 character name of the entry
        /// </summary>
        public string Name
        {
            get
            {
                return new string(_name);
            }
            set
            {
                _name = value.ToCharArray(0, 64);
                IsDirty = true;
            }
        }

        /// <summary>
        /// gets or sets the 32 character typestring
        /// </summary>
        public string TypeName
        {
            get
            {
                return new string(_typeName);
            }
            set
            {
                _typeName = value.ToCharArray(0, 32);
                IsDirty = true;
            }
        }

        /// <summary>
        /// Gets or sets the type for this entry
        /// </summary>
        public HfaType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// The GUInt32 Data Position of this entry
        /// </summary>
        public long DataPos
        {
            get { return _dataPos; }
            set { _dataPos = value; }
        }

        /// <summary>
        /// The GUInt32 Data Size of this entry
        /// </summary>
        public long DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }

        /// <summary>
        /// Gets the data for this entry
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Gets or sets the collection of all the children.
        /// </summary>
        public List<HfaEntry> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>
        /// Gets or sets the first child belonging to this entry.
        /// </summary>
        public HfaEntry Child
        {
            get
            {
                return _children == null ? null : _children[0];
            }
            set
            {
                if (_children == null) _children = new List<HfaEntry>();
                _children[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the HfaEntry that is the child
        /// </summary>
        public HfaEntry GetFirstChild()
        {
            if (_children == null) return null;
            return _children.First();
        }

        #endregion
    }
}