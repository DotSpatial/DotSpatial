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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:06:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// TransverseMercator
    /// </summary>
    public class TransverseMercatorSystems : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo WGS1984lo16;
        public readonly ProjectionInfo WGS1984lo17;
        public readonly ProjectionInfo WGS1984lo18;
        public readonly ProjectionInfo WGS1984lo19;
        public readonly ProjectionInfo WGS1984lo20;
        public readonly ProjectionInfo WGS1984lo21;
        public readonly ProjectionInfo WGS1984lo22;
        public readonly ProjectionInfo WGS1984lo23;
        public readonly ProjectionInfo WGS1984lo24;
        public readonly ProjectionInfo WGS1984lo25;
        public readonly ProjectionInfo WGS1984lo26;
        public readonly ProjectionInfo WGS1984lo27;
        public readonly ProjectionInfo WGS1984lo28;
        public readonly ProjectionInfo WGS1984lo29;
        public readonly ProjectionInfo WGS1984lo30;
        public readonly ProjectionInfo WGS1984lo31;
        public readonly ProjectionInfo WGS1984lo32;
        public readonly ProjectionInfo WGS1984lo33;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TransverseMercator
        /// </summary>
        public TransverseMercatorSystems()
        {
            WGS1984lo16 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=16 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo17 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=17 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo18 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=18 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo19 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=19 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo20 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=20 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo21 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=21 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo22 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=22 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo23 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=23 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo24 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=24 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo25 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=25 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo26 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=26 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo27 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=27 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo28 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=28 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo29 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=29 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo30 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=30 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo31 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=31 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo32 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=32 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
            WGS1984lo33 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=33 +k=1.000000 +x_0=0 +y_0=0 +ellps=WGS84 +datum=WGS84 +units=m +no_defs ");
        }

        #endregion
    }
}

#pragma warning restore 1591