// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 2:17:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************
#pragma warning disable 1591

namespace DotSpatial.Data
{
    /// <summary>
    /// ERDAS Projection Codes
    /// </summary>
    public enum HfaEPRJ
    {
        LATLONG = 0,
        UTM = 1,
        STATE_PLANE = 2,
        ALBERS_CONIC_EQUAL_AREA = 3,
        LAMBERT_CONFORMAL_CONIC = 4,
        MERCATOR = 5,
        POLAR_STEROGRAPHIC = 6,
        POLYCONIC = 7,
        EQUIDISTANT_CONIC = 8,
        TRANSVERSE_MERCATOR = 9,
        STEROGRAPHIC = 10,
        LAMBERT_AZIMUTHAL_EQUAL_AREA = 11,
        AZIMUTHAL_EQUIDISTANT = 12,
        GNOMIONIC = 13,
        ORTHOGRAPHIC = 14,
        GENERAL_VERTICAL_NEAR_SIDE_PERSPECTIVE = 15,
        SINUSOIDAL = 16,
        EQUIRECTANGULAR = 17,
        MILLER_CYLINDRICAL = 18,
        VANDERGRINTEN = 19,
        HOTINE_OBLIQUE_MERCATOR = 20,
        SPACE_OBLIQUE_MERCATOR = 21,
        MODIFIED_TRANSVERSE_MERCATOR = 22,
        EOSAT_SOM = 23,
        ROBINSON = 24,
        SOM_A_AND_B = 25,
        ALASKA_CONFORMAL = 26,
        INTERRUPTED_GOODE_HOMOLOSINE = 27,
        MOLLWEIDE = 28,
        INTERRUPTED_MOLLWEIDE = 29,
        HAMMER = 30,
        WAGNER_IV = 31,
        WAGNER_VII = 32,
        OBLATED_EQUAL_AREA = 33,
        PLATE_CARRE = 34,
        EQUIDISTANT_CYLINDRICAL = 35,
        GAUSS_KRUGER = 36,
        ECKERT_VI = 37,
        ECKERT_V = 38,
        ECKERT_IV = 39,
        ECKERT_III = 40,
        ECKERT_II = 41,
        ECKERT_I = 42,
        GALL_STEREOGRAPHIC = 43,
        BEHRMANN = 44,
        WINKEL_I = 45,
        WINKEL_II = 46,
        QUARTIC_AUTHALIC = 47,
        LOXIMUTHAL = 48,
        BONNE = 49,
        STEREOGRAPHIC_EXTENDED = 50,
        CASSINI = 51,
        TWO_POINT_EQUIDISTANT = 52,
    }
}