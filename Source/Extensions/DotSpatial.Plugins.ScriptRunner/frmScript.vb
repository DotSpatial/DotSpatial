'********************************************************************************************************
'File Name: frmScript.vb
'Description: MapWindow Script System
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
' The original code is
' Adapted for MapWindow by Chris Michaelis (cmichaelis@happysquirrel.com) on Jan 1 2006
' Originally created by Mark Gray of AquaTerra Consultants
'
'The Initial Developer of this version of the Original Code is Christopher Michaelis, done
'by reshifting and moving about the various utility functions from MapWindow's modPublic.vb
'(which no longer exists) and some useful utility functions from Aqua Terra Consulting.
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Feb 8 2006 - Chris Michaelis - Added the "Online Script Directory" features.
'8/9/2006 - Paul Meems (pm) - Started Duth translation
'1/28/2008 - Jiri Kadlec - changed ResourceManager (message strings moved to GlobalResource.resx)
'12/11/2010 - Kurt Wolfe - ported to DotSpatial
'12/28/2011 - Kurt Wolfe - Move plugin type to DotSpatial.Controls.Extension instead of IExtension
'********************************************************************************************************
Imports System.CodeDom.Compiler
Imports System.ComponentModel.Composition.Hosting
Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Threading
Imports System.Windows.Forms
Imports DotSpatial.Controls

Public Class FrmScript
    'Inherits MapWindow.baseForm '5/5/2008 jk
    Inherits Form

    Private ReadOnly _appMgr As AppManager

#Region "Declarations"
    'changed by Jiri Kadlec
    Private ReadOnly resources As New Resources.ResourceManager("MapWindow.GlobalResource", Assembly.GetExecutingAssembly())
    'Public Declare Function LockWindowUpdate Lib "user32" (ByVal hWnd As Integer) As Integer
