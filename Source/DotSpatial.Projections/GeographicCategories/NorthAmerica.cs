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

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for NorthAmerica.
    /// </summary>
    public class NorthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AlaskanIslands;
        public readonly ProjectionInfo AmericanSamoa1962;
        public readonly ProjectionInfo Ammassalik1958;
        public readonly ProjectionInfo AverageTerrestrialSystem1977;
        public readonly ProjectionInfo Barbados1938;
        public readonly ProjectionInfo Bermuda1957;
        public readonly ProjectionInfo Bermuda2000;
        public readonly ProjectionInfo CapeCanaveral;
        public readonly ProjectionInfo CR05;
        public readonly ProjectionInfo Greenland1996;
        public readonly ProjectionInfo Guam1963;
        public readonly ProjectionInfo Helle1954;
        public readonly ProjectionInfo MexicanDatumof1993;
        public readonly ProjectionInfo NAD1927;
        public readonly ProjectionInfo NAD1927CGQ77;
        public readonly ProjectionInfo NAD1927Definition1976;
        public readonly ProjectionInfo NAD1983;
        public readonly ProjectionInfo NAD1983CORS96;
        public readonly ProjectionInfo NAD1983CSRS;
        public readonly ProjectionInfo NAD1983HARN;
        public readonly ProjectionInfo NAD1983NSRS2007;
        public readonly ProjectionInfo Ocotepeque1935;
        public readonly ProjectionInfo OldHawaiian;
        public readonly ProjectionInfo OldHawaiianIntl1924;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo Qornoq;
        public readonly ProjectionInfo Qornoq1927;
        public readonly ProjectionInfo SaintPierreetMiquelon1950;
        public readonly ProjectionInfo Scoresbysund1952;
        public readonly ProjectionInfo StGeorgeIsland;
        public readonly ProjectionInfo StLawrenceIsland;
        public readonly ProjectionInfo StPaulIsland;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NorthAmerica.
        /// </summary>
        public NorthAmerica()
        {
            AlaskanIslands = ProjectionInfo.FromAuthorityCode("ESRI", 37260).SetNames("", "GCS_Alaskan_Islands", "D_Alaskan_Islands");
            AmericanSamoa1962 = ProjectionInfo.FromEpsgCode(4169).SetNames("", "GCS_American_Samoa_1962", "D_American_Samoa_1962");
            Ammassalik1958 = ProjectionInfo.FromEpsgCode(4196).SetNames("", "GCS_Ammassalik_1958", "D_Ammassalik_1958");
            AverageTerrestrialSystem1977 = ProjectionInfo.FromEpsgCode(4122).SetNames("", "GCS_ATS_1977", "D_ATS_1977");
            Barbados1938 = ProjectionInfo.FromEpsgCode(4212).SetNames("", "GCS_Barbados_1938", "D_Barbados_1938");
            Bermuda1957 = ProjectionInfo.FromEpsgCode(4216).SetNames("", "GCS_Bermuda_1957", "D_Bermuda_1957");
            Bermuda2000 = ProjectionInfo.FromEpsgCode(4762).SetNames("", "GCS_Bermuda_2000", "D_Bermuda_2000");
            CapeCanaveral = ProjectionInfo.FromEpsgCode(4717).SetNames("", "GCS_Cape_Canaveral", "D_Cape_Canaveral");
            CR05 = ProjectionInfo.FromAuthorityCode("EPSG", 104143).SetNames("", "GCS_CR05", "D_Costa_Rica_2005"); // missing
            Greenland1996 = ProjectionInfo.FromEpsgCode(4747).SetNames("", "GCS_Greenland_1996", "D_Greenland_1996");
            Guam1963 = ProjectionInfo.FromEpsgCode(4675).SetNames("", "GCS_Guam_1963", "D_Guam_1963");
            Helle1954 = ProjectionInfo.FromEpsgCode(4660).SetNames("", "GCS_Helle_1954", "D_Helle_1954");
            MexicanDatumof1993 = ProjectionInfo.FromEpsgCode(4483).SetNames("", "GCS_Mexican_Datum_of_1993", "D_Mexican_Datum_of_1993");
            NAD1927 = ProjectionInfo.FromEpsgCode(4267).SetNames("", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927CGQ77 = ProjectionInfo.FromEpsgCode(4609).SetNames("", "GCS_NAD_1927_CGQ77", "D_NAD_1927_CGQ77");
            NAD1927Definition1976 = ProjectionInfo.FromEpsgCode(4608).SetNames("", "GCS_NAD_1927_Definition_1976", "D_NAD_1927_Definition_1976");
            NAD1983 = ProjectionInfo.FromEpsgCode(4269).SetNames("", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CORS96 = ProjectionInfo.FromAuthorityCode("EPSG", 104223).SetNames("", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CSRS = ProjectionInfo.FromEpsgCode(4617).SetNames("", "GCS_North_American_1983_CSRS", "D_North_American_1983_CSRS");
            NAD1983HARN = ProjectionInfo.FromEpsgCode(4152).SetNames("", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983NSRS2007 = ProjectionInfo.FromEpsgCode(4759).SetNames("", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            Ocotepeque1935 = ProjectionInfo.FromAuthorityCode("EPSG", 104132).SetNames("", "GCS_Ocotepeque_1935", "D_Ocotepeque_1935"); // missing
            OldHawaiian = ProjectionInfo.FromEpsgCode(4135).SetNames("", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianIntl1924 = ProjectionInfo.FromAuthorityCode("ESRI", 104138).SetNames("", "GCS_Old_Hawaiian_Intl_1924", "D_Old_Hawaiian_Intl_1924"); // missing
            PuertoRico = ProjectionInfo.FromEpsgCode(4139).SetNames("", "GCS_Puerto_Rico", "D_Puerto_Rico");
            Qornoq = ProjectionInfo.FromEpsgCode(4287).SetNames("", "GCS_Qornoq", "D_Qornoq");
            Qornoq1927 = ProjectionInfo.FromEpsgCode(4194).SetNames("", "GCS_Qornoq_1927", "D_Qornoq_1927");
            SaintPierreetMiquelon1950 = ProjectionInfo.FromEpsgCode(4638).SetNames("", "GCS_Saint_Pierre_et_Miquelon_1950", "D_Saint_Pierre_et_Miquelon_1950");
            Scoresbysund1952 = ProjectionInfo.FromEpsgCode(4195).SetNames("", "GCS_Scoresbysund_1952", "D_Scoresbysund_1952");
            StGeorgeIsland = ProjectionInfo.FromEpsgCode(4138).SetNames("", "GCS_St_George_Island", "D_St_George_Island");
            StLawrenceIsland = ProjectionInfo.FromEpsgCode(4136).SetNames("", "GCS_St_Lawrence_Island", "D_St_Lawrence_Island");
            StPaulIsland = ProjectionInfo.FromEpsgCode(4137).SetNames("", "GCS_St_Paul_Island", "D_St_Paul_Island");
        }

        #endregion
    }
}

#pragma warning restore 1591