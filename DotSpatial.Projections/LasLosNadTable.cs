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
//
// The Initial Developer of this Original Code is Steve Riddell. Created 5/27/2011 1:04:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.IO;
using System.Reflection;

namespace DotSpatial.Projections
{
    /// <summary>
    /// Overrides the NadTable methods in the context of an las/los file pair.
    /// </summary>
    public class LasLosNadTable : NadTable
    {
        private const double SecToRad = 4.848136811095359935899141023e-6;

        /// <summary>
        /// An LasLosNadTable constructor
        /// </summary>
        /// <param name="resourceLocation">The Manifest Assembly resource location</param>
        public LasLosNadTable(string resourceLocation)
            : base(resourceLocation)
        {
            Format = GridShiftTableFormat.LOS;
        }

        /// <summary>
        /// An LasLosNadTable constructor
        /// </summary>
        /// <param name="location">The resource location or grid file path</param>
        /// <param name="embedded">Indicates if grid is an embedded resource</param>
        public LasLosNadTable(string location, bool embedded)
            : base(location, embedded)
        {
            Format = GridShiftTableFormat.LOS;
        }

        /// <inheritdoc></inheritdoc>
        public override void ReadHeader()
        {
            string location = FileIsEmbedded ? ManifestResourceString : GridFilePath;
            Name = Path.GetFileNameWithoutExtension(location);
            using (Stream str = GetStream())
            {
                if (str == null) return;
                using (BinaryReader br = new BinaryReader(str))
                {
                    // read past 64 bytes of strings/nulls at start of file
                    br.BaseStream.Seek(64, SeekOrigin.Begin);
                    NumLambdas = br.ReadInt32();
                    NumPhis = br.ReadInt32();
                    br.ReadInt32(); // num z values: always 1
                    PhiLam ll;
                    ll.Lambda = br.ReadSingle() * DEG_TO_RAD;
                    PhiLam cs;
                    cs.Lambda = br.ReadSingle() * DEG_TO_RAD;
                    ll.Phi = br.ReadSingle() * DEG_TO_RAD;
                    LowerLeft = ll;
                    cs.Phi = br.ReadSingle() * DEG_TO_RAD;
                    CellSize = cs;
                    Filled = false;
                }
            }
        }

        /// <inheritdoc></inheritdoc>
        public override void FillData()
        {
            using (Stream strLos = GetStream())
            using (Stream strLas = GetLasStream())
            {
                if (strLas == null || strLos == null) return;
                using (BinaryReader brLas = new BinaryReader(strLas))
                using (BinaryReader brLos = new BinaryReader(strLos))
                {
                    int numPhis = NumPhis;
                    int numLambdas = NumLambdas;
                    // .las/.los header is padded out to 1 full line of lambdas
                    int offsetToData = ((NumLambdas + 1) * sizeof(Single));
                    brLas.BaseStream.Seek(offsetToData, SeekOrigin.Begin);
                    brLos.BaseStream.Seek(offsetToData, SeekOrigin.Begin);
                    PhiLam[][] cvs = new PhiLam[numPhis][];
                    for (int i = 0; i < numPhis; i++)
                    {
                        cvs[i] = new PhiLam[numLambdas];
                        brLas.ReadSingle(); // discard leading 'zero'
                        brLos.ReadSingle(); // discard leading 'zero'
                        cvs[i][0].Phi = brLas.ReadSingle() * SecToRad;
                        cvs[i][0].Lambda = brLos.ReadSingle() * SecToRad;
                        for (int j = 1; j < NumLambdas; j++)
                        {
                            cvs[i][j].Phi += brLas.ReadSingle() * SecToRad;
                            cvs[i][j].Lambda += brLos.ReadSingle() * SecToRad;
                        }
                    }
                    Cvs = cvs;
                    Filled = true;
                }
            }
        }

        /// <summary>
        /// LasLosNadTables are constructs from a pair of input files: .los AND .las
        /// </summary>
        /// <returns></returns>
        private Stream GetLasStream()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            Stream str = FileIsEmbedded ? a.GetManifestResourceStream(ManifestResourceString.ToLower().Replace(".los", ".las")) : File.Open(GridFilePath.ToLower().Replace(".los", ".las"), FileMode.Open, FileAccess.Read);
            return str;
        }
    }
}