#End Region

    Private ReadOnly pDefaultScript_VB As String =
                "Imports DotSpatial.Controls" & vbCrLf _
                & "Imports System.Windows.Forms" & vbCrLf _
                & "Imports Microsoft.VisualBasic" & vbCrLf _
                & "Imports System" & vbCrLf _
                & vbCrLf _
                & "'Each script should (but doesn't have to) have a unique name. Change MyExample here to something meaningful. ScriptMain should remain as ""Main"" however." & vbCrLf _
                & "Public Module MyExample" & vbCrLf _
                & "  Public Sub ScriptMain(appMgr as AppManager)" & vbCrLf _
                & "    MessageBox.Show(""This is a simple script to display the number of loaded layers in the map: "" & appMgr.Map.Layers.Count)" & vbCrLf _
                & "  End Sub" & vbCrLf _
                & "End Module"

    Private ReadOnly pDefaultPlugin_VB As String =
                "Imports DotSpatial.Controls" & vbCrLf _
                & "Imports DotSpatial.Controls.Header" & vbCrLf _
                & "Imports System.Windows.Forms" & vbCrLf _
                & "Imports Microsoft.VisualBasic" & vbCrLf _
                & "Imports System" & vbCrLf &
                vbCrLf &
                vbCrLf &
                "Public Class MyPlugin" & vbCrLf &
                "    Inherits Extension" & vbCrLf &
                "" + vbCrLf &
                "    Dim host As AppManager" & vbCrLf &
                "    Dim btnSample As ToolStripButton" & vbCrLf &
                vbCrLf &
                "    Public Sub New()" & vbCrLf &
                "    End Sub" & vbCrLf &
                "" & vbCrLf &
                "   Public Overrides Sub Activate()" & vbCrLf &
                "" & vbCrLf &
                "       MyBase.Activate()" & vbCrLf &
                "       host = Me.App" & vbCrLf &
                "       Dim SamplePluginMenuKey as String = ""kSampleScriptPlugin""" + vbCrLf &
                "       Dim samplePlugin as String = ""Sample Script Plugin""" + vbCrLf &
                "" & vbCrLf &
                "       host.HeaderControl.Add(New RootItem(SamplePluginMenuKey, samplePlugin))" & vbCrLf &
                "       host.HeaderControl.Add(New SimpleActionItem(SamplePluginMenuKey, samplePlugin,  New EventHandler(AddressOf btnSample_Click))" & vbCrLf &
                "" & vbCrLf &
                "   End Sub" & vbCrLf &
                vbCrLf &
                "   Public Overrides Sub Deactivate()" & vbCrLf &
                "           host.HeaderControl.RemoveAll()" & vbCrLf &
                "           MyBase.Deactivate()" & vbCrLf &
                "   End Sub" & vbCrLf &
                vbCrLf &
                vbCrLf &
                "   Private Sub btnSample_Click(ByVal sender As Object, ByVal e As EventArgs)" & vbCrLf &
                "       MessageBox.Show(""Hello World"")" & vbCrLf &
                "   End Sub" & vbCrLf &
                "End Class"


    Private ReadOnly pFilter As String = "VB.net (*.vb)|*.vb|C#.net (*.cs)|*.cs|All files|*.*"
    Private ReadOnly pFilterVB As String = "VB.net (*.vb)|*.vb|All files|*.*"
    Private ReadOnly pFilterCS As String = "C#.net (*.cs)|*.cs|All files|*.*"

    Public pFileName As String = ""

    Private DataChanged As Boolean = False
    Private IgnoreDataChange As Boolean = False

    Private ReadOnly pDefaultScript_CS As String =
                "using DotSpatial.Controls;" & vbCrLf _
                & "using System.Windows.Forms;" & vbCrLf _
                & "using Microsoft.VisualBasic;" & vbCrLf _
                & "using System;" & vbCrLf &
                vbCrLf &
                "namespace MyNamespace" + vbCrLf &
                "{" + vbCrLf &
                "   // You must change the name of this class to something unique!" & vbCrLf &
                "   class MyExample" + vbCrLf &
                "   {" + vbCrLf &
                "       public static void ScriptMain(AppManager appMgr)" + vbCrLf &
                "       {    " + vbCrLf &
                "           MessageBox.Show(""This is a simple script to display the number of loaded layers in the map: "" + appMgr.Map.Layers.Count);" + vbCrLf &
                "       }" + vbCrLf &
                "   }" & vbCrLf &
                "}" & vbCrLf
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents TbbNew As ToolStripButton
    Friend WithEvents TbbOpen As ToolStripButton
    Friend WithEvents TbbSave As ToolStripButton
    Friend WithEvents Tbbsep As ToolStripSeparator
    Friend WithEvents TbbRun As ToolStripButton
    Friend WithEvents TbbCompile As ToolStripButton
    Friend WithEvents TbbHelp As ToolStripButton
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents MnuNew As ToolStripMenuItem
    Friend WithEvents MnuOpen As ToolStripMenuItem
    Friend WithEvents MnuSave As ToolStripMenuItem
    Friend WithEvents MnuClose As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As ToolStripMenuItem
    Friend WithEvents MnuRun As ToolStripMenuItem
    Friend WithEvents MnuCompile As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As ToolStripMenuItem
    Friend WithEvents MnuViewRun As ToolStripMenuItem
    Friend WithEvents MnuSubmit As ToolStripMenuItem
    Friend WithEvents MnuHelp As ToolStripMenuItem
    Friend WithEvents BottomToolStripPanel As ToolStripPanel
    Friend WithEvents TopToolStripPanel As ToolStripPanel
    Friend WithEvents RightToolStripPanel As ToolStripPanel
    Friend WithEvents LeftToolStripPanel As ToolStripPanel
    Friend WithEvents ContentPanel As ToolStripContentPanel
    Friend WithEvents ToolStripContainer1 As ToolStripContainer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents RdScript As RadioButton
    Friend WithEvents RdPlugin As RadioButton
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents RdVBNet As RadioButton
    Friend WithEvents RdCS As RadioButton
    Friend WithEvents MnuSep As ToolStripSeparator
    <CLSCompliant(False)>
    Public WithEvents TxtScript As CSharpEditor.MainForm

    Private ReadOnly pDefaultPlugin_CS As String =
                "using DotSpatial.Controls;" & vbCrLf _
                & "using DotSpatial.Controls.Header;" & vbCrLf _
                & "using System.Windows.Forms;" & vbCrLf _
                & "using Microsoft.VisualBasic;" & vbCrLf _
                & "using System;" & vbCrLf &
                "" & vbCrLf &
                "namespace MyNamespace" & vbCrLf &
                "{" + vbCrLf &
                "	public class MyExample : Extension" & vbCrLf &
                "	{" + vbCrLf &
                "       private AppManager _host;" + vbCrLf &
                "" + vbCrLf &
                "		// Change this to match the name of your class. This is the constructor." + vbCrLf &
                "		public MyExample()" + vbCrLf &
                "		{" + vbCrLf &
                "" + vbCrLf &
                "		}" + vbCrLf &
                "" + vbCrLf &
                vbCrLf &
                "       public override void Activate()" & vbCrLf &
                "       {" + vbCrLf &
                "" + vbCrLf &
                "           base.Activate();" + vbCrLf &
                "           _host = App;" + vbCrLf &
                "           string SamplePluginMenuKey = ""kSampleScriptPlugin"";" + vbCrLf &
                "           string samplePlugin = ""Sample Scripts Plugin"";" + vbCrLf &
                "            _host.HeaderControl.Add(new RootItem(SamplePluginMenuKey, samplePlugin));" & vbCrLf &
                "            _host.HeaderControl.Add(new SimpleActionItem(SamplePluginMenuKey, samplePlugin,  btnSample_Click));" & vbCrLf &
                "" & vbCrLf &
                "       }" + vbCrLf &
                vbCrLf &
                "       public override void Deactivate()" & vbCrLf &
                "       {" & vbCrLf &
                "           _host.HeaderControl.RemoveAll();" & vbCrLf &
                "           base.Deactivate();" & vbCrLf &
                "       }" & vbCrLf &
                vbCrLf &
                "       private void btnSample_Click(object sender, EventArgs e )" & vbCrLf &
                "       {" & vbCrLf &
                "           MessageBox.Show(""Hello World"");" & vbCrLf &
                "       }" & vbCrLf &
                "   }" & vbCrLf &
                "}"

