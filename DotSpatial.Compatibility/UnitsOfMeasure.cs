// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:37:13 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// UnitOfMeasure
    /// </summary>
    public enum UnitsOfMeasure
    {
        /// <summary>The units are in decimal degrees.</summary>
        DecimalDegrees,
        /// <summary>The units are in millimeters.</summary>
        Millimeters,
        /// <summary>The units are in centimeters.</summary>
        Centimeters,
        /// <summary>The units are in inches.</summary>
        Inches,
        /// <summary>The units are in feet.</summary>
        Feet,
        /// <summary>The units are in Yards.</summary>
        Yards,
        /// <summary>The units are in meters.</summary>
        Meters,
        /// <summary>The units are in miles.</summary>
        Miles,
        /// <summary>The units are in kilometers.</summary>
        Kilometers,
        /// <summary>The units are in nautical miles.</summary>
        NauticalMiles,
        /// <summary>The units are in acres.</summary>
        Acres,
        /// <summary>The units are in hectares.</summary>
        Hectares,
        /// <summary>The units are unknown.</summary>
        Unknown
    }
}