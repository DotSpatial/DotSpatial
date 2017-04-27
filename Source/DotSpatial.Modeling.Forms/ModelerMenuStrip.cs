// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ModelerMenuStrip
// Description:  A menu strip designed to work along with the modeler
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Apr, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A Menu strip that is part of Brian's toolbox
    /// </summary>
    [ToolboxItem(false)]
    public partial class ModelerMenuStrip : MenuStrip
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelerMenuStrip"/> class.
        /// </summary>
        public ModelerMenuStrip()
        {
            InitializeComponent();
        }
    }
}