#Region " Windows Form Designer generated code "

    Public Sub New(appManager As AppManager)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        _appMgr = appManager

        'May/12/2008 Jiri Kadlec - load icon from shared resources to reduce size of the program
        'Me.Icon = My.Resources.MapWindow_new
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents Tools As ToolStrip
    <DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Dim resources As New System.ComponentModel.ComponentResourceManager(GetType(FrmScript))
        ToolStripContainer1 = New ToolStripContainer()
        TxtScript = New CSharpEditor.MainForm()
        GroupBox2 = New GroupBox()
        RdScript = New RadioButton()
        RdPlugin = New RadioButton()
        GroupBox1 = New GroupBox()
        RdVBNet = New RadioButton()
        RdCS = New RadioButton()
        MenuStrip1 = New MenuStrip()
        ToolStripMenuItem1 = New ToolStripMenuItem()
        MnuNew = New ToolStripMenuItem()
        MnuOpen = New ToolStripMenuItem()
        MnuSave = New ToolStripMenuItem()
        MnuSep = New ToolStripSeparator()
        MnuClose = New ToolStripMenuItem()
        ToolStripMenuItem6 = New ToolStripMenuItem()
        MnuRun = New ToolStripMenuItem()
        MnuCompile = New ToolStripMenuItem()
        ToolStripMenuItem2 = New ToolStripMenuItem()
        MnuViewRun = New ToolStripMenuItem()
        MnuSubmit = New ToolStripMenuItem()
        MnuHelp = New ToolStripMenuItem()
        Tools = New ToolStrip()
        ImageList1 = New ImageList(components)
        TbbNew = New ToolStripButton()
        TbbOpen = New ToolStripButton()
        TbbSave = New ToolStripButton()
        Tbbsep = New ToolStripSeparator()
        TbbRun = New ToolStripButton()
        TbbCompile = New ToolStripButton()
        TbbHelp = New ToolStripButton()
        BottomToolStripPanel = New ToolStripPanel()
        TopToolStripPanel = New ToolStripPanel()
        RightToolStripPanel = New ToolStripPanel()
        LeftToolStripPanel = New ToolStripPanel()
        ContentPanel = New ToolStripContentPanel()
        ToolStripContainer1.ContentPanel.SuspendLayout()
        ToolStripContainer1.TopToolStripPanel.SuspendLayout()
        ToolStripContainer1.SuspendLayout()
        GroupBox2.SuspendLayout()
        GroupBox1.SuspendLayout()
        MenuStrip1.SuspendLayout()
        Tools.SuspendLayout()
        SuspendLayout()
        '
        'ToolStripContainer1
        '
        ToolStripContainer1.AllowDrop = True
        '
        'ToolStripContainer1.ContentPanel
        '
        ToolStripContainer1.ContentPanel.Controls.Add(TxtScript)
        ToolStripContainer1.ContentPanel.Controls.Add(GroupBox2)
        ToolStripContainer1.ContentPanel.Controls.Add(GroupBox1)
        resources.ApplyResources(ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
        resources.ApplyResources(ToolStripContainer1, "ToolStripContainer1")
        ToolStripContainer1.Name = "ToolStripContainer1"
        '
        'ToolStripContainer1.TopToolStripPanel
        '
        ToolStripContainer1.TopToolStripPanel.Controls.Add(MenuStrip1)
        ToolStripContainer1.TopToolStripPanel.Controls.Add(Tools)
        '
        'txtScript
        '
        resources.ApplyResources(TxtScript, "txtScript")
        TxtScript.Name = "txtScript"
        '
        'GroupBox2
        '
        GroupBox2.Controls.Add(RdScript)
        GroupBox2.Controls.Add(RdPlugin)
        resources.ApplyResources(GroupBox2, "GroupBox2")
        GroupBox2.Name = "GroupBox2"
        GroupBox2.TabStop = False
        '
        'rdScript
        '
        RdScript.Checked = True
        resources.ApplyResources(RdScript, "rdScript")
        RdScript.Name = "rdScript"
        RdScript.TabStop = True
        '
        'rdPlugin
        '
        resources.ApplyResources(RdPlugin, "rdPlugin")
        RdPlugin.Name = "rdPlugin"
        '
        'GroupBox1
        '
        GroupBox1.Controls.Add(RdVBNet)
        GroupBox1.Controls.Add(RdCS)
        resources.ApplyResources(GroupBox1, "GroupBox1")
        GroupBox1.Name = "GroupBox1"
        GroupBox1.TabStop = False
        '
        'rdVBNet
        '
        RdVBNet.Checked = True
        resources.ApplyResources(RdVBNet, "rdVBNet")
        RdVBNet.Name = "rdVBNet"
        RdVBNet.TabStop = True
        '
        'rdCS
        '
        resources.ApplyResources(RdCS, "rdCS")
        RdCS.Name = "rdCS"
        '
        'MenuStrip1
        '
        resources.ApplyResources(MenuStrip1, "MenuStrip1")
        MenuStrip1.Items.AddRange(New ToolStripItem() {ToolStripMenuItem1, ToolStripMenuItem6, ToolStripMenuItem2, MnuHelp})
        MenuStrip1.Name = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        ToolStripMenuItem1.DropDownItems.AddRange(New ToolStripItem() {MnuNew, MnuOpen, MnuSave, MnuSep, MnuClose})
        ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'mnuNew
        '
        MnuNew.Name = "mnuNew"
        resources.ApplyResources(MnuNew, "mnuNew")
        '
        'mnuOpen
        '
        MnuOpen.Name = "mnuOpen"
        resources.ApplyResources(MnuOpen, "mnuOpen")
        '
        'mnuSave
        '
        MnuSave.Name = "mnuSave"
        resources.ApplyResources(MnuSave, "mnuSave")
        '
        'mnuSep
        '
        MnuSep.Name = "mnuSep"
        resources.ApplyResources(MnuSep, "mnuSep")
        '
        'mnuClose
        '
        MnuClose.Name = "mnuClose"
        resources.ApplyResources(MnuClose, "mnuClose")
        '
        'ToolStripMenuItem6
        '
        ToolStripMenuItem6.DropDownItems.AddRange(New ToolStripItem() {MnuRun, MnuCompile})
        ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        resources.ApplyResources(ToolStripMenuItem6, "ToolStripMenuItem6")
        '
        'mnuRun
        '
        MnuRun.Name = "mnuRun"
        resources.ApplyResources(MnuRun, "mnuRun")
        '
        'mnuCompile
        '
        MnuCompile.Name = "mnuCompile"
        resources.ApplyResources(MnuCompile, "mnuCompile")
        '
        'ToolStripMenuItem2
        '
        ToolStripMenuItem2.DropDownItems.AddRange(New ToolStripItem() {MnuViewRun, MnuSubmit})
        ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        resources.ApplyResources(ToolStripMenuItem2, "ToolStripMenuItem2")
        '
        'mnuViewRun
        '
        MnuViewRun.Name = "mnuViewRun"
        resources.ApplyResources(MnuViewRun, "mnuViewRun")
        '
        'mnuSubmit
        '
        MnuSubmit.Name = "mnuSubmit"
        resources.ApplyResources(MnuSubmit, "mnuSubmit")
        '
        'mnuHelp
        '
        MnuHelp.Name = "mnuHelp"
        resources.ApplyResources(MnuHelp, "mnuHelp")
        '
        'tools
        '
        resources.ApplyResources(Tools, "tools")
        Tools.ImageList = ImageList1
        Tools.Items.AddRange(New ToolStripItem() {TbbNew, TbbOpen, TbbSave, Tbbsep, TbbRun, TbbCompile, TbbHelp})
        Tools.Name = "tools"
        '
        'ImageList1
        '
        ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), ImageListStreamer)
        ImageList1.TransparentColor = Color.Transparent
        ImageList1.Images.SetKeyName(0, "")
        ImageList1.Images.SetKeyName(1, "")
        ImageList1.Images.SetKeyName(2, "")
        ImageList1.Images.SetKeyName(3, "")
        ImageList1.Images.SetKeyName(4, "")
        ImageList1.Images.SetKeyName(5, "")
        '
        'tbbNew
        '
        resources.ApplyResources(TbbNew, "tbbNew")
        TbbNew.Name = "tbbNew"
        '
        'tbbOpen
        '
        resources.ApplyResources(TbbOpen, "tbbOpen")
        TbbOpen.Name = "tbbOpen"
        '
        'tbbSave
        '
        resources.ApplyResources(TbbSave, "tbbSave")
        TbbSave.Name = "tbbSave"
        '
        'tbbsep
        '
        Tbbsep.Name = "tbbsep"
        resources.ApplyResources(Tbbsep, "tbbsep")
        '
        'tbbRun
        '
        resources.ApplyResources(TbbRun, "tbbRun")
        TbbRun.Name = "tbbRun"
        '
        'tbbCompile
        '
        resources.ApplyResources(TbbCompile, "tbbCompile")
        TbbCompile.Name = "tbbCompile"
        '
        'tbbHelp
        '
        resources.ApplyResources(TbbHelp, "tbbHelp")
        TbbHelp.Name = "tbbHelp"
        '
        'BottomToolStripPanel
        '
        resources.ApplyResources(BottomToolStripPanel, "BottomToolStripPanel")
        BottomToolStripPanel.Name = "BottomToolStripPanel"
        BottomToolStripPanel.Orientation = Orientation.Horizontal
        BottomToolStripPanel.RowMargin = New Padding(3, 0, 0, 0)
        '
        'TopToolStripPanel
        '
        resources.ApplyResources(TopToolStripPanel, "TopToolStripPanel")
        TopToolStripPanel.Name = "TopToolStripPanel"
        TopToolStripPanel.Orientation = Orientation.Horizontal
        TopToolStripPanel.RowMargin = New Padding(3, 0, 0, 0)
        '
        'RightToolStripPanel
        '
        resources.ApplyResources(RightToolStripPanel, "RightToolStripPanel")
        RightToolStripPanel.Name = "RightToolStripPanel"
        RightToolStripPanel.Orientation = Orientation.Horizontal
        RightToolStripPanel.RowMargin = New Padding(3, 0, 0, 0)
        '
        'LeftToolStripPanel
        '
        resources.ApplyResources(LeftToolStripPanel, "LeftToolStripPanel")
        LeftToolStripPanel.Name = "LeftToolStripPanel"
        LeftToolStripPanel.Orientation = Orientation.Horizontal
        LeftToolStripPanel.RowMargin = New Padding(3, 0, 0, 0)
        '
        'ContentPanel
        '
        resources.ApplyResources(ContentPanel, "ContentPanel")
        '
        'frmScript
        '
        AllowDrop = True
        resources.ApplyResources(Me, "$this")
        Controls.Add(ToolStripContainer1)
        Name = "frmScript"
        ToolStripContainer1.ContentPanel.ResumeLayout(False)
        ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
        ToolStripContainer1.TopToolStripPanel.PerformLayout()
        ToolStripContainer1.ResumeLayout(False)
        ToolStripContainer1.PerformLayout()
        GroupBox2.ResumeLayout(False)
        GroupBox1.ResumeLayout(False)
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        Tools.ResumeLayout(False)
        Tools.PerformLayout()
        ResumeLayout(False)

    End Sub

