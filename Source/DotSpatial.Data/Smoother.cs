// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/14/2009 2:31:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Runtime.InteropServices;

namespace DotSpatial.Data
{
    /// <summary>
    /// Smoother
    /// </summary>
    public class Smoother
    {
        #region Fields

        private readonly Action _copyResult;
        private readonly Func<int, byte> _getByte;

        private readonly int _height;
        private readonly ProgressMeter _pm;
        private readonly byte[] _result;
        private readonly int _stride;
        private readonly int _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Smoother"/> class.
        /// </summary>
        /// <param name="stride">The stride.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="inRgbData">The rgb data.</param>
        /// <param name="progHandler">The progress handler.</param>
        public Smoother(int stride, int width, int height, byte[] inRgbData, IProgressHandler progHandler)
        {
            _stride = stride;
            _getByte = _ => inRgbData[_];
            _copyResult = () => Array.Copy(_result, 0, inRgbData, 0, _result.Length);
            _result = new byte[inRgbData.Length];
            _width = width;
            _height = height;
            _pm = new ProgressMeter(progHandler, "Smoothing Image", height);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Smoother"/> class with IntPtr for input array.
        /// </summary>
        /// <param name="stride">The stride.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="inRgbData">The rgb data.</param>
        /// <param name="progHandler">The progress handler.</param>
        public Smoother(int stride, int width, int height, IntPtr inRgbData, IProgressHandler progHandler)
        {
            _stride = stride;
            _getByte = _ => Marshal.ReadByte(inRgbData, _);
            _copyResult = () => Marshal.Copy(_result, 0, inRgbData, _result.Length);
            _result = new byte[stride * height];
            _width = width;
            _height = height;
            _pm = new ProgressMeter(progHandler, "Smoothing Image", height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the smoothing by cycling through the values.
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

            _copyResult();
        }

        private static int Offset(int row, int col, int stride)
        {
            return (row * stride) + (col * 4);
        }

        private void DoSmooth(int row, int col)
        {
            int a = 0, b = 0, g = 0, r = 0;
            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    var inValue = GetColor(row + y, col + x);
                    a += inValue.A;
                    b += inValue.B;
                    g += inValue.G;
                    r += inValue.R;
                }
            }

            a = a / 9;
            b = b / 9;
            g = g / 9;
            r = r / 9;
            var sum = new Argb(a, r, g, b);
            PutColor(row, col, sum);
        }

        private Argb GetColor(int row, int col)
        {
            if (row < 0) row = 0;
            if (col < 0) col = 0;
            if (row >= _height) row = _height - 1;
            if (col >= _width) col = _width - 1;
            var offset = Offset(row, col, _stride);
            return new Argb(_getByte(offset + 3), _getByte(offset + 2), _getByte(offset + 1), _getByte(offset));
        }

        private void PutColor(int row, int col, Argb color)
        {
            var offset = Offset(row, col, _stride);
            _result[offset] = color.B;
            _result[offset + 1] = color.G;
            _result[offset + 2] = color.R;
            _result[offset + 3] = color.A;
        }

        #endregion
    }
}