using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.WebMap.WMS
{
    partial class WmsServerParameters
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.lblQuerable = new System.Windows.Forms.Label();
            this.lblNoSubsets = new System.Windows.Forms.Label();
            this.lbCRS = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbStyles = new System.Windows.Forms.ListBox();
            this.lblStyles = new System.Windows.Forms.Label();
            this.lblOpaque = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tbAbstract = new System.Windows.Forms.TextBox();
            this.gbSelectedLayer = new System.Windows.Forms.GroupBox();
            this.tbCustomParameters = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFixedHeight = new System.Windows.Forms.Label();
            this.lblFixedWidth = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCascaded = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvLayers = new System.Windows.Forms.TreeView();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.btnGetCapabilities = new System.Windows.Forms.Button();
            this.tbServerUrl = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gbServerInfo = new System.Windows.Forms.GroupBox();
            this.tbServerAccessConstraints = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbServerOnlineResource = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbServerAbstract = new System.Windows.Forms.TextBox();
            this.tbServerTitle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbLogin = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbSelectedLayer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblQuerable
            // 
            this.lblQuerable.AutoSize = true;
            this.lblQuerable.Location = new System.Drawing.Point(9, 77);
            this.lblQuerable.Name = "lblQuerable";
            this.lblQuerable.Size = new System.Drawing.Size(70, 13);
            this.lblQuerable.TabIndex = 14;
            this.lblQuerable.Text = "Querable: No";
            // 
            // lblNoSubsets
            // 
            this.lblNoSubsets.AutoSize = true;
            this.lblNoSubsets.Location = new System.Drawing.Point(9, 101);
            this.lblNoSubsets.Name = "lblNoSubsets";
            this.lblNoSubsets.Size = new System.Drawing.Size(79, 13);
            this.lblNoSubsets.TabIndex = 18;
            this.lblNoSubsets.Text = "NoSubsets: No";
            // 
            // lbCRS
            // 
            this.lbCRS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCRS.FormattingEnabled = true;
            this.lbCRS.Location = new System.Drawing.Point(9, 19);
            this.lbCRS.Name = "lbCRS";
            this.lbCRS.Size = new System.Drawing.Size(209, 108);
            this.lbCRS.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "CRS:";
            // 
            // lbStyles
            // 
            this.lbStyles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStyles.FormattingEnabled = true;
            this.lbStyles.Location = new System.Drawing.Point(9, 24);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new System.Drawing.Size(209, 30);
            this.lbStyles.TabIndex = 12;
            // 
            // lblStyles
            // 
            this.lblStyles.AutoSize = true;
            this.lblStyles.Location = new System.Drawing.Point(3, 6);
            this.lblStyles.Name = "lblStyles";
            this.lblStyles.Size = new System.Drawing.Size(38, 13);
            this.lblStyles.TabIndex = 21;
            this.lblStyles.Text = "Styles:";
            // 
            // lblOpaque
            // 
            this.lblOpaque.AutoSize = true;
            this.lblOpaque.Location = new System.Drawing.Point(114, 77);
            this.lblOpaque.Name = "lblOpaque";
            this.lblOpaque.Size = new System.Drawing.Size(65, 13);
            this.lblOpaque.TabIndex = 17;
            this.lblOpaque.Text = "Opaque: No";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(231, 69);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbStyles);
            this.panel2.Controls.Add(this.lblStyles);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(231, 69);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbCRS);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 78);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(231, 135);
            this.panel3.TabIndex = 2;
            // 
            // tbAbstract
            // 
            this.tbAbstract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAbstract.Location = new System.Drawing.Point(252, 45);
            this.tbAbstract.Multiline = true;
            this.tbAbstract.Name = "tbAbstract";
            this.tbAbstract.ReadOnly = true;
            this.tbAbstract.Size = new System.Drawing.Size(145, 93);
            this.tbAbstract.TabIndex = 11;
            // 
            // gbSelectedLayer
            // 
            this.gbSelectedLayer.Controls.Add(this.tbCustomParameters);
            this.gbSelectedLayer.Controls.Add(this.label2);
            this.gbSelectedLayer.Controls.Add(this.lblFixedHeight);
            this.gbSelectedLayer.Controls.Add(this.lblFixedWidth);
            this.gbSelectedLayer.Controls.Add(this.label5);
            this.gbSelectedLayer.Controls.Add(this.lblCascaded);
            this.gbSelectedLayer.Controls.Add(this.tbName);
            this.gbSelectedLayer.Controls.Add(this.label4);
            this.gbSelectedLayer.Controls.Add(this.tbTitle);
            this.gbSelectedLayer.Controls.Add(this.label3);
            this.gbSelectedLayer.Controls.Add(this.lblQuerable);
            this.gbSelectedLayer.Controls.Add(this.tbAbstract);
            this.gbSelectedLayer.Controls.Add(this.lblNoSubsets);
            this.gbSelectedLayer.Controls.Add(this.lblOpaque);
            this.gbSelectedLayer.Controls.Add(this.tableLayoutPanel1);
            this.gbSelectedLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelectedLayer.Location = new System.Drawing.Point(248, 3);
            this.gbSelectedLayer.Name = "gbSelectedLayer";
            this.gbSelectedLayer.Size = new System.Drawing.Size(403, 367);
            this.gbSelectedLayer.TabIndex = 25;
            this.gbSelectedLayer.TabStop = false;
            this.gbSelectedLayer.Text = "Layer Details";
            // 
            // tbCustomParameters
            // 
            this.tbCustomParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCustomParameters.Location = new System.Drawing.Point(258, 171);
            this.tbCustomParameters.Multiline = true;
            this.tbCustomParameters.Name = "tbCustomParameters";
            this.tbCustomParameters.Size = new System.Drawing.Size(139, 186);
            this.tbCustomParameters.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 41;
            this.label2.Text = "Custom parameters:";
            // 
            // lblFixedHeight
            // 
            this.lblFixedHeight.AutoSize = true;
            this.lblFixedHeight.Location = new System.Drawing.Point(114, 125);
            this.lblFixedHeight.Name = "lblFixedHeight";
            this.lblFixedHeight.Size = new System.Drawing.Size(78, 13);
            this.lblFixedHeight.TabIndex = 40;
            this.lblFixedHeight.Text = "Fixed Height: 0";
            // 
            // lblFixedWidth
            // 
            this.lblFixedWidth.AutoSize = true;
            this.lblFixedWidth.Location = new System.Drawing.Point(9, 125);
            this.lblFixedWidth.Name = "lblFixedWidth";
            this.lblFixedWidth.Size = new System.Drawing.Size(75, 13);
            this.lblFixedWidth.TabIndex = 39;
            this.lblFixedWidth.Text = "Fixed Width: 0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(249, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "Abstract:";
            // 
            // lblCascaded
            // 
            this.lblCascaded.AutoSize = true;
            this.lblCascaded.Location = new System.Drawing.Point(114, 101);
            this.lblCascaded.Name = "lblCascaded";
            this.lblCascaded.Size = new System.Drawing.Size(67, 13);
            this.lblCascaded.TabIndex = 37;
            this.lblCascaded.Text = "Cascaded: 0";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(48, 45);
            this.tbName.Name = "tbName";
            this.tbName.ReadOnly = true;
            this.tbName.Size = new System.Drawing.Size(185, 20);
            this.tbName.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Name:";
            // 
            // tbTitle
            // 
            this.tbTitle.Location = new System.Drawing.Point(48, 19);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.ReadOnly = true;
            this.tbTitle.Size = new System.Drawing.Size(185, 20);
            this.tbTitle.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Title:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 144);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(237, 216);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // tvLayers
            // 
            this.tvLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLayers.HideSelection = false;
            this.tvLayers.Location = new System.Drawing.Point(3, 3);
            this.tvLayers.Name = "tvLayers";
            this.tvLayers.Size = new System.Drawing.Size(239, 367);
            this.tvLayers.TabIndex = 8;
            this.tvLayers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvLayersAfterSelect);
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.Location = new System.Drawing.Point(15, 29);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(41, 13);
            this.lblServerURL.TabIndex = 22;
            this.lblServerURL.Text = "Server:";
            // 
            // btnGetCapabilities
            // 
            this.btnGetCapabilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetCapabilities.Location = new System.Drawing.Point(564, 24);
            this.btnGetCapabilities.Name = "btnGetCapabilities";
            this.btnGetCapabilities.Size = new System.Drawing.Size(110, 23);
            this.btnGetCapabilities.TabIndex = 1;
            this.btnGetCapabilities.Text = "Get data";
            this.btnGetCapabilities.UseVisualStyleBackColor = true;
            this.btnGetCapabilities.Click += new System.EventHandler(this.BtnGetCapabilitiesClick);
            // 
            // tbServerUrl
            // 
            this.tbServerUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerUrl.Location = new System.Drawing.Point(58, 26);
            this.tbServerUrl.Name = "tbServerUrl";
            this.tbServerUrl.Size = new System.Drawing.Size(488, 20);
            this.tbServerUrl.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(599, 610);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 100;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(517, 610);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 99;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel2.Controls.Add(this.tvLayers, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbSelectedLayer, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 213);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(654, 373);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // gbServerInfo
            // 
            this.gbServerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbServerInfo.Controls.Add(this.tbServerAccessConstraints);
            this.gbServerInfo.Controls.Add(this.label10);
            this.gbServerInfo.Controls.Add(this.tbServerOnlineResource);
            this.gbServerInfo.Controls.Add(this.label9);
            this.gbServerInfo.Controls.Add(this.label8);
            this.gbServerInfo.Controls.Add(this.tbServerAbstract);
            this.gbServerInfo.Controls.Add(this.tbServerTitle);
            this.gbServerInfo.Controls.Add(this.label7);
            this.gbServerInfo.Location = new System.Drawing.Point(21, 78);
            this.gbServerInfo.Name = "gbServerInfo";
            this.gbServerInfo.Size = new System.Drawing.Size(654, 129);
            this.gbServerInfo.TabIndex = 5;
            this.gbServerInfo.TabStop = false;
            this.gbServerInfo.Text = "Server Details";
            // 
            // tbServerAccessConstraints
            // 
            this.tbServerAccessConstraints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerAccessConstraints.Location = new System.Drawing.Point(462, 73);
            this.tbServerAccessConstraints.Multiline = true;
            this.tbServerAccessConstraints.Name = "tbServerAccessConstraints";
            this.tbServerAccessConstraints.ReadOnly = true;
            this.tbServerAccessConstraints.Size = new System.Drawing.Size(186, 43);
            this.tbServerAccessConstraints.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(356, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 49;
            this.label10.Text = "Access Constraints:";
            // 
            // tbServerOnlineResource
            // 
            this.tbServerOnlineResource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerOnlineResource.Location = new System.Drawing.Point(107, 46);
            this.tbServerOnlineResource.Name = "tbServerOnlineResource";
            this.tbServerOnlineResource.ReadOnly = true;
            this.tbServerOnlineResource.Size = new System.Drawing.Size(541, 20);
            this.tbServerOnlineResource.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "Online Resource:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 46;
            this.label8.Text = "Abstract:";
            // 
            // tbServerAbstract
            // 
            this.tbServerAbstract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerAbstract.Location = new System.Drawing.Point(107, 73);
            this.tbServerAbstract.Multiline = true;
            this.tbServerAbstract.Name = "tbServerAbstract";
            this.tbServerAbstract.ReadOnly = true;
            this.tbServerAbstract.Size = new System.Drawing.Size(243, 46);
            this.tbServerAbstract.TabIndex = 6;
            // 
            // tbServerTitle
            // 
            this.tbServerTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerTitle.Location = new System.Drawing.Point(107, 19);
            this.tbServerTitle.Name = "tbServerTitle";
            this.tbServerTitle.ReadOnly = true;
            this.tbServerTitle.Size = new System.Drawing.Size(541, 20);
            this.tbServerTitle.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Title:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Login:";
            // 
            // tbLogin
            // 
            this.tbLogin.Location = new System.Drawing.Point(58, 52);
            this.tbLogin.Name = "tbLogin";
            this.tbLogin.Size = new System.Drawing.Size(194, 20);
            this.tbLogin.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(263, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Password:";
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(325, 52);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(221, 20);
            this.tbPassword.TabIndex = 3;
            // 
            // WMSServerParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 651);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbLogin);
            this.Controls.Add(this.gbServerInfo);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.lblServerURL);
            this.Controls.Add(this.btnGetCapabilities);
            this.Controls.Add(this.tbServerUrl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 650);
            this.Name = "WmsServerParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMS Server Parameters";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gbSelectedLayer.ResumeLayout(false);
            this.gbSelectedLayer.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gbServerInfo.ResumeLayout(false);
            this.gbServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblQuerable;
        private Label lblNoSubsets;
        private ListBox lbCRS;
        private Label label1;
        private ListBox lbStyles;
        private Label lblStyles;
        private Label lblOpaque;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private TextBox tbAbstract;
        private GroupBox gbSelectedLayer;
        private TableLayoutPanel tableLayoutPanel1;
        private TreeView tvLayers;
        private Label lblServerURL;
        private Button btnGetCapabilities;
        private TextBox tbServerUrl;
        private Button btnCancel;
        private Button btnOK;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox tbTitle;
        private Label label3;
        private TextBox tbName;
        private Label label4;
        private Label lblCascaded;
        private Label label5;
        private Label lblFixedHeight;
        private Label lblFixedWidth;
        private GroupBox gbServerInfo;
        private Label label8;
        private TextBox tbServerAbstract;
        private TextBox tbServerTitle;
        private Label label7;
        private TextBox tbServerOnlineResource;
        private Label label9;
        private TextBox tbServerAccessConstraints;
        private Label label10;
        private Label label2;
        private TextBox tbCustomParameters;
        private Label label6;
        private TextBox tbLogin;
        private Label label11;
        private TextBox tbPassword;
    }
}