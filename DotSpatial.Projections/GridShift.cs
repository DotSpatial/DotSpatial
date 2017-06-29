// ********************************************************************************************************
// Product Name: DotSpatial.Projection
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/18/2009 9:31:11 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Diagnostics;
using DotSpatial.Projections.Transforms;

namespace DotSpatial.Projections
{
    /// <summary>
    /// GridShift
    /// </summary>
    public static class GridShift
    {
        private const double HUGE_VAL = double.MaxValue;
        private const int MAX_TRY = 9;
        private const double TOL = 1E-12;

        #region Private Variables

        private static readonly NadTables _shift = new NadTables();

        #endregion

        #region Methods

        /// <summary>
        /// Applies either a forward or backward gridshift based on the specified name
        /// </summary>
        /// <param name="names"></param>
        /// <param name="inverse"></param>
        /// <param name="xy"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public static void Apply(string[] names, bool inverse, double[] xy, int startIndex, long numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                PhiLam input, output;
                input.Phi = xy[i * 2 + 1];
                input.Lambda = xy[i * 2];
                output.Phi = HUGE_VAL;
                output.Lambda = HUGE_VAL;

                /* keep trying till we find a Table that works from the ones listed */
                foreach (string name in names)
                {
                    if (!_shift.Tables.ContainsKey(name)) continue;
                    NadTable table = _shift.Tables[name];
                    bool found = false;
                    // For GSB tables, we need to check for the appropriate sub-table
                    if (table.SubGrids != null && table.SubGrids.Count > 1)
                    {
                        foreach (NadTable subGrid in table.SubGrids)
                        {
                            /* skip tables that don't match our point at all.  */
                            double wLam = subGrid.LowerLeft.Lambda;
                            double eLam = wLam + (subGrid.NumLambdas - 1) * subGrid.CellSize.Lambda;
                            double sPhi = subGrid.LowerLeft.Phi;
                            double nPhi = sPhi + (subGrid.NumPhis - 1) * subGrid.CellSize.Lambda;
                            if (input.Lambda < wLam || input.Lambda > eLam ||
                                input.Phi < sPhi || input.Phi > nPhi) continue;
                            table = subGrid;
                            found = true;
                            break;
                        }
                        if (!found) continue;
                    }
                    else
                    {
                        /* skip tables that don't match our point at all.  */
                        double minLam = table.LowerLeft.Lambda;
                        double maxLam = minLam + (table.NumLambdas - 1) * table.CellSize.Lambda;
                        double minPhi = table.LowerLeft.Phi;
                        double maxPhi = minPhi + (table.NumPhis - 1) * table.CellSize.Lambda;
                        if (input.Lambda < minLam || input.Lambda > maxLam ||
                            input.Phi < minPhi || input.Phi > maxPhi) continue;
                    }

                    // TO DO: handle child nodes?  Not sure what format would require this
                    output = Convert(input, inverse, table);
                    if (output.Lambda == HUGE_VAL)
                    {
                        Debug.WriteLine("GridShift failed");
                        break;
                    }
                    break;
                }

