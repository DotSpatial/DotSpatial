// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Kyle Ellison. Created in November, 15, 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Controls
{
    /// <summary>
    /// CohenSutherland Line(string) clipping algorithm
    /// </summary>
    public static class CohenSutherland
    {
        #region LineClipStatus enum

        /// <summary>
        /// Result of individual line segment clip
        /// </summary>
        [Flags]
        public enum LineClipStatus
        {
            ///<summary>
            /// Initial untested value
            ///</summary>
            Unknown = 0,
            ///<summary>
            /// Line is completely inside the clip area
            ///</summary>
            Inside = 1,
            ///<summary>
            /// Line is completely outside the clip area
            ///</summary>
            Outside = 2,
            ///<summary>
            /// Line was partially contained and first vertex was clipped
            ///</summary>
            ClippedFirst = 4,
            ///<summary>
            /// Line was partially contained and last vertex was clipped
            ///</summary>
            ClippedLast = 8
        };

        #endregion

        private const int Left = 1;
        private const int Right = 2;
        private const int Bottom = 4;
        private const int Top = 8;

        private const int X = 0;
        private const int Y = 1;

        private static int ComputeOutCode(double x, double y, double xmin, double ymin, double xmax, double ymax)
        {
            int code = 0;
            if (y > ymax)
                code |= Top;
            else if (y < ymin)
                code |= Bottom;
            if (x > xmax)
                code |= Right;
            else if (x < xmin)
                code |= Left;
            return code;
        }

        ///<summary>
        /// Clip a line segment.  Coordinates are modified in place.
        ///</summary>
        ///<param name="x1"></param>
        ///<param name="y1"></param>
        ///<param name="x2"></param>
        ///<param name="y2"></param>
        ///<param name="xmin"></param>
        ///<param name="ymin"></param>
        ///<param name="xmax"></param>
        ///<param name="ymax"></param>
        ///<returns>
        /// Clip status indicating how the line was clipped.
        /// </returns>
        public static LineClipStatus ClipLine(ref double x1, ref double y1, ref double x2, ref double y2, double xmin, double ymin, double xmax, double ymax)
        {
            //Outcodes for P0, P1, and whatever point lies outside the clip rectangle
            int hhh = 0;
            bool done = false;
            LineClipStatus returnValue = LineClipStatus.Unknown;
            //compute outcodes
            int outcode0 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);
            int outcode1 = ComputeOutCode(x2, y2, xmin, ymin, xmax, ymax);

            do
            {
                if ((outcode0 | outcode1) == 0)
                {
                    if (returnValue == LineClipStatus.Unknown)
                        returnValue = LineClipStatus.Inside;
                    done = true;
                }
                else if ((outcode0 & outcode1) > 0)
                {
                    returnValue = LineClipStatus.Outside;
                    done = true;
                }
                else
                {
                    //failed both tests, so calculate the line segment to clip
                    //from an outside point to an intersection with clip edge
                    double x = 0, y = 0;
                    //At least one endpoint is outside the clip rectangle; pick it.
                    int outcodeOut;
                    if (outcode0 != 0)
                    {
                        outcodeOut = outcode0;
                        returnValue |= LineClipStatus.ClippedFirst;
                    }
                    else
                    {
                        outcodeOut = outcode1;
                        returnValue |= LineClipStatus.ClippedLast;
                    }
                    //Now find the intersection point;
                    //use formulas y = y0 + slope * (x - x0), x = x0 + (1/slope)* (y - y0)
                    if ((outcodeOut & Top) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymax - y1) / (y2 - y1);
                        y = ymax;
                    }
                    else if ((outcodeOut & Bottom) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymin - y1) / (y2 - y1);
                        y = ymin;
                    }
                    else if ((outcodeOut & Right) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmax - x1) / (x2 - x1);
                        x = xmax;
                    }
                    else if ((outcodeOut & Left) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmin - x1) / (x2 - x1);
                        x = xmin;
                    }
                    //Now we move outside point to intersection point to clip
                    //and get ready for next pass.
                    if (outcodeOut == outcode0)
                    {
                        x1 = x;
                        y1 = y;
                        outcode0 = ComputeOutCode(x1, y1, xmin, ymin, xmax, ymax);
                    }
                    else
                    {
                        x2 = x;
                        y2 = y;
                        outcode1 = ComputeOutCode(x2, y2, xmin, ymin, xmax, ymax);
                    }
                }
                hhh++;
            }
            while (done != true && hhh < 5000);

            return returnValue;
        }

        ///<summary>
        /// Clip a linestring
        ///</summary>
        ///<param name="linestring"></param>
        ///<param name="xmin"></param>
        ///<param name="ymin"></param>
        ///<param name="xmax"></param>
        ///<param name="ymax"></param>
        ///<returns>
        /// List of clipped linestrings.
        /// </returns>
        public static List<List<double[]>> ClipLinestring(List<double[]> linestring, double xmin, double ymin, double xmax, double ymax)
        {
            List<List<double[]>> returnLinestrings = new List<List<double[]>>();
            List<double[]> returnLinestring = new List<double[]>();
            int numLines = linestring.Count - 1;
            LineClipStatus prevClipStatus = LineClipStatus.Outside;

            for (int lineNum = 0; lineNum < numLines; lineNum++)
            {
                double[] p0 = linestring[lineNum];
                double[] p1 = linestring[lineNum + 1];
                double x0 = p0[X];
                double y0 = p0[Y];
                double x1 = p1[X];
                double y1 = p1[Y];
                LineClipStatus clipStatus = ClipLine(ref x0, ref y0, ref x1, ref y1, xmin,
                                                                    ymin, xmax, ymax);

                // See if we need to add no points, last point, or both points
                if (clipStatus != LineClipStatus.Outside)
                {
                    if (prevClipStatus == LineClipStatus.Outside ||
                        (prevClipStatus & LineClipStatus.ClippedLast) == LineClipStatus.ClippedLast)
                    {
                        // Add both
                        returnLinestring.Add(new[] { x0, y0 });
                    }
                    returnLinestring.Add(new[] { x1, y1 });
                }
                if ((clipStatus & LineClipStatus.ClippedLast) == LineClipStatus.ClippedLast)
                {
                    returnLinestrings.Add(returnLinestring);
                    returnLinestring = new List<double[]>();
                    prevClipStatus = LineClipStatus.Outside;
                }
                else
                {
                    prevClipStatus = clipStatus;
                }
            }

            if (returnLinestring.Count > 1)
                returnLinestrings.Add(returnLinestring);

            return returnLinestrings;
        }
    }
}