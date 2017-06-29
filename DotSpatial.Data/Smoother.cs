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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 2:31:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Smoother
    /// </summary>
    public class Smoother
    {
        private void DoSmooth(int row, int col)
        {
            Argb sum = new Argb();
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    Argb inValue = GetColor(row + y, col + x);
                    sum.A += inValue.A;
                    sum.B += inValue.B;
                    sum.G += inValue.G;
                    sum.R += inValue.R;
                }
            }
            sum.A = sum.A / 9;
            sum.B = sum.B / 9;
            sum.G = sum.G / 9;
            sum.R = sum.R / 9;
            PutColor(row, col, sum);
        }

        private Argb GetColor(int row, int col)
        {
            Argb result = new Argb();
            if (row < 0) row = 0;
            if (col < 0) col = 0;
            if (row >= _height) row = _height - 1;
            if (col >= _width) col = _width - 1;
            int offset = row * _stride + col * 4;
            result.B = _rgbData[offset];
            result.G = _rgbData[offset + 1];
            result.R = _rgbData[offset + 2];
            result.A = _rgbData[offset + 3];
            return result;
        }

        private void PutColor(int row, int col, Argb color)
        {
            int offset = row * _stride + col * 4;
            _result[offset] = Argb.ToByte(color.B);
            _result[offset + 1] = Argb.ToByte(color.G);
            _result[offset + 2] = Argb.ToByte(color.R);
            _result[offset + 3] = Argb.ToByte(color.A);
        }

        #region Nested type: Argb

        private struct Argb
        {
            public int A;
            public int B;
            public int G;
            public int R;

            public static byte ToByte(int val)
            {
                if (val > 255) val = 255;
                if (val < 0) val = 0;
                return Convert.ToByte(val);
            }
        }

        #endregion

        #region Private Variables

        private readonly int _height;
        private readonly ProgressMeter _pm;
        private readonly byte[] _result;
        private readonly byte[] _rgbData;
        private readonly int _stride;
        private readonly int _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Smoother
        /// </summary>
        public Smoother(int stride, int width, int height, byte[] inRgbData, IProgressHandler progHandler)
        {
            _stride = stride;
            _rgbData = inRgbData;
            _result = new byte[inRgbData.Length];
            _width = width;
            _height = height;
            _pm = new ProgressMeter(progHandler, "Smoothing Image", height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the smoothing by cycling through the values
        /// </summary>
        public void Smooth()
        {
            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    DoSmooth(row, col);
                }
                _pm.CurrentValue = row;
            }
            Array.Copy(_result, 0, _rgbData, 0, _rgbData.Length);
        }

        #endregion

        #region Properties

        #endregion
    }
}