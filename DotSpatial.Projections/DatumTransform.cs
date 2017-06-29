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

namespace DotSpatial.Projections
{
    /// <summary>
    /// IDatumTransform
    /// </summary>
    [Serializable]
    public class DatumTransform : IDatumTransform
    {
        /// <summary>Arc-seconds to radians conversion factor</summary>
        protected const double SecToRad = 4.848136811095359935899141023e-6;

        private readonly IDatumTransformStage[] _aiDts;

        /// <summary>
        ///
        /// </summary>
        /// <param name="aiDatumTransformStage"></param>
        public DatumTransform(IDatumTransformStage[] aiDatumTransformStage)
        {
            if (aiDatumTransformStage == null)
            {
                throw new Exception("Null DatumTransformStage array passed to constructor");
            }
            //for (int i = 1; i < aiDatumTransformStage.Length; i++) {
            //  if (aiDatumTransformStage[i - 1].ToDatum != aiDatumTransformStage[i].FromDatum) {
            //    throw new Exception("DatumTranformStage array contained consecutive stages with unmatched from/to datums.");
            //  }
            //}
            _aiDts = aiDatumTransformStage;
        }

        /// <summary>
        /// Returns the number of datum transform stages for this datum transform
        /// </summary>
        public int NumStages
        {
            get
            {
                return _aiDts.Length;
            }
        }

        /// <summary>
        /// source datum
        /// </summary>
        public string FromID
        {
            get
            {
                if (_aiDts.Length > 0)
                {
                    return _aiDts[0].FromDatum;
                }
                return null;
            }
        }

        /// <summary>
        /// destination datum
        /// </summary>
        public string ToID
        {
            get
            {
                if (_aiDts.Length > 0)
                {
                    return _aiDts[_aiDts.Length - 1].ToDatum;
                }
                return null;
            }
        }

        #region IDatumTransform Members

        /// <summary>
        /// datum tranform method
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="xy"></param>
        /// <param name="z"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public void Transform(ProjectionInfo source, ProjectionInfo dest, double[] xy, double[] z, int startIndex, int numPoints)
        {
            if (_aiDts.Length == 0) return; // "empty" datum transform

            bool bCoordsAreGeocentric = false;

            /* -------------------------------------------------------------------- */
            /*      Create a temporary Z value if one is not provided.              */
            /* -------------------------------------------------------------------- */
            if (z == null)
            {
                z = new double[xy.Length / 2];
            }

            foreach (IDatumTransformStage idts in _aiDts)
            {
                if (idts.Method == TransformMethod.GridShift)
                {
                    if (bCoordsAreGeocentric)
                    {
                        var gc = new GeocentricGeodetic(idts.FromSpheroid);
                        gc.GeocentricToGeodetic(xy, z, startIndex, numPoints);
                        bCoordsAreGeocentric = false;
                    }

                    var astrGrids = new string[1];
                    astrGrids[0] = "@" + idts.GridShiftTable;
                    GridShift.Apply(astrGrids, idts.ApplyTableInverse, xy, startIndex, numPoints);
                }
                else
                {
                    if (!bCoordsAreGeocentric)
                    {
                        var gc = new GeocentricGeodetic(idts.FromSpheroid);
                        gc.GeodeticToGeocentric(xy, z, startIndex, numPoints);
                        bCoordsAreGeocentric = true;
                    }

                    ApplyParameterizedTransform(idts, xy, z, startIndex, numPoints);
                }
            }

            /* -------------------------------------------------------------------- */
            /*      Convert back to geodetic coordinates, if needed.                */
            /* -------------------------------------------------------------------- */
            if (bCoordsAreGeocentric)
            {
                var gc = new GeocentricGeodetic(_aiDts[_aiDts.Length - 1].ToSpheroid);
                gc.GeocentricToGeodetic(xy, z, startIndex, numPoints);
            }
        }

        #endregion

        /// <summary>
        /// Returns an interface to the stage at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IDatumTransformStage GetStage(int index)
        {
            if (index > _aiDts.Length - 1)
                return null;
            else
                return _aiDts[index];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="idts"></param>
        /// <param name="xy"></param>
        /// <param name="zArr"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        private static void ApplyParameterizedTransform(IDatumTransformStage idts, double[] xy, double[] zArr, int startIndex, int numPoints)
        {
            double[] shift;

            if (idts.Method == TransformMethod.Param3)
            {
                shift = new double[3];
                shift[0] = idts.DeltaX;
                shift[1] = idts.DeltaY;
                shift[2] = idts.DeltaZ;
            }
            else
            {
                shift = new double[7];
                shift[0] = idts.DeltaX;
                shift[1] = idts.DeltaY;
                shift[2] = idts.DeltaZ;
                shift[3] = idts.RotateX * SecToRad;
                shift[4] = idts.RotateY * SecToRad;
                shift[5] = idts.RotateZ * SecToRad;
                shift[6] = idts.DeltaScale;
                shift[6] = (shift[6] / 1E6) + 1;
            }

            if (shift.Length == 3)
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;
                    xy[2 * i] = xy[2 * i] + shift[0]; // dx
                    xy[2 * i + 1] = xy[2 * i + 1] + shift[1]; // dy
                    zArr[i] = zArr[i] + shift[2];
                }
            }
            else
            {
                for (int i = startIndex; i < numPoints; i++)
                {
                    if (double.IsNaN(xy[2 * i]) || double.IsNaN(xy[2 * i + 1])) continue;
                    double x = xy[2 * i];
                    double y = xy[2 * i + 1];
                    double z = zArr[i];
                    xy[2 * i] = shift[6] * x - shift[5] * y + shift[4] * z + shift[0];
                    xy[2 * i + 1] = shift[5] * x + shift[6] * y - shift[3] * z + shift[1];
                    zArr[i] = -shift[4] * x + shift[3] * y + shift[6] * z + shift[2];
                }
            }
        }
    }
}