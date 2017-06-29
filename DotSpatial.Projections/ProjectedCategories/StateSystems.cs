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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:05:31 PM
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
    /// StateSystems
    /// </summary>
    public class StateSystems : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo NAD1927AlaskaAlbersFeet;
        public readonly ProjectionInfo NAD1927AlaskaAlbersMeters;
        public readonly ProjectionInfo NAD1927CaliforniaTealeAlbers;
        public readonly ProjectionInfo NAD1927GeorgiaStatewideAlbers;
        public readonly ProjectionInfo NAD1927TexasStatewideMappingSystem;
        public readonly ProjectionInfo NAD1983CaliforniaTealeAlbers;
        public readonly ProjectionInfo NAD1983GeorgiaStatewideLambert;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambert;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambertFeetIntl;
        public readonly ProjectionInfo NAD1983IdahoTM;
        public readonly ProjectionInfo NAD1983OregonStatewideLambert;
        public readonly ProjectionInfo NAD1983OregonStatewideLambertFeetIntl;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemAlbers;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemLambert;
        public readonly ProjectionInfo NAD1983TexasStatewideMappingSystem;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StateSystems
        /// </summary>
        public StateSystems()
        {
            NAD1927AlaskaAlbersFeet = ProjectionInfo.FromProj4String("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927AlaskaAlbersMeters = ProjectionInfo.FromProj4String("+proj=aea +lat_1=55 +lat_2=65 +lat_0=50 +lon_0=-154 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927CaliforniaTealeAlbers = ProjectionInfo.FromProj4String("+proj=aea +lat_1=34 +lat_2=40.5 +lat_0=0 +lon_0=-120 +x_0=0 +y_0=-4000000 +ellps=clrk66 +datum=NAD27 +units=m +no_defs ");
            NAD1927GeorgiaStatewideAlbers = ProjectionInfo.FromProj4String("+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-83.5 +x_0=0 +y_0=0 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048006096012192 +no_defs ");
            NAD1927TexasStatewideMappingSystem = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=27.41666666666667 +lat_2=34.91666666666666 +lat_0=31.16666666666667 +lon_0=-100 +x_0=914400 +y_0=914400 +ellps=clrk66 +datum=NAD27 +to_meter=0.3048 +no_defs ");
            NAD1983CaliforniaTealeAlbers = ProjectionInfo.FromProj4String("+proj=aea +lat_1=34 +lat_2=40.5 +lat_0=0 +lon_0=-120 +x_0=0 +y_0=-4000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983GeorgiaStatewideLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=31.41666666666667 +lat_2=34.28333333333333 +lat_0=0 +lon_0=-83.5 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNOregonStatewideLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=400000 +y_0=0 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNOregonStatewideLambertFeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +to_meter=0.3048 +no_defs ");
            NAD1983IdahoTM = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=42 +lon_0=-114 +k=0.999600 +x_0=2000000 +y_0=3000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983OregonStatewideLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=400000 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983OregonStatewideLambertFeetIntl = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=43 +lat_2=45.5 +lat_0=41.75 +lon_0=-120.5 +x_0=399999.9999999999 +y_0=0 +ellps=GRS80 +datum=NAD83 +to_meter=0.3048 +no_defs ");
            NAD1983TexasCentricMappingSystemAlbers = ProjectionInfo.FromProj4String("+proj=aea +lat_1=27.5 +lat_2=35 +lat_0=18 +lon_0=-100 +x_0=1500000 +y_0=6000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983TexasCentricMappingSystemLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=27.5 +lat_2=35 +lat_0=18 +lon_0=-100 +x_0=1500000 +y_0=5000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");
            NAD1983TexasStatewideMappingSystem = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=27.41666666666667 +lat_2=34.91666666666666 +lat_0=31.16666666666667 +lon_0=-100 +x_0=1000000 +y_0=1000000 +ellps=GRS80 +datum=NAD83 +units=m +no_defs ");

            NAD1927AlaskaAlbersFeet.Name = "NAD_1927_Alaska_Albers_Feet";
            NAD1927AlaskaAlbersMeters.Name = "NAD_1927_Alaska_Albers_Meters";
            NAD1927CaliforniaTealeAlbers.Name = "NAD_1927_California_Teale_Albers";
            NAD1927GeorgiaStatewideAlbers.Name = "NAD_1927_Georgia_Statewide_Albers";
            NAD1927TexasStatewideMappingSystem.Name = "NAD_1927_Texas_Statewide_Mapping_System";
            NAD1983CaliforniaTealeAlbers.Name = "NAD_1983_California_Teale_Albers";
            NAD1983GeorgiaStatewideLambert.Name = "NAD_1983_Georgia_Statewide_Lambert";
            NAD1983HARNOregonStatewideLambert.Name = "NAD_1983_HARN_Oregon_Statewide_Lambert";
            NAD1983HARNOregonStatewideLambertFeetIntl.Name = "NAD_1983_HARN_Oregon_Statewide_Lambert_Feet_Intl";
            NAD1983IdahoTM.Name = "NAD_1983_Idaho_TM";
            NAD1983OregonStatewideLambert.Name = "NAD_1983_Oregon_Statewide_Lambert";
            NAD1983OregonStatewideLambertFeetIntl.Name = "NAD_1983_Oregon_Statewide_Lambert_Feet_Intl";
            NAD1983TexasCentricMappingSystemAlbers.Name = "NAD_1983_Texas_Centric_Mapping_System_Albers";
            NAD1983TexasCentricMappingSystemLambert.Name = "NAD_1983_Texas_Centric_Mapping_System_Lambert";
            NAD1983TexasStatewideMappingSystem.Name = "NAD_1983_Texas_Statewide_Mapping_System";

            NAD1927AlaskaAlbersFeet.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927AlaskaAlbersMeters.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927CaliforniaTealeAlbers.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927GeorgiaStatewideAlbers.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1927TexasStatewideMappingSystem.GeographicInfo.Name = "GCS_North_American_1927";
            NAD1983CaliforniaTealeAlbers.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983GeorgiaStatewideLambert.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983HARNOregonStatewideLambert.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNOregonStatewideLambertFeetIntl.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983IdahoTM.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983OregonStatewideLambert.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983OregonStatewideLambertFeetIntl.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983TexasCentricMappingSystemAlbers.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983TexasCentricMappingSystemLambert.GeographicInfo.Name = "GCS_North_American_1983";
            NAD1983TexasStatewideMappingSystem.GeographicInfo.Name = "GCS_North_American_1983";

            NAD1927AlaskaAlbersFeet.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927AlaskaAlbersMeters.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927CaliforniaTealeAlbers.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927GeorgiaStatewideAlbers.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1927TexasStatewideMappingSystem.GeographicInfo.Datum.Name = "D_North_American_1927";
            NAD1983CaliforniaTealeAlbers.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983GeorgiaStatewideLambert.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983HARNOregonStatewideLambert.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNOregonStatewideLambertFeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983IdahoTM.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983OregonStatewideLambert.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983OregonStatewideLambertFeetIntl.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983TexasCentricMappingSystemAlbers.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983TexasCentricMappingSystemLambert.GeographicInfo.Datum.Name = "D_North_American_1983";
            NAD1983TexasStatewideMappingSystem.GeographicInfo.Datum.Name = "D_North_American_1983";
        }

        #endregion
    }
}

#pragma warning restore 1591