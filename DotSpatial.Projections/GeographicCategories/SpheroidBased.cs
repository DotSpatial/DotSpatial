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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 2:20:30 PM
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
    /// SpheroidBased
    /// </summary>
    public class SpheroidBased : CoordinateSystemCategory
    {
        #region Fields

        public readonly ProjectionInfo Airy1830;
        public readonly ProjectionInfo Airymodified;
        public readonly ProjectionInfo AustralianNational;
        public readonly ProjectionInfo Authalicsphere;
        public readonly ProjectionInfo AuthalicsphereARCINFO;
        public readonly ProjectionInfo AverageTerrestrialSystem1977;
        public readonly ProjectionInfo Bessel1841;
        public readonly ProjectionInfo BesselNamibia;
        public readonly ProjectionInfo Besselmodified;
        public readonly ProjectionInfo Clarke1858;
        public readonly ProjectionInfo Clarke1866;
        public readonly ProjectionInfo Clarke1866Michigan;
        public readonly ProjectionInfo Clarke1880;
        public readonly ProjectionInfo Clarke1880Arc;
        public readonly ProjectionInfo Clarke1880Benoit;
        public readonly ProjectionInfo Clarke1880IGN;
        public readonly ProjectionInfo Clarke1880RGS;
        public readonly ProjectionInfo Clarke1880SGA;
        public readonly ProjectionInfo Everest1830;
        public readonly ProjectionInfo Everestdefinition1967;
        public readonly ProjectionInfo Everestdefinition1975;
        public readonly ProjectionInfo Everestmodified;
        public readonly ProjectionInfo Everestmodified1969;
        public readonly ProjectionInfo Fischer1960;
        public readonly ProjectionInfo Fischer1968;
        public readonly ProjectionInfo Fischermodified;
        public readonly ProjectionInfo GRS1967;
        public readonly ProjectionInfo GRS1980;
        public readonly ProjectionInfo Helmert1906;
        public readonly ProjectionInfo Hough1960;
        public readonly ProjectionInfo IndonesianNational;
        public readonly ProjectionInfo International1924;
        public readonly ProjectionInfo International1967;
        public readonly ProjectionInfo Krasovsky1940;
        public readonly ProjectionInfo OSU1986geoidalmodel;
        public readonly ProjectionInfo OSU1991geoidalmodel;
        public readonly ProjectionInfo Plessis1817;
        public readonly ProjectionInfo SphereEMEP;
        public readonly ProjectionInfo Struve1860;
        public readonly ProjectionInfo Transitpreciseephemeris;
        public readonly ProjectionInfo WGS1966;
        public readonly ProjectionInfo Walbeck;
        public readonly ProjectionInfo WarOffice;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SpheroidBased
        /// </summary>
        public SpheroidBased()
        {
            Airy1830 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=airy +no_defs ");
            Airymodified = ProjectionInfo.FromProj4String("+proj=longlat +a=6377340.189 +b=6356034.447938534 +no_defs ");
            AustralianNational = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            Authalicsphere = ProjectionInfo.FromProj4String("+proj=longlat +a=6371000 +b=6371000 +no_defs ");
            AuthalicsphereARCINFO = ProjectionInfo.FromProj4String("+proj=longlat +a=6370997 +b=6370997 +no_defs ");
            AverageTerrestrialSystem1977 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378135 +b=6356750.304921594 +no_defs ");
            Bessel1841 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bessel +no_defs ");
            Besselmodified = ProjectionInfo.FromProj4String("+proj=longlat +a=6377492.018 +b=6356173.508712696 +no_defs ");
            BesselNamibia = ProjectionInfo.FromProj4String("+proj=longlat +ellps=bess_nam +no_defs ");
            Clarke1858 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378293.639 +b=6356617.98149216 +no_defs ");
            Clarke1866 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk66 +no_defs ");
            Clarke1866Michigan = ProjectionInfo.FromProj4String("+proj=longlat +a=6378450.047 +b=6356826.620025999 +no_defs ");
            Clarke1880 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.138 +b=6356514.959419348 +no_defs ");
            Clarke1880Arc = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.145 +b=6356514.966395495 +no_defs ");
            Clarke1880Benoit = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300.79 +b=6356566.430000036 +no_defs ");
            Clarke1880IGN = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.999904194 +no_defs ");
            Clarke1880RGS = ProjectionInfo.FromProj4String("+proj=longlat +ellps=clrk80 +no_defs ");
            Clarke1880SGA = ProjectionInfo.FromProj4String("+proj=longlat +a=6378249.2 +b=6356514.996941779 +no_defs ");
            Everestdefinition1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=evrstSS +no_defs ");
            Everestdefinition1975 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377301.243 +b=6356100.228368102 +no_defs ");
            Everest1830 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377276.345 +b=6356075.41314024 +no_defs ");
            Everestmodified = ProjectionInfo.FromProj4String("+proj=longlat +a=6377304.063 +b=6356103.041812424 +no_defs ");
            Everestmodified1969 = ProjectionInfo.FromProj4String("+proj=longlat +a=6377295.664 +b=6356094.667915204 +no_defs ");
            Fischer1960 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378166 +b=6356784.283607107 +no_defs ");
            Fischer1968 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378150 +b=6356768.337244385 +no_defs ");
            Fischermodified = ProjectionInfo.FromProj4String("+proj=longlat +ellps=fschr60m +no_defs ");
            GRS1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS67 +no_defs ");
            GRS1980 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=GRS80 +no_defs ");
            Helmert1906 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=helmert +no_defs ");
            Hough1960 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378270 +b=6356794.343434343 +no_defs ");
            IndonesianNational = ProjectionInfo.FromProj4String("+proj=longlat +a=6378160 +b=6356774.50408554 +no_defs ");
            International1924 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=intl +no_defs ");
            International1967 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=aust_SA +no_defs ");
            Krasovsky1940 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=krass +no_defs ");
            OSU1986geoidalmodel = ProjectionInfo.FromProj4String("+proj=longlat +a=6378136.2 +b=6356751.516671965 +no_defs ");
            OSU1991geoidalmodel = ProjectionInfo.FromProj4String("+proj=longlat +a=6378136.3 +b=6356751.616336684 +no_defs ");
            Plessis1817 = ProjectionInfo.FromProj4String("+proj=longlat +a=6376523 +b=6355862.933255573 +no_defs ");
            SphereEMEP = ProjectionInfo.FromProj4String("+proj=longlat +a=6370000 +b=6370000 +no_defs ");
            Struve1860 = ProjectionInfo.FromProj4String("+proj=longlat +a=6378297 +b=6356655.847080379 +no_defs ");
            Transitpreciseephemeris = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS66 +no_defs ");
            Walbeck = ProjectionInfo.FromProj4String("+proj=longlat +a=6376896 +b=6355834.846687364 +no_defs ");
            WarOffice = ProjectionInfo.FromProj4String("+proj=longlat +a=6378300.583 +b=6356752.270219594 +no_defs ");
            WGS1966 = ProjectionInfo.FromProj4String("+proj=longlat +ellps=WGS66 +no_defs ");

            Airy1830.GeographicInfo.Name = "GCS_Airy_1830";
            Airymodified.GeographicInfo.Name = "GCS_Airy_Modified";
            AustralianNational.GeographicInfo.Name = "GCS_Australian";
            Authalicsphere.GeographicInfo.Name = "GCS_Sphere";
            AuthalicsphereARCINFO.GeographicInfo.Name = "GCS_Sphere_ARC_INFO";
            AverageTerrestrialSystem1977.GeographicInfo.Name = "GCS_ATS_1977";
            Bessel1841.GeographicInfo.Name = "GCS_Bessel_1841";
            Besselmodified.GeographicInfo.Name = "GCS_Bessel_Modified";
            BesselNamibia.GeographicInfo.Name = "GCS_Bessel_Namibia";
            Clarke1858.GeographicInfo.Name = "GCS_Clarke_1858";
            Clarke1866.GeographicInfo.Name = "GCS_Clarke_1866";
            Clarke1866Michigan.GeographicInfo.Name = "GCS_Clarke_1866_Michigan";
            Clarke1880.GeographicInfo.Name = "GCS_Clarke_1880";
            Clarke1880Arc.GeographicInfo.Name = "GCS_Clarke_1880_Arc";
            Clarke1880Benoit.GeographicInfo.Name = "GCS_Clarke_1880_Benoit";
            Clarke1880IGN.GeographicInfo.Name = "GCS_Clarke_1880_IGN";
            Clarke1880RGS.GeographicInfo.Name = "GCS_Clarke_1880_RGS";
            Clarke1880SGA.GeographicInfo.Name = "GCS_Clarke_1880_SGA";
            Everestdefinition1967.GeographicInfo.Name = "GCS_Everest_def_1967";
            Everestdefinition1975.GeographicInfo.Name = "GCS_Everest_def_1975";
            Everest1830.GeographicInfo.Name = "GCS_Everest_1830";
            Everestmodified.GeographicInfo.Name = "GCS_Everest_Modified";
            Everestmodified1969.GeographicInfo.Name = "GCS_Everest_Modified_1969";
            Fischer1960.GeographicInfo.Name = "GCS_Fischer_1960";
            Fischer1968.GeographicInfo.Name = "GCS_Fischer_1968";
            Fischermodified.GeographicInfo.Name = "GCS_Fischer_Modified";
            GRS1967.GeographicInfo.Name = "GCS_GRS_1967";
            GRS1980.GeographicInfo.Name = "GCS_GRS_1980";
            Helmert1906.GeographicInfo.Name = "GCS_Helmert_1906";
            Hough1960.GeographicInfo.Name = "GCS_Hough_1960";
            IndonesianNational.GeographicInfo.Name = "GCS_Indonesian";
            International1924.GeographicInfo.Name = "GCS_International_1924";
            International1967.GeographicInfo.Name = "GCS_International_1967";
            Krasovsky1940.GeographicInfo.Name = "GCS_Krasovsky_1940";
            OSU1986geoidalmodel.GeographicInfo.Name = "GCS_OSU_86F";
            OSU1991geoidalmodel.GeographicInfo.Name = "GCS_OSU_91A";
            Plessis1817.GeographicInfo.Name = "GCS_Plessis_1817";
            SphereEMEP.GeographicInfo.Name = "GCS_Sphere_EMEP";
            Struve1860.GeographicInfo.Name = "GCS_Struve_1860";
            Transitpreciseephemeris.GeographicInfo.Name = "GCS_NWL_9D";
            Walbeck.GeographicInfo.Name = "GCS_Walbeck";
            WarOffice.GeographicInfo.Name = "GCS_War_Office";
            WGS1966.GeographicInfo.Name = "GCS_WGS_1966";

            Airy1830.GeographicInfo.Datum.Name = "D_Airy_1830";
            Airymodified.GeographicInfo.Datum.Name = "D_Airy_Modified";
            AustralianNational.GeographicInfo.Datum.Name = "D_Australian";
            Authalicsphere.GeographicInfo.Datum.Name = "D_Sphere";
            AuthalicsphereARCINFO.GeographicInfo.Datum.Name = "D_Sphere_ARC_INFO";
            AverageTerrestrialSystem1977.GeographicInfo.Datum.Name = "D_ATS_1977";
            Bessel1841.GeographicInfo.Datum.Name = "D_Bessel_1841";
            Besselmodified.GeographicInfo.Datum.Name = "D_Bessel_Modified";
            BesselNamibia.GeographicInfo.Datum.Name = "D_Bessel_Namibia";
            Clarke1858.GeographicInfo.Datum.Name = "D_Clarke_1858";
            Clarke1866.GeographicInfo.Datum.Name = "D_Clarke_1866";
            Clarke1866Michigan.GeographicInfo.Datum.Name = "D_Clarke_1866_Michigan";
            Clarke1880.GeographicInfo.Datum.Name = "D_Clarke_1880";
            Clarke1880Arc.GeographicInfo.Datum.Name = "D_Clarke_1880_Arc";
            Clarke1880Benoit.GeographicInfo.Datum.Name = "D_Clarke_1880_Benoit";
            Clarke1880IGN.GeographicInfo.Datum.Name = "D_Clarke_1880_IGN";
            Clarke1880RGS.GeographicInfo.Datum.Name = "D_Clarke_1880_RGS";
            Clarke1880SGA.GeographicInfo.Datum.Name = "D_Clarke_1880_SGA";
            Everestdefinition1967.GeographicInfo.Datum.Name = "D_Everest_Def_1967";
            Everestdefinition1975.GeographicInfo.Datum.Name = "D_Everest_Def_1975";
            Everest1830.GeographicInfo.Datum.Name = "D_Everest_1830";
            Everestmodified.GeographicInfo.Datum.Name = "D_Everest_Modified";
            Everestmodified1969.GeographicInfo.Datum.Name = "D_Everest_Modified_1969";
            Fischer1960.GeographicInfo.Datum.Name = "D_Fischer_1960";
            Fischer1968.GeographicInfo.Datum.Name = "D_Fischer_1968";
            Fischermodified.GeographicInfo.Datum.Name = "D_Fischer_Modified";
            GRS1967.GeographicInfo.Datum.Name = "D_GRS_1967";
            GRS1980.GeographicInfo.Datum.Name = "D_GRS_1980";
            Helmert1906.GeographicInfo.Datum.Name = "D_Helmert_1906";
            Hough1960.GeographicInfo.Datum.Name = "D_Hough_1960";
            IndonesianNational.GeographicInfo.Datum.Name = "D_Indonesian";
            International1924.GeographicInfo.Datum.Name = "D_International_1924";
            International1967.GeographicInfo.Datum.Name = "D_International_1967";
            Krasovsky1940.GeographicInfo.Datum.Name = "D_Krasovsky_1940";
            OSU1986geoidalmodel.GeographicInfo.Datum.Name = "D_OSU_86F";
            OSU1991geoidalmodel.GeographicInfo.Datum.Name = "D_OSU_91A";
            Plessis1817.GeographicInfo.Datum.Name = "D_Plessis_1817";
            SphereEMEP.GeographicInfo.Datum.Name = "D_Sphere_EMEP";
            Struve1860.GeographicInfo.Datum.Name = "D_Struve_1860";
            Transitpreciseephemeris.GeographicInfo.Datum.Name = "D_NWL_9D";
            Walbeck.GeographicInfo.Datum.Name = "D_Walbeck";
            WarOffice.GeographicInfo.Datum.Name = "D_War_Office";
            WGS1966.GeographicInfo.Datum.Name = "D_WGS_1966";
        }

        #endregion
    }
}

#pragma warning restore 1591