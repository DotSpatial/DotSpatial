<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLoadOutputs
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLoadOutputs))
        Me.btnBrowseFill = New System.Windows.Forms.Button
        Me.lblFill = New System.Windows.Forms.Label
        Me.lblD8 = New System.Windows.Forms.Label
        Me.btnBrowseD8 = New System.Windows.Forms.Button
        Me.lblD8Slope = New System.Windows.Forms.Label
        Me.btnBrowseD8Slope = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtbxFill = New System.Windows.Forms.TextBox
        Me.txtbxD8Slope = New System.Windows.Forms.TextBox
        Me.txtbxD8 = New System.Windows.Forms.TextBox
        Me.fdgOpen = New System.Windows.Forms.OpenFileDialog
        Me.txtbxDinf = New System.Windows.Forms.TextBox
        Me.txtbxDinfSlope = New System.Windows.Forms.TextBox
        Me.lblDinfSlope = New System.Windows.Forms.Label
        Me.btnBrowseDinfSlope = New System.Windows.Forms.Button
        Me.lblDinf = New System.Windows.Forms.Label
        Me.btnBrowseDinf = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtbxMasked = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowseMask = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnBrowseFill
        '
        Me.btnBrowseFill.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseFill.Image = CType(resources.GetObject("btnBrowseFill.Image"), System.Drawing.Image)
        Me.btnBrowseFill.Location = New System.Drawing.Point(360, 61)
        Me.btnBrowseFill.Name = "btnBrowseFill"
        Me.btnBrowseFill.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseFill.TabIndex = 2
        '
        'lblFill
        '
        Me.lblFill.AutoSize = True
        Me.lblFill.Location = New System.Drawing.Point(9, 47)
        Me.lblFill.Name = "lblFill"
        Me.lblFill.Size = New System.Drawing.Size(135, 13)
        Me.lblFill.TabIndex = 5
        Me.lblFill.Text = "Pit-Filled Elevation Grid (fel)"
        '
        'lblD8
        '
        Me.lblD8.AutoSize = True
        Me.lblD8.Location = New System.Drawing.Point(9, 127)
        Me.lblD8.Name = "lblD8"
        Me.lblD8.Size = New System.Drawing.Size(128, 13)
        Me.lblD8.TabIndex = 8
        Me.lblD8.Text = "D8 Flow Direction Grid (p)"
        '
        'btnBrowseD8
        '
        Me.btnBrowseD8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseD8.Image = CType(resources.GetObject("btnBrowseD8.Image"), System.Drawing.Image)
        Me.btnBrowseD8.Location = New System.Drawing.Point(360, 141)
        Me.btnBrowseD8.Name = "btnBrowseD8"
        Me.btnBrowseD8.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseD8.TabIndex = 6
        '
        'lblD8Slope
        '
        Me.lblD8Slope.AutoSize = True
        Me.lblD8Slope.Location = New System.Drawing.Point(9, 86)
        Me.lblD8Slope.Name = "lblD8Slope"
        Me.lblD8Slope.Size = New System.Drawing.Size(99, 13)
        Me.lblD8Slope.TabIndex = 11
        Me.lblD8Slope.Text = "D8 Slope Grid (sd8)"
        '
        'btnBrowseD8Slope
        '
        Me.btnBrowseD8Slope.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseD8Slope.Image = CType(resources.GetObject("btnBrowseD8Slope.Image"), System.Drawing.Image)
        Me.btnBrowseD8Slope.Location = New System.Drawing.Point(360, 102)
        Me.btnBrowseD8Slope.Name = "btnBrowseD8Slope"
        Me.btnBrowseD8Slope.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseD8Slope.TabIndex = 4
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(309, 257)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 32
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtbxFill
        '
        Me.txtbxFill.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxFill.Location = New System.Drawing.Point(12, 63)
        Me.txtbxFill.Name = "txtbxFill"
        Me.txtbxFill.Size = New System.Drawing.Size(343, 20)
        Me.txtbxFill.TabIndex = 51
        '
        'txtbxD8Slope
        '
        Me.txtbxD8Slope.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxD8Slope.Location = New System.Drawing.Point(12, 104)
        Me.txtbxD8Slope.Name = "txtbxD8Slope"
        Me.txtbxD8Slope.Size = New System.Drawing.Size(343, 20)
        Me.txtbxD8Slope.TabIndex = 52
        '
        'txtbxD8
        '
        Me.txtbxD8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxD8.Location = New System.Drawing.Point(12, 144)
        Me.txtbxD8.Name = "txtbxD8"
        Me.txtbxD8.Size = New System.Drawing.Size(343, 20)
        Me.txtbxD8.TabIndex = 53
        '
        'fdgOpen
        '
        Me.fdgOpen.FileName = "OpenFileDialog1"
        '
        'txtbxDinf
        '
        Me.txtbxDinf.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxDinf.Location = New System.Drawing.Point(12, 225)
        Me.txtbxDinf.Name = "txtbxDinf"
        Me.txtbxDinf.Size = New System.Drawing.Size(343, 20)
        Me.txtbxDinf.TabIndex = 59
        '
        'txtbxDinfSlope
        '
        Me.txtbxDinfSlope.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxDinfSlope.Location = New System.Drawing.Point(12, 185)
        Me.txtbxDinfSlope.Name = "txtbxDinfSlope"
        Me.txtbxDinfSlope.Size = New System.Drawing.Size(343, 20)
        Me.txtbxDinfSlope.TabIndex = 58
        '
        'lblDinfSlope
        '
        Me.lblDinfSlope.AutoSize = True
        Me.lblDinfSlope.Location = New System.Drawing.Point(9, 167)
        Me.lblDinfSlope.Name = "lblDinfSlope"
        Me.lblDinfSlope.Size = New System.Drawing.Size(101, 13)
        Me.lblDinfSlope.TabIndex = 57
        Me.lblDinfSlope.Text = "DInf Slope Grid (slp)"
        '
        'btnBrowseDinfSlope
        '
        Me.btnBrowseDinfSlope.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDinfSlope.Image = CType(resources.GetObject("btnBrowseDinfSlope.Image"), System.Drawing.Image)
        Me.btnBrowseDinfSlope.Location = New System.Drawing.Point(360, 183)
        Me.btnBrowseDinfSlope.Name = "btnBrowseDinfSlope"
        Me.btnBrowseDinfSlope.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseDinfSlope.TabIndex = 54
        '
        'lblDinf
        '
        Me.lblDinf.AutoSize = True
        Me.lblDinf.Location = New System.Drawing.Point(9, 208)
        Me.lblDinf.Name = "lblDinf"
        Me.lblDinf.Size = New System.Drawing.Size(146, 13)
        Me.lblDinf.TabIndex = 56
        Me.lblDinf.Text = "DInf Flow Direction Grid (ang)"
        '
        'btnBrowseDinf
        '
        Me.btnBrowseDinf.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDinf.Image = CType(resources.GetObject("btnBrowseDinf.Image"), System.Drawing.Image)
        Me.btnBrowseDinf.Location = New System.Drawing.Point(360, 222)
        Me.btnBrowseDinf.Name = "btnBrowseDinf"
        Me.btnBrowseDinf.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseDinf.TabIndex = 55
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(228, 257)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 60
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtbxMasked
        '
        Me.txtbxMasked.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxMasked.Location = New System.Drawing.Point(12, 24)
        Me.txtbxMasked.Name = "txtbxMasked"
        Me.txtbxMasked.Size = New System.Drawing.Size(343, 20)
        Me.txtbxMasked.TabIndex = 63
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Masked DEM file"
        '
        'btnBrowseMask
        '
        Me.btnBrowseMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseMask.Image = CType(resources.GetObject("btnBrowseMask.Image"), System.Drawing.Image)
        Me.btnBrowseMask.Location = New System.Drawing.Point(360, 22)
        Me.btnBrowseMask.Name = "btnBrowseMask"
        Me.btnBrowseMask.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseMask.TabIndex = 61
        '
        'frmLoadOutputs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(389, 292)
        Me.Controls.Add(Me.txtbxMasked)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnBrowseMask)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtbxDinf)
        Me.Controls.Add(Me.txtbxDinfSlope)
        Me.Controls.Add(Me.lblDinfSlope)
        Me.Controls.Add(Me.btnBrowseDinfSlope)
        Me.Controls.Add(Me.lblDinf)
        Me.Controls.Add(Me.btnBrowseDinf)
        Me.Controls.Add(Me.txtbxD8)
        Me.Controls.Add(Me.txtbxD8Slope)
        Me.Controls.Add(Me.txtbxFill)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblD8Slope)
        Me.Controls.Add(Me.btnBrowseD8Slope)
        Me.Controls.Add(Me.lblD8)
        Me.Controls.Add(Me.btnBrowseD8)
        Me.Controls.Add(Me.lblFill)
        Me.Controls.Add(Me.btnBrowseFill)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLoadOutputs"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Load Pre-existing Preprocessing Files"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowseFill As System.Windows.Forms.Button
    Friend WithEvents lblFill As System.Windows.Forms.Label
    Friend WithEvents lblD8 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseD8 As System.Windows.Forms.Button
    Friend WithEvents lblD8Slope As System.Windows.Forms.Label
    Friend WithEvents btnBrowseD8Slope As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtbxFill As System.Windows.Forms.TextBox
    Friend WithEvents txtbxD8Slope As System.Windows.Forms.TextBox
    Friend WithEvents txtbxD8 As System.Windows.Forms.TextBox
    Friend WithEvents fdgOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtbxDinf As System.Windows.Forms.TextBox
    Friend WithEvents txtbxDinfSlope As System.Windows.Forms.TextBox
    Friend WithEvents lblDinfSlope As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDinfSlope As System.Windows.Forms.Button
    Friend WithEvents lblDinf As System.Windows.Forms.Label
    Friend WithEvents btnBrowseDinf As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtbxMasked As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseMask As System.Windows.Forms.Button
End Class
