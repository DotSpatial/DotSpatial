// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:14:40 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

using System;

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// NorthAmerica
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo ATS1977;
        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo Ammassalik1958;
        public readonly ProjectionInfo Barbados;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo CapeCanaveral;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo Helle1954;
        public readonly ProjectionInfo Jamaica1875;
        public readonly ProjectionInfo Jamaica1969;
        public readonly ProjectionInfo NAD1927CGQ77;
        public readonly ProjectionInfo NAD1927Definition1976;
        public readonly ProjectionInfo NADMichigan;
        public readonly ProjectionInfo NorthAmerican1983CSRS98;
        public readonly ProjectionInfo NorthAmerican1983HARN;
        public readonly ProjectionInfo NorthAmericanDatum1927;
        public readonly ProjectionInfo NorthAmericanDatum1983;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo Qornoq;
        public readonly ProjectionInfo Qornoq1927;
        public readonly ProjectionInfo Scoresbysund1952;
        public readonly ProjectionInfo StGeorgeIsland;
        public readonly ProjectionInfo StLawrenceIsland;
        public readonly ProjectionInfo StPaulIsland;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica
        /// </summary>
        public NorthAmerica()
        {
            AlaskanIslands = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            AmericanSamoa1962 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Ammassalik1958 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ATS1977 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378135 +b=6356750.304921594 +no_defs ");
            Barbados = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Bermuda1957 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Bermuda2000 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS84 +no_defs ");
            CapeCanaveral = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Guam1963 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Helle1954 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Jamaica1875 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Jamaica1969 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            NAD1927CGQ77 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            NAD1927Definition1976 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            NADMichigan = ProjectionInfo.FromProj4String("+proj=longlat +a=6378450.047 +b=6356826.620025999 +no_defs ");
            NorthAmerican1983CSRS98 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            NorthAmerican1983HARN = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            NorthAmericanDatum1927 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +datum=NAD27 +no_defs ");
            NorthAmericanDatum1983 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +datum=NAD83 +no_defs ");
            OldHawaiian = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            PuertoRico = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Qornoq = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Qornoq1927 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Scoresbysund1952 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            StGeorgeIsland = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            StLawrenceIsland = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            StPaulIsland = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");

            AlaskanIslands.Name = "GCS_Alaskan_Islands";
            AlaskanIslands.GeographicInfo.Name = "GCS_Alaskan_Islands";
            AmericanSamoa1962.Name = "GCS_American_Samoa_1962";
            AmericanSamoa1962.GeographicInfo.Name = "GCS_American_Samoa_1962";
            Ammassalik1958.Name = "GCS_Ammassalik_1958";
            Ammassalik1958.GeographicInfo.Name = "GCS_Ammassalik_1958";
            ATS1977.Name = "GCS_ATS_1977";
            ATS1977.GeographicInfo.Name = "GCS_ATS_1977";
            Barbados.Name = "GCS_Barbados";
            Barbados.GeographicInfo.Name = "GCS_Barbados";
            Bermuda1957.Name = "GCS_Bermuda_1957";
            Bermuda1957.GeographicInfo.Name = "GCS_Bermuda_1957";
            Bermuda2000.Name = "GCS_Bermuda_2000";
            Bermuda2000.GeographicInfo.Name = "GCS_Bermuda_2000";
            CapeCanaveral.Name = "GCS_Cape_Canaveral";
            CapeCanaveral.GeographicInfo.Name = "GCS_Cape_Canaveral";
            Helle1954.Name = "GCS_Helle_1954";
            Helle1954.GeographicInfo.Name = "GCS_Helle_1954";
            Guam1963.Name = "GCS_Guam_1963";
            Guam1963.GeographicInfo.Name = "GCS_Guam_1963";
            Jamaica1875.Name = "GCS_Jamaica_1875";
            Jamaica1875.GeographicInfo.Name = "GCS_Jamaica_1875";
            Jamaica1969.Name = "GCS_Jamaica_1969";
            Jamaica1969.GeographicInfo.Name = "GCS_Jamaica_1969";
            NAD1927CGQ77.Name = "GCS_NAD_1927_CGQ77";
            NAD1927CGQ77.GeographicInfo.Name = "GCS_NAD_1927_CGQ77";
            NAD1927Definition1976.Name = "GCS_NAD_1927_Definition_1976";
            NAD1927Definition1976.GeographicInfo.Name = "GCS_NAD_1927_Definition_1976";
            NADMichigan.Name = "GCS_North_American_Michigan";
            NADMichigan.GeographicInfo.Name = "GCS_North_American_Michigan";
            NorthAmerican1983CSRS98.Name = "GCS_North_American_1983_CSRS98";
            NorthAmerican1983CSRS98.GeographicInfo.Name = "GCS_North_American_1983_CSRS98";
            NorthAmerican1983HARN.Name = "GCS_North_American_1983_HARN";
            NorthAmerican1983HARN.GeographicInfo.Name = "GCS_North_American_1983_HARN";
            NorthAmericanDatum1927.Name = "GCS_North_American_1927";
            NorthAmericanDatum1927.GeographicInfo.Name = "GCS_North_American_1927";
            NorthAmericanDatum1983.Name = "GCS_North_American_1983";
            NorthAmericanDatum1983.GeographicInfo.Name = "GCS_North_American_1983";
            OldHawaiian.Name = "GCS_Old_Hawaiian";
            OldHawaiian.GeographicInfo.Name = "GCS_Old_Hawaiian";
            PuertoRico.Name = "GCS_Puerto_Rico";
            PuertoRico.GeographicInfo.Name = "GCS_Puerto_Rico";
            Qornoq.GeographicInfo.Name = String.Empty;
            Qornoq1927.Name = "GCS_Qornoq_1927";
            Qornoq1927.GeographicInfo.Name = "GCS_Qornoq_1927";
            Scoresbysund1952.Name = "GCS_Scoresbysund_1952";
            Scoresbysund1952.GeographicInfo.Name = "GCS_Scoresbysund_1952";
            StGeorgeIsland.Name = "GCS_St_George_Island";
            StGeorgeIsland.GeographicInfo.Name = "GCS_St_George_Island";
            StLawrenceIsland.Name = "GCS_St_Lawrence_Island";
            StLawrenceIsland.GeographicInfo.Name = "GCS_St_Lawrence_Island";
            StPaulIsland.Name = "GCS_St_Paul_Island";
            StPaulIsland.GeographicInfo.Name = "GCS_St_Paul_Island";
        }

        #endregion
    }
}

#pragma warning restore 1591