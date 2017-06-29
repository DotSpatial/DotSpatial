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
using System.Globalization;
using System.IO;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Overrides the NadTable methods in the context of an lla resource file.
    /// </summary>
    public class LlaNadTable : NadTable
    {
        /// <summary>
        /// An LlaNadTable constructor
        /// </summary>
        /// <param name="resourceLocation">The Manifest Assembly resource location</param>
        public LlaNadTable(string resourceLocation)
            : base(resourceLocation)
        {
            Format = GridShiftTableFormat.LLA;
        }

        /// <summary>
        /// An LlaNadTable constructor
        /// </summary>
        /// <param name="location">The Manifest Assembly resource location or file path</param>
        /// <param name="embedded">Indicates if grid file is embedded resource or external file</param>
        public LlaNadTable(string location, bool embedded)
            : base(location, embedded)
        {
            Format = GridShiftTableFormat.LLA;
        }

        /// <inheritdoc></inheritdoc>
        public override void ReadHeader()
        {
            string numText;
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (StreamReader sr = new StreamReader(str))
                {
                    Name = sr.ReadLine();
                    numText = sr.ReadToEnd();
                }
            }
            char[] separators = new[] { ' ', ',', ':', (char)10 };
            string[] values = numText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            NumLambdas = int.Parse(values[0]);
            NumPhis = int.Parse(values[1]);
            PhiLam ll;
            ll.Lambda = double.Parse(values[3], CultureInfo.InvariantCulture) * DEG_TO_RAD;
            ll.Phi = double.Parse(values[5], CultureInfo.InvariantCulture) * DEG_TO_RAD;
            LowerLeft = ll;
            PhiLam cs;
            cs.Lambda = double.Parse(values[4], CultureInfo.InvariantCulture) * DEG_TO_RAD;
            cs.Phi = double.Parse(values[6], CultureInfo.InvariantCulture) * DEG_TO_RAD;
            CellSize = cs;
            Filled = false;
        }

        /// <inheritdoc></inheritdoc>
        public override void FillData()
        {
            string numText;
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (StreamReader sr = new StreamReader(str))
                {
                    sr.ReadLine();
                    numText = sr.ReadToEnd();
                }
            }
            char[] separators = new[] { ' ', ',', ':', (char)10 };
            string[] values = numText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            int p = 7;

            int numPhis = NumPhis;
            int numLambdas = NumLambdas;
            PhiLam[][] cvs = new PhiLam[numPhis][];
            for (int i = 0; i < numPhis; i++)
            {
                cvs[i] = new PhiLam[numLambdas];
                int iCheck = int.Parse(values[p]);
                if (iCheck != i)
                {
                    throw new ProjectionException(ProjectionMessages.IndexMismatch);
                }
                p++;
                double lam = long.Parse(values[p]) * USecToRad;
                cvs[i][0].Lambda = lam;
                p++;
                double phi = long.Parse(values[p]) * USecToRad;
                cvs[i][0].Phi = phi;
                p++;
                for (int j = 1; j < NumLambdas; j++)
                {
                    lam += long.Parse(values[p]) * USecToRad;
                    cvs[i][j].Lambda = lam;
                    p++;
                    phi += long.Parse(values[p]) * USecToRad;
                    cvs[i][j].Phi = phi;
                    p++;
                }
            }
            Cvs = cvs;
            Filled = true;
        }
    }
}