// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/2/2009 3:14:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// DropDownDialog
    /// </summary>
    public class ListBoxDialog : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private ListBox lstItems;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListBoxDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lstItems = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            //
            // btnCancel
            //
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // btnOk
            //
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            //
            // lstItems
            //
            resources.ApplyResources(this.lstItems, "lstItems");
            this.lstItems.FormattingEnabled = true;
            this.lstItems.Items.AddRange(new object[] {
                                                          resources.GetString("lstItems.Items")});
            this.lstItems.Name = "lstItems";
            //
            // ListBoxDialog
            //
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lstItems);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListBoxDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DropDownDialog
        /// </summary>
        public ListBoxDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the items from the dialog box
        /// </summary>
        public void Clear()
        {
            lstItems.Items.Clear();
            lstItems.Items.Add("(None)");
        }

        /// <summary>
        /// Adds the array of objects to the dialog box
        /// </summary>
        /// <param name="items">The items to add</param>
        public void Add(object[] items)
        {
            lstItems.Items.AddRange(items);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the currently selected item in this list dialog box
        /// </summary>
        public object SelectedItem
        {
            get
            {
                return lstItems.SelectedItem;
            }
        }

        #endregion

        #region Protected Methods

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide();
        //}

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}