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
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
Imports System.Threading
Imports System.ComponentModel.Composition.Hosting

Imports DotSpatial.Controls


Namespace MapWindowDS.ScriptRunner

    Public Class frmScript
        'Inherits MapWindow.baseForm '5/5/2008 jk
        Inherits System.Windows.Forms.Form

        Private _appMgr As AppManager

#Region "Declarations"
        'changed by Jiri Kadlec
        Private resources As System.Resources.ResourceManager = _
        New System.Resources.ResourceManager("MapWindow.GlobalResource", System.Reflection.Assembly.GetExecutingAssembly())
        'Public Declare Function LockWindowUpdate Lib "user32" (ByVal hWnd As Integer) As Integer
#End Region

        Private pDefaultScript_VB As String = _
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

        Private pDefaultPlugin_VB As String = _
                "Imports DotSpatial.Controls" & vbCrLf _
              & "Imports DotSpatial.Controls.Header" & vbCrLf _
              & "Imports System.Windows.Forms" & vbCrLf _
              & "Imports Microsoft.VisualBasic" & vbCrLf _
              & "Imports System" & vbCrLf & _
              vbCrLf & _
              vbCrLf & _
            "Public Class MyPlugin" & vbCrLf & _
            "    Inherits Extension" & vbCrLf & _
            "" + vbCrLf & _
            "    Dim host As AppManager" & vbCrLf & _
            "    Dim btnSample As ToolStripButton" & vbCrLf & _
              vbCrLf & _
            "    Public Sub New()" & vbCrLf & _
            "    End Sub" & vbCrLf & _
            "" & vbCrLf & _
            "   Public Overrides Sub Activate()" & vbCrLf & _
            "" & vbCrLf & _
            "       MyBase.Activate()" & vbCrLf & _
            "       host = Me.App" & vbCrLf & _
            "       Dim SamplePluginMenuKey as String = ""kSampleScriptPlugin""" + vbCrLf & _
            "       Dim samplePlugin as String = ""Sample Script Plugin""" + vbCrLf & _
            "" & vbCrLf & _
            "       host.HeaderControl.Add(New RootItem(SamplePluginMenuKey, samplePlugin))" & vbCrLf & _
            "       host.HeaderControl.Add(New SimpleActionItem(SamplePluginMenuKey, samplePlugin,  New EventHandler(AddressOf btnSample_Click))" & vbCrLf & _
            "" & vbCrLf & _
            "   End Sub" & vbCrLf & _
            vbCrLf & _
            "   Public Overrides Sub Deactivate()" & vbCrLf & _
            "           host.HeaderControl.RemoveAll()" & vbCrLf & _
            "           MyBase.Deactivate()" & vbCrLf & _
            "   End Sub" & vbCrLf & _
            vbCrLf & _
            vbCrLf & _
            "   Private Sub btnSample_Click(ByVal sender As Object, ByVal e As EventArgs)" & vbCrLf & _
            "       MessageBox.Show(""Hello World"")" & vbCrLf & _
            "   End Sub" & vbCrLf & _
            "End Class"


        Private pFilter As String = "VB.net (*.vb)|*.vb|C#.net (*.cs)|*.cs|All files|*.*"
        Private pFilterVB As String = "VB.net (*.vb)|*.vb|All files|*.*"
        Private pFilterCS As String = "C#.net (*.cs)|*.cs|All files|*.*"

        Public pFileName As String = ""

        Private DataChanged As Boolean = False
        Private IgnoreDataChange As Boolean = False

        Private pDefaultScript_CS As String = _
                "using DotSpatial.Controls;" & vbCrLf _
              & "using System.Windows.Forms;" & vbCrLf _
              & "using Microsoft.VisualBasic;" & vbCrLf _
              & "using System;" & vbCrLf & _
              vbCrLf & _
            "namespace MyNamespace" + vbCrLf & _
            "{" + vbCrLf & _
            "   // You must change the name of this class to something unique!" & vbCrLf & _
            "   class MyExample" + vbCrLf & _
            "   {" + vbCrLf & _
            "       public static void ScriptMain(AppManager appMgr)" + vbCrLf & _
            "       {    " + vbCrLf & _
            "           MessageBox.Show(""This is a simple script to display the number of loaded layers in the map: "" + appMgr.Map.Layers.Count);" + vbCrLf & _
            "       }" + vbCrLf & _
            "   }" & vbCrLf & _
            "}" & vbCrLf
        Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
        Friend WithEvents tbbNew As System.Windows.Forms.ToolStripButton
        Friend WithEvents tbbOpen As System.Windows.Forms.ToolStripButton
        Friend WithEvents tbbSave As System.Windows.Forms.ToolStripButton
        Friend WithEvents tbbsep As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents tbbRun As System.Windows.Forms.ToolStripButton
        Friend WithEvents tbbCompile As System.Windows.Forms.ToolStripButton
        Friend WithEvents tbbHelp As System.Windows.Forms.ToolStripButton
        Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuNew As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuOpen As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuSave As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuClose As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuRun As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuCompile As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuViewRun As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuSubmit As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents mnuHelp As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
        Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
        Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
        Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
        Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
        Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
        Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
        Friend WithEvents rdScript As System.Windows.Forms.RadioButton
        Friend WithEvents rdPlugin As System.Windows.Forms.RadioButton
        Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
        Friend WithEvents rdVBNet As System.Windows.Forms.RadioButton
        Friend WithEvents rdCS As System.Windows.Forms.RadioButton
        Friend WithEvents mnuSep As System.Windows.Forms.ToolStripSeparator
        <CLSCompliant(False)> _
        Public WithEvents txtScript As CSharpEditor.MainForm

        Private pDefaultPlugin_CS As String = _
                "using DotSpatial.Controls;" & vbCrLf _
              & "using DotSpatial.Controls.Header;" & vbCrLf _
              & "using System.Windows.Forms;" & vbCrLf _
              & "using Microsoft.VisualBasic;" & vbCrLf _
              & "using System;" & vbCrLf & _
            "" & vbCrLf & _
            "namespace MyNamespace" & vbCrLf & _
            "{" + vbCrLf & _
            "	public class MyExample : Extension" & vbCrLf & _
            "	{" + vbCrLf & _
            "       private AppManager _host;" + vbCrLf & _
            "" + vbCrLf & _
            "		// Change this to match the name of your class. This is the constructor." + vbCrLf & _
            "		public MyExample()" + vbCrLf & _
            "		{" + vbCrLf & _
            "" + vbCrLf & _
            "		}" + vbCrLf & _
            "" + vbCrLf & _
            vbCrLf & _
            "       public override void Activate()" & vbCrLf & _
            "       {" + vbCrLf & _
            "" + vbCrLf & _
            "           base.Activate();" + vbCrLf & _
            "           _host = App;" + vbCrLf & _
            "           string SamplePluginMenuKey = ""kSampleScriptPlugin"";" + vbCrLf & _
            "           string samplePlugin = ""Sample Scripts Plugin"";" + vbCrLf & _
            "            _host.HeaderControl.Add(new RootItem(SamplePluginMenuKey, samplePlugin));" & vbCrLf & _
            "            _host.HeaderControl.Add(new SimpleActionItem(SamplePluginMenuKey, samplePlugin,  btnSample_Click));" & vbCrLf & _
            "" & vbCrLf & _
            "       }" + vbCrLf & _
            vbCrLf & _
            "       public override void Deactivate()" & vbCrLf & _
            "       {" & vbCrLf & _
            "           _host.HeaderControl.RemoveAll();" & vbCrLf & _
            "           base.Deactivate();" & vbCrLf & _
            "       }" & vbCrLf & _
            vbCrLf & _
            "       private void btnSample_Click(object sender, EventArgs e )" & vbCrLf & _
            "       {" & vbCrLf & _
            "           MessageBox.Show(""Hello World"");" & vbCrLf & _
            "       }" & vbCrLf & _
            "   }" & vbCrLf & _
            "}"

