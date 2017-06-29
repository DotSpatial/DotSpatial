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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/24/2009 10:28:57 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// KnownTransforms
    /// </summary>
    public enum KnownTransform
    {
        /// <summary>
        /// Aitoff
        /// </summary>
        Aitoff,
        /// <summary>
        /// Albers Equal Area
        /// </summary>
        Albers_Equal_Area,
        /// <summary>
        /// Azimuthal Equidistant
        /// </summary>
        Azimuthal_Equidistant,
        /// <summary>
        /// Bipolar Oblique Conformal Conic
        /// </summary>
        Bipolar_Oblique_Conformal_Conic,
        /// <summary>
        /// Bonne
        /// </summary>
        Bonne,
        /// <summary>
        /// Cassini
        /// </summary>
        Cassini,
        /// <summary>
        /// Craster Parabolic
        /// </summary>
        Craster_Parabolic,
        /// <summary>
        /// Cylindrical Equal Area
        /// </summary>
        Cylindrical_Equal_Area,
        /// <summary>
        /// Eckert 1
        /// </summary>
        Eckert_I,
        /// <summary>
        /// Eckert 2
        /// </summary>
        Eckert_II,
        /// <summary>
        /// Eckert 3
        /// </summary>
        Eckert_III,
        /// <summary>
        /// Eckert 4
        /// </summary>
        Eckert_IV,
        /// <summary>
        /// Eckert 5
        /// </summary>
        Eckert_V,
        /// <summary>
        /// Eckert 6
        /// </summary>
        Eckert_VI,
        /// <summary>
        /// Elliptical Transform
        /// </summary>
        Elliptical_Transform,
        /// <summary>
        /// Equidistant Conic
        /// </summary>
        Equidistant_Conic,
        /// <summary>
        /// Equidistant_Cylindrical
        /// </summary>
        Equidistant_Cylindrical,
        /// <summary>
        /// Foucaut
        /// </summary>
        Foucaut,
        /// <summary>
        /// Gall Stereographic
        /// </summary>
        Gall_Stereographic,
        /// <summary>
        /// General Sinusoidal
        /// </summary>
        General_Sinusoidal,
        /// <summary>
        /// Geostationary Satellite
        /// </summary>
        Geostationary_Satellite,
        /// <summary>
        /// Gnomonic
        /// </summary>
        Gnomonic,
        /// <summary>
        /// Goode Homolosine
        /// </summary>
        Goode_Homolosine,
        /// <summary>
        /// Hammer Aitoff
        /// </summary>
        Hammer_Aitoff,
        /// <summary>
        /// Kavraisky 5
        /// </summary>
        Kavraisky_V,
        /// <summary>
        /// Kavraisky 7
        /// </summary>
        Kavraisky_VII,
        /// <summary>
        /// Krovak
        /// </summary>
        Krovak,
        /// <summary>
        /// Lambert Azimuthal Equal Area
        /// </summary>
        Lambert_Azimuthal_Equal_Area,
        /// <summary>
        /// Lambert Conformal Conic
        /// </summary>
        Lambert_Conformal_Conic,
        /// <summary>
        /// Lambert Equal Area Conic
        /// </summary>
        Lambert_Equal_Area_Conic,
        /// <summary>
        /// Latitude / Longitude
        /// </summary>
        LongLat,
        /// <summary>
        /// Loximuthal
        /// </summary>
        Loximuthal,
        /// <summary>
        /// McBryde Thomas Flat Polar Sine
        /// </summary>
        McBryde_Thomas_Flat_Polar_Sine,
        /// <summary>
        /// Mercator
        /// </summary>
        Mercator,
        /// <summary>
        /// Miller Cylindrical
        /// </summary>
        Miller_Cylindrical,
        /// <summary>
        /// Mollweide
        /// </summary>
        Mollweide,
        /// <summary>
        /// New Zealand Map Grid
        /// </summary>
        New_Zealand_Map_Grid,
        /// <summary>
        /// Oblique Cylindrical Equal Area
        /// </summary>
        Oblique_Cylindrical_Equal_Area,
        /// <summary>
        /// Oblique Mercator
        /// </summary>
        Oblique_Mercator,
        /// <summary>
        /// Oblique Stereographic Alternative
        /// </summary>
        Oblique_Stereographic_Alternative,
        /// <summary>
        /// Orthographic
        /// </summary>
        Orthographic,
        /// <summary>
        /// Polyconic
        /// </summary>
        Polyconic,
        /// <summary>
        /// PutinsP1
        /// </summary>
        Putins_P1,
        /// <summary>
        /// Quartic Authalic
        /// </summary>
        Quartic_Authalic,
        /// <summary>
        /// Robinson
        /// </summary>
        Robinson,
        /// <summary>
        /// Sinusoidal
        /// </summary>
        Sinusoidal,
        /// <summary>
        /// Stereographic
        /// </summary>
        Stereographic,
        /// <summary>
        /// Swiss Oblique Mercator
        /// </summary>
        Swiss_Oblique_Mercator,
        /// <summary>
        /// Transverse Mercator
        /// </summary>
        Transverse_Mercator,
        /// <summary>
        /// Two Point Equidistant
        /// </summary>
        Two_Point_Equidistant,
        /// <summary>
        /// Universal Polar Stereographic
        /// </summary>
        Universal_Polar_Stereographic,
        /// <summary>
        /// Universal Transverse Mercator
        /// </summary>
        Universal_Transverse_Mercator,
        /// <summary>
        /// Vander Grinten 1
        /// </summary>
        Vander_Grinten_I,
        /// <summary>
        /// Wagner 4
        /// </summary>
        Wagner_IV,
        /// <summary>
        /// Wagner 5
        /// </summary>
        Wagner_V,
        /// <summary>
        /// Wagner 6
        /// </summary>
        Wagner6,
        /// <summary>
        /// Winkel 1
        /// </summary>
        Winkel_I,
        /// <summary>
        /// Winkel 2
        /// </summary>
        Winkel_II,
        /// <summary>
        /// Winkel Tripel
        /// </summary>
        Winkel_Tripel
    }
}