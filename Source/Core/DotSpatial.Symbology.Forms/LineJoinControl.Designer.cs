using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class LineJoinControl
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LineJoinControl));
            this.grpLineJoins = new GroupBox();
            this.radBevel = new RadioButton();
            this.radRound = new RadioButton();
            this.radMiter = new RadioButton();
            this.grpLineJoins.SuspendLayout();
            this.SuspendLayout();
            //
            // grpLineJoins
            //
            this.grpLineJoins.AccessibleDescription = null;
            this.grpLineJoins.AccessibleName = null;
            resources.ApplyResources(this.grpLineJoins, "grpLineJoins");
            this.grpLineJoins.BackgroundImage = null;
            this.grpLineJoins.Controls.Add(this.radBevel);
            this.grpLineJoins.Controls.Add(this.radRound);
            this.grpLineJoins.Controls.Add(this.radMiter);
            this.grpLineJoins.Font = null;
            this.grpLineJoins.Name = "grpLineJoins";
            this.grpLineJoins.TabStop = false;
            this.grpLineJoins.Enter += this.GrpLineJoinsEnter;
            //
            // radBevel
            //
            this.radBevel.AccessibleDescription = null;
            this.radBevel.AccessibleName = null;
            resources.ApplyResources(this.radBevel, "radBevel");
            this.radBevel.BackgroundImage = null;
            this.radBevel.Font = null;
            this.radBevel.Name = "radBevel";
            this.radBevel.UseVisualStyleBackColor = true;
            this.radBevel.CheckedChanged += this.RadBevelCheckedChanged;
            //
            // radRound
            //
            this.radRound.AccessibleDescription = null;
            this.radRound.AccessibleName = null;
            resources.ApplyResources(this.radRound, "radRound");
            this.radRound.BackgroundImage = null;
            this.radRound.Checked = true;
            this.radRound.Font = null;
            this.radRound.Name = "radRound";
            this.radRound.TabStop = true;
            this.radRound.UseVisualStyleBackColor = true;
            this.radRound.CheckedChanged += this.RadRoundCheckedChanged;
            //
            // radMiter
            //
            this.radMiter.AccessibleDescription = null;
            this.radMiter.AccessibleName = null;
            resources.ApplyResources(this.radMiter, "radMiter");
            this.radMiter.BackgroundImage = null;
            this.radMiter.Font = null;
            this.radMiter.Name = "radMiter";
            this.radMiter.UseVisualStyleBackColor = true;
            this.radMiter.CheckedChanged += this.RadMiterCheckedChanged;
            //
            // LineJoinControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackgroundImage = null;
            this.Controls.Add(this.grpLineJoins);
            this.Font = null;
            this.Name = "LineJoinControl";
            this.grpLineJoins.ResumeLayout(false);
            this.grpLineJoins.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private GroupBox grpLineJoins;
        private RadioButton radBevel;
        private RadioButton radMiter;
        private RadioButton radRound;
    }
}