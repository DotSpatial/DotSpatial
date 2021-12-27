// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PropertyGridEditor.
    /// </summary>
    public class PolygonSchemePropertyGridEditor : UITypeEditor
    {
        #region Fields

        private IPolygonScheme _editCopy;
        private IPolygonScheme _original;

        #endregion

        #region Methods

        /// <summary>
        /// This should launch an open file dialog instead of the usual thing.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext.</param>
        /// <param name="provider">IServiceProvider.</param>
        /// <param name="value">The object being displayed.</param>
        /// <returns>A new version of the object if the dialog was ok.</returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            _original = value as IPolygonScheme;
            _editCopy = _original.Copy();

            IWindowsFormsEditorService dialogProvider = (IWindowsFormsEditorService)provider?.GetService(typeof(IWindowsFormsEditorService));
            NamedList<IPolygonCategory> cats = new NamedList<IPolygonCategory>(_editCopy.Categories, "Category");
            CollectionPropertyGrid frm = new CollectionPropertyGrid(cats);
            frm.ChangesApplied += FrmChangesApplied;
            frm.AddItemClicked += FrmAddItemClicked;
            dialogProvider?.ShowDialog(frm);
            return _original; // don't bother swapping out the edit copy, just store copies of the categories when changes are applied.
        }

        /// <summary>
        /// Either allows the editor to work or else nips it in the butt.
        /// </summary>
        /// <param name="context">ITypeDescriptorContext.</param>
        /// <returns>UITypeEditorEditStyle.</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        private void FrmAddItemClicked(object sender, EventArgs e)
        {
            _editCopy.Categories.Add(new PolygonCategory());
        }

        private void FrmChangesApplied(object sender, EventArgs e)
        {
            // the scheme is a reference copy to the original layer. This way,
            // applying the scheme here should also update the map.
            _original.CopyProperties(_editCopy);
        }

        #endregion
    }
}