// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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

namespace DotSpatial.Data
{
    /// <summary>
    /// The header for the .shp file and the .shx file that support shapefiles
    /// </summary>
    public class ShapefileHeader
    {
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
        /// Resets all the extent values to 0
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
            using (var bbReader = new FileStream(inFilename, FileMode.Open))
            {

                // Reading BigEndian simply requires us to reverse the byte order.
                FileCode = bbReader.ReadInt32(Endian.BigEndian);

                // Skip the next 20 bytes because they are unused
                bbReader.Seek(20, SeekOrigin.Current);

                // Read the file length in reverse sequence
                FileLength = bbReader.ReadInt32(Endian.BigEndian);

                // From this point on, all the header values are in little Endean

                // Read the version
                Version = bbReader.ReadInt32();

                // Read in the shape type that should be the shape type for the whole shapefile
                ShapeType = (ShapeType) bbReader.ReadInt32();

                // Get the extents, each of which are double values.
                Xmin = bbReader.ReadDouble();
                Ymin = bbReader.ReadDouble();
                Xmax = bbReader.ReadDouble();
                Ymax = bbReader.ReadDouble();
                Zmin = bbReader.ReadDouble();
                Zmax = bbReader.ReadDouble();
                Mmin = bbReader.ReadDouble();
                Mmax = bbReader.ReadDouble();
            }
            
            var fi = new FileInfo(ShxFilename);
            if (fi.Exists)
            {
                ShxLength = Convert.ToInt32(fi.Length / 2); //length is in 16 bit words.
            }
        }

        /// <summary>
        /// Saves changes to the .shp file will also automatically update the .shx file.
        /// </summary>
        public void Save()
        {
            SaveAs(Filename);
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
        /// Writes the current content to the specified file.
        /// </summary>
        /// <param name="destFilename">The string fileName to write to</param>
        /// <param name="destFileLength">The only difference between the shp header and the
        ///  shx header is the file length parameter.</param>
        private void Write(string destFilename, int destFileLength)
        {
            var dir = Path.GetDirectoryName(Path.GetFullPath(Filename));
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var fs = new FileStream(destFilename, FileMode.Create))
            {
                fs.WriteBe(FileCode);        //  Byte 0          File Code       9994        Integer     Big
                var bt = new byte[20];
                fs.Write(bt, 0, 20);          //  Bytes 4 - 20 are unused
                fs.WriteBe(destFileLength);   //  Byte 24         File Length     File Length Integer     Big
                fs.WriteLe(Version);         //  Byte 28         Version         1000        Integer     Little
                fs.WriteLe((int) ShapeType); //  Byte 32         Shape Type      Shape Type  Integer     Little
                fs.WriteLe(Xmin);            //  Byte 36         Bounding Box    Xmin        Double      Little
                fs.WriteLe(Ymin);            //  Byte 44         Bounding Box    Ymin        Double      Little
                fs.WriteLe(Xmax);            //  Byte 52         Bounding Box    Xmax        Double      Little
                fs.WriteLe(Ymax);            //  Byte 60         Bounding Box    Ymax        Double      Little
                fs.WriteLe(Zmin);            //  Byte 68         Bounding Box    Zmin        Double      Little
                fs.WriteLe(Zmax);            //  Byte 76         Bounding Box    Zmax        Double      Little
                fs.WriteLe(Mmin);            //  Byte 84         Bounding Box    Mmin        Double      Little
                fs.WriteLe(Mmax);            //  Byte 92         Bounding Box    Mmax        Double      Little
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer file code that should always be 9994.
        /// </summary>
        public int FileCode { get; set; }

        /// <summary>
        /// Gets or sets the integer file length in bytes
        /// </summary>
        public int FileLength { get; set; }

        /// <summary>
        /// Gets or sets the string fileName to use for this header
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the DotSpatial.Data.Shapefiles.ShapeType enumeration specifying
        /// whether the shapes are points, lines, polygons etc.
        /// </summary>
        public ShapeType ShapeType { get; set; }

        /// <summary>
        /// Changes the extension of the fileName to .shx instead of .shp
        /// </summary>
        public string ShxFilename
        {
            get { return Path.ChangeExtension(Filename, ".shx"); }
        }

        /// <summary>
        /// Gets or sets the version, which should be 1000
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The minimum X coordinate for the values in the shapefile
        /// </summary>
        public double Xmin { get; set; }

        /// <summary>
        /// The maximum X coordinate for the shapes in the shapefile
        /// </summary>
        public double Xmax { get; set; }

        /// <summary>
        /// The minimum Y coordinate for the values in the shapefile
        /// </summary>
        public double Ymin { get; set; }

        /// <summary>
        /// The maximum Y coordinate for the shapes in the shapefile
        /// </summary>
        public double Ymax { get; set; }

        /// <summary>
        /// The minimum Z coordinate for the values in the shapefile
        /// </summary>
        public double Zmin { get; set; }

        /// <summary>
        /// The maximum Z coordinate for the shapes in the shapefile
        /// </summary>
        public double Zmax { get; set; }

        /// <summary>
        /// The minimum M coordinate for the values in the shapefile
        /// </summary>
        public double Mmin { get; set; }

        /// <summary>
        /// The maximum M coordinate for the shapes in the shapefile
        /// </summary>
        public double Mmax { get; set; }

        /// <summary>
        /// Gets or sets the length of the shx file in 16 bit words.
        /// </summary>
        public int ShxLength { get; set; }

        #endregion
    }
}