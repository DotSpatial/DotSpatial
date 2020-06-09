// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// The header for the .shp file and the .shx file that support shapefiles.
    /// </summary>
    public class ShapefileHeader
    {
        #region Fields

        // Always 9994 if it is a shapefile

        // The length of the shp file in bytes
        private string _fileName;

        // The version, which should be 1000

        // Specifies line, polygon, point etc.

        // Extent Values for the entire shapefile
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileHeader"/> class.
        /// </summary>
        public ShapefileHeader()
        {
            Clear();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShapefileHeader"/> class.
        /// </summary>
        /// <param name="inFilename">The string fileName to open as a header.</param>
        public ShapefileHeader(string inFilename)
        {
            Open(inFilename);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer file code that should always be 9994.
        /// </summary>
        public int FileCode { get; set; }

        /// <summary>
        /// Gets or sets the integer file length in bytes.
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// Gets or sets the string fileName to use for this header. If a relative path gets assigned it is changed to the absolute path including the file extension.
        /// </summary>
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = Path.GetFullPath(value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum M coordinate for the shapes in the shapefile.
        /// </summary>
        public double Mmax { get; set; }

        /// <summary>
        ///  Gets or sets tminimum M coordinate for the values in the shapefile.
        /// </summary>
        public double Mmin { get; set; }

        /// <summary>
        /// Gets or sets the DotSpatial.Data.Shapefiles.ShapeType enumeration specifying
        /// whether the shapes are points, lines, polygons etc.
        /// </summary>
        public ShapeType ShapeType { get; set; }

        /// <summary>
        /// Gets the file name of the shx file. This changes the extension of the fileName to .shx instead of .shp.
        /// </summary>
        public string ShxFilename => Path.ChangeExtension(_fileName, ".shx");

        /// <summary>
        /// Gets or sets the length of the shx file in 16 bit words.
        /// </summary>
        public int ShxLength { get; set; }

        /// <summary>
        /// Gets or sets the version, which should be 1000.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the maximum X coordinate for the shapes in the shapefile.
        /// </summary>
        public double Xmax { get; set; }

        /// <summary>
        /// Gets or sets the minimum X coordinate for the values in the shapefile.
        /// </summary>
        public double Xmin { get; set; }

        /// <summary>
        /// Gets or sets tmaximum Y coordinate for the shapes in the shapefile.
        /// </summary>
        public double Ymax { get; set; }

        /// <summary>
        /// Gets or sets tminimum Y coordinate for the values in the shapefile.
        /// </summary>
        public double Ymin { get; set; }

        /// <summary>
        /// Gets or sets tmaximum Z coordinate for the shapes in the shapefile.
        /// </summary>
        public double Zmax { get; set; }

        /// <summary>
        /// Gets or sets the minimum Z coordinate for the values in the shapefile.
        /// </summary>
        public double Zmin { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Resets all the extent values to 0.
        /// </summary>
        public void Clear()
        {
            Version = 1000;
            FileCode = 9994;
            Xmin = 0.0;
            Xmax = 0.0;
            Ymin = 0.0;
            Ymax = 0.0;
            Zmin = 0.0;
            Zmax = 0.0;
            Mmin = 0.0;
            Mmax = 0.0;
        }

        /// <summary>
        /// Exports the shapefile header for the shp file to a stream.
        /// </summary>
        /// <returns>Stream that contains the header for the shp file.</returns>
        public Stream ExportShpHeaderToStream()
        {
            Stream s = new MemoryStream();
            WriteToStream(FileLength, s);
            return s;
        }

        /// <summary>
        /// Exports the shapefile header for the shx file to a stream.
        /// </summary>
        /// <returns>Stream, that contains the header for the shx file.</returns>
        public Stream ExportShxHeaderToStream()
        {
            Stream s = new MemoryStream();
            WriteToStream(ShxLength, s);
            return s;
        }

        /// <summary>
        /// Parses the first 100 bytes of a shapefile into the important values.
        /// </summary>
        /// <param name="inFilename">The fileName to read.</param>
        public void Open(string inFilename)
        {
            Filename = inFilename;

            // Position        Field           Value       Type        ByteOrder
            // --------------------------------------------------------------
            // Byte 0          File Code       9994        Integer     Big
            // Byte 4          Unused          0           Integer     Big
            // Byte 8          Unused          0           Integer     Big
            // Byte 12         Unused          0           Integer     Big
            // Byte 16         Unused          0           Integer     Big
            // Byte 20         Unused          0           Integer     Big
            // Byte 24         File Length     File Length Integer     Big
            // Byte 28         Version         1000        Integer     Little
            // Byte 32         Shape Type      Shape Type  Integer     Little
            // Byte 36         Bounding Box    Xmin        Double      Little
            // Byte 44         Bounding Box    Ymin        Double      Little
            // Byte 52         Bounding Box    Xmax        Double      Little
            // Byte 60         Bounding Box    Ymax        Double      Little
            // Byte 68         Bounding Box    Zmin        Double      Little
            // Byte 76         Bounding Box    Zmax        Double      Little
            // Byte 84         Bounding Box    Mmin        Double      Little
            // Byte 92         Bounding Box    Mmax        Double      Little

            // This may throw an IOException if the file is already in use.
            BufferedBinaryReader bbReader = new BufferedBinaryReader(Filename);

            bbReader.FillBuffer(100); // we only need to read 100 bytes from the header.

            bbReader.Close(); // Close the internal readers connected to the file, but don't close the file itself.

            // Reading BigEndian simply requires us to reverse the byte order.
            FileCode = bbReader.ReadInt32(false);

            // Skip the next 20 bytes because they are unused
            bbReader.Seek(20, SeekOrigin.Current);

            // Read the file length in reverse sequence
            FileLength = bbReader.ReadInt32(false);

            // From this point on, all the header values are in little Endean

            // Read the version
            Version = bbReader.ReadInt32();

            // Read in the shape type that should be the shape type for the whole shapefile
            ShapeType = (ShapeType)bbReader.ReadInt32();

            // Get the extents, each of which are double values.
            Xmin = bbReader.ReadDouble();
            Ymin = bbReader.ReadDouble();
            Xmax = bbReader.ReadDouble();
            Ymax = bbReader.ReadDouble();
            Zmin = bbReader.ReadDouble();
            Zmax = bbReader.ReadDouble();
            Mmin = bbReader.ReadDouble();
            Mmax = bbReader.ReadDouble();

            bbReader.Dispose();

            FileInfo fi = new FileInfo(ShxFilename);
            if (fi.Exists)
            {
                ShxLength = Convert.ToInt32(fi.Length / 2); // length is in 16 bit words.
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
            Write(Filename, FileLength);
            Write(ShxFilename, ShxLength);
        }

        /// <summary>
        /// The shape type is assumed to be fixed, and will control how the input extent is treated as far
        /// as M and Z values, rather than updating the shape type based on the extent.
        /// </summary>
        /// <param name="extent">The extent.</param>
        public void SetExtent(IExtent extent)
        {
            IExtentZ zExt = extent as ExtentMz;
            IExtentM mExt = extent as ExtentM;
            if (ShapeType == ShapeType.MultiPointZ || ShapeType == ShapeType.PointZ || ShapeType == ShapeType.PolygonZ || ShapeType == ShapeType.PolyLineZ)
            {
                if (zExt == null || extent.HasZ == false)
                {
                    Zmin = double.MaxValue;
                    Zmax = double.MinValue;
                }
                else
                {
                    Zmin = zExt.MinZ;
                    Zmax = zExt.MaxZ;
                }
            }

            if (ShapeType == ShapeType.MultiPointM || ShapeType == ShapeType.PointM || ShapeType == ShapeType.PolygonM || ShapeType == ShapeType.PolyLineM)
            {
                if (mExt == null || extent.HasM == false)
                {
                    Mmin = double.MaxValue;
                    Mmax = double.MinValue;
                }
                else
                {
                    Mmin = mExt.MinM;
                    Mmax = mExt.MaxM;
                }
            }

            Xmin = extent.MinX;
            Xmax = extent.MaxX;
            Ymin = extent.MinY;
            Ymax = extent.MaxY;
        }

        /// <summary>
        /// Generates a new envelope based on the extents of this shapefile.
        /// </summary>
        /// <returns>An Envelope.</returns>
        public Envelope ToEnvelope()
        {
            return new Envelope(Xmin, Xmax, Ymin, Ymax, Zmin, Zmax, Mmin, Mmax);
        }

        /// <summary>
        /// Generates a new extent from the shape header. This will infer the whether the ExtentMZ, ExtentM
        /// or Extent class is the best implementation. Casting is required to access the higher
        /// values from the Extent return type.
        /// </summary>
        /// <returns>Extent, which can be Extent, ExtentM, or ExtentMZ.</returns>
        public Extent ToExtent()
        {
            if (ShapeType == ShapeType.MultiPointZ || ShapeType == ShapeType.PointZ || ShapeType == ShapeType.PolygonZ || ShapeType == ShapeType.PolyLineZ)
            {
                return new ExtentMz(Xmin, Ymin, Mmin, Zmin, Xmax, Ymax, Mmax, Zmax);
            }

            if (ShapeType == ShapeType.MultiPointM || ShapeType == ShapeType.PointM || ShapeType == ShapeType.PolygonM || ShapeType == ShapeType.PolyLineM)
            {
                return new ExtentM(Xmin, Ymin, Mmin, Xmax, Ymax, Mmax);
            }

            Extent ext = new Extent(Xmin, Ymin, Xmax, Ymax);

            return ext;
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
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

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
            stream.WriteBe(FileCode);       // Byte 0          File Code       9994        Integer     Big

            byte[] bt = new byte[20];
            stream.Write(bt, 0, 20);        // Bytes 4 - 20 are unused
            stream.WriteBe(destFileLength); // Byte 24         File Length     File Length Integer     Big
            stream.WriteLe(Version);        // Byte 28         Version         1000        Integer     Little
            stream.WriteLe((int)ShapeType); //  Byte 32         Shape Type      Shape Type  Integer     Little
            stream.WriteLe(Xmin);           // Byte 36         Bounding Box    Xmin        Double      Little
            stream.WriteLe(Ymin);           // Byte 44         Bounding Box    Ymin        Double      Little
            stream.WriteLe(Xmax);           // Byte 52         Bounding Box    Xmax        Double      Little
            stream.WriteLe(Ymax);           // Byte 60         Bounding Box    Ymax        Double      Little
            stream.WriteLe(Zmin);           // Byte 68         Bounding Box    Zmin        Double      Little
            stream.WriteLe(Zmax);           // Byte 76         Bounding Box    Zmax        Double      Little
            stream.WriteLe(Mmin);           // Byte 84         Bounding Box    Mmin        Double      Little
            stream.WriteLe(Mmax);           // Byte 92         Bounding Box    Mmax        Double      Little
        }

        #endregion
    }
}