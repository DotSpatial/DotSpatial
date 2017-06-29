// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
        /// <inheritdocs/>
        protected override Version HandledAssemblyVersion
        {
            get
            {
                return new Version("1.0.0.*");
            }
        }

        /// <inheritdocs/>
        protected override string HandledAssemblyName
        {
            get
            {
                return "DotSpatial.Positioning.Forms, Culture=neutral, Version=" + HandledAssemblyVersion.ToString(4) + ", PublicKeyToken=3a45fedac1c4cdab";
            }
        }
    }
}