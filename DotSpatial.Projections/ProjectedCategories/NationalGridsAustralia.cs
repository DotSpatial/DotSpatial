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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:46:07 PM
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
    /// NatGridsAustralia
    /// </summary>
    public class NationalGridsAustralia : CoordinateSystemCategory
    {
        #region Private Variables

        public readonly ProjectionInfo AGD1966ACTGridAGCZone;
        public readonly ProjectionInfo AGD1966AMGZone48;
        public readonly ProjectionInfo AGD1966AMGZone49;
        public readonly ProjectionInfo AGD1966AMGZone50;
        public readonly ProjectionInfo AGD1966AMGZone51;
        public readonly ProjectionInfo AGD1966AMGZone52;
        public readonly ProjectionInfo AGD1966AMGZone53;
        public readonly ProjectionInfo AGD1966AMGZone54;
        public readonly ProjectionInfo AGD1966AMGZone55;
        public readonly ProjectionInfo AGD1966AMGZone56;
        public readonly ProjectionInfo AGD1966AMGZone57;
        public readonly ProjectionInfo AGD1966AMGZone58;
        public readonly ProjectionInfo AGD1966ISG542;
        public readonly ProjectionInfo AGD1966ISG543;
        public readonly ProjectionInfo AGD1966ISG551;
        public readonly ProjectionInfo AGD1966ISG552;
        public readonly ProjectionInfo AGD1966ISG553;
        public readonly ProjectionInfo AGD1966ISG561;
        public readonly ProjectionInfo AGD1966ISG562;
        public readonly ProjectionInfo AGD1966ISG563;
        public readonly ProjectionInfo AGD1966VICGRID;
        public readonly ProjectionInfo AGD1984AMGZone48;
        public readonly ProjectionInfo AGD1984AMGZone49;
        public readonly ProjectionInfo AGD1984AMGZone50;
        public readonly ProjectionInfo AGD1984AMGZone51;
        public readonly ProjectionInfo AGD1984AMGZone52;
        public readonly ProjectionInfo AGD1984AMGZone53;
        public readonly ProjectionInfo AGD1984AMGZone54;
        public readonly ProjectionInfo AGD1984AMGZone55;
        public readonly ProjectionInfo AGD1984AMGZone56;
        public readonly ProjectionInfo AGD1984AMGZone57;
        public readonly ProjectionInfo AGD1984AMGZone58;
        public readonly ProjectionInfo GDA1994MGAZone48;
        public readonly ProjectionInfo GDA1994MGAZone49;
        public readonly ProjectionInfo GDA1994MGAZone50;
        public readonly ProjectionInfo GDA1994MGAZone51;
        public readonly ProjectionInfo GDA1994MGAZone52;
        public readonly ProjectionInfo GDA1994MGAZone53;
        public readonly ProjectionInfo GDA1994MGAZone54;
        public readonly ProjectionInfo GDA1994MGAZone55;
        public readonly ProjectionInfo GDA1994MGAZone56;
        public readonly ProjectionInfo GDA1994MGAZone57;
        public readonly ProjectionInfo GDA1994MGAZone58;
        public readonly ProjectionInfo GDA1994SouthAustraliaLambert;
        public readonly ProjectionInfo GDA1994VICGRID94;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NatGridsAustralia
        /// </summary>
        public NationalGridsAustralia()
        {
            AGD1966ACTGridAGCZone = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=149.0092948333333 +k=1.000086 +x_0=200000 +y_0=4510193.4939 +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone48 = ProjectionInfo.FromProj4String("+proj=utm +zone=48 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone49 = ProjectionInfo.FromProj4String("+proj=utm +zone=49 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone50 = ProjectionInfo.FromProj4String("+proj=utm +zone=50 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone51 = ProjectionInfo.FromProj4String("+proj=utm +zone=51 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone52 = ProjectionInfo.FromProj4String("+proj=utm +zone=52 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone53 = ProjectionInfo.FromProj4String("+proj=utm +zone=53 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone54 = ProjectionInfo.FromProj4String("+proj=utm +zone=54 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone55 = ProjectionInfo.FromProj4String("+proj=utm +zone=55 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone56 = ProjectionInfo.FromProj4String("+proj=utm +zone=56 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone57 = ProjectionInfo.FromProj4String("+proj=utm +zone=57 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966AMGZone58 = ProjectionInfo.FromProj4String("+proj=utm +zone=58 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG542 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=141 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG543 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=143 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG551 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=145 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG552 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=147 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG553 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=149 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG561 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=151 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG562 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=153 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966ISG563 = ProjectionInfo.FromProj4String("+proj=tmerc +lat_0=0 +lon_0=155 +k=0.999940 +x_0=300000 +y_0=5000000 +ellps=aust_SA +units=m +no_defs ");
            AGD1966VICGRID = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-36 +lat_2=-38 +lat_0=-37 +lon_0=145 +x_0=2500000 +y_0=4500000 +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone48 = ProjectionInfo.FromProj4String("+proj=utm +zone=48 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone49 = ProjectionInfo.FromProj4String("+proj=utm +zone=49 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone50 = ProjectionInfo.FromProj4String("+proj=utm +zone=50 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone51 = ProjectionInfo.FromProj4String("+proj=utm +zone=51 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone52 = ProjectionInfo.FromProj4String("+proj=utm +zone=52 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone53 = ProjectionInfo.FromProj4String("+proj=utm +zone=53 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone54 = ProjectionInfo.FromProj4String("+proj=utm +zone=54 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone55 = ProjectionInfo.FromProj4String("+proj=utm +zone=55 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone56 = ProjectionInfo.FromProj4String("+proj=utm +zone=56 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone57 = ProjectionInfo.FromProj4String("+proj=utm +zone=57 +south +ellps=aust_SA +units=m +no_defs ");
            AGD1984AMGZone58 = ProjectionInfo.FromProj4String("+proj=utm +zone=58 +south +ellps=aust_SA +units=m +no_defs ");
            GDA1994MGAZone48 = ProjectionInfo.FromProj4String("+proj=utm +zone=48 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone49 = ProjectionInfo.FromProj4String("+proj=utm +zone=49 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone50 = ProjectionInfo.FromProj4String("+proj=utm +zone=50 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone51 = ProjectionInfo.FromProj4String("+proj=utm +zone=51 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone52 = ProjectionInfo.FromProj4String("+proj=utm +zone=52 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone53 = ProjectionInfo.FromProj4String("+proj=utm +zone=53 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone54 = ProjectionInfo.FromProj4String("+proj=utm +zone=54 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone55 = ProjectionInfo.FromProj4String("+proj=utm +zone=55 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone56 = ProjectionInfo.FromProj4String("+proj=utm +zone=56 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone57 = ProjectionInfo.FromProj4String("+proj=utm +zone=57 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994MGAZone58 = ProjectionInfo.FromProj4String("+proj=utm +zone=58 +south +ellps=GRS80 +units=m +no_defs ");
            GDA1994SouthAustraliaLambert = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-28 +lat_2=-36 +lat_0=-32 +lon_0=135 +x_0=1000000 +y_0=2000000 +ellps=GRS80 +units=m +no_defs ");
            GDA1994VICGRID94 = ProjectionInfo.FromProj4String("+proj=lcc +lat_1=-36 +lat_2=-38 +lat_0=-37 +lon_0=145 +x_0=2500000 +y_0=2500000 +ellps=GRS80 +units=m +no_defs ");

            AGD1966ACTGridAGCZone.Name = "AGD_1966_ACT_Grid_AGC_Zone";
            AGD1966AMGZone48.Name = "AGD_1966_AMG_Zone_48";
            AGD1966AMGZone49.Name = "AGD_1966_AMG_Zone_49";
            AGD1966AMGZone50.Name = "AGD_1966_AMG_Zone_50";
            AGD1966AMGZone51.Name = "AGD_1966_AMG_Zone_51";
            AGD1966AMGZone52.Name = "AGD_1966_AMG_Zone_52";
            AGD1966AMGZone53.Name = "AGD_1966_AMG_Zone_53";
            AGD1966AMGZone54.Name = "AGD_1966_AMG_Zone_54";
            AGD1966AMGZone55.Name = "AGD_1966_AMG_Zone_55";
            AGD1966AMGZone56.Name = "AGD_1966_AMG_Zone_56";
            AGD1966AMGZone57.Name = "AGD_1966_AMG_Zone_57";
            AGD1966AMGZone58.Name = "AGD_1966_AMG_Zone_58";
            AGD1966ISG542.Name = "AGD_1966_ISG_54_2";
            AGD1966ISG543.Name = "AGD_1966_ISG_54_3";
            AGD1966ISG551.Name = "AGD_1966_ISG_55_1";
            AGD1966ISG552.Name = "AGD_1966_ISG_55_2";
            AGD1966ISG553.Name = "AGD_1966_ISG_55_3";
            AGD1966ISG561.Name = "AGD_1966_ISG_56_1";
            AGD1966ISG562.Name = "AGD_1966_ISG_56_2";
            AGD1966ISG563.Name = "AGD_1966_ISG_56_3";
            AGD1966VICGRID.Name = "AGD_1966_VICGRID";
            AGD1984AMGZone48.Name = "AGD_1984_AMG_Zone_48";
            AGD1984AMGZone49.Name = "AGD_1984_AMG_Zone_49";
            AGD1984AMGZone50.Name = "AGD_1984_AMG_Zone_50";
            AGD1984AMGZone51.Name = "AGD_1984_AMG_Zone_51";
            AGD1984AMGZone52.Name = "AGD_1984_AMG_Zone_52";
            AGD1984AMGZone53.Name = "AGD_1984_AMG_Zone_53";
            AGD1984AMGZone54.Name = "AGD_1984_AMG_Zone_54";
            AGD1984AMGZone55.Name = "AGD_1984_AMG_Zone_55";
            AGD1984AMGZone56.Name = "AGD_1984_AMG_Zone_56";
            AGD1984AMGZone57.Name = "AGD_1984_AMG_Zone_57";
            AGD1984AMGZone58.Name = "AGD_1984_AMG_Zone_58";
            GDA1994MGAZone48.Name = "GDA_1994_MGA_Zone_48";
            GDA1994MGAZone49.Name = "GDA_1994_MGA_Zone_49";
            GDA1994MGAZone50.Name = "GDA_1994_MGA_Zone_50";
            GDA1994MGAZone51.Name = "GDA_1994_MGA_Zone_51";
            GDA1994MGAZone52.Name = "GDA_1994_MGA_Zone_52";
            GDA1994MGAZone53.Name = "GDA_1994_MGA_Zone_53";
            GDA1994MGAZone54.Name = "GDA_1994_MGA_Zone_54";
            GDA1994MGAZone55.Name = "GDA_1994_MGA_Zone_55";
            GDA1994MGAZone56.Name = "GDA_1994_MGA_Zone_56";
            GDA1994MGAZone57.Name = "GDA_1994_MGA_Zone_57";
            GDA1994MGAZone58.Name = "GDA_1994_MGA_Zone_58";
            GDA1994SouthAustraliaLambert.Name = "GDA_1994_South_Australia_Lambert";
            GDA1994VICGRID94.Name = "GDA_1994_VICGRID94";

            AGD1966ACTGridAGCZone.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone48.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone49.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone50.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone51.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone52.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone53.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone54.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone55.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone56.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone57.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966AMGZone58.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG542.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG543.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG551.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG552.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG553.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG561.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG562.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966ISG563.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1966VICGRID.GeographicInfo.Name = "GCS_Australian_1966";
            AGD1984AMGZone48.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone49.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone50.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone51.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone52.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone53.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone54.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone55.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone56.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone57.GeographicInfo.Name = "GCS_Australian_1984";
            AGD1984AMGZone58.GeographicInfo.Name = "GCS_Australian_1984";
            GDA1994MGAZone48.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone49.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone50.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone51.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone52.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone53.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone54.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone55.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone56.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone57.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994MGAZone58.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994SouthAustraliaLambert.GeographicInfo.Name = "GCS_GDA_1994";
            GDA1994VICGRID94.GeographicInfo.Name = "GCS_GDA_1994";

            AGD1966ACTGridAGCZone.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone48.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone49.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone50.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone51.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone52.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone53.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone54.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone55.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone56.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone57.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966AMGZone58.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG542.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG543.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG551.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG552.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG553.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG561.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG562.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966ISG563.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1966VICGRID.GeographicInfo.Datum.Name = "D_Australian_1966";
            AGD1984AMGZone48.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone49.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone50.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone51.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone52.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone53.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone54.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone55.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone56.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone57.GeographicInfo.Datum.Name = "D_Australian_1984";
            AGD1984AMGZone58.GeographicInfo.Datum.Name = "D_Australian_1984";
            GDA1994MGAZone48.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone49.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone50.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone51.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone52.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone53.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone54.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone55.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone56.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone57.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994MGAZone58.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994SouthAustraliaLambert.GeographicInfo.Datum.Name = "D_GDA_1994";
            GDA1994VICGRID94.GeographicInfo.Datum.Name = "D_GDA_1994";
        }

        #endregion
    }
}

#pragma warning restore 1591