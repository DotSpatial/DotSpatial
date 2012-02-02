using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace CompressFile
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(40, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 32);
			this.button1.TabIndex = 0;
			this.button1.Text = "Compress file...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(216, 24);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(112, 32);
			this.button2.TabIndex = 1;
			this.button2.Text = "Decompress file...";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// linkLabel1
			// 
			this.linkLabel1.Location = new System.Drawing.Point(104, 72);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(168, 23);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://www.componentace.com";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 101);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Name = "Form1";
			this.Text = "CompressFile demo (c) ComponentAce 2006";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		public static void CopyStream(System.IO.Stream input, System.IO.Stream output)
		{
			byte[] buffer = new byte[2000];
			int len;
			while ((len = input.Read(buffer, 0, 2000)) > 0)
			{
				output.Write(buffer, 0, len);
			}
			output.Flush();
		}

		private void compressFile(string inFile, string outFile)
		{
			System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create);
			zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
			System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);			
			try
			{
				CopyStream(inFileStream, outZStream);
			}
			finally
			{
				outZStream.Close();
				outFileStream.Close();
				inFileStream.Close();
			}
		}

		private void decompressFile(string inFile, string outFile)
		{
			System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create);
			zlib.ZOutputStream outZStream = new zlib.ZOutputStream(outFileStream);
			System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);			
			try
			{
				CopyStream(inFileStream, outZStream);
			}
			finally
			{
				outZStream.Close();
				outFileStream.Close();
				inFileStream.Close();
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.Title = "Select a file to compress";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				saveFileDialog1.Title = "Save compressed file to";				
				saveFileDialog1.FileName = openFileDialog1.FileName + ".compressed";
				if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					compressFile(openFileDialog1.FileName, saveFileDialog1.FileName);
				}
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.Title = "Select a file to decompress";
			if(openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				saveFileDialog1.Title = "Save decompressed file to";				
				saveFileDialog1.FileName = openFileDialog1.FileName + ".decompressed";
				if(saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					decompressFile(openFileDialog1.FileName, saveFileDialog1.FileName);
				}
			}
		
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.componentace.com");
		}	
	}
}
