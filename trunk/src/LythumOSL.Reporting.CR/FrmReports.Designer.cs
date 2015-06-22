namespace LythumOSL.Reporting.CR
{
	partial class FrmReports
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (FrmReports));
			this.CrvMain = new CrystalDecisions.Windows.Forms.CrystalReportViewer ();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel ();
			this.panel1 = new System.Windows.Forms.Panel ();
			this.CmdExport = new System.Windows.Forms.Button ();
			this.CmbExportFormat = new System.Windows.Forms.ComboBox ();
			this.TxtCopiesCount = new System.Windows.Forms.TextBox ();
			this.LblCopiesCount = new System.Windows.Forms.Label ();
			this.CmdPrintAll = new System.Windows.Forms.Button ();
			this.CmdPrintOne = new System.Windows.Forms.Button ();
			this.LbxReports = new System.Windows.Forms.ListBox ();
			this.tableLayoutPanel1.SuspendLayout ();
			this.panel1.SuspendLayout ();
			this.SuspendLayout ();
			// 
			// CrvMain
			// 
			this.CrvMain.ActiveViewIndex = -1;
			this.CrvMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CrvMain.DisplayGroupTree = false;
			this.CrvMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CrvMain.Enabled = false;
			this.CrvMain.Location = new System.Drawing.Point (258, 3);
			this.CrvMain.Name = "CrvMain";
			this.CrvMain.SelectionFormula = "";
			this.CrvMain.ShowCloseButton = false;
			this.CrvMain.ShowGroupTreeButton = false;
			this.CrvMain.ShowRefreshButton = false;
			this.CrvMain.Size = new System.Drawing.Size (584, 390);
			this.CrvMain.TabIndex = 1;
			this.CrvMain.ViewTimeSelectionFormula = "";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 30.17752F));
			this.tableLayoutPanel1.ColumnStyles.Add (new System.Windows.Forms.ColumnStyle (System.Windows.Forms.SizeType.Percent, 69.82249F));
			this.tableLayoutPanel1.Controls.Add (this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add (this.CrvMain, 1, 0);
			this.tableLayoutPanel1.Controls.Add (this.LbxReports, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point (0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add (new System.Windows.Forms.RowStyle (System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size (845, 441);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel1
			// 
			this.tableLayoutPanel1.SetColumnSpan (this.panel1, 2);
			this.panel1.Controls.Add (this.CmdExport);
			this.panel1.Controls.Add (this.CmbExportFormat);
			this.panel1.Controls.Add (this.TxtCopiesCount);
			this.panel1.Controls.Add (this.LblCopiesCount);
			this.panel1.Controls.Add (this.CmdPrintAll);
			this.panel1.Controls.Add (this.CmdPrintOne);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point (3, 399);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size (839, 39);
			this.panel1.TabIndex = 2;
			// 
			// CmdExport
			// 
			this.CmdExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CmdExport.Location = new System.Drawing.Point (706, 0);
			this.CmdExport.Name = "CmdExport";
			this.CmdExport.Size = new System.Drawing.Size (133, 36);
			this.CmdExport.TabIndex = 5;
			this.CmdExport.Text = "Export selected";
			this.CmdExport.UseVisualStyleBackColor = true;
			this.CmdExport.Click += new System.EventHandler (this.CmdExport_Click);
			// 
			// CmbExportFormat
			// 
			this.CmbExportFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CmbExportFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbExportFormat.FormattingEnabled = true;
			this.CmbExportFormat.Location = new System.Drawing.Point (560, 3);
			this.CmbExportFormat.Name = "CmbExportFormat";
			this.CmbExportFormat.Size = new System.Drawing.Size (140, 21);
			this.CmbExportFormat.TabIndex = 4;
			// 
			// TxtCopiesCount
			// 
			this.TxtCopiesCount.BackColor = System.Drawing.SystemColors.Window;
			this.TxtCopiesCount.HideSelection = false;
			this.TxtCopiesCount.Location = new System.Drawing.Point (253, 3);
			this.TxtCopiesCount.Name = "TxtCopiesCount";
			this.TxtCopiesCount.Size = new System.Drawing.Size (50, 20);
			this.TxtCopiesCount.TabIndex = 3;
			this.TxtCopiesCount.TextChanged += new System.EventHandler (this.TxtCopiesCount_TextChanged);
			this.TxtCopiesCount.Validating += new System.ComponentModel.CancelEventHandler (this.TxtCopiesCount_Validating);
			// 
			// LblCopiesCount
			// 
			this.LblCopiesCount.AutoSize = true;
			this.LblCopiesCount.Location = new System.Drawing.Point (309, 3);
			this.LblCopiesCount.Name = "LblCopiesCount";
			this.LblCopiesCount.Size = new System.Drawing.Size (69, 13);
			this.LblCopiesCount.TabIndex = 2;
			this.LblCopiesCount.Text = "Copies count";
			// 
			// CmdPrintAll
			// 
			this.CmdPrintAll.Enabled = false;
			this.CmdPrintAll.Location = new System.Drawing.Point (127, 0);
			this.CmdPrintAll.Name = "CmdPrintAll";
			this.CmdPrintAll.Size = new System.Drawing.Size (120, 36);
			this.CmdPrintAll.TabIndex = 1;
			this.CmdPrintAll.Text = "Print all";
			this.CmdPrintAll.UseVisualStyleBackColor = true;
			this.CmdPrintAll.Click += new System.EventHandler (this.CmdPrintAll_Click);
			// 
			// CmdPrintOne
			// 
			this.CmdPrintOne.Enabled = false;
			this.CmdPrintOne.Location = new System.Drawing.Point (0, 0);
			this.CmdPrintOne.Name = "CmdPrintOne";
			this.CmdPrintOne.Size = new System.Drawing.Size (120, 36);
			this.CmdPrintOne.TabIndex = 0;
			this.CmdPrintOne.Text = "Print selected";
			this.CmdPrintOne.UseVisualStyleBackColor = true;
			this.CmdPrintOne.Click += new System.EventHandler (this.CmdPrintOne_Click);
			// 
			// LbxReports
			// 
			this.LbxReports.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LbxReports.FormattingEnabled = true;
			this.LbxReports.Location = new System.Drawing.Point (3, 3);
			this.LbxReports.Name = "LbxReports";
			this.LbxReports.Size = new System.Drawing.Size (249, 381);
			this.LbxReports.TabIndex = 3;
			this.LbxReports.SelectedIndexChanged += new System.EventHandler (this.LvwList_SelectedIndexChanged);
			// 
			// FrmReports
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (845, 441);
			this.Controls.Add (this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
			this.Name = "FrmReports";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FrmReports";
			this.Load += new System.EventHandler (this.FrmReports_Load);
			this.Resize += new System.EventHandler (this.FrmReports_Resize);
			this.tableLayoutPanel1.ResumeLayout (false);
			this.panel1.ResumeLayout (false);
			this.panel1.PerformLayout ();
			this.ResumeLayout (false);

		}

		#endregion

		internal CrystalDecisions.Windows.Forms.CrystalReportViewer CrvMain;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button CmdPrintAll;
		private System.Windows.Forms.Button CmdPrintOne;
		private System.Windows.Forms.Label LblCopiesCount;
		private System.Windows.Forms.TextBox TxtCopiesCount;
		private System.Windows.Forms.Button CmdExport;
		private System.Windows.Forms.ComboBox CmbExportFormat;
		private System.Windows.Forms.ListBox LbxReports;
	}
}