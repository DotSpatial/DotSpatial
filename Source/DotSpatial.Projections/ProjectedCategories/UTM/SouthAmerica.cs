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
    /// This class contains predefined CoordinateSystems for SouthAmerica.
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AratuUTMZone22S;
        public readonly ProjectionInfo AratuUTMZone23S;
        public readonly ProjectionInfo AratuUTMZone24S;
        public readonly ProjectionInfo BogotaUTMZone17N;
        public readonly ProjectionInfo BogotaUTMZone18N;
        public readonly ProjectionInfo CampoInchauspeUTM19S;
        public readonly ProjectionInfo CampoInchauspeUTM20S;
        public readonly ProjectionInfo ChuaUTMZone23S;
        public readonly ProjectionInfo CorregoAlegreUTMZone21S;
        public readonly ProjectionInfo CorregoAlegreUTMZone22S;
        public readonly ProjectionInfo CorregoAlegreUTMZone23S;
        public readonly ProjectionInfo CorregoAlegreUTMZone24S;
        public readonly ProjectionInfo CorregoAlegreUTMZone25S;
        public readonly ProjectionInfo CSG1967UTMZone21N;
        public readonly ProjectionInfo CSG1967UTMZone22N;
        public readonly ProjectionInfo HitoXVIII1963UTMZone19S;
        public readonly ProjectionInfo LaCanoaUTMZone18N;
        public readonly ProjectionInfo LaCanoaUTMZone19N;
        public readonly ProjectionInfo LaCanoaUTMZone20N;
        public readonly ProjectionInfo LaCanoaUTMZone21N;
        public readonly ProjectionInfo Naparima1955UTMZone20N;
        public readonly ProjectionInfo Naparima1972UTMZone20N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatum1956UTMZone17N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatum1956UTMZone21S;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone17S;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone18N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone18S;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone19N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone19S;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone20N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone20S;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone21N;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatumUTMZone22S;
        public readonly ProjectionInfo REGVENUTMZone18N;
        public readonly ProjectionInfo REGVENUTMZone19N;
        public readonly ProjectionInfo REGVENUTMZone20N;
        public readonly ProjectionInfo RGFG1995UTMZone21N;
        public readonly ProjectionInfo RGFG1995UTMZone22N;
        public readonly ProjectionInfo SapperHill1943UTMZone20S;
        public readonly ProjectionInfo SapperHill1943UTMZone21S;
        public readonly ProjectionInfo SIRGAS2000UTMZone11N;
        public readonly ProjectionInfo SIRGAS2000UTMZone12N;
        public readonly ProjectionInfo SIRGAS2000UTMZone13N;
        public readonly ProjectionInfo SIRGAS2000UTMZone14N;
        public readonly ProjectionInfo SIRGAS2000UTMZone15N;
        public readonly ProjectionInfo SIRGAS2000UTMZone16N;
        public readonly ProjectionInfo SIRGAS2000UTMZone17N;
        public readonly ProjectionInfo SIRGAS2000UTMZone17S;
        public readonly ProjectionInfo SIRGAS2000UTMZone18N;
        public readonly ProjectionInfo SIRGAS2000UTMZone18S;
        public readonly ProjectionInfo SIRGAS2000UTMZone19N;
        public readonly ProjectionInfo SIRGAS2000UTMZone19S;
        public readonly ProjectionInfo SIRGAS2000UTMZone20N;
        public readonly ProjectionInfo SIRGAS2000UTMZone20S;
        public readonly ProjectionInfo SIRGAS2000UTMZone21N;
        public readonly ProjectionInfo SIRGAS2000UTMZone21S;
        public readonly ProjectionInfo SIRGAS2000UTMZone22N;
        public readonly ProjectionInfo SIRGAS2000UTMZone22S;
        public readonly ProjectionInfo SIRGAS2000UTMZone23S;
        public readonly ProjectionInfo SIRGAS2000UTMZone24S;
        public readonly ProjectionInfo SIRGAS2000UTMZone25S;
        public readonly ProjectionInfo SIRGASUTMZone17N;
        public readonly ProjectionInfo SIRGASUTMZone17S;
        public readonly ProjectionInfo SIRGASUTMZone18N;
        public readonly ProjectionInfo SIRGASUTMZone18S;
        public readonly ProjectionInfo SIRGASUTMZone19N;
        public readonly ProjectionInfo SIRGASUTMZone19S;
        public readonly ProjectionInfo SIRGASUTMZone20N;
        public readonly ProjectionInfo SIRGASUTMZone20S;
        public readonly ProjectionInfo SIRGASUTMZone21N;
        public readonly ProjectionInfo SIRGASUTMZone21S;
        public readonly ProjectionInfo SIRGASUTMZone22N;
        public readonly ProjectionInfo SIRGASUTMZone22S;
        public readonly ProjectionInfo SIRGASUTMZone23S;
        public readonly ProjectionInfo SIRGASUTMZone24S;
        public readonly ProjectionInfo SIRGASUTMZone25S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone17S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone18N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone18S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone19N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone19S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone20N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone20S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone21N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone21S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone22N;
        public readonly ProjectionInfo SouthAmerican1969UTMZone22S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone23S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone24S;
        public readonly ProjectionInfo SouthAmerican1969UTMZone25S;
        public readonly ProjectionInfo Zanderij1972UTMZone21N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica.
        /// </summary>
        public SouthAmerica()
        {
            AratuUTMZone22S = ProjectionInfo.FromEpsgCode(20822).SetNames("Aratu_UTM_Zone_22S", "GCS_Aratu", "D_Aratu");
            AratuUTMZone23S = ProjectionInfo.FromEpsgCode(20823).SetNames("Aratu_UTM_Zone_23S", "GCS_Aratu", "D_Aratu");
            AratuUTMZone24S = ProjectionInfo.FromEpsgCode(20824).SetNames("Aratu_UTM_Zone_24S", "GCS_Aratu", "D_Aratu");
            BogotaUTMZone17N = ProjectionInfo.FromEpsgCode(21817).SetNames("Bogota_UTM_Zone_17N", "GCS_Bogota", "D_Bogota");
            BogotaUTMZone18N = ProjectionInfo.FromEpsgCode(21818).SetNames("Bogota_UTM_Zone_18N", "GCS_Bogota", "D_Bogota");
            CampoInchauspeUTM19S = ProjectionInfo.FromEpsgCode(2315).SetNames("Campo_Inchauspe_UTM_19S", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            CampoInchauspeUTM20S = ProjectionInfo.FromEpsgCode(2316).SetNames("Campo_Inchauspe_UTM_20S", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ChuaUTMZone23S = ProjectionInfo.FromAuthorityCode("EPSG", 103213).SetNames("Chua_UTM_Zone_23S", "GCS_Chua", "D_Chua"); // missing
            CorregoAlegreUTMZone21S = ProjectionInfo.FromEpsgCode(22521).SetNames("Corrego_Alegre_UTM_Zone_21S", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CorregoAlegreUTMZone22S = ProjectionInfo.FromEpsgCode(22522).SetNames("Corrego_Alegre_UTM_Zone_22S", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CorregoAlegreUTMZone23S = ProjectionInfo.FromEpsgCode(22523).SetNames("Corrego_Alegre_UTM_Zone_23S", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CorregoAlegreUTMZone24S = ProjectionInfo.FromEpsgCode(22524).SetNames("Corrego_Alegre_UTM_Zone_24S", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CorregoAlegreUTMZone25S = ProjectionInfo.FromEpsgCode(22525).SetNames("Corrego_Alegre_UTM_Zone_25S", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CSG1967UTMZone21N = ProjectionInfo.FromEpsgCode(3312).SetNames("CSG_1967_UTM_Zone_21N", "GCS_CSG_1967", "D_CSG_1967");
            CSG1967UTMZone22N = ProjectionInfo.FromEpsgCode(2971).SetNames("CSG_1967_UTM_22N", "GCS_CSG_1967", "D_CSG_1967");
            HitoXVIII1963UTMZone19S = ProjectionInfo.FromEpsgCode(2084).SetNames("Hito_XVIII_1963_UTM_19S", "GCS_Hito_XVIII_1963", "D_Hito_XVIII_1963");
            LaCanoaUTMZone18N = ProjectionInfo.FromEpsgCode(24718).SetNames("La_Canoa_UTM_Zone_18N", "GCS_La_Canoa", "D_La_Canoa");
            LaCanoaUTMZone19N = ProjectionInfo.FromEpsgCode(24719).SetNames("La_Canoa_UTM_Zone_19N", "GCS_La_Canoa", "D_La_Canoa");
            LaCanoaUTMZone20N = ProjectionInfo.FromEpsgCode(24720).SetNames("La_Canoa_UTM_Zone_20N", "GCS_La_Canoa", "D_La_Canoa");
            LaCanoaUTMZone21N = ProjectionInfo.FromAuthorityCode("EPSG", 24721).SetNames("La_Canoa_UTM_Zone_21N", "GCS_La_Canoa", "D_La_Canoa"); // missing
            Naparima1955UTMZone20N = ProjectionInfo.FromEpsgCode(2067).SetNames("Naparima_1955_UTM_Zone_20N", "GCS_Naparima_1955", "D_Naparima_1955");
            Naparima1972UTMZone20N = ProjectionInfo.FromEpsgCode(27120).SetNames("Naparima_1972_UTM_Zone_20N", "GCS_Naparima_1972", "D_Naparima_1972");
            ProvisionalSouthAmericanDatum1956UTMZone17N = ProjectionInfo.FromEpsgCode(24817).SetNames("PSAD_1956_UTM_Zone_17N", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatum1956UTMZone21S = ProjectionInfo.FromEpsgCode(24881).SetNames("PSAD_1956_UTM_Zone_21S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone17S = ProjectionInfo.FromEpsgCode(24877).SetNames("PSAD_1956_UTM_Zone_17S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone18N = ProjectionInfo.FromEpsgCode(24818).SetNames("PSAD_1956_UTM_Zone_18N", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone18S = ProjectionInfo.FromEpsgCode(24878).SetNames("PSAD_1956_UTM_Zone_18S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone19N = ProjectionInfo.FromEpsgCode(24819).SetNames("PSAD_1956_UTM_Zone_19N", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone19S = ProjectionInfo.FromEpsgCode(24879).SetNames("PSAD_1956_UTM_Zone_19S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone20N = ProjectionInfo.FromEpsgCode(24820).SetNames("PSAD_1956_UTM_Zone_20N", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone20S = ProjectionInfo.FromEpsgCode(24880).SetNames("PSAD_1956_UTM_Zone_20S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone21N = ProjectionInfo.FromEpsgCode(24821).SetNames("PSAD_1956_UTM_Zone_21N", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            ProvisionalSouthAmericanDatumUTMZone22S = ProjectionInfo.FromEpsgCode(24882).SetNames("PSAD_1956_UTM_Zone_22S", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            REGVENUTMZone18N = ProjectionInfo.FromEpsgCode(2201).SetNames("REGVEN_UTM_Zone_18N", "GCS_REGVEN", "D_REGVEN");
            REGVENUTMZone19N = ProjectionInfo.FromEpsgCode(2202).SetNames("REGVEN_UTM_Zone_19N", "GCS_REGVEN", "D_REGVEN");
            REGVENUTMZone20N = ProjectionInfo.FromEpsgCode(2203).SetNames("REGVEN_UTM_Zone_20N", "GCS_REGVEN", "D_REGVEN");
            RGFG1995UTMZone21N = ProjectionInfo.FromEpsgCode(3313).SetNames("RGFG_1995_UTM_Zone_21N", "GCS_RGFG_1995", "D_RGFG_1995");
            RGFG1995UTMZone22N = ProjectionInfo.FromEpsgCode(2972).SetNames("RGFG_1995_UTM_22N", "GCS_RGFG_1995", "D_RGFG_1995");
            SapperHill1943UTMZone20S = ProjectionInfo.FromEpsgCode(29220).SetNames("Sapper_Hill_1943_UTM_Zone_20S", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SapperHill1943UTMZone21S = ProjectionInfo.FromEpsgCode(29221).SetNames("Sapper_Hill_1943_UTM_Zone_21S", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SIRGAS2000UTMZone11N = ProjectionInfo.FromEpsgCode(31965).SetNames("SIRGAS_2000_UTM_Zone_11N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone12N = ProjectionInfo.FromEpsgCode(31966).SetNames("SIRGAS_2000_UTM_Zone_12N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone13N = ProjectionInfo.FromEpsgCode(31967).SetNames("SIRGAS_2000_UTM_Zone_13N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone14N = ProjectionInfo.FromEpsgCode(31968).SetNames("SIRGAS_2000_UTM_Zone_14N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone15N = ProjectionInfo.FromEpsgCode(31969).SetNames("SIRGAS_2000_UTM_Zone_15N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone16N = ProjectionInfo.FromEpsgCode(31970).SetNames("SIRGAS_2000_UTM_Zone_16N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone17N = ProjectionInfo.FromEpsgCode(31971).SetNames("SIRGAS_2000_UTM_Zone_17N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone17S = ProjectionInfo.FromEpsgCode(31977).SetNames("SIRGAS_2000_UTM_Zone_17S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone18N = ProjectionInfo.FromEpsgCode(31972).SetNames("SIRGAS_2000_UTM_Zone_18N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone18S = ProjectionInfo.FromEpsgCode(31978).SetNames("SIRGAS_2000_UTM_Zone_18S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone19N = ProjectionInfo.FromEpsgCode(31973).SetNames("SIRGAS_2000_UTM_Zone_19N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone19S = ProjectionInfo.FromEpsgCode(31979).SetNames("SIRGAS_2000_UTM_Zone_19S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone20N = ProjectionInfo.FromEpsgCode(31974).SetNames("SIRGAS_2000_UTM_Zone_20N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone20S = ProjectionInfo.FromEpsgCode(31980).SetNames("SIRGAS_2000_UTM_Zone_20S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone21N = ProjectionInfo.FromEpsgCode(31975).SetNames("SIRGAS_2000_UTM_Zone_21N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone21S = ProjectionInfo.FromEpsgCode(31981).SetNames("SIRGAS_2000_UTM_Zone_21S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone22N = ProjectionInfo.FromEpsgCode(31976).SetNames("SIRGAS_2000_UTM_Zone_22N", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone22S = ProjectionInfo.FromEpsgCode(31982).SetNames("SIRGAS_2000_UTM_Zone_22S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone23S = ProjectionInfo.FromEpsgCode(31983).SetNames("SIRGAS_2000_UTM_Zone_23S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone24S = ProjectionInfo.FromEpsgCode(31984).SetNames("SIRGAS_2000_UTM_Zone_24S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGAS2000UTMZone25S = ProjectionInfo.FromEpsgCode(31985).SetNames("SIRGAS_2000_UTM_Zone_25S", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SIRGASUTMZone17N = ProjectionInfo.FromEpsgCode(31986).SetNames("SIRGAS_UTM_Zone_17N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone17S = ProjectionInfo.FromEpsgCode(31992).SetNames("SIRGAS_UTM_Zone_17S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone18N = ProjectionInfo.FromEpsgCode(31987).SetNames("SIRGAS_UTM_Zone_18N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone18S = ProjectionInfo.FromEpsgCode(31993).SetNames("SIRGAS_UTM_Zone_18S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone19N = ProjectionInfo.FromEpsgCode(31988).SetNames("SIRGAS_UTM_Zone_19N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone19S = ProjectionInfo.FromEpsgCode(31994).SetNames("SIRGAS_UTM_Zone_19S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone20N = ProjectionInfo.FromEpsgCode(31989).SetNames("SIRGAS_UTM_Zone_20N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone20S = ProjectionInfo.FromEpsgCode(31995).SetNames("SIRGAS_UTM_Zone_20S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone21N = ProjectionInfo.FromEpsgCode(31990).SetNames("SIRGAS_UTM_Zone_21N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone21S = ProjectionInfo.FromEpsgCode(31996).SetNames("SIRGAS_UTM_Zone_21S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone22N = ProjectionInfo.FromEpsgCode(31991).SetNames("SIRGAS_UTM_Zone_22N", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone22S = ProjectionInfo.FromEpsgCode(31997).SetNames("SIRGAS_UTM_Zone_22S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone23S = ProjectionInfo.FromEpsgCode(31998).SetNames("SIRGAS_UTM_Zone_23S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone24S = ProjectionInfo.FromEpsgCode(31999).SetNames("SIRGAS_UTM_Zone_24S", "GCS_SIRGAS", "D_SIRGAS");
            SIRGASUTMZone25S = ProjectionInfo.FromEpsgCode(32000).SetNames("SIRGAS_UTM_Zone_25S", "GCS_SIRGAS", "D_SIRGAS");
            SouthAmerican1969UTMZone17S = ProjectionInfo.FromEpsgCode(29187).SetNames("SAD_1969_UTM_Zone_17S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone18N = ProjectionInfo.FromEpsgCode(29168).SetNames("SAD_1969_UTM_Zone_18N", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone18S = ProjectionInfo.FromEpsgCode(29188).SetNames("SAD_1969_UTM_Zone_18S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone19N = ProjectionInfo.FromEpsgCode(29169).SetNames("SAD_1969_UTM_Zone_19N", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone19S = ProjectionInfo.FromEpsgCode(29189).SetNames("SAD_1969_UTM_Zone_19S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone20N = ProjectionInfo.FromEpsgCode(29170).SetNames("SAD_1969_UTM_Zone_20N", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone20S = ProjectionInfo.FromEpsgCode(29190).SetNames("SAD_1969_UTM_Zone_20S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone21N = ProjectionInfo.FromEpsgCode(29171).SetNames("SAD_1969_UTM_Zone_21N", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone21S = ProjectionInfo.FromEpsgCode(29191).SetNames("SAD_1969_UTM_Zone_21S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone22N = ProjectionInfo.FromEpsgCode(29172).SetNames("SAD_1969_UTM_Zone_22N", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone22S = ProjectionInfo.FromEpsgCode(29192).SetNames("SAD_1969_UTM_Zone_22S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone23S = ProjectionInfo.FromEpsgCode(29193).SetNames("SAD_1969_UTM_Zone_23S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone24S = ProjectionInfo.FromEpsgCode(29194).SetNames("SAD_1969_UTM_Zone_24S", "GCS_South_American_1969", "D_South_American_1969");
            SouthAmerican1969UTMZone25S = ProjectionInfo.FromEpsgCode(29195).SetNames("SAD_1969_UTM_Zone_25S", "GCS_South_American_1969", "D_South_American_1969");
            Zanderij1972UTMZone21N = ProjectionInfo.FromEpsgCode(31121).SetNames("Zanderij_1972_UTM_Zone_21N", "GCS_Zanderij", "D_Zanderij");
        }

        #endregion
    }
}

#pragma warning restore 1591