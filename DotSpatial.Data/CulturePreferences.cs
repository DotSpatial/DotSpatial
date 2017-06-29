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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2008 6:38:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// CulturePreferences
    /// </summary>
    public static class CulturePreferences
    {
        /// <summary>
        /// This culture information is useful for things like Number Formatting.
        /// This defaults to CurrentCulture, but can be specified through preferences or
        /// whatever.
        /// </summary>
        public static CultureInfo CultureInformation = CultureInfo.CurrentCulture;
    }
}