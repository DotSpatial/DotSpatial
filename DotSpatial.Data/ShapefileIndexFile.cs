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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/9/2010.
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
    /// Index file class for the .shx file
    /// </summary>
    public class ShapefileIndexFile
    {
        /// <summary>
        /// Gets or sets the header
        /// </summary>
        public ShapefileHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the fileName
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the list of shape headers
        /// </summary>
        public List<ShapeHeader> Shapes { get; set; }

        /// <summary>
        /// Opens the index file of the specified fileName.  If the fileName is not the .shx extension,
        /// then the fileName will be changed too that extension first.
        /// </summary>
        /// <param name="fileName"></param>
        public void Open(string fileName)
        {
            Filename = Path.ChangeExtension(fileName, ".shx");
            Header = new ShapefileHeader(Filename);
            Shapes = ReadIndexFile(Filename);
        }

        /// <summary>
        /// Saves the file back to the original file, or the currently specified "Filename" property.
        /// </summary>
        public void Save()
        {
            WriteHeader(Header, Filename, Shapes.Count);
            FileStream bw = new FileStream(Filename, FileMode.Append);
            try
            {
                foreach (ShapeHeader shape in Shapes)
                {
                    bw.WriteBe(shape.Offset);
                    bw.WriteBe(shape.ContentLength);
                }
            }
            finally
            {
                bw.Close();
            }
        }

        /// <summary>
        /// Saves the file to the specified fileName.  If the extension is not a correct shx extension,
        /// it will be changed to that extensions.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName)
        {
            Filename = Path.ChangeExtension(fileName, ".shx");
            Save();
        }

        /// <summary>
        /// Reads the entire index file in order to get a breakdown of how shapes are broken up.
        /// </summary>
        /// <param name="fileName">A string fileName of the .shx file to read.</param>
        /// <returns>A List of ShapeHeaders that give offsets and lengths so that reading can be optimized</returns>
        protected List<ShapeHeader> ReadIndexFile(string fileName)
        {
            string shxFilename = fileName;
            string ext = Path.GetExtension(fileName);

            if (ext != ".shx")
            {
                shxFilename = Path.ChangeExtension(fileName, ".shx");
            }
            if (shxFilename == null)
            {
                throw new NullReferenceException(fileName);
            }
            if (File.Exists(shxFilename) == false)
            {
                throw new FileNotFoundException(fileName);
            }

            // This will store the header elements that we read from the file.
            List<ShapeHeader> result = new List<ShapeHeader>();

            // Use a the length of the file to dimension the byte array
            BufferedBinaryReader bbReader = new BufferedBinaryReader(shxFilename);

            if (bbReader.FileLength == 100)
            {
                // the file is empty, so we are done
                bbReader.Close();
                return result;
            }

            // Skip the header and begin reading from the first record
            bbReader.Seek(100, SeekOrigin.Begin);

            Header.ShxLength = (int)bbReader.FileLength / 2;
            long length = bbReader.FileLength - 100;

            long numRecords = length / 8; // Each record consists of 2 Big-endian integers for a total of 8 bytes.
            for (long i = 0; i < numRecords; i++)
            {
                ShapeHeader sh = new ShapeHeader
                                     {
                                         Offset = bbReader.ReadInt32(false),
                                         ContentLength = bbReader.ReadInt32(false)
                                     };
                result.Add(sh);
            }
            bbReader.Close();
            return result;
        }

        /// <summary>
        /// Writes the current content to the specified file.
        /// </summary>
        /// <param name="header">The header to write</param>
        /// <param name="fileName">Basically the same code can be used for the shp and shx files</param>
        /// <param name="numShapes">The integer number of shapes to write to the file</param>
        private static void WriteHeader(ShapefileHeader header, string fileName, int numShapes)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (dir != null)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }

            FileStream bbWriter = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None, 100);

            bbWriter.WriteBe(header.FileCode);                     //  Byte 0          File Code       9994        Integer     Big

            byte[] bt = new byte[20];
            bbWriter.Write(bt, 0, 20);                                   //  Bytes 4 - 20 are unused

            // This is overwritten later
            bbWriter.WriteBe(50 + 4 * numShapes);                //  Byte 24         File Length     File Length Integer     Big

            bbWriter.WriteLe(header.Version);                             //  Byte 28         Version         1000        Integer     Little

            bbWriter.WriteLe((int)header.ShapeType);                      //  Byte 32         Shape Type      Shape Type  Integer     Little

            bbWriter.WriteLe(header.Xmin);                                //  Byte 36         Bounding Box    Xmin        Double      Little

            bbWriter.WriteLe(header.Ymin);                                //  Byte 44         Bounding Box    Ymin        Double      Little

            bbWriter.WriteLe(header.Xmax);                                //  Byte 52         Bounding Box    Xmax        Double      Little

            bbWriter.WriteLe(header.Ymax);                                //  Byte 60         Bounding Box    Ymax        Double      Little

            bbWriter.WriteLe(header.Zmin);                                //  Byte 68         Bounding Box    Zmin        Double      Little

            bbWriter.WriteLe(header.Zmax);                                //  Byte 76         Bounding Box    Zmax        Double      Little

            bbWriter.WriteLe(header.Mmin);                                //  Byte 84         Bounding Box    Mmin        Double      Little

            bbWriter.WriteLe(header.Mmax);                                //  Byte 92         Bounding Box    Mmax        Double      Little

            // ------------ WRITE TO SHP FILE -------------------------

            bbWriter.Close();
        }
    }
}