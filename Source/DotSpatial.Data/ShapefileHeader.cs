// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// The header for the .shp file and the .shx file that support shapefiles
    /// </summary>
    public class ShapefileHeader
    {
        #region Private Variables

        // Always 9994 if it is a shapefile
        private int _fileCode;

        // The length of the shp file in bytes
        private int _fileLength;
        private string _fileName;
        private double _mMax;
        private double _mMin;

        // The version, which should be 1000

        // Specifies line, polygon, point etc.
        private ShapeType _shapeType;
        private int _shxLength;
        private int _version;

        // Extent Values for the entire shapefile
        private double _xMax;
        private double _xMin;
        private double _yMax;
        private double _yMin;
        private double _zMax;
        private double _zMin;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new, blank ShapeHeader
        /// </summary>
        public ShapefileHeader()
        {
            Clear();
        }

        /// <summary>
        /// Opens the specified fileName directly
        /// </summary>
        /// <param name="inFilename">The string fileName to open as a header.</param>
        public ShapefileHeader(string inFilename)
        {
            Open(inFilename);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all the extent values to 0.
        /// </summary>
        public void Clear()
        {
            _version = 1000;
            _fileCode = 9994;
            _xMin = 0.0;
            _xMax = 0.0;
            _yMin = 0.0;
            _yMax = 0.0;
            _zMin = 0.0;
            _zMax = 0.0;
            _mMin = 0.0;
            _mMax = 0.0;
        }

        /// <summary>
        /// Exports the shapefile header for the shp file to a stream.
        /// </summary>
        /// <returns>Stream that contains the header for the shp file.</returns>
        public Stream ExportShpHeaderToStream()
        {
            Stream s = new MemoryStream();
            WriteToStream(_fileLength, s);
            return s;
        }

        /// <summary>
        /// Exports the shapefile header for the shx file to a stream.
        /// </summary>
        /// <returns>Stream, that contains the header for the shx file.</returns>
        public Stream ExportShxHeaderToStream()
        {
            Stream s = new MemoryStream();
            WriteToStream(_shxLength, s);
            return s;
        }

        /// <summary>
        /// Parses the first 100 bytes of a shapefile into the important values
        /// </summary>
        /// <param name="inFilename">The fileName to read</param>
        public void Open(string inFilename)
        {
            Filename = inFilename;

            //  Position        Field           Value       Type        ByteOrder
            //  --------------------------------------------------------------
            //  Byte 0          File Code       9994        Integer     Big
            //  Byte 4          Unused          0           Integer     Big
            //  Byte 8          Unused          0           Integer     Big
            //  Byte 12         Unused          0           Integer     Big
            //  Byte 16         Unused          0           Integer     Big
            //  Byte 20         Unused          0           Integer     Big
            //  Byte 24         File Length     File Length Integer     Big
            //  Byte 28         Version         1000        Integer     Little
            //  Byte 32         Shape Type      Shape Type  Integer     Little
            //  Byte 36         Bounding Box    Xmin        Double      Little
            //  Byte 44         Bounding Box    Ymin        Double      Little
            //  Byte 52         Bounding Box    Xmax        Double      Little
            //  Byte 60         Bounding Box    Ymax        Double      Little
            //  Byte 68         Bounding Box    Zmin        Double      Little
            //  Byte 76         Bounding Box    Zmax        Double      Little
            //  Byte 84         Bounding Box    Mmin        Double      Little
            //  Byte 92         Bounding Box    Mmax        Double      Little

            // This may throw an IOException if the file is already in use.
            BufferedBinaryReader bbReader = new BufferedBinaryReader(Filename);

            bbReader.FillBuffer(100); // we only need to read 100 bytes from the header.

            bbReader.Close(); // Close the internal readers connected to the file, but don't close the file itself.

            // Reading BigEndian simply requires us to reverse the byte order.
            _fileCode = bbReader.ReadInt32(false);

            // Skip the next 20 bytes because they are unused
            bbReader.Seek(20, SeekOrigin.Current);

            // Read the file length in reverse sequence
            _fileLength = bbReader.ReadInt32(false);

            // From this point on, all the header values are in little Endean

            // Read the version
            _version = bbReader.ReadInt32();

            // Read in the shape type that should be the shape type for the whole shapefile
            _shapeType = (ShapeType)bbReader.ReadInt32();

            // Get the extents, each of which are double values.
            _xMin = bbReader.ReadDouble();
            _yMin = bbReader.ReadDouble();
            _xMax = bbReader.ReadDouble();
            _yMax = bbReader.ReadDouble();
            _zMin = bbReader.ReadDouble();
            _zMax = bbReader.ReadDouble();
            _mMin = bbReader.ReadDouble();
            _mMax = bbReader.ReadDouble();

            bbReader.Dispose();

            FileInfo fi = new FileInfo(ShxFilename);
            if (fi.Exists)
            {
                _shxLength = Convert.ToInt32(fi.Length / 2); //length is in 16 bit words.
            }
        }

        /// <summary>
        /// Saves changes to the .shp file will also automatically update the .shx file.
        /// </summary>
        public void Save()
        {
            SaveAs(_fileName);
        }

        /// <summary>
        /// Saves changes to the .shp file and will also automatically create the header for the .shx file.
        /// This will no longer automatically delete an existing shapefile.
        /// </summary>
        /// <param name="outFilename">The string fileName to create.</param>
        public void SaveAs(string outFilename)
        {
            Filename = outFilename;
            Write(Filename, _fileLength);
            Write(ShxFilename, _shxLength);
        }

        /// <summary>
        /// Writes the current content to the specified file.
        /// </summary>
        /// <param name="destFilename">The string fileName to write to.</param>
        /// <param name="destFileLength">The only difference between the shp header and the
        ///  shx header is the file length parameter.</param>
        private void Write(string destFilename, int destFileLength)
        {
            var dir = Path.GetDirectoryName(Filename);
            if (dir != null && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var fs = new FileStream(destFilename, FileMode.Append, FileAccess.Write, FileShare.None))
            {
                WriteToStream(destFileLength, fs);
                fs.Close();
            }
        }

        /// <summary>
        /// Writes the current content to the given stream.
        /// </summary>
        /// <param name="destFileLength">The only difference between the shp header and the
        ///  shx header is the file length parameter.</param>
        /// <param name="stream">Stream the content is written to.</param>
        private void WriteToStream(int destFileLength, Stream stream)
        {
            stream.WriteBe(_fileCode);       //  Byte 0          File Code       9994        Integer     Big

            byte[] bt = new byte[20];
            stream.Write(bt, 0, 20);         //  Bytes 4 - 20 are unused

            stream.WriteBe(destFileLength);  //  Byte 24         File Length     File Length Integer     Big
            stream.WriteLe(_version);        //  Byte 28         Version         1000        Integer     Little
            stream.WriteLe((int)_shapeType); //  Byte 32         Shape Type      Shape Type  Integer     Little
            stream.WriteLe(_xMin);           //  Byte 36         Bounding Box    Xmin        Double      Little
            stream.WriteLe(_yMin);           //  Byte 44         Bounding Box    Ymin        Double      Little
            stream.WriteLe(_xMax);           //  Byte 52         Bounding Box    Xmax        Double      Little
            stream.WriteLe(_yMax);           //  Byte 60         Bounding Box    Ymax        Double      Little
            stream.WriteLe(_zMin);           //  Byte 68         Bounding Box    Zmin        Double      Little
            stream.WriteLe(_zMax);           //  Byte 76         Bounding Box    Zmax        Double      Little
            stream.WriteLe(_mMin);           //  Byte 84         Bounding Box    Mmin        Double      Little
            stream.WriteLe(_mMax);           //  Byte 92         Bounding Box    Mmax        Double      Little
        }

        #endregion

        /// <summary>
        /// Gets or sets the integer file code that should always be 9994.
        /// </summary>
        public int FileCode
        {
            get { return _fileCode; }
            set
            {
                _fileCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer file length in bytes
        /// </summary>
        public int FileLength
        {
            get { return _fileLength; }
            set
            {
                _fileLength = value;
            }
        }

        /// <summary>
        /// Gets or sets the string fileName to use for this header. If a relative path gets assigned it is changed to the absolute path including the file extension.
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = Path.GetFullPath(value); }
        }

        /// <summary>
        /// Gets or sets the DotSpatial.Data.Shapefiles.ShapeType enumeration specifying
        /// whether the shapes are points, lines, polygons etc.
        /// </summary>
        public ShapeType ShapeType
        {
            get { return _shapeType; }
            set
            {
                _shapeType = value;
            }
        }

        /// <summary>
        /// Changes the extension of the fileName to .shx instead of .shp
        /// </summary>
        public string ShxFilename
        {
            get { return Path.ChangeExtension(_fileName, ".shx"); }
        }

        /// <summary>
        /// Gets or sets the version, which should be 1000
        /// </summary>
        public int Version
        {
            get { return _version; }
            set
            {
                _version = value;
            }
        }

        /// <summary>
        /// The minimum X coordinate for the values in the shapefile
        /// </summary>
        public double Xmin
        {
            get { return _xMin; }
            set
            {
                _xMin = value;
            }
        }

        /// <summary>
        /// The maximum X coordinate for the shapes in the shapefile
        /// </summary>
        public double Xmax
        {
            get { return _xMax; }
            set
            {
                _xMax = value;
            }
        }

        /// <summary>
        /// The minimum Y coordinate for the values in the shapefile
        /// </summary>
        public double Ymin
        {
            get { return _yMin; }
            set
            {
                _yMin = value;
            }
        }

        /// <summary>
        /// The maximum Y coordinate for the shapes in the shapefile
        /// </summary>
        public double Ymax
        {
            get { return _yMax; }
            set
            {
                _yMax = value;
            }
        }

        /// <summary>
        /// The minimum Z coordinate for the values in the shapefile
        /// </summary>
        public double Zmin
        {
            get { return _zMin; }
            set
            {
                _zMin = value;
            }
        }

        /// <summary>
        /// The maximum Z coordinate for the shapes in the shapefile
        /// </summary>
        public double Zmax
        {
            get { return _zMax; }
            set
            {
                _zMax = value;
            }
        }

        /// <summary>
        /// The minimum M coordinate for the values in the shapefile
        /// </summary>
        public double Mmin
        {
            get { return _mMin; }
            set
            {
                _mMin = value;
            }
        }

        /// <summary>
        /// The maximum M coordinate for the shapes in the shapefile
        /// </summary>
        public double Mmax
        {
            get { return _mMax; }
            set
            {
                _mMax = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the shx file in 16 bit words.
        /// </summary>
        public int ShxLength
        {
            get { return _shxLength; }
            set { _shxLength = value; }
        }

        /// <summary>
        /// Generates a new envelope based on the extents of this shapefile.
        /// </summary>
        /// <returns>An Envelope</returns>
        public Envelope ToEnvelope()
        {
            Envelope env = new Envelope(_xMin, _xMax, _yMin, _yMax);
            env.InitM(_mMin, _mMax);
            env.InitZ(_zMin, _zMax);
            return env;
        }

        /// <summary>
        /// Generates a new extent from the shape header.  This will infer the whether the ExtentMZ, ExtentM
        /// or Extent class is the best implementation.  Casting is required to access the higher
        /// values from the Extent return type.
        /// </summary>
        /// <returns>Extent, which can be Extent, ExtentM, or ExtentMZ</returns>
        public Extent ToExtent()
        {
            if (ShapeType == ShapeType.MultiPointZ ||
                ShapeType == ShapeType.PointZ ||
                ShapeType == ShapeType.PolygonZ ||
                ShapeType == ShapeType.PolyLineZ)
            {
                return new ExtentMZ(_xMin, _yMin, _mMin, _zMin, _xMax, _yMax, _mMax, _zMax);
            }
            if (ShapeType == ShapeType.MultiPointM ||
                ShapeType == ShapeType.PointM ||
                ShapeType == ShapeType.PolygonM ||
                ShapeType == ShapeType.PolyLineM)
            {
                return new ExtentM(_xMin, _yMin, _mMin, _xMax, _yMax, _mMax);
            }
            Extent ext = new Extent(_xMin, _yMin, _xMax, _yMax);

            return ext;
        }

        /// <summary>
        /// The shape type is assumed to be fixed, and will control how the input extent is treated as far
        /// as M and Z values, rather than updating the shape type based on the extent.
        /// </summary>
        public void SetExtent(IExtent extent)
        {
            IExtentZ zExt = extent as ExtentMZ;
            IExtentM mExt = extent as ExtentM;
            if ((ShapeType == ShapeType.MultiPointZ ||
                 ShapeType == ShapeType.PointZ ||
                 ShapeType == ShapeType.PolygonZ ||
                 ShapeType == ShapeType.PolyLineZ))
            {
                if (zExt == null || extent.HasZ == false)
                {
                    _zMin = double.MaxValue;
                    _zMax = double.MinValue;
                }
                else
                {
                    _zMin = zExt.MinZ;
                    _zMax = zExt.MaxZ;
                }
            }
            if (ShapeType == ShapeType.MultiPointM ||
                ShapeType == ShapeType.PointM ||
                ShapeType == ShapeType.PolygonM ||
                ShapeType == ShapeType.PolyLineM)
            {
                if (mExt == null || extent.HasM == false)
                {
                    _mMin = double.MaxValue;
                    _mMax = double.MinValue;
                }
                else
                {
                    _mMin = mExt.MinM;
                    _mMax = mExt.MaxM;
                }
            }
            _xMin = extent.MinX;
            _xMax = extent.MaxX;
            _yMin = extent.MinY;
            _yMax = extent.MaxY;
        }
    }
}