<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdvancedOptions_v3
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdvancedOptions_v3))
        Me.grpbxFields = New System.Windows.Forms.GroupBox
        Me.chkbxCalcMergeShed = New System.Windows.Forms.CheckBox
        Me.chkbxWshedFields = New System.Windows.Forms.CheckBox
        Me.chkbxStreamFields = New System.Windows.Forms.CheckBox
        Me.gbxOutput = New System.Windows.Forms.GroupBox
        Me.chkbxMergedShed = New System.Windows.Forms.CheckBox
        Me.chkbxAreaDinf = New System.Windows.Forms.CheckBox
        Me.chkbxDinf = New System.Windows.Forms.CheckBox
        Me.chkbxShedGrid = New System.Windows.Forms.CheckBox
        Me.chkbxShedToShape = New System.Windows.Forms.CheckBox
        Me.chkBxStreamShape = New System.Windows.Forms.CheckBox
        Me.chkbxStreamOrder = New System.Windows.Forms.CheckBox
        Me.chkbxSelectAll = New System.Windows.Forms.CheckBox
        Me.chkbxFullNet = New System.Windows.Forms.CheckBox
        Me.chkbxGridNet = New System.Windows.Forms.CheckBox
        Me.chkbxD8Area = New System.Windows.Forms.CheckBox
        Me.chkbxD8 = New System.Windows.Forms.CheckBox
        Me.chkbxFillPits = New System.Windows.Forms.CheckBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.grpbxAdditional = New System.Windows.Forms.GroupBox
        Me.chkbxCheckEdge = New System.Windows.Forms.CheckBox
        Me.chkbxUseDinf = New System.Windows.Forms.CheckBox
        Me.grpbxOutputDir = New System.Windows.Forms.GroupBox
        Me.txtbxOut = New System.Windows.Forms.TextBox
        Me.btnBrowseOut = New System.Windows.Forms.Button
        Me.grpbxFields.SuspendLayout()
        Me.gbxOutput.SuspendLayout()
        Me.grpbxAdditional.SuspendLayout()
        Me.grpbxOutputDir.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpbxFields
        '
        Me.grpbxFields.Controls.Add(Me.chkbxCalcMergeShed)
        Me.grpbxFields.Controls.Add(Me.chkbxWshedFields)
        Me.grpbxFields.Controls.Add(Me.chkbxStreamFields)
        Me.grpbxFields.Location = New System.Drawing.Point(3, 425)
        Me.grpbxFields.Name = "grpbxFields"
        Me.grpbxFields.Size = New System.Drawing.Size(343, 110)
        Me.grpbxFields.TabIndex = 19
        Me.grpbxFields.TabStop = False
        Me.grpbxFields.Text = "Additional Calculated Fields"
        '
        'chkbxCalcMergeShed
        '
        Me.chkbxCalcMergeShed.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkbxCalcMergeShed.Checked = True
        Me.chkbxCalcMergeShed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxCalcMergeShed.Location = New System.Drawing.Point(109, 64)
        Me.chkbxCalcMergeShed.Name = "chkbxCalcMergeShed"
        Me.chkbxCalcMergeShed.Size = New System.Drawing.Size(177, 40)
        Me.chkbxCalcMergeShed.TabIndex = 2
        Me.chkbxCalcMergeShed.Text = "Calculate Additional Outlet Merged Watershed Fields"
        '
        'chkbxWshedFields
        '
        Me.chkbxWshedFields.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxWshedFields.Checked = True
        Me.chkbxWshedFields.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxWshedFields.Location = New System.Drawing.Point(194, 20)
        Me.chkbxWshedFields.Name = "chkbxWshedFields"
        Me.chkbxWshedFields.Size = New System.Drawing.Size(128, 40)
        Me.chkbxWshedFields.TabIndex = 1
        Me.chkbxWshedFields.Text = "Calculate Additional Watershed Fields"
        '
        'chkbxStreamFields
        '
        Me.chkbxStreamFields.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkbxStreamFields.Checked = True
        Me.chkbxStreamFields.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxStreamFields.Location = New System.Drawing.Point(9, 20)
        Me.chkbxStreamFields.Name = "chkbxStreamFields"
        Me.chkbxStreamFields.Size = New System.Drawing.Size(128, 40)
        Me.chkbxStreamFields.TabIndex = 0
        Me.chkbxStreamFields.Text = "Calculate Additional Stream Fields"
        '
        'gbxOutput
        '
        Me.gbxOutput.Controls.Add(Me.chkbxMergedShed)
        Me.gbxOutput.Controls.Add(Me.chkbxAreaDinf)
        Me.gbxOutput.Controls.Add(Me.chkbxDinf)
        Me.gbxOutput.Controls.Add(Me.chkbxShedGrid)
        Me.gbxOutput.Controls.Add(Me.chkbxShedToShape)
        Me.gbxOutput.Controls.Add(Me.chkBxStreamShape)
        Me.gbxOutput.Controls.Add(Me.chkbxStreamOrder)
        Me.gbxOutput.Controls.Add(Me.chkbxSelectAll)
        Me.gbxOutput.Controls.Add(Me.chkbxFullNet)
        Me.gbxOutput.Controls.Add(Me.chkbxGridNet)
        Me.gbxOutput.Controls.Add(Me.chkbxD8Area)
        Me.gbxOutput.Controls.Add(Me.chkbxD8)
        Me.gbxOutput.Controls.Add(Me.chkbxFillPits)
        Me.gbxOutput.Location = New System.Drawing.Point(3, 71)
        Me.gbxOutput.Name = "gbxOutput"
        Me.gbxOutput.Size = New System.Drawing.Size(343, 248)
        Me.gbxOutput.TabIndex = 18
        Me.gbxOutput.TabStop = False
        Me.gbxOutput.Text = "Available Intermediate Output Layers to add to Map"
        '
        'chkbxMergedShed
        '
        Me.chkbxMergedShed.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkbxMergedShed.Checked = True
        Me.chkbxMergedShed.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxMergedShed.Location = New System.Drawing.Point(194, 204)
        Me.chkbxMergedShed.Name = "chkbxMergedShed"
        Me.chkbxMergedShed.Size = New System.Drawing.Size(140, 33)
        Me.chkbxMergedShed.TabIndex = 15
        Me.chkbxMergedShed.TabStop = False
        Me.chkbxMergedShed.Text = "Outlet Merged Watershed Shapefile"
        '
        'chkbxAreaDinf
        '
        Me.chkbxAreaDinf.Location = New System.Drawing.Point(9, 109)
        Me.chkbxAreaDinf.Name = "chkbxAreaDinf"
        Me.chkbxAreaDinf.Size = New System.Drawing.Size(144, 24)
        Me.chkbxAreaDinf.TabIndex = 14
        Me.chkbxAreaDinf.TabStop = False
        Me.chkbxAreaDinf.Text = "DInf Contributing Area"
        '
        'chkbxDinf
        '
        Me.chkbxDinf.Location = New System.Drawing.Point(194, 72)
        Me.chkbxDinf.Name = "chkbxDinf"
        Me.chkbxDinf.Size = New System.Drawing.Size(144, 33)
        Me.chkbxDinf.TabIndex = 13
        Me.chkbxDinf.TabStop = False
        Me.chkbxDinf.Text = "DInf Flow Directions"
        '
        'chkbxShedGrid
        '
        Me.chkbxShedGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxShedGrid.Location = New System.Drawing.Point(9, 175)
        Me.chkbxShedGrid.Name = "chkbxShedGrid"
        Me.chkbxShedGrid.Size = New System.Drawing.Size(152, 24)
        Me.chkbxShedGrid.TabIndex = 11
        Me.chkbxShedGrid.TabStop = False
        Me.chkbxShedGrid.Text = "Watershed Grid"
        '
        'chkbxShedToShape
        '
        Me.chkbxShedToShape.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkbxShedToShape.Checked = True
        Me.chkbxShedToShape.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxShedToShape.Location = New System.Drawing.Point(9, 208)
        Me.chkbxShedToShape.Name = "chkbxShedToShape"
        Me.chkbxShedToShape.Size = New System.Drawing.Size(144, 24)
        Me.chkbxShedToShape.TabIndex = 10
        Me.chkbxShedToShape.TabStop = False
        Me.chkbxShedToShape.Text = "Watershed Shapefile"
        '
        'chkBxStreamShape
        '
        Me.chkBxStreamShape.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkBxStreamShape.Checked = True
        Me.chkBxStreamShape.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkBxStreamShape.Location = New System.Drawing.Point(194, 171)
        Me.chkBxStreamShape.Name = "chkBxStreamShape"
        Me.chkBxStreamShape.Size = New System.Drawing.Size(128, 33)
        Me.chkBxStreamShape.TabIndex = 9
        Me.chkBxStreamShape.TabStop = False
        Me.chkBxStreamShape.Text = "Stream Shapefile "
        '
        'chkbxStreamOrder
        '
        Me.chkbxStreamOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxStreamOrder.Location = New System.Drawing.Point(194, 138)
        Me.chkbxStreamOrder.Name = "chkbxStreamOrder"
        Me.chkbxStreamOrder.Size = New System.Drawing.Size(128, 33)
        Me.chkbxStreamOrder.TabIndex = 8
        Me.chkbxStreamOrder.TabStop = False
        Me.chkbxStreamOrder.Text = "Stream Order Grid and Network"
        '
        'chkbxSelectAll
        '
        Me.chkbxSelectAll.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkbxSelectAll.Location = New System.Drawing.Point(109, 14)
        Me.chkbxSelectAll.Name = "chkbxSelectAll"
        Me.chkbxSelectAll.Size = New System.Drawing.Size(77, 32)
        Me.chkbxSelectAll.TabIndex = 12
        Me.chkbxSelectAll.TabStop = False
        Me.chkbxSelectAll.Text = "Select All"
        '
        'chkbxFullNet
        '
        Me.chkbxFullNet.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkbxFullNet.Location = New System.Drawing.Point(9, 142)
        Me.chkbxFullNet.Name = "chkbxFullNet"
        Me.chkbxFullNet.Size = New System.Drawing.Size(152, 24)
        Me.chkbxFullNet.TabIndex = 6
        Me.chkbxFullNet.TabStop = False
        Me.chkbxFullNet.Text = "Full River Network Raster"
        '
        'chkbxGridNet
        '
        Me.chkbxGridNet.Location = New System.Drawing.Point(194, 105)
        Me.chkbxGridNet.Name = "chkbxGridNet"
        Me.chkbxGridNet.Size = New System.Drawing.Size(128, 33)
        Me.chkbxGridNet.TabIndex = 5
        Me.chkbxGridNet.TabStop = False
        Me.chkbxGridNet.Text = "Strahler Order and Flow Path Lengths"
        '
        'chkbxD8Area
        '
        Me.chkbxD8Area.Location = New System.Drawing.Point(9, 76)
        Me.chkbxD8Area.Name = "chkbxD8Area"
        Me.chkbxD8Area.Size = New System.Drawing.Size(144, 24)
        Me.chkbxD8Area.TabIndex = 3
        Me.chkbxD8Area.TabStop = False
        Me.chkbxD8Area.Text = "D8 Contributing Area"
        '
        'chkbxD8
        '
        Me.chkbxD8.Location = New System.Drawing.Point(194, 39)
        Me.chkbxD8.Name = "chkbxD8"
        Me.chkbxD8.Size = New System.Drawing.Size(120, 33)
        Me.chkbxD8.TabIndex = 1
        Me.chkbxD8.TabStop = False
        Me.chkbxD8.Text = "D8 Flow Directions"
        '
        'chkbxFillPits
        '
        Me.chkbxFillPits.Location = New System.Drawing.Point(9, 43)
        Me.chkbxFillPits.Name = "chkbxFillPits"
        Me.chkbxFillPits.Size = New System.Drawing.Size(104, 24)
        Me.chkbxFillPits.TabIndex = 0
        Me.chkbxFillPits.TabStop = False
        Me.chkbxFillPits.Text = "Pit Filled"
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnClose.Location = New System.Drawing.Point(271, 541)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 20
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'grpbxAdditional
        '
        Me.grpbxAdditional.Controls.Add(Me.chkbxCheckEdge)
        Me.grpbxAdditional.Controls.Add(Me.chkbxUseDinf)
        Me.grpbxAdditional.Location = New System.Drawing.Point(3, 325)
        Me.grpbxAdditional.Name = "grpbxAdditional"
        Me.grpbxAdditional.Size = New System.Drawing.Size(343, 95)
        Me.grpbxAdditional.TabIndex = 20
        Me.grpbxAdditional.TabStop = False
        Me.grpbxAdditional.Text = "Delineation Options"
        '
        'chkbxCheckEdge
        '
        Me.chkbxCheckEdge.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkbxCheckEdge.Checked = True
        Me.chkbxCheckEdge.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxCheckEdge.Location = New System.Drawing.Point(9, 50)
        Me.chkbxCheckEdge.Name = "chkbxCheckEdge"
        Me.chkbxCheckEdge.Size = New System.Drawing.Size(296, 39)
        Me.chkbxCheckEdge.TabIndex = 1
        Me.chkbxCheckEdge.Text = "Check for Edge Contamination (Removes sub-basins with in-flow from edge cells)"
        '
        'chkbxUseDinf
        '
        Me.chkbxUseDinf.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkbxUseDinf.Location = New System.Drawing.Point(8, 19)
        Me.chkbxUseDinf.Name = "chkbxUseDinf"
        Me.chkbxUseDinf.Size = New System.Drawing.Size(326, 25)
        Me.chkbxUseDinf.TabIndex = 0
        Me.chkbxUseDinf.Text = "Use D-infinity For more accurate delineation"
        '
        'grpbxOutputDir
        '
        Me.grpbxOutputDir.Controls.Add(Me.txtbxOut)
        Me.grpbxOutputDir.Controls.Add(Me.btnBrowseOut)
        Me.grpbxOutputDir.Location = New System.Drawing.Point(3, 8)
        Me.grpbxOutputDir.Name = "grpbxOutputDir"
        Me.grpbxOutputDir.Size = New System.Drawing.Size(343, 62)
        Me.grpbxOutputDir.TabIndex = 21
        Me.grpbxOutputDir.TabStop = False
        Me.grpbxOutputDir.Text = "Relative Output Directory"
        '
        'txtbxOut
        '
        Me.txtbxOut.Location = New System.Drawing.Point(8, 22)
        Me.txtbxOut.Name = "txtbxOut"
        Me.txtbxOut.Size = New System.Drawing.Size(296, 20)
        Me.txtbxOut.TabIndex = 4
        '
        'btnBrowseOut
        '
        Me.btnBrowseOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseOut.Image = CType(resources.GetObject("btnBrowseOut.Image"), System.Drawing.Image)
        Me.btnBrowseOut.Location = New System.Drawing.Point(310, 19)
        Me.btnBrowseOut.Name = "btnBrowseOut"
        Me.btnBrowseOut.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseOut.TabIndex = 3
        '
        'frmAdvancedOptions_v3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 568)
        Me.Controls.Add(Me.grpbxOutputDir)
        Me.Controls.Add(Me.grpbxAdditional)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.grpbxFields)
        Me.Controls.Add(Me.gbxOutput)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "frmAdvancedOptions_v3"
        Me.ShowInTaskbar = False
        Me.Text = "Advanced Options"
        Me.TopMost = True
        Me.grpbxFields.ResumeLayout(False)
        Me.gbxOutput.ResumeLayout(False)
        Me.grpbxAdditional.ResumeLayout(False)
        Me.grpbxOutputDir.ResumeLayout(False)
        Me.grpbxOutputDir.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpbxFields As System.Windows.Forms.GroupBox
    Friend WithEvents chkbxWshedFields As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxStreamFields As System.Windows.Forms.CheckBox
    Friend WithEvents gbxOutput As System.Windows.Forms.GroupBox
    Friend WithEvents chkbxShedGrid As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxShedToShape As System.Windows.Forms.CheckBox
    Friend WithEvents chkBxStreamShape As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxStreamOrder As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxSelectAll As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxFullNet As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxGridNet As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxD8Area As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxD8 As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxFillPits As System.Windows.Forms.CheckBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents grpbxAdditional As System.Windows.Forms.GroupBox
    Friend WithEvents chkbxUseDinf As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxCheckEdge As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxAreaDinf As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxDinf As System.Windows.Forms.CheckBox
    Friend WithEvents grpbxOutputDir As System.Windows.Forms.GroupBox
    Friend WithEvents txtbxOut As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseOut As System.Windows.Forms.Button
    Friend WithEvents chkbxMergedShed As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxCalcMergeShed As System.Windows.Forms.CheckBox
End Class
