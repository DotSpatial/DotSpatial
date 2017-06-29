// *******************************************************************************************************
// Product:  DotSpatial.Symbology.Forms.IIWin32WindowOwner
// Description:  Contains IWin32Window Owner
// Copyright & License: See www.DotSpatial.org.
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// Max Miroshnikov    |  3/2013            |  Initial commit
// *******************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Contains IWin32Window Owner
    /// </summary>
    public interface IIWin32WindowOwner
    {
        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        IWin32Window Owner { get; set; }  
    }
}