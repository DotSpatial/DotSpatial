// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 11:32:00 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ExpressionEditor
    /// </summary>
    public class ExpressionEditor : UITypeEditor
    {
        #region Fields

        private ITypeDescriptorContext _context;

        #endregion

        #region Methods

        /// <summary>
        /// This describes how to launch the form etc.
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <param name="provider">The service provider.</param>
        /// <param name="value">The expression.</param>
        /// <returns>The resulting expression.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _context = context;

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider?.GetService(typeof(IWindowsFormsEditorService));
            SqlExpressionDialog dlgExpression = new SqlExpressionDialog();
            string original = (string)value;
            dlgExpression.Expression = (string)value;

            // Try to find the Table
            IFeatureCategory category = context?.Instance as IFeatureCategory;
            if (category != null)
            {
                IFeatureScheme scheme = category.GetParentItem() as IFeatureScheme;
                if (scheme != null)
                {
                    IFeatureLayer layer = scheme.GetParentItem() as IFeatureLayer;
                    if (layer != null)
                    {
                        dlgExpression.Table = layer.DataSet.DataTable;
                    }
                }
                else
                {
                    IFeatureLayer layer = category.GetParentItem() as IFeatureLayer;
                    if (layer != null)
                    {
                        dlgExpression.Table = layer.DataSet.DataTable;
                    }
                }
            }

            dlgExpression.ChangesApplied += DlgExpressionChangesApplied;
            var result = dialogProvider?.ShowDialog(dlgExpression);
            dlgExpression.ChangesApplied -= DlgExpressionChangesApplied;
            return result != DialogResult.OK ? original : dlgExpression.Expression;
        }

        /// <summary>
        /// This tells the editor that it should open a dialog form when editing the value from a ... button
        /// </summary>
        /// <param name="context">The type descriptor context.</param>
        /// <returns>The UITypeEditorEditStyle</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        private void DlgExpressionChangesApplied(object sender, EventArgs e)
        {
            SqlExpressionDialog dlg = sender as SqlExpressionDialog;
            if (dlg != null)
            {
                string exp = dlg.Expression;
                _context.PropertyDescriptor?.SetValue(_context.Instance, exp);
            }
        }

        #endregion
    }
}