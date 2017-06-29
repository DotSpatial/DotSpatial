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
    /// Handles the specific case of dat files like the ntv1 file format.
    /// </summary>
    public class DatNadTable : NadTable
    {
        /// <summary>
        /// Creates a new instance of GsbNadTable
        /// </summary>
        public DatNadTable(string resourceLocation)
            : base(resourceLocation)
        {
        }

        /// <summary>
        /// Creates a new instance of GsbNadTable
        /// </summary>
        public DatNadTable(string location, bool embedded)
            : base(location, embedded)
        {
            Format = GridShiftTableFormat.DAT;
        }

        /// <inheritdoc></inheritdoc>
        public override void ReadHeader()
        {
            byte[] header = new byte[176];
            using (Stream str = GetStream())
            {
                if (str == null) return;
                // Read important header content
                using (BinaryReader br = new BinaryReader(str))
                {
                    br.Read(header, 0, 176);
                }
            }

            PhiLam ll;
            ll.Phi = GetDouble(header, 24);
            ll.Lambda = -GetDouble(header, 72);

            double urPhi = GetDouble(header, 40);
            double urLam = -GetDouble(header, 56);

            PhiLam cs = new PhiLam();
            cs.Phi = GetDouble(header, 88);
            cs.Lambda = GetDouble(header, 104);

            NumLambdas = (int)(Math.Abs(urLam - ll.Lambda) / cs.Lambda + .5) + 1;
            NumPhis = (int)(Math.Abs(urPhi - ll.Phi) / cs.Phi + .5) + 1;
            ll.Lambda *= DEG_TO_RAD;
            ll.Phi *= DEG_TO_RAD;
            cs.Lambda *= DEG_TO_RAD;
            cs.Phi *= DEG_TO_RAD;

            LowerLeft = ll;
            CellSize = cs;
        }

        /// <inheritdoc></inheritdoc>
        public override void FillData()
        {
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (var br = new BinaryReader(str))
                {
                    int numPhis = NumPhis;
                    int numLambdas = NumLambdas;

                    PhiLam[][] cvs = new PhiLam[numPhis][];

                    // Skip past rest of header
                    str.Seek(176, SeekOrigin.Begin);
                    for (int row = 0; row < numPhis; row++)
                    {
                        cvs[row] = new PhiLam[numLambdas];
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
    }
}