using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace CSharpEditor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm 
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private IContainer components = null;
        
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        
        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            this.textEditorControl1 = new TextEditorControl();
            this.statusStrip1 = new StatusStrip();
            this.parserThreadLabel = new ToolStripStatusLabel();
            this.imageList1 = new ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Dock = DockStyle.Fill;
            this.textEditorControl1.IsReadOnly = false;
            this.textEditorControl1.Location = new Point(0, 0);
            this.textEditorControl1.Margin = new Padding(4, 4, 4, 4);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.ShowEOLMarkers = true;
            this.textEditorControl1.ShowSpaces = true;
            this.textEditorControl1.ShowTabs = true;
            this.textEditorControl1.Size = new Size(741, 307);
            this.textEditorControl1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new ToolStripItem[] {
            this.parserThreadLabel});
            this.statusStrip1.Location = new Point(0, 307);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new Size(741, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // parserThreadLabel
            // 
            this.parserThreadLabel.Name = "parserThreadLabel";
            this.parserThreadLabel.Size = new Size(151, 20);
            this.parserThreadLabel.Text = "toolStripStatusLabel1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Icons.16x16.Class.png");
            this.imageList1.Images.SetKeyName(1, "Icons.16x16.Method.png");
            this.imageList1.Images.SetKeyName(2, "Icons.16x16.Property.png");
            this.imageList1.Images.SetKeyName(3, "Icons.16x16.Field.png");
            this.imageList1.Images.SetKeyName(4, "Icons.16x16.Enum.png");
            this.imageList1.Images.SetKeyName(5, "Icons.16x16.NameSpace.png");
            this.imageList1.Images.SetKeyName(6, "Icons.16x16.Event.png");
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 16F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.textEditorControl1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Size = new Size(741, 332);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal ImageList imageList1;
        private ToolStripStatusLabel parserThreadLabel;
        private StatusStrip statusStrip1;
        private TextEditorControl textEditorControl1;
    }
}
