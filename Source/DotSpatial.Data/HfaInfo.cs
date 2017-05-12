// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:18:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaInfo
    /// </summary>
    public class HfaInfo
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaInfo"/> class.
        /// </summary>
        public HfaInfo()
        {
            Dictionary = new Dictionary<string, HfaType>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the access type
        /// </summary>
        public HfaAccess Access { get; set; }

        /// <summary>
        /// Gets or sets a dictionary for looking up types based on string type names.
        /// </summary>
        public Dictionary<string, HfaType> Dictionary { get; set; }

        /// <summary>
        /// Gets or sets the dictionary position.
        /// </summary>
        public long DictionaryPos { get; set; }

        /// <summary>
        /// Gets or sets the end of file.
        /// </summary>
        public long EndOfFile { get; set; }

        /// <summary>
        /// Gets or sets the entry header length.
        /// </summary>
        public int EntryHeaderLength { get; set; }

        /// <summary>
        /// Gets or sets the fileName sans path.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the file stream.
        /// </summary>
        public FileStream Fp { get; set; }

        /// <summary>
        /// Gets or sets the ige file name.
        /// </summary>
        public string IgeFilename { get; set; }

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the root position.
        /// </summary>
        public long RootPos { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tree has changed.
        /// </summary>
        public bool TreeDirty { get; set; }

        /// <summary>
        /// Gets or sets the integer version.
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}