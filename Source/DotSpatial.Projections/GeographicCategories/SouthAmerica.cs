// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:18:17 PM
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
    /// This class contains predefined CoordinateSystems for SouthAmerica.
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Aratu;
        public readonly ProjectionInfo Bogota;
        public readonly ProjectionInfo BogotaBogota;
        public readonly ProjectionInfo CampoInchauspe;
        public readonly ProjectionInfo ChosMalal1914;
        public readonly ProjectionInfo Chua;
        public readonly ProjectionInfo CorregoAlegre;
        public readonly ProjectionInfo CSG1967;
        public readonly ProjectionInfo GuyaneFrancaise;
        public readonly ProjectionInfo HitoXVIII1963;
        public readonly ProjectionInfo LaCanoa;
        public readonly ProjectionInfo Lake;
        public readonly ProjectionInfo LomaQuintana;
        public readonly ProjectionInfo MAGNA;
        public readonly ProjectionInfo MountDillon;
        public readonly ProjectionInfo Naparima1955;
        public readonly ProjectionInfo Naparima1972;
        public readonly ProjectionInfo PampadelCastillo;
        public readonly ProjectionInfo POSGAR;
        public readonly ProjectionInfo POSGAR1994;
        public readonly ProjectionInfo POSGAR1998;
        public readonly ProjectionInfo ProvisionalSouthAmericanDatum1956;
        public readonly ProjectionInfo REGVEN;
        public readonly ProjectionInfo RGFG1995;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SIRGAS;
        public readonly ProjectionInfo SIRGAS2000;
        public readonly ProjectionInfo SouthAmericanDatum1969;
        public readonly ProjectionInfo Trinidad1903;
        public readonly ProjectionInfo Yacare;
        public readonly ProjectionInfo Zanderij;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica.
        /// </summary>
        public SouthAmerica()
        {
            Aratu = ProjectionInfo.FromEpsgCode(4208).SetNames("", "GCS_Aratu", "D_Aratu");
            Bogota = ProjectionInfo.FromEpsgCode(4218).SetNames("", "GCS_Bogota", "D_Bogota");
            BogotaBogota = ProjectionInfo.FromEpsgCode(4802).SetNames("", "GCS_Bogota_Bogota", "D_Bogota");
            CampoInchauspe = ProjectionInfo.FromEpsgCode(4221).SetNames("", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ChosMalal1914 = ProjectionInfo.FromEpsgCode(4160).SetNames("", "GCS_Chos_Malal_1914", "D_Chos_Malal_1914");
            Chua = ProjectionInfo.FromEpsgCode(4224).SetNames("", "GCS_Chua", "D_Chua");
            CorregoAlegre = ProjectionInfo.FromEpsgCode(4225).SetNames("", "GCS_Corrego_Alegre", "D_Corrego_Alegre");
            CSG1967 = ProjectionInfo.FromEpsgCode(4623).SetNames("", "GCS_CSG_1967", "D_CSG_1967");
            GuyaneFrancaise = ProjectionInfo.FromEpsgCode(4235).SetNames("", "GCS_Guyane_Francaise", "D_Guyane_Francaise");
            HitoXVIII1963 = ProjectionInfo.FromEpsgCode(4254).SetNames("", "GCS_Hito_XVIII_1963", "D_Hito_XVIII_1963");
            LaCanoa = ProjectionInfo.FromEpsgCode(4247).SetNames("", "GCS_La_Canoa", "D_La_Canoa");
            Lake = ProjectionInfo.FromEpsgCode(4249).SetNames("", "GCS_Lake", "D_Lake");
            LomaQuintana = ProjectionInfo.FromEpsgCode(4288).SetNames("", "GCS_Loma_Quintana", "D_Loma_Quintana");
            MAGNA = ProjectionInfo.FromEpsgCode(4686).SetNames("", "GCS_MAGNA", "D_MAGNA");
            MountDillon = ProjectionInfo.FromEpsgCode(4157).SetNames("", "GCS_Mount_Dillon", "D_Mount_Dillon");
            Naparima1955 = ProjectionInfo.FromEpsgCode(4158).SetNames("", "GCS_Naparima_1955", "D_Naparima_1955");
            Naparima1972 = ProjectionInfo.FromEpsgCode(4271).SetNames("", "GCS_Naparima_1972", "D_Naparima_1972");
            PampadelCastillo = ProjectionInfo.FromEpsgCode(4161).SetNames("", "GCS_Pampa_del_Castillo", "D_Pampa_del_Castillo");
            POSGAR = ProjectionInfo.FromEpsgCode(4172).SetNames("", "GCS_POSGAR", "D_POSGAR");
            POSGAR1994 = ProjectionInfo.FromEpsgCode(4694).SetNames("", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1998 = ProjectionInfo.FromEpsgCode(4190).SetNames("", "GCS_POSGAR_1998", "D_POSGAR_1998");
            ProvisionalSouthAmericanDatum1956 = ProjectionInfo.FromEpsgCode(4248).SetNames("", "GCS_Provisional_S_American_1956", "D_Provisional_S_American_1956");
            REGVEN = ProjectionInfo.FromEpsgCode(4189).SetNames("", "GCS_REGVEN", "D_REGVEN");
            RGFG1995 = ProjectionInfo.FromEpsgCode(4624).SetNames("", "GCS_RGFG_1995", "D_RGFG_1995");
            SapperHill1943 = ProjectionInfo.FromEpsgCode(4292).SetNames("", "GCS_Sapper_Hill_1943", "D_Sapper_Hill_1943");
            SIRGAS = ProjectionInfo.FromEpsgCode(4170).SetNames("", "GCS_SIRGAS", "D_SIRGAS");
            SIRGAS2000 = ProjectionInfo.FromEpsgCode(4674).SetNames("", "GCS_SIRGAS_2000", "D_SIRGAS_2000");
            SouthAmericanDatum1969 = ProjectionInfo.FromEpsgCode(4618).SetNames("", "GCS_South_American_1969", "D_South_American_1969");
            Trinidad1903 = ProjectionInfo.FromEpsgCode(4302).SetNames("", "GCS_Trinidad_1903", "D_Trinidad_1903");
            Yacare = ProjectionInfo.FromEpsgCode(4309).SetNames("", "GCS_Yacare", "D_Yacare");
            Zanderij = ProjectionInfo.FromEpsgCode(4311).SetNames("", "GCS_Zanderij", "D_Zanderij");
        }

        #endregion
    }
}

#pragma warning restore 1591