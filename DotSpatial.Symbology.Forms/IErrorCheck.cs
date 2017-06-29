// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from SketchPad.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/10/2008 11:13:03 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology.Forms
{
    public interface IErrorCheck
    {
        #region Properties

        /// <summary>
        /// Boolean, true if there is an error on this device.
        /// </summary>
        bool HasError
        {
            get;
        }

        /// <summary>
        /// Specifies the current error message.
        /// </summary>
        string ErrorMessage
        {
            get;
        }

        /// <summary>
        /// Gets the cleanly formatted name for this control for an error message
        /// </summary>
        string MessageName
        {
            get;
        }

        #endregion
    }
}