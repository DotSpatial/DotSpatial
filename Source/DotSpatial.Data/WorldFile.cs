// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// WorldFiles complement images, giving georeference information for those images. The basic idea is to calculate
    /// everything based on the top left corner of the image.
    /// </summary>
    public class WorldFile
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldFile"/> class.
        /// </summary>
        public WorldFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldFile"/> class based on the specified image fileName.
        /// </summary>
        /// <param name="imageFilename">Attempts to open the fileName for the world file for the image if it exists.</param>
        public WorldFile(string imageFilename)
        {
            Filename = GenerateFilename(imageFilename);
            if (File.Exists(imageFilename))
            {
                Open();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the coordinates in the Affine order:
        /// X' = [0] + [1] X + [2] Y
        /// Y' = [3] + [4] X + [5] Y
        /// </summary>
        public double[] Affine { get; set; }

        /// <summary>
        /// Gets or sets the cell height.
        /// </summary>
        public double CellHeight
        {
            get
            {
                return Affine[5];
            }

            set
            {
                Affine[5] = value;
            }
        }

        /// <summary>
        /// Gets or sets the cell width.
        /// </summary>
        public double CellWidth
        {
            get
            {
                return Affine[1];
            }

            set
            {
                Affine[1] = value;
            }
        }

        /// <summary>
        /// Gets a string list of file extensions that might apply to world files.
        /// </summary>
        public string DialogFilter
        {
            get
            {
                return "generic (*.wld)|*.wld|Bitmap (*.bpw)|*.bpw|EMF (*.efw)|*efw|Exif (*.exw)|GIF (*.gif)|*.gif|Icon (*.iow)|*.iow|JPEG (*.jgw)|*.jgw|Memory Bitmap (*.mpw)|*.mpw|PNG (*.pgw)|*.pgw|Tif (*.tfw)|*.tfw|WMF (*.wfw)|*.wfw";
            }
        }

        /// <summary>
        /// Gets or sets the fileName to use for this world file.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets how much the longitude or X position of a cell in the image depends on the row position of the cell.
        /// </summary>
        public double HorizontalSkew
        {
            get
            {
                return Affine[2];
            }

            set
            {
                Affine[2] = value;
            }
        }

        /// <summary>
        /// Gets or sets longitude or X position corresponding to the center of the cell in the top left corner of the image
        /// </summary>
        public double TopLeftX
        {
            get
            {
                return Affine[0];
            }

            set
            {
                Affine[0] = value;
            }
        }

        /// <summary>
        /// Gets or sets the latitude or Y position corresponding to the center of the cell in the top left corner of the image
        /// </summary>
        public double TopLeftY
        {
            get
            {
                return Affine[3];
            }

            set
            {
                Affine[3] = value;
            }
        }

        /// <summary>
        /// Gets or sets how much the latitude or Y position of a cell in the image depends on the column position of the cell.
        /// </summary>
        public double VerticalSkew
        {
            get
            {
                return Affine[4];
            }

            set
            {
                Affine[4] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given the fileName of an image, this creates a new fileName with the appropriate extension.
        /// This will also set the fileName of this world file to that extension.
        /// </summary>
        /// <param name="imageFilename">The fileName of the image</param>
        /// <returns>the fileName of the world file</returns>
        public static string GenerateFilename(string imageFilename)
        {
            string result = ".wld";
            string ext = Path.GetExtension(imageFilename)?.ToLower();
            switch (ext)
            {
                case ".bmp":
                    result = ".bpw";
                    break;
                case ".emf":
                    result = ".efw";
                    break;
                case ".exf":
                    result = ".exw";
                    break;
                case ".gif":
                    result = ".gfw";
                    break;
                case ".ico":
                    result = ".iow";
                    break;
                case ".jpg":
                    result = ".jgw";
                    break;
                case ".mbp":
                    result = ".mww";
                    break;
                case ".png":
                    result = ".pgw";
                    break;
                case ".tif":
                    result = ".tfw";
                    break;
                case ".wmf":
                    result = ".wft";
                    break;
            }

            return Path.ChangeExtension(imageFilename, result);
        }

        /// <summary>
        /// Returns the string extensions that accompanies one of the dot net image formats.
        /// </summary>
        /// <param name="format">The Imaging.ImageFormat for the image itself</param>
        /// <returns>The string extension</returns>
        public string GetExtension(ImageFormat format)
        {
            if (format == ImageFormat.Bmp) return ".bpw";
            if (format == ImageFormat.Emf) return ".efw";
            if (format == ImageFormat.Exif) return ".exw";
            if (format == ImageFormat.Gif) return ".gfw";
            if (format == ImageFormat.Icon) return ".iow";
            if (format == ImageFormat.Jpeg) return ".jgw";
            if (format == ImageFormat.MemoryBmp) return ".mpw";
            if (format == ImageFormat.Png) return ".pgw";
            if (format == ImageFormat.Tiff) return ".tfw";
            if (format == ImageFormat.Wmf) return ".wfw";
            return ".wld";
        }

        /// <summary>
        /// Opens the worldfile specified by the Filename property and loads the values
        /// </summary>
        public void Open()
        {
            if (File.Exists(Filename))
            {
                using (var sr = new StreamReader(Filename))
                {
                    Affine = new double[6];
                    Affine[1] = NextValue(sr); // Dx
                    Affine[2] = NextValue(sr); // Skew X
                    Affine[4] = NextValue(sr); // Skew Y
                    Affine[5] = NextValue(sr); // Dy
                    Affine[0] = NextValue(sr); // Top Left X
                    Affine[3] = NextValue(sr); // Top Left Y
                }
            }
        }

        /// <summary>
        /// Opens an existing worldfile based on the specified fileName.
        /// </summary>
        /// <param name="fileName">File name of the worldfile that should be opened.</param>
        public void Open(string fileName)
        {
            Filename = fileName;
            Open();
        }

        /// <summary>
        /// Saves the current affine coordinates to the current fileName
        /// </summary>
        public void Save()
        {
            if (File.Exists(Filename)) File.Delete(Filename);
            StreamWriter sw = new StreamWriter(Filename);
            sw.WriteLine(Affine[1].ToString(CultureInfo.InvariantCulture)); // Dx
            sw.WriteLine(Affine[2].ToString(CultureInfo.InvariantCulture)); // Skew X
            sw.WriteLine(Affine[4].ToString(CultureInfo.InvariantCulture)); // Skew Y
            sw.WriteLine(Affine[5].ToString(CultureInfo.InvariantCulture)); // Dy
            sw.WriteLine(Affine[0].ToString(CultureInfo.InvariantCulture)); // Top Left X
            sw.WriteLine(Affine[3].ToString(CultureInfo.InvariantCulture)); // Top Left Y
            sw.Close();
        }

        /// <summary>
        /// Saves the current coordinates to a file
        /// </summary>
        /// <param name="fileName">Gets or sets the fileName to use for an image</param>
        public void SaveAs(string fileName)
        {
            Filename = fileName;
            Save();
        }

        /// <summary>
        /// Creates a Matrix that is in float coordinates that represents this world file
        /// </summary>
        /// <returns>A Matrix that transforms an image to the geographic coordinates</returns>
        public Matrix ToMatrix()
        {
            float m11 = Convert.ToSingle(Affine[1]);
            float m12 = Convert.ToSingle(Affine[2]);
            float m21 = Convert.ToSingle(Affine[4]);
            float m22 = Convert.ToSingle(Affine[5]);
            float dx = Convert.ToSingle(Affine[0]);
            float dy = Convert.ToSingle(Affine[3]);
            return new Matrix(m11, m12, m21, m22, dx, dy);
        }

        private static double NextValue(StreamReader sr)
        {
            string nextLine;
            while ((nextLine = sr.ReadLine()) == string.Empty && !sr.EndOfStream)
            {
                // Skip any blank lines
            }

            if (nextLine != null) return double.Parse(nextLine, CultureInfo.InvariantCulture);
            return 0;
        }

        #endregion

    }
}