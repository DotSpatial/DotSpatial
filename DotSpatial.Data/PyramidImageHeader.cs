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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 10:13:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Xml.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// PyramidImageHeader
    /// </summary>
    public class PyramidImageHeader
    {
        #region Private Variables

        private double[] _affine;
        private int _numColums;
        private int _numRows;
        private long _offset;

        #endregion

        #region Constructors

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
            _affine = new double[6];
            Array.Copy(affine, _affine, 6);
            _affine[1] = _affine[1] * Math.Pow(2, scale);
            _affine[2] = _affine[2] * Math.Pow(2, scale);
            _affine[4] = _affine[4] * Math.Pow(2, scale);
            _affine[5] = _affine[5] * Math.Pow(2, scale);
        }

        /// <summary>
        /// Sets the number of rows based on the integer scale
        /// </summary>
        /// <param name="originalNumRows"></param>
        /// <param name="scale">integer starts at 0 for the original image</param>
        public void SetNumRows(int originalNumRows, int scale)
        {
            _numRows = (int)(originalNumRows / Math.Pow(2, scale));
        }

        /// <summary>
        /// Sets the number of columns based on the integer scale
        /// </summary>
        /// <param name="originalNumColumns"></param>
        /// <param name="scale">integer starts at 0 for the original image</param>
        public void SetNumColumns(int originalNumColumns, int scale)
        {
            _numColums = (int)(originalNumColumns / Math.Pow(2, scale));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Offsets
        /// </summary>
        [XmlAttribute("Offset")]
        public long Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        /// <summary>
        /// Integer number of rows
        /// </summary>
        [XmlAttribute("NumRows")]
        public int NumRows
        {
            get { return _numRows; }
            set { _numRows = value; }
        }

        /// <summary>
        /// Integer number of columns
        /// </summary>
        [XmlAttribute("NumColumns")]
        public int NumColumns
        {
            get { return _numColums; }
            set { _numColums = value; }
        }

        /// <summary>
        /// Gets or sets the affine coefficients for this image in ABCDEF order
        /// X' = A + BX + CY
        /// Y' = D + EX + FY
        /// </summary>
        [XmlAttribute("Affine")]
        public double[] Affine
        {
            get { return _affine; }
            set { _affine = value; }
        }

        #endregion
    }
}