#End Region

    Private Sub Tools_ButtonClick(sender As Object, e As ToolStripItemClickedEventArgs) Handles Tools.ItemClicked
        PerformAction(CStr(e.ClickedItem.Name))
    End Sub

    Private Function Language() As String
        If RdVBNet.Checked Then
            Return "vb"
        ElseIf RdCS.Checked Then
            Return "cs"
        Else
            Return ""
        End If
    End Function

    Private Sub PerformAction(action As String)
        action = action.ToLower().Replace("&", "")
        Select Case action
            Case "tbbnew", "new"
                If RdVBNet.Checked Then
                    TxtScript.Text = pDefaultScript_VB
                Else
                    TxtScript.Text = pDefaultScript_CS
                End If

            Case "tbbopen", "open"
                Dim cdOpen As New OpenFileDialog
                Try
                    cdOpen.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdOpen.InitialDirectory)
                Catch
                End Try
                cdOpen.Filter = pFilter

                ' Paul Meems 3 aug 2009 Added:
                If RdVBNet.Checked Then
                    cdOpen.FilterIndex = 1
                Else
                    cdOpen.FilterIndex = 2
                End If

                cdOpen.FileName = pFileName
                If cdOpen.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    pFileName = cdOpen.FileName
                    Try
                        SaveSetting("MapWindow", "Defaults", "ScriptPath", Path.GetDirectoryName(pFileName))
                    Catch
                    End Try
                    If (cdOpen.FileName.ToLower.EndsWith("cs")) Then
                        RdCS.Checked = True
                    Else
                        RdVBNet.Checked = True
                    End If
                    IgnoreDataChange = True
                    TxtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
                    IgnoreDataChange = False
                End If

            Case "tbbsave", "save"
                Dim cdSave As New SaveFileDialog
                If RdVBNet.Checked Then
                    cdSave.Filter = pFilterVB
                Else
                    cdSave.Filter = pFilterCS
                End If
                Try
                    cdSave.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdSave.InitialDirectory)
                Catch
                End Try
                cdSave.FileName = pFileName
                If cdSave.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    pFileName = cdSave.FileName
                    Try
                        SaveSetting("MapWindow", "Defaults", "ScriptPath", Path.GetDirectoryName(pFileName))
                    Catch
                    End Try
                    MapWinUtility.Strings.SaveFileString(pFileName, TxtScript.Text)
                End If

                IgnoreDataChange = True
                IgnoreDataChange = False

            Case "tbbrun", "run script", "run"
                Run()

            Case "tbbcompile", "plugin", "plug-in", "compile plug-in", "compile plugin", "compile"
                Compile()

            Case "tbbhelp", "help"
                'Dim hlp As New frmScriptHelp
                'hlp.Icon = Me.Icon
                'hlp.ShowDialog()

            Case "tbbclose", "close"
                Close()
        End Select
    End Sub

    Private Sub Run()

        Dim errors As String = ""
        Dim args(0) As Object

        Dim sPluginFolder As String = Assembly.GetEntryAssembly().Location
        Dim dirInfo As DirectoryInfo = Directory.GetParent(sPluginFolder)
        sPluginFolder = dirInfo.FullName & "\Plugins\ScriptRunner"

        Dim assy As Assembly
        assy = MapWinUtility.Scripting.PrepareScript(Language, Nothing, TxtScript.Text, errors, sPluginFolder)
        If (assy Is Nothing) Then
            Return
        End If

        If RdPlugin.Checked Then
            Dim types As Type() = assy.GetTypes()
            For Each tmpType As Type In types
                If tmpType.IsSubclassOf(GetType(Extension)) Then
                    _appMgr.Catalog.Catalogs.Add(New AssemblyCatalog(assy))
                    Exit For
                End If
            Next
        Else
            args(0) = _appMgr
            MapWinUtility.Scripting.Run(assy, errors, args)
        End If



        If errors IsNot Nothing AndAlso errors.Trim().Length > 0 Then
            'should the logger be used here?
            'MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, resources.GetString("msgScriptError.Text"))
        End If
    End Sub

    Private Sub Compile()

        Dim cdSave As New SaveFileDialog
        Dim errors As String = ""
        Dim outPath As String = ""
        Dim assy As Assembly
        cdSave.Filter = "DLL files (*.dll)|*.dll"
        'cdSave.InitialDirectory = frmMain.Plugins.PluginFolder
        cdSave.OverwritePrompt = True
        Dim MustRename As Boolean = False

        If cdSave.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            outPath = cdSave.FileName
            If File.Exists(outPath) Then
                'Get the key, so we can turn it off and unload it:
                'Dim info As New PluginInfo()
                'info.Init(outPath, GetType(PluginInterfaces.IBasePlugin).GUID)
                'If Not info.Key = "" Then
                '    frmMain.Plugins.StopPlugin(info.Key)
                '    Dim plugin As Interfaces.IPlugin
                '    For Each plugin In frmMain.m_PluginManager.LoadedPlugins
                '        If plugin.Name = info.Name Then
                '            plugin.Terminate()
                '            plugin = Nothing
                '        End If
                '    Next
                '    'Do not scan here -- or it will immediately reload the plug-in.
                '    'frmMain.m_PluginManager.ScanPluginFolder()
                '    'frmMain.SynchPluginMenu()
                'End If
                'info = Nothing 'no Dispose on this object; mark it as nothing

                'Try
                '    System.IO.File.Delete(outPath)
                'Catch ex As Exception
                '    MustRename = True
                'End Try

                'If MustRename Then
                '    'Cannot delete the old file; the assembly is still referenced
                '    'by .NET for some reason. Since it's not loaded in MW,
                '    'just change the name and move on.
                '    For z As Integer = 1 To 500
                '        If Not System.IO.File.Exists(System.IO.Path.GetFileNameWithoutExtension(outPath) + "-" + z.ToString() + ".dll") Then
                '            outPath = System.IO.Path.GetFileNameWithoutExtension(outPath) + "-" + z.ToString() + ".dll"
                '            MapWinUtility.Logger.Msg("Notice -- The old file could not be deleted." + vbCrLf + "The newest version of your plug-in will be called: " + vbCrLf + vbCrLf + outPath + vbCrLf + vbCrLf + _
                '                    "You may need to close MapWindow and delete the old version for the new plug-in to load properly.", MsgBoxStyle.Information, "Renamed Plugin")
                '            Exit For
                '        End If
                '    Next z
                'End If
            End If
            assy = MapWinUtility.Scripting.Compile(Language, TxtScript.Text,
                                                   errors,
                                                   outPath)
            If errors.Length = 0 Then
                'frmMain.Plugins.AddFromFile(outPath)
            Else
                'jk
                MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, resources.GetString("msgScriptError.Text"))
            End If
        End If
    End Sub

    Private Sub FrmScript_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop, ToolStripContainer1.DragDrop
        Try
            If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                Dim a As Array = e.Data.GetData(DataFormats.FileDrop)
                If a.Length > 0 Then
                    If File.Exists(a(0)) Then
                        IgnoreDataChange = True
                        TxtScript.Text = MapWinUtility.Strings.WholeFileString(a(0))
                        IgnoreDataChange = False
                        e.Data.SetData(Nothing)
                        Exit Sub
                    End If
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub FrmScript_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter, ToolStripContainer1.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub FrmScript_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        TxtScript.Shutdown()
    End Sub

    Private Sub FrmScript_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtScript.AllowDrop = True
        If pFileName = "" Then
            'Load the default. Don't overwrite something already in the box
            '(a loaded saved script)

            IgnoreDataChange = True
            If File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                TxtScript.Text = MapWinUtility.Strings.WholeFileString(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
            Else
                TxtScript.Text = pDefaultScript_CS
            End If

            DataChanged = False
            ' Paul Meems 3 aug 2009 Added:
            If TxtScript.Text.IndexOf("using ") < 0 Then
                RdVBNet.Checked = True
                TxtScript.SetVb()
            Else
                RdCS.Checked = True
                TxtScript.SetCs()
            End If
            'If GetSetting("MapWindow", "LastScript", "CS", "True") = "False" Then
            '    rdVBNet.Checked = True
            '    txtScript.SetVB()
            'Else
            '    rdCS.Checked = True
            '    txtScript.SetCS()
            'End If
            IgnoreDataChange = False
        End If
        TxtScript.Init()

        'Height adjust -- internationalization seems to randomly reset the txtscript size
        TxtScript.Width = Width - 25
        TxtScript.Height = Height - 144

    End Sub

    Private Sub RdLangOrOutput_CheckedChanged(sender As Object, e As EventArgs) Handles RdVBNet.CheckedChanged, RdCS.CheckedChanged, RdPlugin.CheckedChanged, RdScript.CheckedChanged
        If IgnoreDataChange Then Exit Sub

        If DataChanged Then
            'PM
            'Dim b As MsgBoxResult = mapwinutility.logger.msg("Do you wish to save your current script first?", MsgBoxStyle.YesNo, "Save first?")
            Dim b As MsgBoxResult = MapWinUtility.Logger.Msg(resources.GetString("msgSaveCurrentScript.Text"), MsgBoxStyle.YesNo)
            If b = MsgBoxResult.Yes Then
                Dim cdSave As New SaveFileDialog With {
                    .Filter = pFilter,
                    .FileName = pFileName
                }
                If cdSave.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                    pFileName = cdSave.FileName
                    MapWinUtility.Strings.SaveFileString(pFileName, TxtScript.Text)
                End If
            End If
        End If

        Cursor = Cursors.WaitCursor

        IgnoreDataChange = True

        'Change the enabled menu buttons appropriately
        '(only allow run on scripts, only allow compile on plugins)
        MnuCompile.Enabled = Not RdScript.Checked
        MnuRun.Enabled = RdScript.Checked
        For i As Integer = 0 To Tools.Items.Count - 1
            If CStr(Tools.Items(i).Tag) = "Compile" Then
                Tools.Items(i).Enabled = Not RdScript.Checked
            End If
            If CStr(Tools.Items(i).Tag) = "Run" Then
                Tools.Items(i).Enabled = RdScript.Checked
            End If
        Next

        If RdScript.Checked Then
            If RdVBNet.Checked Then
                TxtScript.Text = pDefaultScript_VB
                TxtScript.SetVb()
            Else
                TxtScript.Text = pDefaultScript_CS
                TxtScript.SetCs()
            End If
        Else
            If RdVBNet.Checked Then
                TxtScript.Text = pDefaultPlugin_VB
                TxtScript.SetVb()
            Else
                TxtScript.Text = pDefaultPlugin_CS
                TxtScript.SetCs()
            End If
        End If

        'If not visible, loading - don't save the state yet
        If Visible Then SaveSetting("MapWindow", "LastScript", "CS", RdCS.Checked.ToString())

        Cursor = Cursors.Default

        IgnoreDataChange = False
        DataChanged = False
    End Sub

    Private Sub TxtScript_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtScript.KeyDown
        If e.KeyCode = Keys.F5 Then
            PerformAction("Run")
        End If
    End Sub

    Private Sub TxtScript_TextChanged(sender As Object, e As EventArgs)
        If IgnoreDataChange Then Exit Sub

        DataChanged = True
    End Sub

    Public Sub RunSavedScript()
        If pFileName = "" Then Exit Sub 'nothing to do
        If Not File.Exists(pFileName) Then
            'PM
            'mapwinutility.logger.msg("Warning: Ignoring the script name provided on the command line because it doesn't exist." & vbCrLf & vbCrLf & pFileName, MsgBoxStyle.Exclamation, "Nonexistant Script Passed")
            Dim sMsg = String.Format(resources.GetString("msgIgnoreScript.Text"), pFileName)
            MapWinUtility.Logger.Msg(sMsg, MsgBoxStyle.Exclamation, resources.GetString("msgScriptNotExists.Text"))
            Exit Sub
        End If

        Show()
        WindowState = FormWindowState.Minimized

        IgnoreDataChange = True
        TxtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
        IgnoreDataChange = False

        'Make it pretty
        Dim errors As String = ""
        Dim args(0) As Object
        'args(0) = frmMain
        args(0) = Nothing

        Dim sPluginFolder As String = ""
        Dim sLanguage = Path.GetExtension(pFileName)
        Dim assy As Assembly

        'MapWinUtility.Scripting.Run(System.IO.Path.GetExtension(pFileName), Nothing, txtScript.Text, errors, rdPlugin.Checked, CObj(frmMain), args)
        assy = MapWinUtility.Scripting.PrepareScript(sLanguage, Nothing, TxtScript.Text, errors, sPluginFolder)
        MapWinUtility.Scripting.Run(assy, errors, args)

        If String.IsNullOrWhiteSpace(errors) Then
            MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, "Script Error")
        End If

        Close()
    End Sub

    Private Sub ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MnuHelp.Click, MnuSubmit.Click, MnuViewRun.Click, MnuRun.Click, MnuCompile.Click, MnuNew.Click, MnuSave.Click, MnuOpen.Click, MnuClose.Click, MnuCompile.Click
        PerformAction((CType(sender, ToolStripMenuItem).Text.Replace("&", "")))
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles MnuViewRun.Click
        If Not MapWinUtility.MiscUtils.CheckInternetConnection("http://MapWindow.org/site_is_up_flag.txt", 5000) Then
            'PM
            'mapwinutility.logger.msg("There is no active internet connection, or there has been some other communications problem." + vbCrLf + vbCrLf + "Please check your connection and try again.", MsgBoxStyle.Exclamation, "No Connection")
            MapWinUtility.Logger.Msg(resources.GetString("msgNoConnection.Text"), MsgBoxStyle.Exclamation, "No Connection")
            Exit Sub
        Else
            Cursor = Cursors.WaitCursor

            'Dim onlinescriptform As New frmOnlineScriptDirectory
            'onlinescriptform.Show()

            Cursor = Cursors.Default
        End If
    End Sub

    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles MnuSubmit.Click
        If Not MapWinUtility.MiscUtils.CheckInternetConnection("http://MapWindow.org/site_is_up_flag.txt", 5000) Then
            'PM
            'mapwinutility.logger.msg("There is no active internet connection, or there has been some other communications problem." + vbCrLf + vbCrLf + "Please check your connection and try again.", MsgBoxStyle.Exclamation, "No Connection")
            MapWinUtility.Logger.Msg(resources.GetString("msgNoConnection.Text"), MsgBoxStyle.Exclamation, resources.GetString("titleNoConnection.Text"))
            Exit Sub
        Else
            'Dim onlinescriptform As New frmOnlineScriptSubmit
            'onlinescriptform.Show()
        End If
    End Sub

    Private Sub FrmScript_Closing(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not TxtScript.Text = "" Then
            SaveSetting("MapWindow", "LastScript", "CS", RdCS.Checked.ToString())
            If File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                Kill(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
            End If

            MapWinUtility.Strings.SaveFileString(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat", TxtScript.Text)
        End If
    End Sub

    Private Sub FrmScript_LocationChanged(sender As Object, e As EventArgs) Handles Me.LocationChanged

    End Sub

End Class