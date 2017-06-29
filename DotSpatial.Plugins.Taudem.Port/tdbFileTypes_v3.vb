'********************************************************************************************************
' FILENAME:      tdbFileTypes.vb
' DESCRIPTION:  A class to hold and manipulate the list of files
'   used by the taudem project
' NOTES: This form is called from mwTauDemBASINSWrap.vb
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
'you may not use this file except in compliance with the License. You may obtain a copy of the License at
'http://www.mozilla.org/MPL/
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and
'limitations under the License.
'
'
'Last Update:   10/18/05, ARA
' Change Log:
' Date          Changed By      Notes
'10/18/05       ARA             Added mozilla comments
'10/24/05       ARA             Fixed header filename
'********************************************************************************************************

Public Class tdbFileTypes_v3
    Public dem As String
    Public fel As String
    Public fdr As String
    Public fdrn As String
    Public mask As String
    Public decayMult As String
    Public dsca As String
    Public sllAccum As String
    Public p As String
    Public sd8 As String
    Public ang As String
    Public slp As String
    Public ad8 As String
    Public sca As String
    Public q As String
    Public gord As String
    Public plen As String
    Public tlen As String
    Public src As String
    Public ord As String
    Public w As String
    Public sar As String
    Public sord As String
    Public dist As String
    Public tree As String
    Public coord As String
    Public tri As String
    Public ann As String
    Public outletshpfile As String
    Public netoutletshpfile As String
    Public net As String
    Public wshed As String
    Public mergewshed As String
    Public weights As String
    Public modelspcfile As String
    Public basinparfile As String
    Public raincoordfile As String
    Public flowcoordfile As String
    Public dg As String
    Public dep As String
    Public di As String
    Public tla As String
    Public tc As String
    Public tsup As String
    Public tdep As String
    Public cs As String
    Public ctpt As String
    Public racc As String
    Public dmax As String
    Public ass As String
    Public rz As String
    Public dfs As String
    Public dd As String
    Public du As String
    Public slpd As String
    Public tfilenames() As String
    Public gfilenames() As String

    Public Sub FormFileNames(ByVal demfn As String, ByVal outFolder As String, ByVal checkDirExists As Boolean)
        dem = demfn
        Dim outPath As String
        Dim absoutdir As String

        If IO.Path.GetFileName(demfn) = "sta.adf" Then
            absoutdir = getAbsolutePath(outFolder, IO.Path.GetDirectoryName(demfn) + ".tif")
        Else
            absoutdir = getAbsolutePath(outFolder, demfn)
        End If

        If Not IO.Directory.Exists(absoutdir) Then
            If checkDirExists Then
                If MsgBox("The output directory " + absoutdir + " does not exist. Click OK to create this directory now or cancel to use the default output directory of " + IO.Path.GetDirectoryName(dem), MsgBoxStyle.OkCancel, "Create output directory?") = MsgBoxResult.Ok Then
                    IO.Directory.CreateDirectory(absoutdir)
                Else
                    absoutdir = IO.Path.GetDirectoryName(dem) + "\"
                End If
            End If
        End If

        '
        ' Convert to bgd if not a bgd already. TAUDEM doesn't take tifs well yet
        ' CWG 23/1/2011 changed to use tifs for Taudem V5
        '
        dem = demfn
        If System.IO.Path.GetExtension(dem) <> ".tif" Then
            CheckGridIsTif(dem)
        End If

        outPath = absoutdir + IO.Path.GetFileName(dem)
        fel = AppendExt(outPath, "fel")
        fdr = AppendExt(outPath, "fdr")
        fdrn = AppendExt(outPath, "fdrn")
        dsca = AppendExt(outPath, "dsca")
        sllAccum = AppendExt(outPath, "cla")
        p = AppendExt(outPath, "p")
        sd8 = AppendExt(outPath, "sd8")
        ang = AppendExt(outPath, "ang")
        slp = AppendExt(outPath, "slp")
        ad8 = AppendExt(outPath, "ad8")
        sca = AppendExt(outPath, "sca")
        q = AppendExt(outPath, "q")
        gord = AppendExt(outPath, "gord")
        plen = AppendExt(outPath, "plen")
        tlen = AppendExt(outPath, "tlen")
        src = AppendExt(outPath, "src")
        ord = AppendExt(outPath, "ord")
        w = AppendExt(outPath, "w")
        sar = AppendExt(outPath, "sar")
        sord = AppendExt(outPath, "sord")
        dist = AppendExt(outPath, "dist")
        tri = AppendExt(outPath, "tri")
        ann = AppendExt(outPath, "ann")
        tree = getmain(outPath) & "tree.dat"
        coord = getmain(outPath) & "coord.dat"
        net = getmain(outPath) & "net.shp"
        netoutletshpfile = getmain(outPath) & "_netOutlet.shp"
        wshed = getmain(outPath) & "w.shp"
        mergewshed = getmain(outPath) & "w_merged.shp"
        dep = AppendExt(outPath, "dep")
        dg = AppendExt(outPath, "dg")
        di = AppendExt(outPath, "di")
        tla = AppendExt(outPath, "tla")
        tc = AppendExt(outPath, "tc")
        tdep = AppendExt(outPath, "tdep")
        cs = AppendExt(outPath, "cs")
        ctpt = AppendExt(outPath, "ctpt")
        tsup = AppendExt(outPath, "tsup")
        racc = AppendExt(outPath, "racc")
        dmax = AppendExt(outPath, "dmax")
        ass = AppendExt(outPath, "ass")
        rz = AppendExt(outPath, "rz")
        dfs = AppendExt(outPath, "dfs")
        dd = AppendExt(outPath, "dd")
        du = AppendExt(outPath, "du")
        slpd = AppendExt(outPath, "slpd")
        ' topsetup files
        modelspcfile = getfolder(outPath) & "\modelspc.dat"
        basinparfile = getfolder(outPath) & "\basinpars.txt"
        raincoordfile = getfolder(outPath) & "\raing.shp"
        flowcoordfile = getfolder(outPath) & "\flowcoord.txt"
        ReDim gfilenames(0)
        ReDim tfilenames(0)
    End Sub

    Public Function AppendExt(ByVal fname As String, ByVal Ext As String) As String
        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "." Then Exit For
        Next i
        If (i = 0) Then
            AppendExt = fname & Ext
        Else
            AppendExt = Left(fname, i - 1) & Ext & Mid(fname, i)
        End If
    End Function

    Public Function getfolder(ByVal fname As String) As String

        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "\" Then Exit For
        Next i
        If i = 0 Then
            getfolder = ""
        Else
            getfolder = Left(fname, i - 1)
        End If
    End Function

    Public Function getmain(ByVal fname As String) As String
        ' Gets the part of a string before the last .
        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "." Then Exit For
        Next i
        If i = 0 Then
            getmain = fname
        Else
            getmain = Left(fname, i - 1)
        End If
    End Function

    Public Function getext(ByVal fname As String) As String
        ' Gets the part of a string after the last .
        ' Blank if not extension
        Dim i As Integer
        For i = Len(fname) To 1 Step -1
            If Mid(fname, i, 1) = "." Then Exit For
        Next i
        If i = 0 Then
            getext = ""
        Else
            getext = Mid(fname, i + 1)
        End If
    End Function


    Public Function GetRelativePath(ByVal Filename As String, ByVal ProjectFile As String) As String


        GetRelativePath = ""
        Dim a() As String, b() As String
        Dim i As Integer, j As Integer, k As Integer, Offset As Integer

        If Len(Filename) = 0 Or Len(ProjectFile) = 0 Then
            Return ""
        End If

        Try
            'If the drive is different then use the full path
            If System.IO.Path.GetPathRoot(Filename).ToLower() <> System.IO.Path.GetPathRoot(ProjectFile).ToLower() Then
                GetRelativePath = Filename
                Exit Function
            End If
            '
            'load a()
            ReDim a(0)
            a(0) = Filename
            i = 0
            Do
                i = i + 1
                ReDim Preserve a(i)
                Try
                    a(i) = System.IO.Directory.GetParent(a(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until a(i) = ""
            '
            'load b()
            ReDim b(0)
            b(0) = ProjectFile
            i = 0
            Do
                i = i + 1
                ReDim Preserve b(i)
                Try
                    b(i) = System.IO.Directory.GetParent(b(i - 1)).FullName.ToLower()
                Catch
                End Try
            Loop Until b(i) = ""
            '
            'look for match
            For i = 0 To UBound(a)
                For j = 0 To UBound(b)
                    If a(i) = b(j) Then
                        'found match
                        GoTo [CONTINUE]
                    End If
                Next j
            Next i
[CONTINUE]:
            ' j is num steps to get from BasePath to common path
            ' so I need this many of "..\"
            For k = 1 To j - 1
                GetRelativePath = GetRelativePath & "..\"
            Next k

            'everything past a(i) needs to be appended now.
            If a(i).EndsWith("\") Then
                Offset = 0
            Else
                Offset = 1
            End If
            GetRelativePath = GetRelativePath & Filename.Substring(Len(a(i)) + Offset)
        Catch e As System.Exception
            Return ""
        End Try
    End Function

    Public Function getAbsolutePath(ByVal relativePath As String, ByVal fromFile As String) As String
        If relativePath <> "" Then
            ChDir(IO.Path.GetDirectoryName(fromFile))
            getAbsolutePath = IO.Path.GetFullPath(relativePath) + "\"
        Else
            Return IO.Path.GetDirectoryName(fromFile) + "\"
        End If
    End Function

    Public Function CheckGridIsTif(ByRef gridpath As String) As Boolean
        If Not System.IO.Path.GetExtension(gridpath) = ".tif" Then
            If MsgBox("TauDEM requires that grids are GeoTiff files (.tif).  Do you want your file converted?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Return False
            End If

            'Dim tifFilePath As String
            'If IO.Path.GetFileName(gridpath) = "sta.adf" Then
            '	Dim outPath As String = IO.Path.GetDirectoryName(gridpath)
            '	tifFilepath = outPath & "\" & IO.Path.GetFileName(outPath) & ".tif"
            'Else
            '	tifFilePath = System.IO.Path.ChangeExtension(gridpath, ".tif")
            '      End If

            'If System.IO.File.Exists(tifFilePath) Then
            '	If MsgBox(tifFilePath & " already exists.  Overwrite it?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
            '		Return False
            '	End If
            '      End If

            'Try ' CWG ChangeGridFormat can fail with a memory violation on Esri grids
            '	If Not MapWinGeoProc.DataManagement.ChangeGridFormat( _
            '		gridpath, tifFilePath, MapWinGIS.GridFileType.UseExtension, MapWinGIS.GridDataType.FloatDataType, 1.0F) Then
            '		MsgBox("Failed to convert grid " & gridpath & " to GeoTiff.  Try using GIS Tools")
            '		Return False
            '	End If
            'Catch
            '	MsgBox("Failed to convert grid " & gridpath & " to GeoTiff.  Try using GIS Tools")
            '	Return False
            'End Try
            'gridpath = tifFilePath
        End If

        Return True
    End Function

End Class
