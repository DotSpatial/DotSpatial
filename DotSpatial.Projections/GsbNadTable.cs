// ********************************************************************************************************
// Product Name: DotSpatial.Projections
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
// The Original Code is from DotSpatial.Projections
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/17/2010 11:41:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.IO;

namespace DotSpatial.Projections
{
    /// <summary>
    /// This is a special case of a NadTable and has specific reading techniques that need to be handled
    /// </summary>
    public class GsbNadTable : NadTable
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GsbNadTable
        /// </summary>
        public GsbNadTable(string resourceLocation, long offset)
            : base(resourceLocation, offset, true)
        {
        }

        /// <summary>
        /// Creates a new instance of GsbNadTable
        /// </summary>
        public GsbNadTable(string location, long offset, bool embedded)
            : base(location, offset, embedded)
        {
            Format = GridShiftTableFormat.GSB;
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public override void ReadHeader()
        {
            // If the DataOffset is 0, figure out the sub-table structure,
            // otherwise we are reading a subtable.
            if (DataOffset == 0)
            {
                Initialize();
                return;
            }
            ReadSubGrid();
        }

        /// <summary>
        /// This handles the creation of the
        /// </summary>
        private void Initialize()
        {
            byte[] overviewHeader = new byte[176];
            using (Stream str = GetStream())
            {
                if (str == null) return;
                str.Read(overviewHeader, 0, 176);
            }
            byte[] bCount = new byte[4];
            Array.Copy(overviewHeader, 8 + 32, bCount, 0, 4);
            if (!BitConverter.IsLittleEndian) Array.Reverse(bCount);
            int numGrids = BitConverter.ToInt32(bCount, 0);
            long offset = 176;
            for (int iGrid = 0; iGrid < numGrids; iGrid++)
            {
                string location = FileIsEmbedded ? ManifestResourceString : GridFilePath;
                NadTable sub = new GsbNadTable(location, offset, FileIsEmbedded);
                sub.ReadHeader();
                offset += 176 + sub.NumPhis * sub.NumLambdas * 16;
                SubGrids.Add(sub);
            }
        }

        private void ReadSubGrid()
        {
            byte[] header = new byte[176];
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (BinaryReader br = new BinaryReader(str))
                {
                    str.Seek(DataOffset, SeekOrigin.Begin);
                    br.Read(header, 0, 176);
                }
            }

            PhiLam ll;
            ll.Phi = GetDouble(header, 4 * 16 + 8);
            ll.Lambda = -GetDouble(header, 7 * 16 + 8);

            double urPhi = GetDouble(header, 5 * 16 + 8);
            double urLam = -GetDouble(header, 6 * 16 + 8);

            PhiLam cs;
            cs.Phi = GetDouble(header, 8 * 16 + 8);
            cs.Lambda = GetDouble(header, 9 * 16 + 8);
            CellSize = cs;

            NumLambdas = (int)(Math.Abs(urLam - ll.Lambda) / cs.Lambda + .5) + 1;
            NumPhis = (int)(Math.Abs(urPhi - ll.Phi) / cs.Phi + .5) + 1;

            ll.Lambda *= DEG_TO_RAD;
            ll.Phi *= DEG_TO_RAD;
            LowerLeft = ll;

            cs.Lambda *= DEG_TO_RAD;
            cs.Phi *= DEG_TO_RAD;
            CellSize = cs;
        }

        /// <inheritdoc/>
        public override void FillData()
        {
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (BinaryReader br = new BinaryReader(str))
                {
                    str.Seek(DataOffset, SeekOrigin.Begin);
                    str.Seek(176, SeekOrigin.Current);

                    int numPhis = NumPhis;
                    int numLambdas = NumLambdas;
                    PhiLam[][] cvs = new PhiLam[numPhis][];

                    // Skip past rest of header
                    for (int row = 0; row < numPhis; row++)
                    {
                        cvs[row] = new PhiLam[NumLambdas];
                        // NTV order is flipped compared with a normal CVS table
                        for (int col = numLambdas - 1; col >= 0; col--)
                        {
                            // shift values are given in "arc-seconds" and need to be converted to radians.
                            cvs[row][col].Phi = ReadDouble(br) * (Math.PI / 180) / 3600;
                            cvs[row][col].Lambda = ReadDouble(br) * (Math.PI / 180) / 3600;
                        }
                    }
                    Cvs = cvs;
                }
            }
        }

        /// <summary>
        /// Gets the double value from the specified position in the byte array
        /// Using BigEndian format.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        protected override double GetDouble(byte[] array, int offset)
        {
            byte[] bValue = new byte[8];
            Array.Copy(array, offset, bValue, 0, 8);
            if (!BitConverter.IsLittleEndian) Array.Reverse(bValue);
            double temp = BitConverter.ToDouble(bValue, 0);
            return temp;
        }

        #endregion
    }
}