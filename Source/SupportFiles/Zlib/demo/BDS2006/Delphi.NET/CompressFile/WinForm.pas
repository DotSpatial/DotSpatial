unit WinForm;

interface

uses
  System.Drawing, System.Collections, System.ComponentModel,
  System.Windows.Forms, System.Data, System.IO, zlib;

type
  TWinForm = class(System.Windows.Forms.Form)
  {$REGION 'Designer Managed Code'}
  strict private
    /// <summary>
    /// Required designer variable.
    /// </summary>
    Components: System.ComponentModel.Container;
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    // Methods

      strict private procedure button1_Click(sender: TObject; e: EventArgs);
      strict private procedure button2_Click(sender: TObject; e: EventArgs);
      strict private procedure compressFile(inFile: string; outFile: string);
      public class procedure CopyStream(input: Stream; output: Stream); static;
      strict private procedure decompressFile(inFile: string; outFile: string);
      strict protected procedure Dispose(disposing: boolean); override;
      strict private procedure Form1_Load(sender: TObject; e: EventArgs);
      strict private procedure InitializeComponent;
      strict private procedure linkLabel1_LinkClicked(sender: TObject; e: LinkLabelLinkClickedEventArgs);
      // Fields
      strict private button1: Button;
      strict private button2: Button;
      strict private linkLabel1: LinkLabel;
      strict private openFileDialog1: OpenFileDialog;
      strict private saveFileDialog1: SaveFileDialog;



    procedure TWinForm_Load(sender: System.Object; e: System.EventArgs);
  {$ENDREGION}
  strict protected
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>

  private
    { Private Declarations }
  public
    constructor Create;
  end;

  [assembly: RuntimeRequiredAttribute(TypeOf(TWinForm))]

implementation

{$AUTOBOX ON}

{$REGION 'Windows Form Designer generated code'}
/// <summary>
/// Required method for Designer support -- do not modify
/// the contents of this method with the code editor.
/// </summary>
procedure TWinForm.InitializeComponent;
begin
      self.button1 := Button.Create;
      self.button2 := Button.Create;
      self.openFileDialog1 := OpenFileDialog.Create;
      self.saveFileDialog1 := SaveFileDialog.Create;
      self.linkLabel1 := LinkLabel.Create;
      inherited SuspendLayout;
      self.button1.Location := Point.Create(40, 24);
      self.button1.Name := 'button1';
      self.button1.Size := System.Drawing.Size.Create(128, 32);
      self.button1.TabIndex := 0;
      self.button1.Text := 'Compress file...';
      Include(self.button1.Click, self.button1_Click);

      self.button2.Location := Point.Create(216, 24);
      self.button2.Name := 'button2';
      self.button2.Size := System.Drawing.Size.Create(112, 32);
      self.button2.TabIndex := 1;
      self.button2.Text := 'Decompress file...';
      Include(self.button2.Click, Self.button2_Click);

      self.linkLabel1.Location := Point.Create(104, 72);
      self.linkLabel1.Name := 'linkLabel1';
      self.linkLabel1.Size := System.Drawing.Size.Create(168, 23);
      self.linkLabel1.TabIndex := 2;
      self.linkLabel1.TabStop := true;
      self.linkLabel1.Text := 'http://www.componentace.com';
      Include(self.linkLabel1.LinkClicked, linkLabel1_LinkClicked);
      self.AutoScaleBaseSize := System.Drawing.Size.Create(5, 13);
      inherited ClientSize :=System.Drawing. Size.Create(368, 101);
      inherited Controls.Add(self.linkLabel1);
      inherited Controls.Add(self.button2);
      inherited Controls.Add(self.button1);
      inherited Name := 'Form1';
      self.Text := 'CompressFile demo (c) ComponentAce 2006';
      inherited ResumeLayout(false)

end;
{$ENDREGION}

procedure TWinForm.Dispose(Disposing: Boolean);
begin
  if Disposing then
  begin
    if Components <> nil then
      Components.Dispose();
  end;
  inherited Dispose(Disposing);
end;

constructor TWinForm.Create;
begin
  inherited Create;
  //
  // Required for Windows Form Designer support
  //
  InitializeComponent;
  //
  // TODO: Add any constructor code after InitializeComponent call
  //
end;

procedure TWinForm.TWinForm_Load(sender: System.Object; e: System.EventArgs);
begin

end;

procedure TWinForm.button1_Click(sender: TObject; e: EventArgs);
begin
  self.openFileDialog1.Title := 'Select a file to compress';
  if (self.openFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK) then
  begin
    self.saveFileDialog1.Title := 'Save compressed file to';
    self.saveFileDialog1.FileName := self.openFileDialog1.FileName + '.compressed';
    if (self.saveFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK) then
        self.compressFile(self.openFileDialog1.FileName, self.saveFileDialog1.FileName)
  end
end;

procedure TWinForm.button2_Click(sender: TObject; e: EventArgs);
begin
  self.openFileDialog1.Title := 'Select a file to decompress';
  if (self.openFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK) then
  begin
    self.saveFileDialog1.Title := 'Save decompressed file to';
    self.saveFileDialog1.FileName := self.openFileDialog1.FileName + '.decompressed';
    if (self.saveFileDialog1.ShowDialog = System.Windows.Forms.DialogResult.OK) then
          self.decompressFile(self.openFileDialog1.FileName, self.saveFileDialog1.FileName)
  end
end;

procedure TWinForm.compressFile(inFile, outFile: string);
var
  outFileStream, zStream, inFileStream: Stream;
begin
  outFileStream := FileStream.Create(outFile, FileMode.Create);
  zStream := ZOutputStream.Create(outFileStream, -1);
  inFileStream := FileStream.Create(inFile, FileMode.Open);
  try
    CopyStream(inFileStream, zStream)
  finally
    zStream.Close;
    outFileStream.Close;
    inFileStream.Close
  end

end;

class procedure TWinForm.CopyStream(input, output: Stream);
var
  num1: Integer;
  buffer1: array[0..2000 - 1] of Byte;
begin
  buffer1 := New(array[2000] of Byte);
  num1 := input.Read(buffer1, 0, 2000);
  while (num1 > 0) do
  begin
    output.Write(buffer1, 0, num1);
    num1 := input.Read(buffer1, 0, 2000);
  end;
  output.Flush
end;

procedure TWinForm.decompressFile(inFile, outFile: string);
var
  stream1, stream2, stream3: Stream;
begin
  stream1 := FileStream.Create(outFile, FileMode.Create);
  stream2 := ZOutputStream.Create(stream1);
  stream3 := FileStream.Create(inFile, FileMode.Open);
  try
    CopyStream(stream3, stream2)
  finally
    stream2.Close;
    stream1.Close;
    stream3.Close
  end

end;

procedure TWinForm.Form1_Load(sender: TObject; e: EventArgs);
begin

end;

procedure TWinForm.linkLabel1_LinkClicked(sender: TObject;
  e: LinkLabelLinkClickedEventArgs);
begin


end;

end.
