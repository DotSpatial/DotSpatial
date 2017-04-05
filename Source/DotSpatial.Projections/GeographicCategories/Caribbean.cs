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

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Caribbean.
    /// </summary>
    public class Caribbean : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Anguilla1957;
        public readonly ProjectionInfo Antigua1943;
        public readonly ProjectionInfo Barbados1938;
        public readonly ProjectionInfo Dominica1945;
        public readonly ProjectionInfo FortDesaix;
        public readonly ProjectionInfo FortMarigot;
        public readonly ProjectionInfo FortThomas1955;
        public readonly ProjectionInfo GrandCayman1961;
        public readonly ProjectionInfo Grenada1953;
        public readonly ProjectionInfo JAD2001;
        public readonly ProjectionInfo Jamaica1875;
        public readonly ProjectionInfo Jamaica1969;
        public readonly ProjectionInfo LC5Astro1961;
        public readonly ProjectionInfo LittleCayman1961;
        public readonly ProjectionInfo Montserrat1958;
        public readonly ProjectionInfo PuertoRico;
        public readonly ProjectionInfo RRAF1991;
        public readonly ProjectionInfo SainteAnne;
        public readonly ProjectionInfo StKitts1955;
        public readonly ProjectionInfo StLucia1955;
        public readonly ProjectionInfo StVincent1945;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Caribbean.
        /// </summary>
        public Caribbean()
        {
            Anguilla1957 = ProjectionInfo.FromEpsgCode(4600).SetNames("", "GCS_Anguilla_1957", "D_Anguilla_1957");
            Antigua1943 = ProjectionInfo.FromEpsgCode(4601).SetNames("", "GCS_Antigua_1943", "D_Antigua_1943");
            Barbados1938 = ProjectionInfo.FromEpsgCode(4212).SetNames("", "GCS_Barbados_1938", "D_Barbados_1938");
            Dominica1945 = ProjectionInfo.FromEpsgCode(4602).SetNames("", "GCS_Dominica_1945", "D_Dominica_1945");
            FortDesaix = ProjectionInfo.FromEpsgCode(4625).SetNames("", "GCS_Fort_Desaix", "D_Fort_Desaix");
            FortMarigot = ProjectionInfo.FromEpsgCode(4621).SetNames("", "GCS_Fort_Marigot", "D_Fort_Marigot");
            FortThomas1955 = ProjectionInfo.FromAuthorityCode("ESRI", 37240).SetNames("", "GCS_Fort_Thomas_1955", "D_Fort_Thomas_1955");
            GrandCayman1961 = ProjectionInfo.FromEpsgCode(4723).SetNames("", "GCS_Grand_Cayman_1959", "D_Grand_Cayman_1959");
            Grenada1953 = ProjectionInfo.FromEpsgCode(4603).SetNames("", "GCS_Grenada_1953", "D_Grenada_1953");
            JAD2001 = ProjectionInfo.FromEpsgCode(4758).SetNames("", "GCS_JAD_2001", "D_Jamaica_2001");
            Jamaica1875 = ProjectionInfo.FromEpsgCode(4241).SetNames("", "GCS_Jamaica_1875", "D_Jamaica_1875");
            Jamaica1969 = ProjectionInfo.FromEpsgCode(4242).SetNames("", "GCS_Jamaica_1969", "D_Jamaica_1969");
            LC5Astro1961 = ProjectionInfo.FromAuthorityCode("ESRI", 37243).SetNames("", "GCS_LC5_1961", "D_LC5_1961");
            LittleCayman1961 = ProjectionInfo.FromEpsgCode(4726).SetNames("", "GCS_Little_Cayman_1961", "D_Little_Cayman_1961");
            Montserrat1958 = ProjectionInfo.FromEpsgCode(4604).SetNames("", "GCS_Montserrat_1958", "D_Montserrat_1958");
            PuertoRico = ProjectionInfo.FromEpsgCode(4139).SetNames("", "GCS_Puerto_Rico", "D_Puerto_Rico");
            RRAF1991 = ProjectionInfo.FromEpsgCode(4640).SetNames("", "GCS_RRAF_1991_incorrect_spheroid", "D_RRAF_1991_incorrect_spheroid");
            SainteAnne = ProjectionInfo.FromEpsgCode(4622).SetNames("", "GCS_Sainte_Anne", "D_Sainte_Anne");
            StKitts1955 = ProjectionInfo.FromEpsgCode(4605).SetNames("", "GCS_St_Kitts_1955", "D_St_Kitts_1955");
            StLucia1955 = ProjectionInfo.FromEpsgCode(4606).SetNames("", "GCS_St_Lucia_1955", "D_St_Lucia_1955");
            StVincent1945 = ProjectionInfo.FromEpsgCode(4607).SetNames("", "GCS_St_Vincent_1945", "D_St_Vincent_1945");
        }

        #endregion
    }
}

#pragma warning restore 1591