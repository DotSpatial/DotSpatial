// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.UTM
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Europe.
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Datum73UTMZone29N;
        public readonly ProjectionInfo ED1950ED77UTMZone38N;
        public readonly ProjectionInfo ED1950ED77UTMZone39N;
        public readonly ProjectionInfo ED1950ED77UTMZone40N;
        public readonly ProjectionInfo ED1950ED77UTMZone41N;
        public readonly ProjectionInfo ELD1979UTMZone32N;
        public readonly ProjectionInfo ELD1979UTMZone33N;
        public readonly ProjectionInfo ELD1979UTMZone34N;
        public readonly ProjectionInfo ELD1979UTMZone35N;
        public readonly ProjectionInfo ETRS1989ETRSTM26;
        public readonly ProjectionInfo ETRS1989ETRSTM27;
        public readonly ProjectionInfo ETRS1989ETRSTM28;
        public readonly ProjectionInfo ETRS1989ETRSTM29;
        public readonly ProjectionInfo ETRS1989ETRSTM30;
        public readonly ProjectionInfo ETRS1989ETRSTM31;
        public readonly ProjectionInfo ETRS1989ETRSTM32;
        public readonly ProjectionInfo ETRS1989ETRSTM33;
        public readonly ProjectionInfo ETRS1989ETRSTM34;
        public readonly ProjectionInfo ETRS1989ETRSTM35;
        public readonly ProjectionInfo ETRS1989ETRSTM36;
        public readonly ProjectionInfo ETRS1989ETRSTM37;
        public readonly ProjectionInfo ETRS1989ETRSTM38;
        public readonly ProjectionInfo ETRS1989ETRSTM39;
        public readonly ProjectionInfo ETRS1989UTMZone26N;
        public readonly ProjectionInfo ETRS1989UTMZone27N;
        public readonly ProjectionInfo ETRS1989UTMZone28N;
        public readonly ProjectionInfo ETRS1989UTMZone29N;
        public readonly ProjectionInfo ETRS1989UTMZone30N;
        public readonly ProjectionInfo ETRS1989UTMZone31N;
        public readonly ProjectionInfo ETRS1989UTMZone32N;
        public readonly ProjectionInfo ETRS1989UTMZone33N;
        public readonly ProjectionInfo ETRS1989UTMZone34N;
        public readonly ProjectionInfo ETRS1989UTMZone35N;
        public readonly ProjectionInfo ETRS1989UTMZone36N;
        public readonly ProjectionInfo ETRS1989UTMZone37N;
        public readonly ProjectionInfo ETRS1989UTMZone38N;
        public readonly ProjectionInfo ETRS1989UTMZone39N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone28N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone29N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone30N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone31N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone32N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone33N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone34N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone35N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone36N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone37N;
        public readonly ProjectionInfo EuropeanDatum1950UTMZone38N;
        public readonly ProjectionInfo FD1954UTMZone29N;
        public readonly ProjectionInfo Hjorsey1955UTMZone26N;
        public readonly ProjectionInfo Hjorsey1955UTMZone27N;
        public readonly ProjectionInfo Hjorsey1955UTMZone28N;
        public readonly ProjectionInfo HTRS96UTMZone33N;
        public readonly ProjectionInfo HTRS96UTMZone34N;
        public readonly ProjectionInfo IRENET95UTMZone29N;
        public readonly ProjectionInfo NGO1948UTMZone32N;
        public readonly ProjectionInfo NGO1948UTMZone33N;
        public readonly ProjectionInfo NGO1948UTMZone34N;
        public readonly ProjectionInfo NGO1948UTMZone35N;
        public readonly ProjectionInfo Qornoq1927UTMZone22N;
        public readonly ProjectionInfo Qornoq1927UTMZone23N;
        public readonly ProjectionInfo REGCAN95UTMZone27N;
        public readonly ProjectionInfo REGCAN95UTMZone28N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe.
        /// </summary>
        public Europe()
        {
            Datum73UTMZone29N = ProjectionInfo.FromEpsgCode(27429).SetNames("Datum_73_UTM_Zone_29N", "GCS_Datum_73", "D_Datum_73");
            ED1950ED77UTMZone38N = ProjectionInfo.FromEpsgCode(2058).SetNames("ED_1950_ED77_UTM_Zone_38N", "GCS_European_1950_ED77", "D_European_1950_ED77");
            ED1950ED77UTMZone39N = ProjectionInfo.FromEpsgCode(2059).SetNames("ED_1950_ED77_UTM_Zone_39N", "GCS_European_1950_ED77", "D_European_1950_ED77");
            ED1950ED77UTMZone40N = ProjectionInfo.FromEpsgCode(2060).SetNames("ED_1950_ED77_UTM_Zone_40N", "GCS_European_1950_ED77", "D_European_1950_ED77");
            ED1950ED77UTMZone41N = ProjectionInfo.FromEpsgCode(2061).SetNames("ED_1950_ED77_UTM_Zone_41N", "GCS_European_1950_ED77", "D_European_1950_ED77");
            ELD1979UTMZone32N = ProjectionInfo.FromEpsgCode(2077).SetNames("ELD_1979_UTM_Zone_32N", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979UTMZone33N = ProjectionInfo.FromEpsgCode(2078).SetNames("ELD_1979_UTM_Zone_33N", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979UTMZone34N = ProjectionInfo.FromEpsgCode(2079).SetNames("ELD_1979_UTM_Zone_34N", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ELD1979UTMZone35N = ProjectionInfo.FromEpsgCode(2080).SetNames("ELD_1979_UTM_Zone_35N", "GCS_European_Libyan_Datum_1979", "D_European_Libyan_1979");
            ETRS1989ETRSTM26 = ProjectionInfo.FromEpsgCode(3038).SetNames("ETRS_1989_ETRS-TM26", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM27 = ProjectionInfo.FromEpsgCode(3039).SetNames("ETRS_1989_ETRS-TM27", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM28 = ProjectionInfo.FromEpsgCode(3040).SetNames("ETRS_1989_ETRS-TM28", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM29 = ProjectionInfo.FromEpsgCode(3041).SetNames("ETRS_1989_ETRS-TM29", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM30 = ProjectionInfo.FromEpsgCode(3042).SetNames("ETRS_1989_ETRS-TM30", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM31 = ProjectionInfo.FromEpsgCode(3043).SetNames("ETRS_1989_ETRS-TM31", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM32 = ProjectionInfo.FromEpsgCode(3044).SetNames("ETRS_1989_ETRS-TM32", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM33 = ProjectionInfo.FromEpsgCode(3045).SetNames("ETRS_1989_ETRS-TM33", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM34 = ProjectionInfo.FromEpsgCode(3046).SetNames("ETRS_1989_ETRS-TM34", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM35 = ProjectionInfo.FromEpsgCode(3047).SetNames("ETRS_1989_ETRS-TM35", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM36 = ProjectionInfo.FromEpsgCode(3048).SetNames("ETRS_1989_ETRS-TM36", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM37 = ProjectionInfo.FromEpsgCode(3049).SetNames("ETRS_1989_ETRS-TM37", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM38 = ProjectionInfo.FromEpsgCode(3050).SetNames("ETRS_1989_ETRS-TM38", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSTM39 = ProjectionInfo.FromEpsgCode(3051).SetNames("ETRS_1989_ETRS-TM39", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone26N = ProjectionInfo.FromAuthorityCode("ESRI", 102097).SetNames("ETRS_1989_UTM_Zone_26N", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UTMZone27N = ProjectionInfo.FromAuthorityCode("ESRI", 102098).SetNames("ETRS_1989_UTM_Zone_27N", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            ETRS1989UTMZone28N = ProjectionInfo.FromEpsgCode(25828).SetNames("ETRS_1989_UTM_Zone_28N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone29N = ProjectionInfo.FromEpsgCode(25829).SetNames("ETRS_1989_UTM_Zone_29N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone30N = ProjectionInfo.FromEpsgCode(25830).SetNames("ETRS_1989_UTM_Zone_30N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone31N = ProjectionInfo.FromEpsgCode(25831).SetNames("ETRS_1989_UTM_Zone_31N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone32N = ProjectionInfo.FromEpsgCode(25832).SetNames("ETRS_1989_UTM_Zone_32N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone33N = ProjectionInfo.FromEpsgCode(25833).SetNames("ETRS_1989_UTM_Zone_33N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone34N = ProjectionInfo.FromEpsgCode(25834).SetNames("ETRS_1989_UTM_Zone_34N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone35N = ProjectionInfo.FromEpsgCode(25835).SetNames("ETRS_1989_UTM_Zone_35N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone36N = ProjectionInfo.FromEpsgCode(25836).SetNames("ETRS_1989_UTM_Zone_36N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone37N = ProjectionInfo.FromEpsgCode(25837).SetNames("ETRS_1989_UTM_Zone_37N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone38N = ProjectionInfo.FromEpsgCode(25838).SetNames("ETRS_1989_UTM_Zone_38N", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989UTMZone39N = ProjectionInfo.FromAuthorityCode("ESRI", 102099).SetNames("ETRS_1989_UTM_Zone_39N", "GCS_ETRS_1989", "D_ETRS_1989"); // missing
            EuropeanDatum1950UTMZone28N = ProjectionInfo.FromEpsgCode(23028).SetNames("ED_1950_UTM_Zone_28N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone29N = ProjectionInfo.FromEpsgCode(23029).SetNames("ED_1950_UTM_Zone_29N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone30N = ProjectionInfo.FromEpsgCode(23030).SetNames("ED_1950_UTM_Zone_30N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone31N = ProjectionInfo.FromEpsgCode(23031).SetNames("ED_1950_UTM_Zone_31N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone32N = ProjectionInfo.FromEpsgCode(23032).SetNames("ED_1950_UTM_Zone_32N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone33N = ProjectionInfo.FromEpsgCode(23033).SetNames("ED_1950_UTM_Zone_33N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone34N = ProjectionInfo.FromEpsgCode(23034).SetNames("ED_1950_UTM_Zone_34N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone35N = ProjectionInfo.FromEpsgCode(23035).SetNames("ED_1950_UTM_Zone_35N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone36N = ProjectionInfo.FromEpsgCode(23036).SetNames("ED_1950_UTM_Zone_36N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone37N = ProjectionInfo.FromEpsgCode(23037).SetNames("ED_1950_UTM_Zone_37N", "GCS_European_1950", "D_European_1950");
            EuropeanDatum1950UTMZone38N = ProjectionInfo.FromEpsgCode(23038).SetNames("ED_1950_UTM_Zone_38N", "GCS_European_1950", "D_European_1950");
            FD1954UTMZone29N = ProjectionInfo.FromEpsgCode(3374).SetNames("FD_1954_UTM_Zone_29N", "GCS_FD_1954", "D_Faroe_Datum_1954");
            Hjorsey1955UTMZone26N = ProjectionInfo.FromEpsgCode(3054).SetNames("Hjorsey_1955_UTM_Zone_26N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            Hjorsey1955UTMZone27N = ProjectionInfo.FromEpsgCode(3055).SetNames("Hjorsey_1955_UTM_Zone_27N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            Hjorsey1955UTMZone28N = ProjectionInfo.FromEpsgCode(3056).SetNames("Hjorsey_1955_UTM_Zone_28N", "GCS_Hjorsey_1955", "D_Hjorsey_1955");
            HTRS96UTMZone33N = ProjectionInfo.FromEpsgCode(3767).SetNames("HTRS96_UTM_Zone_33N", "GCS_HTRS96", "D_Croatian_Terrestrial_Reference_System");
            HTRS96UTMZone34N = ProjectionInfo.FromEpsgCode(3768).SetNames("HTRS96_UTM_Zone_34N", "GCS_HTRS96", "D_Croatian_Terrestrial_Reference_System");
            IRENET95UTMZone29N = ProjectionInfo.FromEpsgCode(2158).SetNames("IRENET95_UTM_Zone_29N", "GCS_IRENET95", "D_IRENET95");
            NGO1948UTMZone32N = ProjectionInfo.FromAuthorityCode("ESRI", 102132).SetNames("NGO_1948_UTM_Zone_32N", "GCS_NGO_1948", "D_NGO_1948");
            NGO1948UTMZone33N = ProjectionInfo.FromAuthorityCode("ESRI", 102133).SetNames("NGO_1948_UTM_Zone_33N", "GCS_NGO_1948", "D_NGO_1948");
            NGO1948UTMZone34N = ProjectionInfo.FromAuthorityCode("ESRI", 102134).SetNames("NGO_1948_UTM_Zone_34N", "GCS_NGO_1948", "D_NGO_1948");
            NGO1948UTMZone35N = ProjectionInfo.FromAuthorityCode("ESRI", 102135).SetNames("NGO_1948_UTM_Zone_35N", "GCS_NGO_1948", "D_NGO_1948");
            Qornoq1927UTMZone22N = ProjectionInfo.FromEpsgCode(2216).SetNames("Qornoq_1927_UTM_Zone_22N", "GCS_Qornoq_1927", "D_Qornoq_1927");
            Qornoq1927UTMZone23N = ProjectionInfo.FromEpsgCode(2217).SetNames("Qornoq_1927_UTM_Zone_23N", "GCS_Qornoq_1927", "D_Qornoq_1927");
            REGCAN95UTMZone27N = ProjectionInfo.FromAuthorityCode("EPSG", 103214).SetNames("REGCAN95_UTM_Zone_27N", "GCS_REGCAN95", "D_Red_Geodesica_de_Canarias_1995"); // missing
            REGCAN95UTMZone28N = ProjectionInfo.FromAuthorityCode("EPSG", 103215).SetNames("REGCAN95_UTM_Zone_28N", "GCS_REGCAN95", "D_Red_Geodesica_de_Canarias_1995"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591