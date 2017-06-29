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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/15/2009 1:14:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections
{
    /// <summary>
    /// Proj4Ellipsoids
    /// </summary>
    public enum Proj4Ellipsoid
    {
        /// <summary>
        /// Custom will use the a parameter for the major axis and the
        /// rf parameter for the flattening divisor
        /// </summary>
        Custom,
        /// <summary>
        /// MERIT 1983
        /// </summary>
        Merit_1983,
        /// <summary>
        /// Soviet Geodetic System 85
        /// </summary>
        SovietGeodeticSystem_1985,
        /// <summary>
        /// Geodetic Reference System 1980(IUGG, 1980)
        /// </summary>
        GRS_1980,
        /// <summary>
        /// International Astronomical Union 1976
        /// </summary>
        IAU_1976,
        /// <summary>
        /// Sir George Biddell Airy 1830 (Britain)
        /// </summary>
        Airy_1830,
        /// <summary>
        /// App. Physics. 1965
        /// </summary>
        AppPhysics_1965,
        /// <summary>
        /// Naval Weapons Lab., 1965
        /// </summary>
        NavalWeaponsLab_1965,
        /// <summary>
        /// Modified Airy
        /// </summary>
        AiryModified,
        /// <summary>
        /// Andrae 1876 (Den., Iclnd.)
        /// </summary>
        Andrae_1876,
        /// <summary>
        /// Austrailian National and South American 1969
        /// </summary>
        Austrailia_SouthAmerica,
        /// <summary>
        /// Geodetic Reference System 67 (IUGG 1967)
        /// </summary>
        GRS_1967,
        /// <summary>
        /// Bessel 1841
        /// </summary>
        Bessel_1841,
        /// <summary>
        /// Bessel 1841 (Namibia)
        /// </summary>
        BesselNamibia,
        /// <summary>
        /// Clarke 1866
        /// </summary>
        Clarke_1866,
        /// <summary>
        /// Clarke 1880 Modified
        /// </summary>
        ClarkeModified_1880,
        /// <summary>
        /// Comm. des Poids et Mesures 1799
        /// </summary>
        CPM_1799,
        /// <summary>
        /// Delambre 1810 (Belgium)
        /// </summary>
        Delambre_1810,
        /// <summary>
        /// Engelis 1985
        /// </summary>
        Engelis_1985,
        /// <summary>
        /// Everest 1830
        /// </summary>
        Everest_1830,
        /// <summary>
        /// Everest 1948
        /// </summary>
        Everest_1948,
        /// <summary>
        /// Everest 1956
        /// </summary>
        Everest_1956,
        /// <summary>
        /// Everest 1969
        /// </summary>
        Everest_1969,
        /// <summary>
        /// Everest (Sabah and Sarawak)
        /// </summary>
        Everest_SS,
        /// <summary>
        /// Everest (Pakistan)
        /// </summary>
        Everest_Pakistan,
        /// <summary>
        /// Fischer (Mercury Datum) 1960
        /// </summary>
        Fischer_1960,
        /// <summary>
        /// Modified Fischer 1960
        /// </summary>
        FischerModified_1960,
        /// <summary>
        /// Fischer 1968
        /// </summary>
        Fischer_1968,
        /// <summary>
        /// Helmert 1906
        /// </summary>
        Helmert_1906,
        /// <summary>
        /// Hough
        /// </summary>
        Hough,
        /// <summary>
        /// Indonesian 1974
        /// </summary>
        Indonesian_1974,
        /// <summary>
        /// International 1909
        /// </summary>
        International_1909,
        /// <summary>
        /// Krassovsky 1942
        /// </summary>
        Krassovsky_1942,
        /// <summary>
        /// Kaula 1961
        /// </summary>
        Kaula_1961,
        /// <summary>
        /// Lerch 1979
        /// </summary>
        Lerch_1979,
        /// <summary>
        /// Maupertius 1738
        /// </summary>
        Maupertius_1738,
        /// <summary>
        /// New International 1967
        /// </summary>
        InternationalNew_1967,
        /// <summary>
        /// Plessis 1817 (France)
        /// </summary>
        Plessis_1817,
        /// <summary>
        /// Southeast Asia
        /// </summary>
        SoutheastAsia,
        /// <summary>
        /// Walbekc (Germany)
        /// </summary>
        Walbeck,
        /// <summary>
        /// World Geodetic System 1960
        /// </summary>
        WGS_1960,
        /// <summary>
        /// World Geodetic System 1966
        /// </summary>
        WGS_1966,
        /// <summary>
        /// World Geodetic System 1972
        /// </summary>
        WGS_1972,
        /// <summary>
        /// World Geodetic System 1984
        /// </summary>
        WGS_1984,
        /// <summary>
        /// Normal Sphere
        /// </summary>
        Sphere
    }
}