<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmLoadDelinOutputs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmLoadDelinOutputs))
        Me.btnBrowseAd8 = New System.Windows.Forms.Button
        Me.lblad8 = New System.Windows.Forms.Label
        Me.lblgord = New System.Windows.Forms.Label
        Me.btnBrowseGord = New System.Windows.Forms.Button
        Me.lblsca = New System.Windows.Forms.Label
        Me.btnBrowseSca = New System.Windows.Forms.Button
        Me.btnOK = New System.Windows.Forms.Button
        Me.txtbxAd8 = New System.Windows.Forms.TextBox
        Me.txtbxSca = New System.Windows.Forms.TextBox
        Me.txtbxGord = New System.Windows.Forms.TextBox
        Me.fdgOpen = New System.Windows.Forms.OpenFileDialog
        Me.txtbxTlen = New System.Windows.Forms.TextBox
        Me.txtbxPlen = New System.Windows.Forms.TextBox
        Me.lblplen = New System.Windows.Forms.Label
        Me.btnBrowsePlen = New System.Windows.Forms.Button
        Me.lbltlen = New System.Windows.Forms.Label
        Me.btnBrowseTlen = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtbxNet = New System.Windows.Forms.TextBox
        Me.txtbxTree = New System.Windows.Forms.TextBox
        Me.lbltree = New System.Windows.Forms.Label
        Me.btnBrowseTree = New System.Windows.Forms.Button
        Me.lblnet = New System.Windows.Forms.Label
        Me.btnBrowseNet = New System.Windows.Forms.Button
        Me.txtbxCoord = New System.Windows.Forms.TextBox
        Me.txtbxOrd = New System.Windows.Forms.TextBox
        Me.txtbxSrc = New System.Windows.Forms.TextBox
        Me.lblord = New System.Windows.Forms.Label
        Me.btnBrowseOrd = New System.Windows.Forms.Button
        Me.lblcoord = New System.Windows.Forms.Label
        Me.btnBrowseCoord = New System.Windows.Forms.Button
        Me.lblsrc = New System.Windows.Forms.Label
        Me.btnBrowseSrc = New System.Windows.Forms.Button
        Me.txtbxW = New System.Windows.Forms.TextBox
        Me.lblw = New System.Windows.Forms.Label
        Me.btnBrowseW = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnBrowseAd8
        '
        Me.btnBrowseAd8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseAd8.Image = CType(resources.GetObject("btnBrowseAd8.Image"), System.Drawing.Image)
        Me.btnBrowseAd8.Location = New System.Drawing.Point(370, 23)
        Me.btnBrowseAd8.Name = "btnBrowseAd8"
        Me.btnBrowseAd8.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseAd8.TabIndex = 2
        '
        'lblad8
        '
        Me.lblad8.AutoSize = True
        Me.lblad8.Location = New System.Drawing.Point(6, 9)
        Me.lblad8.Name = "lblad8"
        Me.lblad8.Size = New System.Drawing.Size(154, 13)
        Me.lblad8.TabIndex = 5
        Me.lblad8.Text = "D8 Contributing Area Grid (ad8)"
        '
        'lblgord
        '
        Me.lblgord.AutoSize = True
        Me.lblgord.Location = New System.Drawing.Point(6, 90)
        Me.lblgord.Name = "lblgord"
        Me.lblgord.Size = New System.Drawing.Size(167, 13)
        Me.lblgord.TabIndex = 8
        Me.lblgord.Text = "Strahler Network Order Grid (gord)"
        '
        'btnBrowseGord
        '
        Me.btnBrowseGord.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseGord.Image = CType(resources.GetObject("btnBrowseGord.Image"), System.Drawing.Image)
        Me.btnBrowseGord.Location = New System.Drawing.Point(370, 104)
        Me.btnBrowseGord.Name = "btnBrowseGord"
        Me.btnBrowseGord.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseGord.TabIndex = 6
        '
        'lblsca
        '
        Me.lblsca.AutoSize = True
        Me.lblsca.Location = New System.Drawing.Point(6, 48)
        Me.lblsca.Name = "lblsca"
        Me.lblsca.Size = New System.Drawing.Size(197, 13)
        Me.lblsca.TabIndex = 11
        Me.lblsca.Text = "Dinf Specific Catchment Area Grid  (sca)"
        '
        'btnBrowseSca
        '
        Me.btnBrowseSca.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseSca.Image = CType(resources.GetObject("btnBrowseSca.Image"), System.Drawing.Image)
        Me.btnBrowseSca.Location = New System.Drawing.Point(370, 64)
        Me.btnBrowseSca.Name = "btnBrowseSca"
        Me.btnBrowseSca.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseSca.TabIndex = 4
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Location = New System.Drawing.Point(319, 461)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 32
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtbxAd8
        '
        Me.txtbxAd8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxAd8.Location = New System.Drawing.Point(9, 25)
        Me.txtbxAd8.Name = "txtbxAd8"
        Me.txtbxAd8.Size = New System.Drawing.Size(356, 20)
        Me.txtbxAd8.TabIndex = 51
        '
        'txtbxSca
        '
        Me.txtbxSca.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxSca.Location = New System.Drawing.Point(9, 66)
        Me.txtbxSca.Name = "txtbxSca"
        Me.txtbxSca.Size = New System.Drawing.Size(356, 20)
        Me.txtbxSca.TabIndex = 52
        '
        'txtbxGord
        '
        Me.txtbxGord.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxGord.Location = New System.Drawing.Point(9, 107)
        Me.txtbxGord.Name = "txtbxGord"
        Me.txtbxGord.Size = New System.Drawing.Size(356, 20)
        Me.txtbxGord.TabIndex = 53
        '
        'fdgOpen
        '
        Me.fdgOpen.FileName = "OpenFileDialog1"
        '
        'txtbxTlen
        '
        Me.txtbxTlen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxTlen.Location = New System.Drawing.Point(9, 188)
        Me.txtbxTlen.Name = "txtbxTlen"
        Me.txtbxTlen.Size = New System.Drawing.Size(356, 20)
        Me.txtbxTlen.TabIndex = 59
        '
        'txtbxPlen
        '
        Me.txtbxPlen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxPlen.Location = New System.Drawing.Point(9, 147)
        Me.txtbxPlen.Name = "txtbxPlen"
        Me.txtbxPlen.Size = New System.Drawing.Size(356, 20)
        Me.txtbxPlen.TabIndex = 58
        '
        'lblplen
        '
        Me.lblplen.AutoSize = True
        Me.lblplen.Location = New System.Drawing.Point(6, 129)
        Me.lblplen.Name = "lblplen"
        Me.lblplen.Size = New System.Drawing.Size(170, 13)
        Me.lblplen.TabIndex = 57
        Me.lblplen.Text = "Longest Upslope length Grid (plen)"
        '
        'btnBrowsePlen
        '
        Me.btnBrowsePlen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowsePlen.Image = CType(resources.GetObject("btnBrowsePlen.Image"), System.Drawing.Image)
        Me.btnBrowsePlen.Location = New System.Drawing.Point(370, 145)
        Me.btnBrowsePlen.Name = "btnBrowsePlen"
        Me.btnBrowsePlen.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowsePlen.TabIndex = 54
        '
        'lbltlen
        '
        Me.lbltlen.AutoSize = True
        Me.lbltlen.Location = New System.Drawing.Point(6, 171)
        Me.lbltlen.Name = "lbltlen"
        Me.lbltlen.Size = New System.Drawing.Size(157, 13)
        Me.lbltlen.TabIndex = 56
        Me.lbltlen.Text = "Total Upslope Length Grid (tlen)"
        '
        'btnBrowseTlen
        '
        Me.btnBrowseTlen.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseTlen.Image = CType(resources.GetObject("btnBrowseTlen.Image"), System.Drawing.Image)
        Me.btnBrowseTlen.Location = New System.Drawing.Point(370, 185)
        Me.btnBrowseTlen.Name = "btnBrowseTlen"
        Me.btnBrowseTlen.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseTlen.TabIndex = 55
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(238, 461)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 60
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtbxNet
        '
        Me.txtbxNet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxNet.Location = New System.Drawing.Point(9, 433)
        Me.txtbxNet.Name = "txtbxNet"
        Me.txtbxNet.Size = New System.Drawing.Size(356, 20)
        Me.txtbxNet.TabIndex = 75
        '
        'txtbxTree
        '
        Me.txtbxTree.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxTree.Location = New System.Drawing.Point(9, 351)
        Me.txtbxTree.Name = "txtbxTree"
        Me.txtbxTree.Size = New System.Drawing.Size(356, 20)
        Me.txtbxTree.TabIndex = 74
        '
        'lbltree
        '
        Me.lbltree.AutoSize = True
        Me.lbltree.Location = New System.Drawing.Point(6, 333)
        Me.lbltree.Name = "lbltree"
        Me.lbltree.Size = New System.Drawing.Size(141, 13)
        Me.lbltree.TabIndex = 73
        Me.lbltree.Text = "Network Tree (nnnntree.dat)"
        '
        'btnBrowseTree
        '
        Me.btnBrowseTree.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseTree.Image = CType(resources.GetObject("btnBrowseTree.Image"), System.Drawing.Image)
        Me.btnBrowseTree.Location = New System.Drawing.Point(370, 349)
        Me.btnBrowseTree.Name = "btnBrowseTree"
        Me.btnBrowseTree.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseTree.TabIndex = 70
        '
        'lblnet
        '
        Me.lblnet.AutoSize = True
        Me.lblnet.Location = New System.Drawing.Point(6, 416)
        Me.lblnet.Name = "lblnet"
        Me.lblnet.Size = New System.Drawing.Size(146, 13)
        Me.lblnet.TabIndex = 72
        Me.lblnet.Text = "Stream Reach Shapefile (net)"
        '
        'btnBrowseNet
        '
        Me.btnBrowseNet.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseNet.Image = CType(resources.GetObject("btnBrowseNet.Image"), System.Drawing.Image)
        Me.btnBrowseNet.Location = New System.Drawing.Point(370, 430)
        Me.btnBrowseNet.Name = "btnBrowseNet"
        Me.btnBrowseNet.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseNet.TabIndex = 71
        '
        'txtbxCoord
        '
        Me.txtbxCoord.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxCoord.Location = New System.Drawing.Point(9, 311)
        Me.txtbxCoord.Name = "txtbxCoord"
        Me.txtbxCoord.Size = New System.Drawing.Size(356, 20)
        Me.txtbxCoord.TabIndex = 69
        '
        'txtbxOrd
        '
        Me.txtbxOrd.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxOrd.Location = New System.Drawing.Point(9, 270)
        Me.txtbxOrd.Name = "txtbxOrd"
        Me.txtbxOrd.Size = New System.Drawing.Size(356, 20)
        Me.txtbxOrd.TabIndex = 68
        '
        'txtbxSrc
        '
        Me.txtbxSrc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxSrc.Location = New System.Drawing.Point(9, 229)
        Me.txtbxSrc.Name = "txtbxSrc"
        Me.txtbxSrc.Size = New System.Drawing.Size(356, 20)
        Me.txtbxSrc.TabIndex = 67
        '
        'lblord
        '
        Me.lblord.AutoSize = True
        Me.lblord.Location = New System.Drawing.Point(6, 252)
        Me.lblord.Name = "lblord"
        Me.lblord.Size = New System.Drawing.Size(115, 13)
        Me.lblord.TabIndex = 66
        Me.lblord.Text = "Stream Order Grid (ord)"
        '
        'btnBrowseOrd
        '
        Me.btnBrowseOrd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseOrd.Image = CType(resources.GetObject("btnBrowseOrd.Image"), System.Drawing.Image)
        Me.btnBrowseOrd.Location = New System.Drawing.Point(370, 268)
        Me.btnBrowseOrd.Name = "btnBrowseOrd"
        Me.btnBrowseOrd.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseOrd.TabIndex = 62
        '
        'lblcoord
        '
        Me.lblcoord.AutoSize = True
        Me.lblcoord.Location = New System.Drawing.Point(6, 294)
        Me.lblcoord.Name = "lblcoord"
        Me.lblcoord.Size = New System.Drawing.Size(178, 13)
        Me.lblcoord.TabIndex = 65
        Me.lblcoord.Text = "Network Coordinates (nnncoord.dat)"
        '
        'btnBrowseCoord
        '
        Me.btnBrowseCoord.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseCoord.Image = CType(resources.GetObject("btnBrowseCoord.Image"), System.Drawing.Image)
        Me.btnBrowseCoord.Location = New System.Drawing.Point(370, 308)
        Me.btnBrowseCoord.Name = "btnBrowseCoord"
        Me.btnBrowseCoord.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseCoord.TabIndex = 64
        '
        'lblsrc
        '
        Me.lblsrc.AutoSize = True
        Me.lblsrc.Location = New System.Drawing.Point(6, 213)
        Me.lblsrc.Name = "lblsrc"
        Me.lblsrc.Size = New System.Drawing.Size(119, 13)
        Me.lblsrc.TabIndex = 63
        Me.lblsrc.Text = "Stream Raster Grid (src)"
        '
        'btnBrowseSrc
        '
        Me.btnBrowseSrc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseSrc.Image = CType(resources.GetObject("btnBrowseSrc.Image"), System.Drawing.Image)
        Me.btnBrowseSrc.Location = New System.Drawing.Point(370, 227)
        Me.btnBrowseSrc.Name = "btnBrowseSrc"
        Me.btnBrowseSrc.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseSrc.TabIndex = 61
        '
        'txtbxW
        '
        Me.txtbxW.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxW.Location = New System.Drawing.Point(9, 393)
        Me.txtbxW.Name = "txtbxW"
        Me.txtbxW.Size = New System.Drawing.Size(356, 20)
        Me.txtbxW.TabIndex = 78
        '
        'lblw
        '
        Me.lblw.AutoSize = True
        Me.lblw.Location = New System.Drawing.Point(6, 376)
        Me.lblw.Name = "lblw"
        Me.lblw.Size = New System.Drawing.Size(98, 13)
        Me.lblw.TabIndex = 77
        Me.lblw.Text = "Watershed Grid (w)"
        '
        'btnBrowseW
        '
        Me.btnBrowseW.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseW.Image = CType(resources.GetObject("btnBrowseW.Image"), System.Drawing.Image)
        Me.btnBrowseW.Location = New System.Drawing.Point(370, 390)
        Me.btnBrowseW.Name = "btnBrowseW"
        Me.btnBrowseW.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseW.TabIndex = 76
        '
        'frmLoadDelinOutputs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(402, 492)
        Me.Controls.Add(Me.txtbxW)
        Me.Controls.Add(Me.lblw)
        Me.Controls.Add(Me.btnBrowseW)
        Me.Controls.Add(Me.txtbxNet)
        Me.Controls.Add(Me.txtbxTree)
        Me.Controls.Add(Me.lbltree)
        Me.Controls.Add(Me.btnBrowseTree)
        Me.Controls.Add(Me.lblnet)
        Me.Controls.Add(Me.btnBrowseNet)
        Me.Controls.Add(Me.txtbxCoord)
        Me.Controls.Add(Me.txtbxOrd)
        Me.Controls.Add(Me.txtbxSrc)
        Me.Controls.Add(Me.lblord)
        Me.Controls.Add(Me.btnBrowseOrd)
        Me.Controls.Add(Me.lblcoord)
        Me.Controls.Add(Me.btnBrowseCoord)
        Me.Controls.Add(Me.lblsrc)
        Me.Controls.Add(Me.btnBrowseSrc)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtbxTlen)
        Me.Controls.Add(Me.txtbxPlen)
        Me.Controls.Add(Me.lblplen)
        Me.Controls.Add(Me.btnBrowsePlen)
        Me.Controls.Add(Me.lbltlen)
        Me.Controls.Add(Me.btnBrowseTlen)
        Me.Controls.Add(Me.txtbxGord)
        Me.Controls.Add(Me.txtbxSca)
        Me.Controls.Add(Me.txtbxAd8)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblsca)
        Me.Controls.Add(Me.btnBrowseSca)
        Me.Controls.Add(Me.lblgord)
        Me.Controls.Add(Me.btnBrowseGord)
        Me.Controls.Add(Me.lblad8)
        Me.Controls.Add(Me.btnBrowseAd8)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLoadDelinOutputs"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Load Pre-existing Stream Delineation  Files"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowseAd8 As System.Windows.Forms.Button
    Friend WithEvents lblad8 As System.Windows.Forms.Label
    Friend WithEvents lblgord As System.Windows.Forms.Label
    Friend WithEvents btnBrowseGord As System.Windows.Forms.Button
    Friend WithEvents lblsca As System.Windows.Forms.Label
    Friend WithEvents btnBrowseSca As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents txtbxAd8 As System.Windows.Forms.TextBox
    Friend WithEvents txtbxSca As System.Windows.Forms.TextBox
    Friend WithEvents txtbxGord As System.Windows.Forms.TextBox
    Friend WithEvents fdgOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtbxTlen As System.Windows.Forms.TextBox
    Friend WithEvents txtbxPlen As System.Windows.Forms.TextBox
    Friend WithEvents lblplen As System.Windows.Forms.Label
    Friend WithEvents btnBrowsePlen As System.Windows.Forms.Button
    Friend WithEvents lbltlen As System.Windows.Forms.Label
    Friend WithEvents btnBrowseTlen As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtbxNet As System.Windows.Forms.TextBox
    Friend WithEvents txtbxTree As System.Windows.Forms.TextBox
    Friend WithEvents lbltree As System.Windows.Forms.Label
    Friend WithEvents btnBrowseTree As System.Windows.Forms.Button
    Friend WithEvents lblnet As System.Windows.Forms.Label
    Friend WithEvents btnBrowseNet As System.Windows.Forms.Button
    Friend WithEvents txtbxCoord As System.Windows.Forms.TextBox
    Friend WithEvents txtbxOrd As System.Windows.Forms.TextBox
    Friend WithEvents txtbxSrc As System.Windows.Forms.TextBox
    Friend WithEvents lblord As System.Windows.Forms.Label
    Friend WithEvents btnBrowseOrd As System.Windows.Forms.Button
    Friend WithEvents lblcoord As System.Windows.Forms.Label
    Friend WithEvents btnBrowseCoord As System.Windows.Forms.Button
    Friend WithEvents lblsrc As System.Windows.Forms.Label
    Friend WithEvents btnBrowseSrc As System.Windows.Forms.Button
    Friend WithEvents txtbxW As System.Windows.Forms.TextBox
    Friend WithEvents lblw As System.Windows.Forms.Label
    Friend WithEvents btnBrowseW As System.Windows.Forms.Button
End Class
