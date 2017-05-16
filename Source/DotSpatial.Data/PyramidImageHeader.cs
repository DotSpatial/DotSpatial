// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Xml.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// PyramidImageHeader
    /// </summary>
    public class PyramidImageHeader
    {
        #region Properties

        /// <summary>
        /// Gets or sets the affine coefficients for this image in ABCDEF order.
        /// X' = A + BX + CY
        /// Y' = D + EX + FY
        /// </summary>
        [XmlAttribute("Affine")]
        public double[] Affine { get; set; }

        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        [XmlAttribute("NumColumns")]
        public int NumColumns { get; set; }

        /// <summary>
        /// Gets or sets the number of rows.
        /// </summary>
        [XmlAttribute("NumRows")]
        public int NumRows { get; set; }

        /// <summary>
        ///  Gets or sets the offsets.
        /// </summary>
        [XmlAttribute("Offset")]
        public long Offset { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Scale is an integer value that starts at 0 for the original image and represents the number of
        /// times the number of columns and number of rows are divided by 2.
        /// </summary>
        /// <param name="affine">The affine array of the original, unmodified image</param>
        /// <param name="scale">The integer scale starting at 0 for the original image, and increasing by one for each down-sampling</param>
        public void SetAffine(double[] affine, int scale)
        {
            Affine = new double[6];
            Array.Copy(affine, Affine, 6);
            Affine[1] = Affine[1] * Math.Pow(2, scale);
            Affine[2] = Affine[2] * Math.Pow(2, scale);
            Affine[4] = Affine[4] * Math.Pow(2, scale);
            Affine[5] = Affine[5] * Math.Pow(2, scale);
        }

        /// <summary>
        /// Sets the number of columns based on the integer scale.
        /// </summary>
        /// <param name="originalNumColumns">The number of columns.</param>
        /// <param name="scale">integer starts at 0 for the original image</param>
        public void SetNumColumns(int originalNumColumns, int scale)
        {
            NumColumns = (int)(originalNumColumns / Math.Pow(2, scale));
        }

        /// <summary>
        /// Sets the number of rows based on the integer scale.
        /// </summary>
        /// <param name="originalNumRows">The number of rows.</param>
        /// <param name="scale">integer starts at 0 for the original image</param>
        public void SetNumRows(int originalNumRows, int scale)
        {
            NumRows = (int)(originalNumRows / Math.Pow(2, scale));
        }

        #endregion
    }
}