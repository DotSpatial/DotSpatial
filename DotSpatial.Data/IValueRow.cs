// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// A public interface definition for a single row of values that should be supported
    /// by any of the generic data row types.
    /// </summary>
    public interface IValueRow
    {
        /// <summary>
        /// Gets or sets the value in the position of column.
        /// </summary>
        /// <param name="cell">The 0 based integer column index to access on this row.</param>
        /// <returns>An object reference to the actual data value, which can be many types.</returns>
        double this[int cell]
        {
            get;

            set;
        }
    }
}