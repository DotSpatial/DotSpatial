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
    /// SouthAmerica
    /// </summary>
    public class SouthAmerica : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo Aratu;
        public readonly ProjectionInfo Bogota;
        public readonly ProjectionInfo BogotaBogota;
        public readonly ProjectionInfo CampoInchauspe;
        public readonly ProjectionInfo ChosMalal1914;
        public readonly ProjectionInfo Chua;
        public readonly ProjectionInfo CorregoAlegre;
        public readonly ProjectionInfo GuyaneFrancaise;
        public readonly ProjectionInfo HitoXVIII1963;
        public readonly ProjectionInfo LaCanoa;
        public readonly ProjectionInfo Lake;
        public readonly ProjectionInfo LomaQuintana;
        public readonly ProjectionInfo MountDillon;
        public readonly ProjectionInfo Naparima1955;
        public readonly ProjectionInfo Naparima1972;
        public readonly ProjectionInfo POSGAR;
        public readonly ProjectionInfo POSGAR1998;
        public readonly ProjectionInfo PampadelCastillo;
        public readonly ProjectionInfo ProvisionalSouthAmer;
        public readonly ProjectionInfo REGVEN;
        public readonly ProjectionInfo SIRGAS;
        public readonly ProjectionInfo SapperHill1943;
        public readonly ProjectionInfo SouthAmericanDatum1969;
        public readonly ProjectionInfo Trinidad1903;
        public readonly ProjectionInfo Yacare;
        public readonly ProjectionInfo Zanderij;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SouthAmerica
        /// </summary>
        public SouthAmerica()
        {
            Aratu = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Bogota = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            BogotaBogota = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +pm=-74.08091666666667 +no_defs ");
            CampoInchauspe = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            ChosMalal1914 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Chua = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            CorregoAlegre = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            GuyaneFrancaise = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            HitoXVIII1963 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            LaCanoa = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Lake = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            LomaQuintana = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            MountDillon = ProjectionInfo.FromProj4String("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Naparima1955 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Naparima1972 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            PampadelCastillo = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            POSGAR = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            POSGAR1998 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            ProvisionalSouthAmer = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            REGVEN = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            SapperHill1943 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            SIRGAS = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            SouthAmericanDatum1969 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            Trinidad1903 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Yacare = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            Zanderij = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");

            Aratu.Name = "GCS_Aratu";
            Aratu.GeographicInfo.Name = "GCS_Aratu";
            Bogota.Name = "GCS_Bogota";
            Bogota.GeographicInfo.Name = "GCS_Bogota";
            BogotaBogota.Name = "GCS_Bogota_Bogota";
            BogotaBogota.GeographicInfo.Name = "GCS_Bogota_Bogota";
            CampoInchauspe.Name = "GCS_Campo_Inchauspe";
            CampoInchauspe.GeographicInfo.Name = "GCS_Campo_Inchauspe";
            ChosMalal1914.Name = "GCS_Chos_Malal_1914";
            ChosMalal1914.GeographicInfo.Name = "GCS_Chos_Malal_1914";
            Chua.Name = "GCS_Chua";
            Chua.GeographicInfo.Name = "GCS_Chua";
            CorregoAlegre.Name = "GCS_Corrego_Alegre";
            CorregoAlegre.GeographicInfo.Name = "GCS_Corrego_Alegre";
            GuyaneFrancaise.Name = "GCS_Guyane_Francaise";
            GuyaneFrancaise.GeographicInfo.Name = "GCS_Guyane_Francaise";
            HitoXVIII1963.Name = "GCS_Hito_XVIII_1963";
            HitoXVIII1963.GeographicInfo.Name = "GCS_Hito_XVIII_1963";
            LaCanoa.Name = "GCS_La_Canoa";
            LaCanoa.GeographicInfo.Name = "GCS_La_Canoa";
            Lake.Name = "GCS_Lake";
            Lake.GeographicInfo.Name = "GCS_Lake";
            LomaQuintana.Name = "GCS_Loma_Quintana";
            LomaQuintana.GeographicInfo.Name = "GCS_Loma_Quintana";
            MountDillon.Name = "GCS_Mount_Dillon";
            MountDillon.GeographicInfo.Name = "GCS_Mount_Dillon";
            Naparima1955.Name = "GCS_Naparima_1955";
            Naparima1955.GeographicInfo.Name = "GCS_Naparima_1955";
            Naparima1972.Name = "GCS_Naparima_1972";
            Naparima1972.GeographicInfo.Name = "GCS_Naparima_1972";
            PampadelCastillo.Name = "GCS_Pampa_del_Castillo";
            PampadelCastillo.GeographicInfo.Name = "GCS_Pampa_del_Castillo";
            POSGAR.Name = "GCS_POSGAR";
            POSGAR.GeographicInfo.Name = "GCS_POSGAR";
            POSGAR1998.Name = "GCS_POSGAR_1998";
            POSGAR1998.GeographicInfo.Name = "GCS_POSGAR_1998";
            ProvisionalSouthAmer.Name = "GCS_Provisional_S_American_1956";
            ProvisionalSouthAmer.GeographicInfo.Name = "GCS_Provisional_S_American_1956";
            REGVEN.Name = "GCS_REGVEN";
            REGVEN.GeographicInfo.Name = "GCS_REGVEN";
            SapperHill1943.Name = "GCS_Sapper_Hill_1943";
            SapperHill1943.GeographicInfo.Name = "GCS_Sapper_Hill_1943";
            SIRGAS.Name = "GCS_SIRGAS";
            SIRGAS.GeographicInfo.Name = "GCS_SIRGAS";
            SouthAmericanDatum1969.Name = "GCS_South_American_1969";
            SouthAmericanDatum1969.GeographicInfo.Name = "GCS_South_American_1969";
            Trinidad1903.Name = "GCS_Trinidad_1903";
            Trinidad1903.GeographicInfo.Name = "GCS_Trinidad_1903";
            Yacare.Name = "GCS_Yacare";
            Yacare.GeographicInfo.Name = "GCS_Yacare";
            Zanderij.Name = "GCS_Zanderij";
            Zanderij.GeographicInfo.Name = "GCS_Zanderij";

            Aratu.GeographicInfo.Datum.Name = "D_Aratu";
            Bogota.GeographicInfo.Datum.Name = "D_Bogota";
            BogotaBogota.GeographicInfo.Datum.Name = "D_Bogota";
            CampoInchauspe.GeographicInfo.Datum.Name = "D_Campo_Inchauspe";
            ChosMalal1914.GeographicInfo.Datum.Name = "D_Chos_Malal_1914";
            Chua.GeographicInfo.Datum.Name = "D_Chua";
            CorregoAlegre.GeographicInfo.Datum.Name = "D_Corrego_Alegre";
            GuyaneFrancaise.GeographicInfo.Datum.Name = "D_Guyane_Francaise";
            HitoXVIII1963.GeographicInfo.Datum.Name = "D_Hito_XVIII_1963";
            LaCanoa.GeographicInfo.Datum.Name = "D_La_Canoa";
            Lake.GeographicInfo.Datum.Name = "D_Lake";
            LomaQuintana.GeographicInfo.Datum.Name = "D_Loma_Quintana";
            MountDillon.GeographicInfo.Datum.Name = "D_Mount_Dillon";
            Naparima1955.GeographicInfo.Datum.Name = "D_Naparima_1955";
            Naparima1972.GeographicInfo.Datum.Name = "D_Naparima_1972";
            PampadelCastillo.GeographicInfo.Datum.Name = "D_Pampa_del_Castillo";
            POSGAR.GeographicInfo.Datum.Name = "D_POSGAR";
            POSGAR1998.GeographicInfo.Datum.Name = "D_POSGAR_1998";
            ProvisionalSouthAmer.GeographicInfo.Datum.Name = "D_Provisional_S_American_1956";
            REGVEN.GeographicInfo.Datum.Name = "D_REGVEN";
            SapperHill1943.GeographicInfo.Datum.Name = "D_Sapper_Hill_1943";
            SIRGAS.GeographicInfo.Datum.Name = "D_SIRGAS";
            SouthAmericanDatum1969.GeographicInfo.Datum.Name = "D_South_American_1969";
            Trinidad1903.GeographicInfo.Datum.Name = "D_Trinidad_1903";
            Yacare.GeographicInfo.Datum.Name = "D_Yacare";
            Zanderij.GeographicInfo.Datum.Name = "D_Zanderij";
        }

        #endregion
    }
}

#pragma warning restore 1591