#Region " Windows Form Designer generated code "

        Public Sub New(ByVal appManager As AppManager)
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            _appMgr = appManager

            'May/12/2008 Jiri Kadlec - load icon from shared resources to reduce size of the program
            'Me.Icon = My.Resources.MapWindow_new
        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Friend WithEvents tools As System.Windows.Forms.ToolStrip
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScript))
            Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer()
            Me.txtScript = New CSharpEditor.MainForm()
            Me.GroupBox2 = New System.Windows.Forms.GroupBox()
            Me.rdScript = New System.Windows.Forms.RadioButton()
            Me.rdPlugin = New System.Windows.Forms.RadioButton()
            Me.GroupBox1 = New System.Windows.Forms.GroupBox()
            Me.rdVBNet = New System.Windows.Forms.RadioButton()
            Me.rdCS = New System.Windows.Forms.RadioButton()
            Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuNew = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuOpen = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuSave = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuSep = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuClose = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuRun = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuCompile = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuViewRun = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuSubmit = New System.Windows.Forms.ToolStripMenuItem()
            Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
            Me.tools = New System.Windows.Forms.ToolStrip()
            Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
            Me.tbbNew = New System.Windows.Forms.ToolStripButton()
            Me.tbbOpen = New System.Windows.Forms.ToolStripButton()
            Me.tbbSave = New System.Windows.Forms.ToolStripButton()
            Me.tbbsep = New System.Windows.Forms.ToolStripSeparator()
            Me.tbbRun = New System.Windows.Forms.ToolStripButton()
            Me.tbbCompile = New System.Windows.Forms.ToolStripButton()
            Me.tbbHelp = New System.Windows.Forms.ToolStripButton()
            Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel()
            Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel()
            Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel()
            Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel()
            Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
            Me.ToolStripContainer1.ContentPanel.SuspendLayout()
            Me.ToolStripContainer1.TopToolStripPanel.SuspendLayout()
            Me.ToolStripContainer1.SuspendLayout()
            Me.GroupBox2.SuspendLayout()
            Me.GroupBox1.SuspendLayout()
            Me.MenuStrip1.SuspendLayout()
            Me.tools.SuspendLayout()
            Me.SuspendLayout()
            '
            'ToolStripContainer1
            '
            Me.ToolStripContainer1.AllowDrop = True
            '
            'ToolStripContainer1.ContentPanel
            '
            Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.txtScript)
            Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.GroupBox2)
            Me.ToolStripContainer1.ContentPanel.Controls.Add(Me.GroupBox1)
            resources.ApplyResources(Me.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel")
            resources.ApplyResources(Me.ToolStripContainer1, "ToolStripContainer1")
            Me.ToolStripContainer1.Name = "ToolStripContainer1"
            '
            'ToolStripContainer1.TopToolStripPanel
            '
            Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.MenuStrip1)
            Me.ToolStripContainer1.TopToolStripPanel.Controls.Add(Me.tools)
            '
            'txtScript
            '
            resources.ApplyResources(Me.txtScript, "txtScript")
            Me.txtScript.Name = "txtScript"
            '
            'GroupBox2
            '
            Me.GroupBox2.Controls.Add(Me.rdScript)
            Me.GroupBox2.Controls.Add(Me.rdPlugin)
            resources.ApplyResources(Me.GroupBox2, "GroupBox2")
            Me.GroupBox2.Name = "GroupBox2"
            Me.GroupBox2.TabStop = False
            '
            'rdScript
            '
            Me.rdScript.Checked = True
            resources.ApplyResources(Me.rdScript, "rdScript")
            Me.rdScript.Name = "rdScript"
            Me.rdScript.TabStop = True
            '
            'rdPlugin
            '
            resources.ApplyResources(Me.rdPlugin, "rdPlugin")
            Me.rdPlugin.Name = "rdPlugin"
            '
            'GroupBox1
            '
            Me.GroupBox1.Controls.Add(Me.rdVBNet)
            Me.GroupBox1.Controls.Add(Me.rdCS)
            resources.ApplyResources(Me.GroupBox1, "GroupBox1")
            Me.GroupBox1.Name = "GroupBox1"
            Me.GroupBox1.TabStop = False
            '
            'rdVBNet
            '
            Me.rdVBNet.Checked = True
            resources.ApplyResources(Me.rdVBNet, "rdVBNet")
            Me.rdVBNet.Name = "rdVBNet"
            Me.rdVBNet.TabStop = True
            '
            'rdCS
            '
            resources.ApplyResources(Me.rdCS, "rdCS")
            Me.rdCS.Name = "rdCS"
            '
            'MenuStrip1
            '
            resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
            Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ToolStripMenuItem6, Me.ToolStripMenuItem2, Me.mnuHelp})
            Me.MenuStrip1.Name = "MenuStrip1"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNew, Me.mnuOpen, Me.mnuSave, Me.mnuSep, Me.mnuClose})
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
            '
            'mnuNew
            '
            Me.mnuNew.Name = "mnuNew"
            resources.ApplyResources(Me.mnuNew, "mnuNew")
            '
            'mnuOpen
            '
            Me.mnuOpen.Name = "mnuOpen"
            resources.ApplyResources(Me.mnuOpen, "mnuOpen")
            '
            'mnuSave
            '
            Me.mnuSave.Name = "mnuSave"
            resources.ApplyResources(Me.mnuSave, "mnuSave")
            '
            'mnuSep
            '
            Me.mnuSep.Name = "mnuSep"
            resources.ApplyResources(Me.mnuSep, "mnuSep")
            '
            'mnuClose
            '
            Me.mnuClose.Name = "mnuClose"
            resources.ApplyResources(Me.mnuClose, "mnuClose")
            '
            'ToolStripMenuItem6
            '
            Me.ToolStripMenuItem6.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRun, Me.mnuCompile})
            Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
            resources.ApplyResources(Me.ToolStripMenuItem6, "ToolStripMenuItem6")
            '
            'mnuRun
            '
            Me.mnuRun.Name = "mnuRun"
            resources.ApplyResources(Me.mnuRun, "mnuRun")
            '
            'mnuCompile
            '
            Me.mnuCompile.Name = "mnuCompile"
            resources.ApplyResources(Me.mnuCompile, "mnuCompile")
            '
            'ToolStripMenuItem2
            '
            Me.ToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuViewRun, Me.mnuSubmit})
            Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
            resources.ApplyResources(Me.ToolStripMenuItem2, "ToolStripMenuItem2")
            '
            'mnuViewRun
            '
            Me.mnuViewRun.Name = "mnuViewRun"
            resources.ApplyResources(Me.mnuViewRun, "mnuViewRun")
            '
            'mnuSubmit
            '
            Me.mnuSubmit.Name = "mnuSubmit"
            resources.ApplyResources(Me.mnuSubmit, "mnuSubmit")
            '
            'mnuHelp
            '
            Me.mnuHelp.Name = "mnuHelp"
            resources.ApplyResources(Me.mnuHelp, "mnuHelp")
            '
            'tools
            '
            resources.ApplyResources(Me.tools, "tools")
            Me.tools.ImageList = Me.ImageList1
            Me.tools.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tbbNew, Me.tbbOpen, Me.tbbSave, Me.tbbsep, Me.tbbRun, Me.tbbCompile, Me.tbbHelp})
            Me.tools.Name = "tools"
            '
            'ImageList1
            '
            Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
            Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
            Me.ImageList1.Images.SetKeyName(0, "")
            Me.ImageList1.Images.SetKeyName(1, "")
            Me.ImageList1.Images.SetKeyName(2, "")
            Me.ImageList1.Images.SetKeyName(3, "")
            Me.ImageList1.Images.SetKeyName(4, "")
            Me.ImageList1.Images.SetKeyName(5, "")
            '
            'tbbNew
            '
            resources.ApplyResources(Me.tbbNew, "tbbNew")
            Me.tbbNew.Name = "tbbNew"
            '
            'tbbOpen
            '
            resources.ApplyResources(Me.tbbOpen, "tbbOpen")
            Me.tbbOpen.Name = "tbbOpen"
            '
            'tbbSave
            '
            resources.ApplyResources(Me.tbbSave, "tbbSave")
            Me.tbbSave.Name = "tbbSave"
            '
            'tbbsep
            '
            Me.tbbsep.Name = "tbbsep"
            resources.ApplyResources(Me.tbbsep, "tbbsep")
            '
            'tbbRun
            '
            resources.ApplyResources(Me.tbbRun, "tbbRun")
            Me.tbbRun.Name = "tbbRun"
            '
            'tbbCompile
            '
            resources.ApplyResources(Me.tbbCompile, "tbbCompile")
            Me.tbbCompile.Name = "tbbCompile"
            '
            'tbbHelp
            '
            resources.ApplyResources(Me.tbbHelp, "tbbHelp")
            Me.tbbHelp.Name = "tbbHelp"
            '
            'BottomToolStripPanel
            '
            resources.ApplyResources(Me.BottomToolStripPanel, "BottomToolStripPanel")
            Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
            Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
            Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            '
            'TopToolStripPanel
            '
            resources.ApplyResources(Me.TopToolStripPanel, "TopToolStripPanel")
            Me.TopToolStripPanel.Name = "TopToolStripPanel"
            Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
            Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            '
            'RightToolStripPanel
            '
            resources.ApplyResources(Me.RightToolStripPanel, "RightToolStripPanel")
            Me.RightToolStripPanel.Name = "RightToolStripPanel"
            Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
            Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            '
            'LeftToolStripPanel
            '
            resources.ApplyResources(Me.LeftToolStripPanel, "LeftToolStripPanel")
            Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
            Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
            Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            '
            'ContentPanel
            '
            resources.ApplyResources(Me.ContentPanel, "ContentPanel")
            '
            'frmScript
            '
            Me.AllowDrop = True
            resources.ApplyResources(Me, "$this")
            Me.Controls.Add(Me.ToolStripContainer1)
            Me.Name = "frmScript"
            Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
            Me.ToolStripContainer1.TopToolStripPanel.ResumeLayout(False)
            Me.ToolStripContainer1.TopToolStripPanel.PerformLayout()
            Me.ToolStripContainer1.ResumeLayout(False)
            Me.ToolStripContainer1.PerformLayout()
            Me.GroupBox2.ResumeLayout(False)
            Me.GroupBox1.ResumeLayout(False)
            Me.MenuStrip1.ResumeLayout(False)
            Me.MenuStrip1.PerformLayout()
            Me.tools.ResumeLayout(False)
            Me.tools.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Sub tools_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tools.ItemClicked
            PerformAction(CStr(e.ClickedItem.Name))
        End Sub

        Private Function Language() As String
            If rdVBNet.Checked Then
                Return "vb"
            ElseIf rdCS.Checked Then
                Return "cs"
            Else
                Return ""
            End If
        End Function

        Private Sub PerformAction(ByVal action As String)
            action = action.ToLower().Replace("&", "")
            Select Case action
                Case "tbbnew", "new"
                    If rdVBNet.Checked Then
                        txtScript.Text = pDefaultScript_VB
                    Else
                        txtScript.Text = pDefaultScript_CS
                    End If

                Case "tbbopen", "open"
                    Dim cdOpen As New Windows.Forms.OpenFileDialog
                    Try
                        cdOpen.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdOpen.InitialDirectory)
                    Catch
                    End Try
                    cdOpen.Filter = pFilter

                    ' Paul Meems 3 aug 2009 Added:
                    If rdVBNet.Checked Then
                        cdOpen.FilterIndex = 1
                    Else
                        cdOpen.FilterIndex = 2
                    End If

                    cdOpen.FileName = pFileName
                    If cdOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        pFileName = cdOpen.FileName
                        Try
                            SaveSetting("MapWindow", "Defaults", "ScriptPath", System.IO.Path.GetDirectoryName(pFileName))
                        Catch
                        End Try
                        If (cdOpen.FileName.ToLower.EndsWith("cs")) Then
                            rdCS.Checked = True
                        Else
                            rdVBNet.Checked = True
                        End If
                        IgnoreDataChange = True
                        txtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
                        IgnoreDataChange = False
                    End If

                Case "tbbsave", "save"
                    Dim cdSave As New Windows.Forms.SaveFileDialog
                    If rdVBNet.Checked Then
                        cdSave.Filter = pFilterVB
                    Else
                        cdSave.Filter = pFilterCS
                    End If
                    Try
                        cdSave.InitialDirectory = GetSetting("MapWindow", "Defaults", "ScriptPath", cdSave.InitialDirectory)
                    Catch
                    End Try
                    cdSave.FileName = pFileName
                    If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        pFileName = cdSave.FileName
                        Try
                            SaveSetting("MapWindow", "Defaults", "ScriptPath", System.IO.Path.GetDirectoryName(pFileName))
                        Catch
                        End Try
                        MapWinUtility.Strings.SaveFileString(pFileName, txtScript.Text)
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
                    Me.Close()
            End Select
        End Sub

        Private Sub Run()

            Dim errors As String = ""
            Dim args(0) As Object

            Dim sPluginFolder As String = Assembly.GetEntryAssembly().Location
            Dim dirInfo As DirectoryInfo = Directory.GetParent(sPluginFolder)
            sPluginFolder = dirInfo.FullName & "\Plugins\ScriptRunner"

            Dim assy As Assembly
            assy = MapWinUtility.Scripting.PrepareScript(Language, Nothing, txtScript.Text, errors, sPluginFolder)
            If (assy Is Nothing) Then
                Return
            End If

            If rdPlugin.Checked Then
                Dim types As Type() = assy.GetTypes()
                For Each tmpType As Type In types
                    If tmpType.IsSubclassOf(GetType(DotSpatial.Controls.Extension)) Then
                        _appMgr.Catalog.Catalogs.Add(New AssemblyCatalog(assy))
                        Exit For
                    End If
                Next
            Else
                args(0) = _appMgr
                MapWinUtility.Scripting.Run(assy, errors, args)
            End If



            If Not errors Is Nothing AndAlso errors.Trim().Length > 0 Then
                'should the logger be used here?
                'MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, resources.GetString("msgScriptError.Text"))
            End If
        End Sub

        Private Sub Compile()

            Dim cdSave As New Windows.Forms.SaveFileDialog
            Dim errors As String = ""
            Dim outPath As String = ""
            Dim assy As System.Reflection.Assembly
            cdSave.Filter = "DLL files (*.dll)|*.dll"
            'cdSave.InitialDirectory = frmMain.Plugins.PluginFolder
            cdSave.OverwritePrompt = True
            Dim MustRename As Boolean = False

            If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                outPath = cdSave.FileName
                If System.IO.File.Exists(outPath) Then
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
                assy = MapWinUtility.Scripting.Compile(Language, txtScript.Text, _
                              errors, _
                              outPath)
                If errors.Length = 0 Then
                    'frmMain.Plugins.AddFromFile(outPath)
                Else
                    'jk
                    MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, resources.GetString("msgScriptError.Text"))
                End If
            End If
        End Sub

        Private Sub frmScript_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragDrop, ToolStripContainer1.DragDrop
            Try
                If e.Data.GetDataPresent(DataFormats.FileDrop) Then
                    Dim a As Array = e.Data.GetData(DataFormats.FileDrop)
                    If a.Length > 0 Then
                        If System.IO.File.Exists(a(0)) Then
                            IgnoreDataChange = True
                            txtScript.Text = MapWinUtility.Strings.WholeFileString(a(0))
                            IgnoreDataChange = False
                            e.Data.SetData(Nothing)
                            Exit Sub
                        End If
                    End If
                End If
            Catch
            End Try
        End Sub

        Private Sub frmScript_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles Me.DragEnter, ToolStripContainer1.DragEnter
            e.Effect = DragDropEffects.Copy
        End Sub

        Private Sub frmScript_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            txtScript.Shutdown()
        End Sub

        Private Sub frmScript_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            txtScript.AllowDrop = True
            If pFileName = "" Then
                'Load the default. Don't overwrite something already in the box
                '(a loaded saved script)

                IgnoreDataChange = True
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                    txtScript.Text = MapWinUtility.Strings.WholeFileString(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
                Else
                    txtScript.Text = pDefaultScript_CS
                End If

                DataChanged = False
                ' Paul Meems 3 aug 2009 Added:
                If txtScript.Text.IndexOf("using ") < 0 Then
                    rdVBNet.Checked = True
                    txtScript.SetVB()
                Else
                    rdCS.Checked = True
                    txtScript.SetCS()
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
            txtScript.Init()

            'Height adjust -- internationalization seems to randomly reset the txtscript size
            txtScript.Width = Me.Width - 25
            txtScript.Height = Me.Height - 144

        End Sub

        Private Sub rdLangOrOutput_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdVBNet.CheckedChanged, rdCS.CheckedChanged, rdPlugin.CheckedChanged, rdScript.CheckedChanged
            If IgnoreDataChange Then Exit Sub

            If DataChanged Then
                'PM
                'Dim b As MsgBoxResult = mapwinutility.logger.msg("Do you wish to save your current script first?", MsgBoxStyle.YesNo, "Save first?")
                Dim b As MsgBoxResult = MapWinUtility.Logger.Msg(resources.GetString("msgSaveCurrentScript.Text"), MsgBoxStyle.YesNo)
                If b = MsgBoxResult.Yes Then
                    Dim cdSave As New Windows.Forms.SaveFileDialog
                    cdSave.Filter = pFilter
                    cdSave.FileName = pFileName
                    If cdSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        pFileName = cdSave.FileName
                        MapWinUtility.Strings.SaveFileString(pFileName, txtScript.Text)
                    End If
                End If
            End If

            Me.Cursor = Cursors.WaitCursor

            IgnoreDataChange = True

            'Change the enabled menu buttons appropriately
            '(only allow run on scripts, only allow compile on plugins)
            mnuCompile.Enabled = Not rdScript.Checked
            mnuRun.Enabled = rdScript.Checked
            For i As Integer = 0 To tools.Items.Count - 1
                If CStr(tools.Items(i).Tag) = "Compile" Then
                    tools.Items(i).Enabled = Not rdScript.Checked
                End If
                If CStr(tools.Items(i).Tag) = "Run" Then
                    tools.Items(i).Enabled = rdScript.Checked
                End If
            Next

            If rdScript.Checked Then
                If rdVBNet.Checked Then
                    txtScript.Text = pDefaultScript_VB
                    txtScript.SetVB()
                Else
                    txtScript.Text = pDefaultScript_CS
                    txtScript.SetCS()
                End If
            Else
                If rdVBNet.Checked Then
                    txtScript.Text = pDefaultPlugin_VB
                    txtScript.SetVB()
                Else
                    txtScript.Text = pDefaultPlugin_CS
                    txtScript.SetCS()
                End If
            End If

            'If not visible, loading - don't save the state yet
            If (Me.Visible) Then SaveSetting("MapWindow", "LastScript", "CS", rdCS.Checked.ToString())

            Me.Cursor = Cursors.Default

            IgnoreDataChange = False
            DataChanged = False
        End Sub

        Private Sub txtScript_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtScript.KeyDown
            If e.KeyCode = Keys.F5 Then
                PerformAction("Run")
            End If
        End Sub

        Private Sub txtScript_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            If IgnoreDataChange Then Exit Sub

            DataChanged = True
        End Sub

        Public Sub RunSavedScript()
            If pFileName = "" Then Exit Sub 'nothing to do
            If Not System.IO.File.Exists(pFileName) Then
                'PM
                'mapwinutility.logger.msg("Warning: Ignoring the script name provided on the command line because it doesn't exist." & vbCrLf & vbCrLf & pFileName, MsgBoxStyle.Exclamation, "Nonexistant Script Passed")
                Dim sMsg = String.Format(resources.GetString("msgIgnoreScript.Text"), pFileName)
                MapWinUtility.Logger.Msg(sMsg, MsgBoxStyle.Exclamation, resources.GetString("msgScriptNotExists.Text"))
                Exit Sub
            End If

            Me.Show()
            Me.WindowState = FormWindowState.Minimized

            IgnoreDataChange = True
            txtScript.Text = MapWinUtility.Strings.WholeFileString(pFileName)
            IgnoreDataChange = False

            'Make it pretty
            Dim errors As String = ""
            Dim args(0) As Object
            'args(0) = frmMain
            args(0) = Nothing

            Dim sPluginFolder As String = ""
            Dim sLanguage = System.IO.Path.GetExtension(pFileName)
            Dim assy As Assembly

            'MapWinUtility.Scripting.Run(System.IO.Path.GetExtension(pFileName), Nothing, txtScript.Text, errors, rdPlugin.Checked, CObj(frmMain), args)
            assy = MapWinUtility.Scripting.PrepareScript(sLanguage, Nothing, txtScript.Text, errors, sPluginFolder)
            MapWinUtility.Scripting.Run(assy, errors, args)

            If Not errors Is Nothing And Not errors.Trim() = "" Then
                MapWinUtility.Logger.Msg(errors, MsgBoxStyle.Exclamation, "Script Error")
            End If

            Me.Close()
        End Sub

        Private Sub ToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelp.Click, mnuSubmit.Click, mnuViewRun.Click, mnuRun.Click, mnuCompile.Click, mnuNew.Click, mnuSave.Click, mnuOpen.Click, mnuClose.Click, mnuCompile.Click
            PerformAction((CType(sender, System.Windows.Forms.ToolStripMenuItem).Text.Replace("&", "")))
        End Sub

        Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuViewRun.Click
            If Not MapWinUtility.MiscUtils.CheckInternetConnection("http://MapWindow.org/site_is_up_flag.txt", 5000) Then
                'PM
                'mapwinutility.logger.msg("There is no active internet connection, or there has been some other communications problem." + vbCrLf + vbCrLf + "Please check your connection and try again.", MsgBoxStyle.Exclamation, "No Connection")
                MapWinUtility.Logger.Msg(resources.GetString("msgNoConnection.Text"), MsgBoxStyle.Exclamation, "No Connection")
                Exit Sub
            Else
                Me.Cursor = Cursors.WaitCursor

                'Dim onlinescriptform As New frmOnlineScriptDirectory
                'onlinescriptform.Show()

                Me.Cursor = Cursors.Default
            End If
        End Sub

        Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSubmit.Click
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

        Private Sub frmScript_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
            If Not txtScript.Text = "" Then
                SaveSetting("MapWindow", "LastScript", "CS", rdCS.Checked.ToString())
                If System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat") Then
                    Kill(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat")
                End If

                MapWinUtility.Strings.SaveFileString(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) & "\mw_LastScript.dat", txtScript.Text)
            End If
        End Sub

        Private Sub frmScript_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

        End Sub

    End Class

End Namespace
