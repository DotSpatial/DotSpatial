// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/25/2010 3:28:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Kyle Ellison 11/02/2010 Draw map background using map control BackColor
//
// ********************************************************************************************************

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// The Undefined Projection Action enumeration
    /// </summary>
    public enum UndefinedProjectionAction
    {
        /// <summary>
        /// No action should be taken.
        /// </summary>
        Nothing,

        /// <summary>
        /// Always assume an undefined projection is Latitude Longitude
        /// </summary>
        WGS84,

        /// <summary>
        /// Always rely on the existing Map projection
        /// </summary>
        Map,

        /// <summary>
        /// Use a projection that was specified from the list
        /// </summary>
        Chosen
    }
}