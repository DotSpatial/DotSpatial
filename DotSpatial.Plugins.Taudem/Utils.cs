//File name: Utils.cs
//Description: Public class, future home of many MapWinGIS.Utils functions;
//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
//you may not use this file except in compliance with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
//ANY KIND, either express or implied. See the License for the specific language governing rights and
//limitations under the License.
//
//The Original Code is MapWindow Open Source.
//
//Contributor(s): (Open source contributors should list themselves and their modifications here).
//1-12-06 ah - Angela Hillier - Provided initial API and parameter descriptions
//16.Mar 2008 - jk - Jiri Kadlec - Added new version of Area() function for shapes in decimal degrees
//********************************************************************************************************

using DotSpatial.Data;

namespace MapWinGeoProc
{
    /// <summary>
    /// Utils provides a collection of methods ranging from file conversion to finding a point on a shape.
    /// </summary>
    ///
    public class Utils
    {
        /// <summary>
        /// Calculates the area of a part, without taking into consideration any other aspects of the polygon.
        /// </summary>
        /// <param name="polygon">A MapWinGIS.Shape POLYGON, POLYGONZ, or POLYGONM</param>
        /// <param name="PartIndex">The integer index of the part to obtain the area of.
        /// This value will be ignored if the shape only has one part, and the function
        /// will calculate the area of the entire shape.</param>
        /// <returns>A double value that is equal to the area of the part.</returns>
        /// <remarks>Coded by Ted Dunsford 6/23/2006, derived from Angela's Area algorithm
        /// Code reference http://astronomy.swin.edu.au/~pbourke/geometry/polyarea/
        /// Cached in MapWinGeoProc\clsUtils\Documentation\
        /// I don't think that we ever want to return a negative area from this function,
        /// even if the part is a hole, because it is being calculated outside of the
        /// context of any other parts.  Only the collective Area function should worry about
        /// ascribing a sign value to the individual part areas.
        ///</remarks>
        public static double AreaOfPart(IFeature polygon, int PartIndex)
        {
            return polygon.Area();
        }
    }
}