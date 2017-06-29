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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:04:41 PM
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
    /// StatePlaneOther
    /// </summary>
    public class StatePlaneOther : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300;
        public readonly ProjectionInfo NAD1983HARNGuamMapGrid;
        public readonly ProjectionInfo NAD1983HARNUTMZone2S;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentralFIPS2112;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentralOldFIPS2102;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganEastOldFIPS2101;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganNorthFIPS2111;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganSouthFIPS2113;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganWestOldFIPS2103;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii1FIPS5101;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii2FIPS5102;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii3FIPS5103;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii4FIPS5104;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii5FIPS5105;
        public readonly ProjectionInfo PuertoRicoStatePlanePuertoRicoFIPS5201;
        public readonly ProjectionInfo PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StatePlaneOther
        /// </summary>
        public StatePlaneOther()
        {
            AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-14.26666666666667 +lat_0=-14.26666666666667 +lon_0=-170 +k_0=1 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            NAD1983HARNGuamMapGrid = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=13.5 +lon_0=144.75 +k=1.000000 +x_0=100000 +y_0=200000 +ellps=GRS80 +units=m +no_defs ");
            NAD1983HARNUTMZone2S = ProjectionInfo.FromProj4String("+proj=utm +zone=2 +south +ellps=GRS80 +units=m +no_defs ");
            NADMichiganStatePlaneMichiganCentralFIPS2112 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=44.18333333333333 +lat_2=45.7 +lat_0=43.31666666666667 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganCentralOldFIPS2102 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.5 +lon_0=-85.75 +k=0.999909 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganEastOldFIPS2101 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.5 +lon_0=-83.66666666666667 +k=0.999943 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganNorthFIPS2111 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=45.48333333333333 +lat_2=47.08333333333334 +lat_0=44.78333333333333 +lon_0=-87 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganSouthFIPS2113 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=42.1 +lat_2=43.66666666666666 +lat_0=41.5 +lon_0=-84.33333333333333 +x_0=609601.2192024385 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            NADMichiganStatePlaneMichiganWestOldFIPS2103 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=41.5 +lon_0=-88.75 +k=0.999909 +x_0=152400.3048006096 +y_0=0 +a=6378450.047 +b=6356826.620025999 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii1FIPS5101 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=18.83333333333333 +lon_0=-155.5 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii2FIPS5102 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=20.33333333333333 +lon_0=-156.6666666666667 +k=0.999967 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii3FIPS5103 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.16666666666667 +lon_0=-158 +k=0.999990 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii4FIPS5104 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.83333333333333 +lon_0=-159.5 +k=0.999990 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            OldHawaiianStatePlaneHawaii5FIPS5105 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=21.66666666666667 +lon_0=-160.1666666666667 +k=1.000000 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            PuertoRicoStatePlanePuertoRicoFIPS5201 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=152400.3048006096 +y_0=0 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");
            PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=18.03333333333333 +lat_2=18.43333333333333 +lat_0=17.83333333333333 +lon_0=-66.43333333333334 +x_0=152400.3048006096 +y_0=30480.06096012193 +ellps=clrk66 +to_meter=0.3048006096012192 +no_defs ");

            AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300.Name = "American_Samoa_1962_StatePlane_American_Samoa_FIPS_5300";
            NAD1983HARNGuamMapGrid.Name = "NAD_1983_HARN_Guam_Map_Grid";
            NAD1983HARNUTMZone2S.Name = "NAD_1983_HARN_UTM_Zone_2S";
            NADMichiganStatePlaneMichiganCentralFIPS2112.Name = "NAD_Michigan_StatePlane_Michigan_Central_FIPS_2112";
            NADMichiganStatePlaneMichiganCentralOldFIPS2102.Name = "NAD_Michigan_StatePlane_Michigan_Central_Old_FIPS_2102";
            NADMichiganStatePlaneMichiganEastOldFIPS2101.Name = "NAD_Michigan_StatePlane_Michigan_East_Old_FIPS_2101";
            NADMichiganStatePlaneMichiganNorthFIPS2111.Name = "NAD_Michigan_StatePlane_Michigan_North_FIPS_2111";
            NADMichiganStatePlaneMichiganSouthFIPS2113.Name = "NAD_Michigan_StatePlane_Michigan_South_FIPS_2113";
            NADMichiganStatePlaneMichiganWestOldFIPS2103.Name = "NAD_Michigan_StatePlane_Michigan_West_Old_FIPS_2103";
            OldHawaiianStatePlaneHawaii1FIPS5101.Name = "Old_Hawaiian_StatePlane_Hawaii_1_FIPS_5101";
            OldHawaiianStatePlaneHawaii2FIPS5102.Name = "Old_Hawaiian_StatePlane_Hawaii_2_FIPS_5102";
            OldHawaiianStatePlaneHawaii3FIPS5103.Name = "Old_Hawaiian_StatePlane_Hawaii_3_FIPS_5103";
            OldHawaiianStatePlaneHawaii4FIPS5104.Name = "Old_Hawaiian_StatePlane_Hawaii_4_FIPS_5104";
            OldHawaiianStatePlaneHawaii5FIPS5105.Name = "Old_Hawaiian_StatePlane_Hawaii_5_FIPS_5105";
            PuertoRicoStatePlanePuertoRicoFIPS5201.Name = "Puerto_Rico_StatePlane_Puerto_Rico_FIPS_5201";
            PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202.Name = "Puerto_Rico_StatePlane_Virgin_Islands_St_Croix_FIPS_5202";

            AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300.GeographicInfo.Name = "GCS_American_Samoa_1962";
            NAD1983HARNGuamMapGrid.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NAD1983HARNUTMZone2S.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NADMichiganStatePlaneMichiganCentralFIPS2112.GeographicInfo.Name = "GCS_North_American_Michigan";
            NADMichiganStatePlaneMichiganCentralOldFIPS2102.GeographicInfo.Name = "GCS_North_American_Michigan";
            NADMichiganStatePlaneMichiganEastOldFIPS2101.GeographicInfo.Name = "GCS_North_American_Michigan";
            NADMichiganStatePlaneMichiganNorthFIPS2111.GeographicInfo.Name = "GCS_North_American_Michigan";
            NADMichiganStatePlaneMichiganSouthFIPS2113.GeographicInfo.Name = "GCS_North_American_Michigan";
            NADMichiganStatePlaneMichiganWestOldFIPS2103.GeographicInfo.Name = "GCS_North_American_Michigan";
            OldHawaiianStatePlaneHawaii1FIPS5101.GeographicInfo.Name = "GCS_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii2FIPS5102.GeographicInfo.Name = "GCS_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii3FIPS5103.GeographicInfo.Name = "GCS_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii4FIPS5104.GeographicInfo.Name = "GCS_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii5FIPS5105.GeographicInfo.Name = "GCS_Old_Hawaiian";
            PuertoRicoStatePlanePuertoRicoFIPS5201.GeographicInfo.Name = "GCS_Puerto_Rico";
            PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202.GeographicInfo.Name = "GCS_Puerto_Rico";

            AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300.GeographicInfo.Datum.Name = "D_American_Samoa_1962";
            NAD1983HARNGuamMapGrid.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NAD1983HARNUTMZone2S.GeographicInfo.Datum.Name = "D_North_American_1983_HARN";
            NADMichiganStatePlaneMichiganCentralFIPS2112.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            NADMichiganStatePlaneMichiganCentralOldFIPS2102.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            NADMichiganStatePlaneMichiganEastOldFIPS2101.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            NADMichiganStatePlaneMichiganNorthFIPS2111.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            NADMichiganStatePlaneMichiganSouthFIPS2113.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            NADMichiganStatePlaneMichiganWestOldFIPS2103.GeographicInfo.Datum.Name = "D_North_American_Michigan";
            OldHawaiianStatePlaneHawaii1FIPS5101.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii2FIPS5102.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii3FIPS5103.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii4FIPS5104.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            OldHawaiianStatePlaneHawaii5FIPS5105.GeographicInfo.Datum.Name = "D_Old_Hawaiian";
            PuertoRicoStatePlanePuertoRicoFIPS5201.GeographicInfo.Datum.Name = "D_Puerto_Rico";
            PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202.GeographicInfo.Datum.Name = "D_Puerto_Rico";
        }

        #endregion
    }
}

#pragma warning restore 1591