// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
        #region Private Variables

        ITypeDescriptorContext _context;

        #endregion

        #region Methods

        /// <summary>
        /// This describes how to launch the form etc.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _context = context;

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            SQLExpressionDialog dlgExpression = new SQLExpressionDialog();
            string original = (string)value;
            dlgExpression.Expression = (string)value;

            // Try to find the Table
            IFeatureCategory category = context.Instance as IFeatureCategory;
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
            var result = dialogProvider.ShowDialog(dlgExpression);
            dlgExpression.ChangesApplied -= DlgExpressionChangesApplied;
            return result != DialogResult.OK ? original : dlgExpression.Expression;
        }

        private void DlgExpressionChangesApplied(object sender, EventArgs e)
        {
            SQLExpressionDialog dlg = sender as SQLExpressionDialog;
            if (dlg != null)
            {
                string exp = dlg.Expression;
                _context.PropertyDescriptor.SetValue(_context.Instance, exp);
            }
        }

        /// <summary>
        /// This tells the editor that it should open a dialog form when editing the value from a ... button
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        #endregion

        #region Properties

        #endregion
    }
}