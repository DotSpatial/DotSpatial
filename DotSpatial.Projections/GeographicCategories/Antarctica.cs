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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:03:57 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// Antarctica
    /// </summary>
    public class Antarctica : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo AustralianAntarctic1998;
        public readonly ProjectionInfo CampAreaAstro;
        public readonly ProjectionInfo DeceptionIsland;
        public readonly ProjectionInfo Petrels1972;
        public readonly ProjectionInfo PointeGeologiePerroud1950;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Antarctica
        /// </summary>
        public Antarctica()
        {
            AustralianAntarctic1998 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            CampAreaAstro = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            DeceptionIsland = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Petrels1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PointeGeologiePerroud1950 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            
            AustralianAntarctic1998.GeographicInfo.Name = "GCS_Australian_Antarctic_1998";
            CampAreaAstro.GeographicInfo.Name = "GCS_Camp_Area";
            DeceptionIsland.GeographicInfo.Name = "GCS_Deception_Island";
            Petrels1972.GeographicInfo.Name = "GCS_Petrels_1972";
            PointeGeologiePerroud1950.GeographicInfo.Name = "GCS_Pointe_Geologie_Perroud_1950";

            AustralianAntarctic1998.GeographicInfo.Datum.Name = "D_Australian_Antarctic_1998";
            CampAreaAstro.GeographicInfo.Datum.Name = "D_Camp_Area";
            DeceptionIsland.GeographicInfo.Datum.Name = "D_Deception_Island";
            Petrels1972.GeographicInfo.Datum.Name = "D_Petrels_1972";
            PointeGeologiePerroud1950.GeographicInfo.Datum.Name = "D_Pointe_Geologie_Perroud_1950";
        }

        #endregion
    }
}

#pragma warning restore 1591