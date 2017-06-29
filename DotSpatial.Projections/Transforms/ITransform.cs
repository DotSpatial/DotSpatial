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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/23/2009 3:58:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// ITransform
    /// </summary>
    public interface ITransform : ICloneable
    {
        #region Methods

        /// <summary>
        /// Initializes the parameters from the projection info
        /// </summary>
        /// <param name="proj">The projection information used to control this transform</param>
        void Init(ProjectionInfo proj);

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop,
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        void Forward(double[] xy, int startIndex, int numPoints);

        /// <summary>
        /// Transforms all the coordinates by cycling through them in a loop,
        /// transforming each one.  Only the 0 and 1 values of each coordinate will be
        /// transformed, higher dimensional values will be copied.
        /// </summary>
        void Inverse(double[] xy, int startIndex, int numPoints);

        /// <summary>
        /// Special factor calculations for a factors calculation
        /// </summary>
        /// <param name="lp"></param>
        /// <param name="p"></param>
        /// <param name="fac"></param>
        void Special(double[] lp, ProjectionInfo p, Factors fac);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string name of this projection.  This should uniquely define the projection,
        /// and controls what appears in the .prj files.  This name is required.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string name of this projection as it is known to proj4, or should appear
        /// in a proj4 string.  This name is required to read and write to proj4 strings.
        /// </summary>
        string Proj4Name
        {
            get;
        }

        /// <summary>
        /// This is the list of alternate names to check besides the Proj4Name.  This will not be used
        /// for writing Proj4 strings, but may be helpful for reading them.
        /// </summary>
        string[] Proj4Aliases
        {
            get;
        }

        #endregion
    }
}