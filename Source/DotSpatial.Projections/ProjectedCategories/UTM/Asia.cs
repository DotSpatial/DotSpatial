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
    /// This class contains predefined CoordinateSystems for Asia.
    /// </summary>
    public class Asia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AfgooyeUTMZone38N;
        public readonly ProjectionInfo AfgooyeUTMZone39N;
        public readonly ProjectionInfo AinelAbd1970UTMZone36N;
        public readonly ProjectionInfo AinelAbd1970UTMZone37N;
        public readonly ProjectionInfo AinelAbd1970UTMZone38N;
        public readonly ProjectionInfo AinelAbd1970UTMZone39N;
        public readonly ProjectionInfo AinelAbd1970UTMZone40N;
        public readonly ProjectionInfo Dabola1981UTMZone28N;
        public readonly ProjectionInfo Dabola1981UTMZone29N;
        public readonly ProjectionInfo DoualaUTMZone32N;
        public readonly ProjectionInfo FahudUTMZone39N;
        public readonly ProjectionInfo FahudUTMZone40N;
        public readonly ProjectionInfo HongKong1980UTMZone49N;
        public readonly ProjectionInfo HongKong1980UTMZone50N;
        public readonly ProjectionInfo IGM1995UTMZone32N;
        public readonly ProjectionInfo IGM1995UTMZone33N;
        public readonly ProjectionInfo IGRSUTMZone37N;
        public readonly ProjectionInfo IGRSUTMZone38N;
        public readonly ProjectionInfo IGRSUTMZone39N;
        public readonly ProjectionInfo Indian1954UTMZone46N;
        public readonly ProjectionInfo Indian1954UTMZone47N;
        public readonly ProjectionInfo Indian1954UTMZone48N;
        public readonly ProjectionInfo Indian1960UTMZone48N;
        public readonly ProjectionInfo Indian1960UTMZone49N;
        public readonly ProjectionInfo Indian1975UTMZone47N;
        public readonly ProjectionInfo Indian1975UTMZone48N;
        public readonly ProjectionInfo JGD2000UTMZone51N;
        public readonly ProjectionInfo JGD2000UTMZone52N;
        public readonly ProjectionInfo JGD2000UTMZone53N;
        public readonly ProjectionInfo JGD2000UTMZone54N;
        public readonly ProjectionInfo JGD2000UTMZone55N;
        public readonly ProjectionInfo JGD2000UTMZone56N;
        public readonly ProjectionInfo Karbala1979PolserviceUTMZone37N;
        public readonly ProjectionInfo Karbala1979PolserviceUTMZone38N;
        public readonly ProjectionInfo Karbala1979PolserviceUTMZone39N;
        public readonly ProjectionInfo MONREF1997UTMZone46N;
        public readonly ProjectionInfo MONREF1997UTMZone47N;
        public readonly ProjectionInfo MONREF1997UTMZone48N;
        public readonly ProjectionInfo MONREF1997UTMZone49N;
        public readonly ProjectionInfo MONREF1997UTMZone50N;
        public readonly ProjectionInfo Nahrwan1967UTMZone37N;
        public readonly ProjectionInfo Nahrwan1967UTMZone38N;
        public readonly ProjectionInfo Nahrwan1967UTMZone39N;
        public readonly ProjectionInfo Nahrwan1967UTMZone40N;
        public readonly ProjectionInfo NakhlEGhanemUTMZone39N;
        public readonly ProjectionInfo PDO1993UTMZone39N;
        public readonly ProjectionInfo PDO1993UTMZone40N;
        public readonly ProjectionInfo QND1995UTMZone39N;
        public readonly ProjectionInfo SambojaUTMZone50S;
        public readonly ProjectionInfo TokyoUTMZone51N;
        public readonly ProjectionInfo TokyoUTMZone52N;
        public readonly ProjectionInfo TokyoUTMZone53N;
        public readonly ProjectionInfo TokyoUTMZone54N;
        public readonly ProjectionInfo TokyoUTMZone55N;
        public readonly ProjectionInfo TokyoUTMZone56N;
        public readonly ProjectionInfo TrucialCoast1948UTMZone39N;
        public readonly ProjectionInfo TrucialCoast1948UTMZone40N;
        public readonly ProjectionInfo VN2000UTMZone48N;
        public readonly ProjectionInfo VN2000UTMZone49N;
        public readonly ProjectionInfo YemenNGN1996UTMZone38N;
        public readonly ProjectionInfo YemenNGN1996UTMZone39N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Asia.
        /// </summary>
        public Asia()
        {
            AfgooyeUTMZone38N = ProjectionInfo.FromEpsgCode(20538).SetNames("Afgooye_UTM_Zone_38N", "GCS_Afgooye", "D_Afgooye");
            AfgooyeUTMZone39N = ProjectionInfo.FromEpsgCode(20539).SetNames("Afgooye_UTM_Zone_39N", "GCS_Afgooye", "D_Afgooye");
            AinelAbd1970UTMZone36N = ProjectionInfo.FromEpsgCode(20436).SetNames("Ain_el_Abd_UTM_Zone_36N", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            AinelAbd1970UTMZone37N = ProjectionInfo.FromEpsgCode(20437).SetNames("Ain_el_Abd_UTM_Zone_37N", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            AinelAbd1970UTMZone38N = ProjectionInfo.FromEpsgCode(20438).SetNames("Ain_el_Abd_UTM_Zone_38N", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            AinelAbd1970UTMZone39N = ProjectionInfo.FromEpsgCode(20439).SetNames("Ain_el_Abd_UTM_Zone_39N", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            AinelAbd1970UTMZone40N = ProjectionInfo.FromEpsgCode(20440).SetNames("Ain_el_Abd_UTM_Zone_40N", "GCS_Ain_el_Abd_1970", "D_Ain_el_Abd_1970");
            Dabola1981UTMZone28N = ProjectionInfo.FromEpsgCode(3461).SetNames("Dabola_1981_UTM_Zone_28N", "GCS_Dabola_1981", "D_Dabola_1981");
            Dabola1981UTMZone29N = ProjectionInfo.FromEpsgCode(3462).SetNames("Dabola_1981_UTM_Zone_29N", "GCS_Dabola_1981", "D_Dabola_1981");
            DoualaUTMZone32N = ProjectionInfo.FromEpsgCode(22832).SetNames("Douala_UTM_Zone_32N", "GCS_Douala", "D_Douala");
            FahudUTMZone39N = ProjectionInfo.FromEpsgCode(23239).SetNames("Fahud_UTM_Zone_39N", "GCS_Fahud", "D_Fahud");
            FahudUTMZone40N = ProjectionInfo.FromEpsgCode(23240).SetNames("Fahud_UTM_Zone_40N", "GCS_Fahud", "D_Fahud");
            HongKong1980UTMZone49N = ProjectionInfo.FromAuthorityCode("ESRI", 102141).SetNames("Hong_Kong_1980_UTM_Zone_49N", "GCS_Hong_Kong_1980", "D_Hong_Kong_1980");
            HongKong1980UTMZone50N = ProjectionInfo.FromAuthorityCode("ESRI", 102142).SetNames("Hong_Kong_1980_UTM_Zone_50N", "GCS_Hong_Kong_1980", "D_Hong_Kong_1980");
            IGM1995UTMZone32N = ProjectionInfo.FromEpsgCode(3064).SetNames("IGM_1995_UTM_Zone_32N", "GCS_IGM_1995", "D_IGM_1995");
            IGM1995UTMZone33N = ProjectionInfo.FromEpsgCode(3065).SetNames("IGM_1995_UTM_Zone_33N", "GCS_IGM_1995", "D_IGM_1995");
            IGRSUTMZone37N = ProjectionInfo.FromEpsgCode(3890).SetNames("IGRS_UTM_Zone_37N", "GCS_IGRS", "D_Iraqi_Geospatial_Reference_System");
            IGRSUTMZone38N = ProjectionInfo.FromEpsgCode(3891).SetNames("IGRS_UTM_Zone_38N", "GCS_IGRS", "D_Iraqi_Geospatial_Reference_System");
            IGRSUTMZone39N = ProjectionInfo.FromEpsgCode(3892).SetNames("IGRS_UTM_Zone_39N", "GCS_IGRS", "D_Iraqi_Geospatial_Reference_System");
            Indian1954UTMZone46N = ProjectionInfo.FromEpsgCode(23946).SetNames("Indian_1954_UTM_Zone_46N", "GCS_Indian_1954", "D_Indian_1954");
            Indian1954UTMZone47N = ProjectionInfo.FromEpsgCode(23947).SetNames("Indian_1954_UTM_Zone_47N", "GCS_Indian_1954", "D_Indian_1954");
            Indian1954UTMZone48N = ProjectionInfo.FromEpsgCode(23948).SetNames("Indian_1954_UTM_Zone_48N", "GCS_Indian_1954", "D_Indian_1954");
            Indian1960UTMZone48N = ProjectionInfo.FromEpsgCode(3148).SetNames("Indian_1960_UTM_Zone_48N", "GCS_Indian_1960", "D_Indian_1960");
            Indian1960UTMZone49N = ProjectionInfo.FromEpsgCode(3149).SetNames("Indian_1960_UTM_Zone_49N", "GCS_Indian_1960", "D_Indian_1960");
            Indian1975UTMZone47N = ProjectionInfo.FromEpsgCode(24047).SetNames("Indian_1975_UTM_Zone_47N", "GCS_Indian_1975", "D_Indian_1975");
            Indian1975UTMZone48N = ProjectionInfo.FromEpsgCode(24048).SetNames("Indian_1975_UTM_Zone_48N", "GCS_Indian_1975", "D_Indian_1975");
            JGD2000UTMZone51N = ProjectionInfo.FromEpsgCode(3097).SetNames("JGD_2000_UTM_Zone_51N", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000UTMZone52N = ProjectionInfo.FromEpsgCode(3098).SetNames("JGD_2000_UTM_Zone_52N", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000UTMZone53N = ProjectionInfo.FromEpsgCode(3099).SetNames("JGD_2000_UTM_Zone_53N", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000UTMZone54N = ProjectionInfo.FromEpsgCode(3100).SetNames("JGD_2000_UTM_Zone_54N", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000UTMZone55N = ProjectionInfo.FromEpsgCode(3101).SetNames("JGD_2000_UTM_Zone_55N", "GCS_JGD_2000", "D_JGD_2000");
            JGD2000UTMZone56N = ProjectionInfo.FromAuthorityCode("ESRI", 102150).SetNames("JGD_2000_UTM_Zone_56N", "GCS_JGD_2000", "D_JGD_2000"); // missing
            Karbala1979PolserviceUTMZone37N = ProjectionInfo.FromEpsgCode(3391).SetNames("Karbala_1979_Polservice_UTM_Zone_37N", "GCS_Karbala_1979_Polservice", "D_Karbala_1979_Polservice");
            Karbala1979PolserviceUTMZone38N = ProjectionInfo.FromEpsgCode(3392).SetNames("Karbala_1979_Polservice_UTM_Zone_38N", "GCS_Karbala_1979_Polservice", "D_Karbala_1979_Polservice");
            Karbala1979PolserviceUTMZone39N = ProjectionInfo.FromEpsgCode(3393).SetNames("Karbala_1979_Polservice_UTM_Zone_39N", "GCS_Karbala_1979_Polservice", "D_Karbala_1979_Polservice");
            MONREF1997UTMZone46N = ProjectionInfo.FromAuthorityCode("ESRI", 102224).SetNames("MONREF_1997_UTM_Zone_46N", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            MONREF1997UTMZone47N = ProjectionInfo.FromAuthorityCode("ESRI", 102225).SetNames("MONREF_1997_UTM_Zone_47N", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            MONREF1997UTMZone48N = ProjectionInfo.FromAuthorityCode("ESRI", 102226).SetNames("MONREF_1997_UTM_Zone_48N", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            MONREF1997UTMZone49N = ProjectionInfo.FromAuthorityCode("ESRI", 102227).SetNames("MONREF_1997_UTM_Zone_49N", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            MONREF1997UTMZone50N = ProjectionInfo.FromAuthorityCode("ESRI", 102228).SetNames("MONREF_1997_UTM_Zone_50N", "GCS_MONREF_1997", "D_ITRF_2000"); // missing
            Nahrwan1967UTMZone37N = ProjectionInfo.FromEpsgCode(27037).SetNames("Nahrwan_1967_UTM_Zone_37N", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            Nahrwan1967UTMZone38N = ProjectionInfo.FromEpsgCode(27038).SetNames("Nahrwan_1967_UTM_Zone_38N", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            Nahrwan1967UTMZone39N = ProjectionInfo.FromEpsgCode(27039).SetNames("Nahrwan_1967_UTM_Zone_39N", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            Nahrwan1967UTMZone40N = ProjectionInfo.FromEpsgCode(27040).SetNames("Nahrwan_1967_UTM_Zone_40N", "GCS_Nahrwan_1967", "D_Nahrwan_1967");
            NakhlEGhanemUTMZone39N = ProjectionInfo.FromEpsgCode(3307).SetNames("Nakhl-e_Ghanem_UTM_Zone_39N", "GCS_Nakhl-e_Ghanem", "D_Nakhl-e_Ghanem");
            PDO1993UTMZone39N = ProjectionInfo.FromEpsgCode(3439).SetNames("PDO_1993_UTM_Zone_39N", "GCS_PDO_1993", "D_PDO_1993");
            PDO1993UTMZone40N = ProjectionInfo.FromEpsgCode(3440).SetNames("PDO_1993_UTM_Zone_40N", "GCS_PDO_1993", "D_PDO_1993");
            QND1995UTMZone39N = ProjectionInfo.FromAuthorityCode("ESRI", 102143).SetNames("QND_1995_UTM_39N", "GCS_QND_1995", "D_QND_1995"); // missing
            SambojaUTMZone50S = ProjectionInfo.FromEpsgCode(2550).SetNames("Samboja_UTM_Zone_50S", "GCS_Samboja", "D_Samboja");
            TokyoUTMZone51N = ProjectionInfo.FromEpsgCode(3092).SetNames("Tokyo_UTM_Zone_51N", "GCS_Tokyo", "D_Tokyo");
            TokyoUTMZone52N = ProjectionInfo.FromEpsgCode(3093).SetNames("Tokyo_UTM_Zone_52N", "GCS_Tokyo", "D_Tokyo");
            TokyoUTMZone53N = ProjectionInfo.FromEpsgCode(3094).SetNames("Tokyo_UTM_Zone_53N", "GCS_Tokyo", "D_Tokyo");
            TokyoUTMZone54N = ProjectionInfo.FromEpsgCode(3095).SetNames("Tokyo_UTM_Zone_54N", "GCS_Tokyo", "D_Tokyo");
            TokyoUTMZone55N = ProjectionInfo.FromEpsgCode(3096).SetNames("Tokyo_UTM_Zone_55N", "GCS_Tokyo", "D_Tokyo");
            TokyoUTMZone56N = ProjectionInfo.FromAuthorityCode("ESRI", 102156).SetNames("Tokyo_UTM_Zone_56N", "GCS_Tokyo", "D_Tokyo");
            TrucialCoast1948UTMZone39N = ProjectionInfo.FromEpsgCode(30339).SetNames("TC_1948_UTM_Zone_39N", "GCS_Trucial_Coast_1948", "D_Trucial_Coast_1948");
            TrucialCoast1948UTMZone40N = ProjectionInfo.FromEpsgCode(30340).SetNames("TC_1948_UTM_Zone_40N", "GCS_Trucial_Coast_1948", "D_Trucial_Coast_1948");
            VN2000UTMZone48N = ProjectionInfo.FromEpsgCode(3405).SetNames("VN_2000_UTM_Zone_48N", "GCS_VN_2000", "D_Vietnam_2000");
            VN2000UTMZone49N = ProjectionInfo.FromEpsgCode(3406).SetNames("VN_2000_UTM_Zone_49N", "GCS_VN_2000", "D_Vietnam_2000");
            YemenNGN1996UTMZone38N = ProjectionInfo.FromEpsgCode(2089).SetNames("Yemen_NGN_1996_UTM_Zone_38N", "GCS_Yemen_NGN_1996", "D_Yemen_NGN_1996");
            YemenNGN1996UTMZone39N = ProjectionInfo.FromEpsgCode(2090).SetNames("Yemen_NGN_1996_UTM_Zone_39N", "GCS_Yemen_NGN_1996", "D_Yemen_NGN_1996");
        }

        #endregion
    }
}

#pragma warning restore 1591