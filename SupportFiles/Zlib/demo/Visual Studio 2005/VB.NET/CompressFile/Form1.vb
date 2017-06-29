Public Class Form1
    Inherits System.Windows.Forms.Form
    ' Fields
    Private button1 As Button
    Private button2 As Button
    Private linkLabel1 As LinkLabel
    Private openFileDialog1 As OpenFileDialog
    Private saveFileDialog1 As SaveFileDialog

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.button1 = New System.Windows.Forms.Button
        Me.button2 = New System.Windows.Forms.Button
        Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.linkLabel1 = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'button1
        '
        Me.button1.Location = New System.Drawing.Point(40, 24)
        Me.button1.Name = "button1"
        Me.button1.Size = New System.Drawing.Size(128, 32)
        Me.button1.TabIndex = 0
        Me.button1.Text = "Compress file..."
        AddHandler Me.button1.Click, New EventHandler(AddressOf Me.button1_Click)

        '
        'button2
        '
        Me.button2.Location = New System.Drawing.Point(216, 24)
        Me.button2.Name = "button2"
        Me.button2.Size = New System.Drawing.Size(112, 32)
        Me.button2.TabIndex = 1
        Me.button2.Text = "Decompress file..."
        AddHandler Me.button2.Click, New EventHandler(AddressOf Me.button2_Click)

        '
        'linkLabel1
        '
        Me.linkLabel1.Location = New System.Drawing.Point(104, 72)
        Me.linkLabel1.Name = "linkLabel1"
        Me.linkLabel1.Size = New System.Drawing.Size(168, 23)
        Me.linkLabel1.TabIndex = 2
        Me.linkLabel1.TabStop = True
        Me.linkLabel1.Text = "http://www.componentace.com"
        AddHandler Me.linkLabel1.LinkClicked, New LinkLabelLinkClickedEventHandler(AddressOf Me.linkLabel1_LinkClicked)

        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(368, 101)
        Me.Controls.Add(Me.linkLabel1)
        Me.Controls.Add(Me.button2)
        Me.Controls.Add(Me.button1)
        Me.Name = "Form1"
        Me.Text = "CompressFile demo (c) ComponentAce 2006"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Shared Sub CopyStream(ByRef input As System.IO.Stream, ByRef output As System.IO.Stream)
        Dim num1 As Integer
        Dim buffer1 As Byte() = New Byte(2000 - 1) {}
        num1 = input.Read(buffer1, 0, 2000)
        Do While (num1 > 0)
            output.Write(buffer1, 0, num1)
            num1 = input.Read(buffer1, 0, 2000)
        Loop
        output.Flush()
    End Sub


    Private Sub compressFile(ByVal inFile As String, ByVal outFile As String)
        Dim outFileStream As New System.IO.FileStream(outFile, System.IO.FileMode.Create)
        Dim zStream As New zlib.ZOutputStream(outFileStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION)
        Dim inFileStream As New System.IO.FileStream(inFile, System.IO.FileMode.Open)
        Try
            Form1.CopyStream(inFileStream, zStream)
        Finally
            zStream.Close()
            outFileStream.Close()
            inFileStream.Close()
        End Try
    End Sub

    Private Sub decompressFile(ByVal inFile As String, ByVal outFile As String)
        Dim outFileStream As New System.IO.FileStream(outFile, System.IO.FileMode.Create)
        Dim zStream As New zlib.ZOutputStream(outFileStream)
        Dim inFileStream As New System.IO.FileStream(inFile, System.IO.FileMode.Open)
        Try
            Form1.CopyStream(inFileStream, zStream)
        Finally
            zStream.Close()
            outFileStream.Close()
            inFileStream.Close()
        End Try
    End Sub








    Private Sub button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.openFileDialog1.Title = "Select a file to compress"
        If (Me.openFileDialog1.ShowDialog = DialogResult.OK) Then
            Me.saveFileDialog1.Title = "Save compressed file to"
            Me.saveFileDialog1.FileName = (Me.openFileDialog1.FileName & ".compressed")
            If (Me.saveFileDialog1.ShowDialog = DialogResult.OK) Then
                Me.compressFile(Me.openFileDialog1.FileName, Me.saveFileDialog1.FileName)
            End If
        End If
    End Sub

    Private Sub button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.openFileDialog1.Title = "Select a file to decompress"
        If (Me.openFileDialog1.ShowDialog = DialogResult.OK) Then
            Me.saveFileDialog1.Title = "Save decompressed file to"
            Me.saveFileDialog1.FileName = (Me.openFileDialog1.FileName & ".decompressed")
            If (Me.saveFileDialog1.ShowDialog = DialogResult.OK) Then
                Me.decompressFile(Me.openFileDialog1.FileName, Me.saveFileDialog1.FileName)
            End If
        End If
    End Sub



    Private Sub linkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Process.Start("http://www.componentace.com")

    End Sub
End Class