                if (output.Lambda == HUGE_VAL)
                {
                    Debug.WriteLine(
                        "pj_apply_gridshift(): failed to find a grid shift Table for location: ("
                        + xy[i * 2] * 180 / Math.PI + ", " + xy[i * 2 + 1] * 180 / Math.PI + ")");
                }
                else
                {
                    xy[i * 2] = output.Lambda;
                    xy[i * 2 + 1] = output.Phi;
                }
            }
        }

        private static PhiLam Convert(PhiLam input, bool inverse, NadTable table)
        {
            if (input.Lambda == HUGE_VAL) return input;
            // Normalize input to ll origin
            if (!table.Filled) table.FillData();
            PhiLam tb = input;
            tb.Lambda -= table.LowerLeft.Lambda;
            tb.Phi -= table.LowerLeft.Phi;
            tb.Lambda = Proj.Adjlon(tb.Lambda - Math.PI) + Math.PI;
            PhiLam t = NadInterpolate(tb, table);
            if (inverse)
            {
                PhiLam del, dif;
                int i = MAX_TRY;
                if (t.Lambda == HUGE_VAL) return t;
                t.Lambda = tb.Lambda + t.Lambda;
                t.Phi = tb.Phi - t.Phi;
                do
                {
                    del = NadInterpolate(t, table);
                    /* This case used to return failure, but I have
                           changed it to return the first order approximation
                           of the inverse shift.  This avoids cases where the
                           grid shift *into* this grid came from another grid.
                           While we aren't returning optimally correct results
                           I feel a close result in this case is better than
                           no result.  NFW
                           To demonstrate use -112.5839956 49.4914451 against
                           the NTv2 grid shift file from Canada. */
                    if (del.Lambda == HUGE_VAL)
                    {
                        Debug.WriteLine(ProjectionMessages.InverseShiftFailed);
                        break;
                    }
                    t.Lambda -= dif.Lambda = t.Lambda - del.Lambda - tb.Lambda;
                    t.Phi -= dif.Phi = t.Phi + del.Phi - tb.Phi;
                } while (i-- > 0 && Math.Abs(dif.Lambda) > TOL && Math.Abs(dif.Phi) > TOL);
                if (i < 0)
                {
                    Debug.WriteLine(ProjectionMessages.InvShiftConvergeFailed);
                    t.Lambda = t.Phi = HUGE_VAL;
                    return t;
                }
                input.Lambda = Proj.Adjlon(t.Lambda + table.LowerLeft.Lambda);
                input.Phi = t.Phi + table.LowerLeft.Phi;
            }
            else
            {
                if (t.Lambda == HUGE_VAL)
                {
                    input = t;
                }
                else
                {
                    input.Lambda -= t.Lambda;
                    input.Phi += t.Phi;
                }
            }
            return input;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static PhiLam NadInterpolate(PhiLam t, NadTable ct)
        {
            PhiLam result, remainder;
            result.Phi = HUGE_VAL;
            result.Lambda = HUGE_VAL;
            // find indices and normalize by the cell size (so fractions range from 0 to 1)

            int iLam = (int)Math.Floor(t.Lambda /= ct.CellSize.Lambda);
            int iPhi = (int)Math.Floor(t.Phi /= ct.CellSize.Phi);

            // use the index to determine the remainder

            remainder.Lambda = t.Lambda - iLam;
            remainder.Phi = t.Phi - iPhi;

            //int offLam = 0; // normally we look to the right and bottom neighbor cells
            //int offPhi = 0;
            //if (remainder.Lambda < .5) offLam = -1; // look to cell left of the current cell
            //if (remainder.Phi < .5) offPhi = -1; // look to cell above the of the current cell

            //// because the fractional weights are between cells, we need to adjust the
            //// "remainder" so that it is now relative to the center of the top left
            //// cell, taking into account that the definition of the top left cell
            //// depends on whether the original remainder was larger than .5
            //remainder.Phi = (remainder.Phi > .5) ? remainder.Phi - .5 : remainder.Phi + .5;
            //remainder.Lambda = (remainder.Lambda > .5) ? remainder.Lambda - .5 : remainder.Phi + .5;

            if (iLam < 0)
            {
                if (iLam == -1 && remainder.Lambda > 0.99999999999)
                {
                    iLam++;
                    remainder.Lambda = 0;
                }
                else
                {
                    return result;
                }
            }
            else if (iLam + 1 >= ct.NumLambdas)
            {
                if (iLam + 1 == ct.NumLambdas && remainder.Lambda < 1e-11)
                {
                    iLam--;
                }
                else
                {
                    return result;
                }
            }
            if (iPhi < 0)
            {
                if (iPhi == -1 && remainder.Phi > 0.99999999999)
                {
                    iPhi++;
                    remainder.Phi = 0;
                }
                else
                {
                    return result;
                }
            }
            else if (iPhi + 1 >= ct.NumPhis)
            {
                if (iPhi + 1 == ct.NumPhis && remainder.Phi < 1e-11)
                {
                    iPhi--;
                    remainder.Phi = 1;
                }
                else
                {
                    return result;
                }
            }

            PhiLam f00 = GetValue(iPhi, iLam, ct);
            PhiLam f01 = GetValue(iPhi + 1, iLam, ct);
            PhiLam f10 = GetValue(iPhi, iLam + 1, ct);
            PhiLam f11 = GetValue(iPhi + 1, iLam + 1, ct);

            // The cell weight is equivalent to the area of a cell sized square centered
            // on the actual point that overlaps with the cell.

            // Since the overlap must add up to 1, any portion that does not overlap
            // on the left must overlap on the right, hence (1-remainder.Lambda)

            double m00 = (1 - remainder.Lambda) * (1 - remainder.Phi);
            double m01 = (1 - remainder.Lambda) * remainder.Phi;
            double m10 = remainder.Lambda * (1 - remainder.Phi);
            double m11 = remainder.Lambda * remainder.Phi;

            result.Lambda = m00 * f00.Lambda + m01 * f01.Lambda + m10 * f10.Lambda + m11 * f11.Lambda;
            result.Phi = m00 * f00.Phi + m01 * f01.Phi + m10 * f10.Phi + m11 * f11.Phi;

            return result;
        }

        /// <summary>
        /// Checks the edges to make sure that we are not attempting to interpolate
        /// from cells that don't exist.
        /// </summary>
        /// <param name="iPhi">The cell index in the phi direction</param>
        /// <param name="iLam">The cell index in the lambda direction</param>
        /// <param name="table">The Table with the values</param>
        /// <returns>A PhiLam that has the shift coefficeints.</returns>
        private static PhiLam GetValue(int iPhi, int iLam, NadTable table)
        {
            if (iPhi < 0) iPhi = 0;
            if (iPhi >= table.NumPhis) iPhi = table.NumPhis - 1;
            if (iLam < 0) iLam = 0;
            if (iLam >= table.NumLambdas) iLam = table.NumPhis - 1;
            return table.Cvs[iPhi][iLam];
        }

        /// <summary>
        /// Load grids from the default GeogTransformsGrids subfolder
        /// </summary>
        public static void InitializeExternalGrids()
        {
            _shift.InitializeExternalGrids();
        }

        /// <summary>
        /// Load grids from the specified folder
        /// </summary>
        /// <param name="gridsFolder"></param>
        /// <param name="recursive"></param>
        public static void InitializeExternalGrids(string gridsFolder, bool recursive)
        {
            _shift.InitializeExternalGrids(gridsFolder, recursive);
        }

        #endregion

        #region Properties

        #endregion
    }
}