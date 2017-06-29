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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/13/2010 9:30:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Supports bounding in the M Dimension
    /// </summary>
    public interface IExtentM
    {
        /// <summary>
        /// Gets or sets the minimum M value
        /// </summary>
        double MinM { get; set; }

        /// <summary>
        /// Gets or sets the maximum M value
        /// </summary>
        double MaxM { get; set; }
    }
}