using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutPropertyGrid
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutPropertyGrid));
            this._propertyGrid = new PropertyGrid();
            this.SuspendLayout();

            // _propertyGrid
            resources.ApplyResources(this._propertyGrid, "_propertyGrid");
            this._propertyGrid.Name = "_propertyGrid";

            // LayoutPropertyGrid
            this.Controls.Add(this._propertyGrid);
            this.Name = "LayoutPropertyGrid";
            this.KeyUp += new KeyEventHandler(this.LayoutPropertyGridKeyUp);
            this.ResumeLayout(false);
        }

        #endregion

        private LayoutControl _layoutControl;
        private PropertyGrid _propertyGrid;
    }
}