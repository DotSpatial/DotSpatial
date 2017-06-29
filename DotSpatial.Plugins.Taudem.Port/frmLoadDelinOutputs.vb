' The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
' you may not use this file except in compliance with the License. You may obtain a copy of the License at 
' http://www.mozilla.org/MPL/ 
' Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
' ANY KIND, either express or implied. See the License for the specific terms governing rights and 
' limitations under the License. 
'
'Part of code for MWTauDEM library supporting TauDEM (http://hydrology.usu.edu/taudem/taudem5.0/)
'for MapWindow (http://www.mapwindow.org)

Public Class frmLoadDelinOutputs
    Public ad8Path As String
    Public scaPath As String
    Public gordPath As String
    Public plenPath As String
    Public tlenPath As String
    Public srcPath As String
    Public ordPath As String
    Public coordPath As String
    Public treePath As String
    Public netPath As String
    Public wPath As String
    Public selectedDem As String

    Private Sub frmLoadDelinOutputs_VisibleChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        If Me.Visible = True Then
            'ad8Path = ""
            'scaPath = ""
            'gordPath = ""
            'plenPath = ""
            'tlenPath = ""
            'srcPath = ""
            'ordPath = ""
            'coordPath = ""
            'treePath = ""
            'netPath = ""
            'wPath = ""

            If tdbChoiceList.useDinf Then
                txtbxSca.Enabled = True
                btnBrowseSca.Enabled = True
            Else
                txtbxSca.Enabled = False
                btnBrowseSca.Enabled = False
            End If
        End If
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        ' Paul Meems, 23-Aug-2011, Added:
        ad8Path = ""
        scaPath = ""
        gordPath = ""
        plenPath = ""
        tlenPath = ""
        srcPath = ""
        ordPath = ""
        coordPath = ""
        treePath = ""
        netPath = ""
        wPath = ""

        If txtbxAd8.Text <> "" AndAlso IO.File.Exists(txtbxAd8.Text) Then
            ad8Path = txtbxAd8.Text
        End If

        If txtbxSca.Text <> "" AndAlso IO.File.Exists(txtbxSca.Text) Then
            scaPath = txtbxSca.Text
        End If

        If txtbxGord.Text <> "" AndAlso IO.File.Exists(txtbxGord.Text) Then
            gordPath = txtbxGord.Text
        End If

        If txtbxPlen.Text <> "" AndAlso IO.File.Exists(txtbxPlen.Text) Then
            plenPath = txtbxPlen.Text
        End If

        If txtbxTlen.Text <> "" AndAlso IO.File.Exists(txtbxTlen.Text) Then
            tlenPath = txtbxTlen.Text
        End If

        If txtbxSrc.Text <> "" AndAlso IO.File.Exists(txtbxSrc.Text) Then
            srcPath = txtbxSrc.Text
        End If

        If txtbxOrd.Text <> "" AndAlso IO.File.Exists(txtbxOrd.Text) Then
            ordPath = txtbxOrd.Text
        End If

        If txtbxCoord.Text <> "" AndAlso IO.File.Exists(txtbxCoord.Text) Then
            coordPath = txtbxCoord.Text
        End If

        If txtbxTree.Text <> "" AndAlso IO.File.Exists(txtbxTree.Text) Then
            treePath = txtbxTree.Text
        End If

        If txtbxNet.Text <> "" AndAlso IO.File.Exists(txtbxNet.Text) Then
            netPath = txtbxNet.Text
        End If

        If txtbxW.Text <> "" AndAlso IO.File.Exists(txtbxW.Text) Then
            wPath = txtbxW.Text
        End If

        'If ad8Path = "" Or (tdbChoiceList.useDinf And scaPath = "") Or gordPath = "" Or plenPath = "" Or tlenPath = "" Or srcPath = "" Or ordPath = "" Or coordPath = "" Or treePath = "" Or netPath = "" Or wPath = "" Then
        '    MsgBox("There must be a path selected for each of the pre-existing threshold delineation intermediate grids.", MsgBoxStyle.OkOnly, "Load Intermediate Files")
        '    ad8Path = ""
        '    scaPath = ""
        '    gordPath = ""
        '    plenPath = ""
        '    tlenPath = ""
        '    srcPath = ""
        '    ordPath = ""
        '    coordPath = ""
        '    treePath = ""
        '    netPath = ""
        '    wPath = ""
        '    txtbxAd8.Text = ""
        '    txtbxSca.Text = ""
        '    txtbxGord.Text = ""
        '    txtbxPlen.Text = ""
        '    txtbxTlen.Text = ""
        '    txtbxSrc.Text = ""
        '    txtbxOrd.Text = ""
        '    txtbxCoord.Text = ""
        '    txtbxTree.Text = ""
        '    txtbxNet.Text = ""
        '    txtbxW.Text = ""
        '    Me.DialogResult = Windows.Forms.DialogResult.Cancel
        'End If
    End Sub

    Private Function GetDefaultFilename(ByVal append As String, Optional ByVal extention As String = ".tif") As String
        Dim selectedDemBase As String = System.IO.Path.GetFileNameWithoutExtension(selectedDem)

        Dim suggestedFilename As String = selectedDemBase & append & extention

        ' Check if the file exists:
        If Not System.IO.File.Exists(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(selectedDem), suggestedFilename)) Then
            suggestedFilename = String.Empty
        End If

        Return suggestedFilename
    End Function

    Private Function GetFilenameFromUser(ByVal append As String, Optional ByVal filter As String = "", Optional ByVal extention As String = ".tif") As String
        If filter = "" Then
            '  fdgOpen.Filter = tdbFileList.GetGridFilter()
        Else
            fdgOpen.Filter = filter
        End If

        fdgOpen.FilterIndex = 1
        fdgOpen.InitialDirectory = System.IO.Path.GetDirectoryName(selectedDem)
        fdgOpen.CheckFileExists = True

        'Default filename:
        fdgOpen.FileName = GetDefaultFilename(append, extention)
        If fdgOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return fdgOpen.FileName
        End If

        Return String.Empty

    End Function

    Private Sub btnBrowseAd8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseAd8.Click
        txtbxAd8.Text = GetFilenameFromUser("ad8")
    End Sub

    Private Sub btnBrowseSca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSca.Click
        txtbxSca.Text = GetFilenameFromUser("sca")
    End Sub

    Private Sub btnBrowseGord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseGord.Click
        txtbxGord.Text = GetFilenameFromUser("gord")
    End Sub

    Private Sub btnBrowsePlen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowsePlen.Click
        txtbxPlen.Text = GetFilenameFromUser("plen")
    End Sub

    Private Sub btnBrowseTlen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseTlen.Click
        txtbxTlen.Text = GetFilenameFromUser("tlen")
    End Sub

    Private Sub btnBrowseSrc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseSrc.Click
        txtbxSrc.Text = GetFilenameFromUser("src")
    End Sub

    Private Sub btnBrowseOrd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOrd.Click
        txtbxOrd.Text = GetFilenameFromUser("ord")
    End Sub

    Private Sub btnBrowseCoord_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseCoord.Click
        txtbxCoord.Text = GetFilenameFromUser("coord", "Data File|*.dat", ".dat")
    End Sub

    Private Sub btnBrowseTree_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseTree.Click
        txtbxTree.Text = GetFilenameFromUser("tree", "Data File|*.dat", ".dat")
    End Sub

    Private Sub btnBrowseNet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseNet.Click
        txtbxNet.Text = GetFilenameFromUser("net", "Shape File|*.shp", ".shp")
    End Sub

    Private Sub btnBrowseW_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseW.Click
        txtbxW.Text = GetFilenameFromUser("w")
    End Sub
End Class