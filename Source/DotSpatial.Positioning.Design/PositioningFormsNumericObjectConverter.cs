// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/23/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

using System;

namespace DotSpatial.Positioning.Design
{
    /// <summary>
    /// The PositioningFormsNumericObjectConverter handles the object conversion but is tailored to work with the DotSpatial.Positioning.Forms class.
    /// </summary>
    public abstract class PositioningFormsNumericObjectConverter : PositioningNumericObjectConverter
    {
        /// <inheritdoc />
        protected override Version HandledAssemblyVersion
        {
            get
            {
                return new Version("1.0.0.*");
            }
        }

        /// <inheritdoc />
        protected override string HandledAssemblyName
        {
            get
            {
                return "DotSpatial.Positioning.Forms, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3a45fedac1c4cdab";
            }
        }
    }
}