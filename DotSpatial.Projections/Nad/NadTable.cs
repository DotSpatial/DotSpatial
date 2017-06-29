// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 1:47:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DotSpatial.Projections
{
    /// <summary>
    /// NadRecord is a single entry from an lla file
    /// </summary>
    [Serializable]
    public class NadTable
    {
        /// <summary>
        /// Converts degree values into radians
        /// </summary>
        protected const double DEG_TO_RAD = Math.PI / 180;

        /// <summary>
        /// I think this converts micro-seconds of arc to radians
        /// </summary>
        protected const double USecToRad = 4.848136811095359935899141023e-12;

        /// <summary>
        /// The delta lambda and delta phi for a single cell
        /// </summary>
        private PhiLam _cellSize;

        /// <summary>
        /// The set of conversion matrix coefficients for lambda
        /// </summary>
        private PhiLam[][] _cvs;

        private long _dataOffset;

        private bool _fileIsEmbedded;
        private bool _filled;
        private GridShiftTableFormat _format;

        private string _gridFilePath;

        /// <summary>
        /// The lower left coordinate
        /// </summary>
        private PhiLam _lowerLeft;

        private string _manifestResourceString;

        /// <summary>
        /// The character based id for this record
        /// </summary>
        private string _name;

        /// <summary>
        /// The total count of coordinates in the lambda direction
        /// </summary>
        private int _numLambdas;

        /// <summary>
        /// The total count of coordinates in the phi direction
        /// </summary>
        private int _numPhis;

        private List<NadTable> _subGrids;

        /// <summary>
        /// Creates a blank nad Table
        /// </summary>
        public NadTable()
        {
            _subGrids = new List<NadTable>();
        }

        /// <summary>
        /// This initializes a new table with the assumption that the offset is 0
        /// </summary>
        /// <param name="resourceLocation">The resource location</param>
        public NadTable(string resourceLocation)
            : this(resourceLocation, 0)
        {
        }

        /// <summary>
        /// This initializes a new table with the assumption that the offset is 0
        /// </summary>
        /// <param name="resourceLocation">The resource location</param>
        /// <param name="embedded">Indicates if grid file is in embedded resource or external file</param>
        public NadTable(string resourceLocation, bool embedded)
            : this(resourceLocation, 0, embedded)
        {
        }

        /// <summary>
        /// This initializes a new table with the assumption that the offset needs to be specified
        /// because in gsb files, more than one table is listed, and this is a subtable.
        /// </summary>
        /// <param name="resourceLocation">The resource location</param>
        /// <param name="offset">The offset marking the start of the header in the file</param>
        public NadTable(string resourceLocation, long offset)
            : this(resourceLocation, offset, true)
        {
        }

        /// <summary>
        /// This initializes a new table with the assumption that the offset needs to be specified
        /// because in gsb files, more than one table is listed, and this is a subtable. This also allows
        /// specifying if the grid file is included as an embedded resource.
        /// </summary>
        /// <param name="location">The resource (or file) location</param>
        /// <param name="offset">The offset marking the start of the header in the file</param>
        /// <param name="embedded">Indicates if embedded resource or external file</param>
        public NadTable(string location, long offset, bool embedded)
        {
            _subGrids = new List<NadTable>();

            if (embedded)
            {
                _manifestResourceString = location;
            }
            else
            {
                _gridFilePath = location;
            }
            _fileIsEmbedded = embedded;
            _dataOffset = offset;
        }

        /// <summary>
        /// Given the resource setup, this causes the file to read the
        /// </summary>
        public virtual void ReadHeader()
        {
            // This code is implemented differently in subclasses
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void FillData()
        {
            // THis code is implemented differently in subclasses
        }

        /// <summary>
        /// This method parses the extension of a resource location or
        /// path and creates the new NadTable type.
        /// </summary>
        /// <param name="resourceLocation"></param>
        /// <returns></returns>
        public static NadTable FromSourceName(string resourceLocation)
        {
            return FromSourceName(resourceLocation, true);
        }

        /// <summary>
        /// This method parses the extension of a resource location or
        /// path and creates the new NadTable type.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="embedded"></param>
        /// <returns></returns>
        public static NadTable FromSourceName(string location, bool embedded)
        {
            NadTable result = null;
            string ext = Path.GetExtension(location).ToLower();
            switch (ext)
            {
                case ".lla":
                    result = new LlaNadTable(location, embedded);
                    break;
                case ".gsb":
                    result = new GsbNadTable(location, 0, embedded);
                    break;
                case ".dat":
                    result = new DatNadTable(location, embedded);
                    break;
                case ".los":
                    result = new LasLosNadTable(location, embedded);
                    break;
            }
            if (result != null) result.ReadHeader();
            return result;
        }

        #region Methods

        /// <summary>
        /// Gets or sets the string name for this record
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the lower left corner in radians
        /// </summary>
        public PhiLam LowerLeft
        {
            get { return _lowerLeft; }
            set { _lowerLeft = value; }
        }

        /// <summary>
        /// Gets or sets the angular cell size in radians
        /// </summary>
        public PhiLam CellSize
        {
            get { return _cellSize; }
            set { _cellSize = value; }
        }

        /// <summary>
        /// Gets or sets the integer count of phi coefficients
        /// </summary>
        public int NumPhis
        {
            get { return _numPhis; }
            set { _numPhis = value; }
        }

        /// <summary>
        /// Gets or sets the integer count of lambda coefficients
        /// </summary>
        public int NumLambdas
        {
            get { return _numLambdas; }
            set { _numLambdas = value; }
        }

        /// <summary>
        /// These represent smaller, higher resolution subgrids that should fall within the extents
        /// of the larger grid.  If this list exists, and there is a fit here, it should be used
        /// in preference to the low-resolution main grid.
        /// </summary>
        public List<NadTable> SubGrids
        {
            get { return _subGrids; }
            set { _subGrids = value; }
        }

        /// <summary>
        /// Gets or sets the array of lambda coefficients organized
        /// in a spatial Table (phi major)
        /// </summary>
        public PhiLam[][] Cvs
        {
            get { return _cvs; }
            set { _cvs = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the values have been filled.
        /// </summary>
        public bool Filled
        {
            get { return _filled; }
            set { _filled = value; }
        }

        /// <summary>
        /// Gets or sets the location this table should look for source data.
        /// </summary>
        public string ManifestResourceString
        {
            get { return _manifestResourceString; }
            set { _manifestResourceString = value; }
        }

        /// <summary>
        /// Gets or sets the long integer data offset where the stream should skip to to begin reading data
        /// </summary>
        public long DataOffset
        {
            get { return _dataOffset; }
            set { _dataOffset = value; }
        }

        /// <summary>
        /// Gets or sets the format being used.
        /// </summary>
        public GridShiftTableFormat Format
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// True if grid file is an embedded resource
        /// </summary>
        public bool FileIsEmbedded
        {
            get { return _fileIsEmbedded; }
            set { _fileIsEmbedded = value; }
        }

        /// <summary>
        /// If FileIsEmbedded is false, this contains the full path to the grid file
        /// </summary>
        public string GridFilePath
        {
            get { return _gridFilePath; }
            set { _gridFilePath = value; }
        }

        /// <summary>
        /// Reads a double in BigEndian format (consistent with ntv1 and ntv2 formats.)
        /// </summary>
        /// <param name="br">The binary reader</param>
        /// <returns>The double value parsed from the binary in big endian byte order.</returns>
        protected static double ReadDouble(BinaryReader br)
        {
            byte[] temp = new byte[8];
            br.Read(temp, 0, 8);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(temp);
            }
            double value = BitConverter.ToDouble(temp, 0);
            return value;
        }

        /// <summary>
        /// Gets the double value from the specified position in the byte array
        /// Using BigEndian format.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected virtual double GetDouble(byte[] array, int offset)
        {
            byte[] bValue = new byte[8];
            Array.Copy(array, offset, bValue, 0, 8);
            if (BitConverter.IsLittleEndian) Array.Reverse(bValue);
            return BitConverter.ToDouble(bValue, 0);
        }

        /// <summary>
        /// Get the stream to the grid
        /// </summary>
        /// <returns></returns>
        protected Stream GetStream()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream str = FileIsEmbedded ? a.GetManifestResourceStream(_manifestResourceString) : File.Open(_gridFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return str;
        }

        #endregion
    }
}