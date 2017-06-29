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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:18:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaInfo
    /// </summary>
    public class HfaInfo
    {
        #region Private Variables

        private HfaAccess _access;
        private Dictionary<string, HfaType> _dictionary;
        private long _dictionaryPos;
        private long _endOfFile; // using long instead of guint32
        private int _entryHeaderLength;
        private string _fileName;
        private FileStream _fp;
        private string _igeFilename;
        private string _path;
        private long _rootPos;
        private Boolean _treeDirty;
        private int _version;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaInfo
        /// </summary>
        public HfaInfo()
        {
            _dictionary = new Dictionary<string, HfaType>();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the access type
        /// </summary>
        public HfaAccess Access
        {
            get { return _access; }
            set { _access = value; }
        }

        /// <summary>
        /// Gets or sets a dictionary for looking up types based on string type names.
        /// </summary>
        public Dictionary<string, HfaType> Dictionary
        {
            get { return _dictionary; }
            set { _dictionary = value; }
        }

        /// <summary>
        /// The directory path
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// The string fileName sans path
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// sans path
        /// </summary>
        public string IgeFilename
        {
            get { return _igeFilename; }
            set { _igeFilename = value; }
        }

        /// <summary>
        /// End of file
        /// </summary>
        public long EndOfFile
        {
            get { return _endOfFile; }
            set { _endOfFile = value; }
        }

        /// <summary>
        /// Root position
        /// </summary>
        public long RootPos
        {
            get { return _rootPos; }
            set { _rootPos = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public long DictionaryPos
        {
            get { return _dictionaryPos; }
            set { _dictionaryPos = value; }
        }

        /// <summary>
        /// The entry header length
        /// </summary>
        public int EntryHeaderLength
        {
            get { return _entryHeaderLength; }
            set { _entryHeaderLength = value; }
        }

        /// <summary>
        /// The integer version
        /// </summary>
        public int Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Boolean, true if the tree has changed
        /// </summary>
        public bool TreeDirty
        {
            get { return _treeDirty; }
            set { _treeDirty = value; }
        }

        /// <summary>
        /// The file stream
        /// </summary>
        public FileStream Fp
        {
            get { return _fp; }
            set { _fp = value; }
        }

        #endregion
    }
}