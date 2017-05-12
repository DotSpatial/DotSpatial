// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
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
        #region Fields

        private char[] _name;
        private char[] _typeName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaEntry"/> class.
        /// </summary>
        /// <param name="hfaIn">The HfaInfo.</param>
        /// <param name="nPos">The position.</param>
        /// <param name="parent">The parent.</param>
        /// <param name="prev">The previous HfaEntry.</param>
        public HfaEntry(HfaInfo hfaIn, long nPos, HfaEntry parent, HfaEntry prev)
        {
            // Initialize fields
            _name = new char[64];
            _typeName = new char[32];
            Hfa = hfaIn;
            FilePos = nPos;
            Parent = parent;
            Prev = prev;
            Hfa.Fp.Seek(nPos, SeekOrigin.Begin);
            Children = new List<HfaEntry>();

            // Read entry information from the file
            int[] anEntryNums = new int[6];
            byte[] entryBytes = new byte[24];
            Hfa.Fp.Read(entryBytes, 0, 24);
            Buffer.BlockCopy(entryBytes, 0, anEntryNums, 0, 24);

            NextPos = anEntryNums[0];
            ChildPos = anEntryNums[3];
            DataPos = anEntryNums[4];
            DataSize = anEntryNums[5];

            // Read the name
            byte[] nameBytes = new byte[64];
            Hfa.Fp.Read(nameBytes, 0, 64);

            _name = Encoding.Default.GetChars(nameBytes);

            // Read the type
            byte[] typeBytes = new byte[32];
            Hfa.Fp.Read(typeBytes, 0, 32);
            _typeName = Encoding.Default.GetChars(typeBytes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaEntry"/> class with the intention that it would
        /// be written to disk later.
        /// </summary>
        /// <param name="hfaIn">The HfaInfo.</param>
        /// <param name="name">The name.</param>
        /// <param name="typename">The type name.</param>
        /// <param name="parent">The parent.</param>
        public HfaEntry(HfaInfo hfaIn, string name, string typename, HfaEntry parent)
        {
            // Initialize
            Hfa = hfaIn;
            Parent = parent;
            Name = name;
            TypeName = typename;
            Children = new List<HfaEntry>();

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
        /// Initializes a new instance of the <see cref="HfaEntry"/> class.
        /// </summary>
        public HfaEntry()
        {
            _name = new char[64];
            _typeName = new char[32];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the first child belonging to this entry.
        /// </summary>
        public HfaEntry Child
        {
            get
            {
                return Children?[0];
            }

            set
            {
                if (Children == null) Children = new List<HfaEntry>();
                Children[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the long position of the child.
        /// </summary>
        public long ChildPos { get; set; }

        /// <summary>
        /// Gets or sets the collection of all the children.
        /// </summary>
        public List<HfaEntry> Children { get; set; }

        /// <summary>
        /// Gets or sets the data for this entry.
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the GUInt32 Data Position of this entry.
        /// </summary>
        public long DataPos { get; set; }

        /// <summary>
        /// Gets or sets the GUInt32 Data Size of this entry.
        /// </summary>
        public long DataSize { get; set; }

        /// <summary>
        /// Gets or sets the long integer file position.
        /// </summary>
        public long FilePos { get; set; }

        /// <summary>
        /// Gets or sets the HFA Info.
        /// </summary>
        public HfaInfo Hfa { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is changed.
        /// </summary>
        public bool IsDirty { get; set; }

        /// <summary>
        /// Gets or sets the 64 character name of the entry.
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
        /// Gets or sets the HfaEntry that is the next entry.
        /// </summary>
        public HfaEntry Next { get; set; }

        /// <summary>
        /// Gets or sets the long integer position of the next entry in the file.
        /// </summary>
        public long NextPos { get; set; }

        /// <summary>
        /// Gets or sets the hfa parent.
        /// </summary>
        public HfaEntry Parent { get; set; }

        /// <summary>
        /// Gets or sets the previous entry.
        /// </summary>
        public HfaEntry Prev { get; set; }

        /// <summary>
        /// Gets or sets the type for this entry.
        /// </summary>
        public HfaType Type { get; set; }

        /// <summary>
        /// gets or sets the 32 character typestring.
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

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the HfaEntry that is the child.
        /// </summary>
        /// <returns>The first child or null.</returns>
        public HfaEntry GetFirstChild()
        {
            return Children?.First();
        }

        /// <summary>
        /// This parses a complete "path" separated by periods in order
        /// to search for a specific child node.
        /// </summary>
        /// <param name="name">Name of the child.</param>
        /// <returns>The child with the given name or null.</returns>
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

        /// <summary>
        /// Loads the data bytes for this element from the file.
        /// </summary>
        public void LoadData()
        {
            if (Data != null || DataSize == 0) return;

            Hfa.Fp.Seek(DataPos, SeekOrigin.Begin);
            Hfa.Fp.Read(Data, 0, (int)DataSize);
            Type = Hfa.Dictionary[TypeName];
        }

        /// <summary>
        /// Parses a name which may have an unwanted : or multiple sub-tree
        /// names separated with periods.
        /// </summary>
        /// <param name="name">Name that gets parsed.</param>
        /// <returns>The first part of the name.</returns>
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
        /// <param name="name">The name that gets parsed.</param>
        /// <returns>The subtree name.</returns>
        private static string GetSubtreeName(string name)
        {
            if (name.Contains("."))
            {
                int start = name.IndexOf('.') + 1;
                return name.Substring(start, name.Length - start);
            }

            return null;
        }

        #endregion
    }
}