// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
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