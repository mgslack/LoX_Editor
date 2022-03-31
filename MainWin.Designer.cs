
namespace LoX_Editor
{
    partial class MainWin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.tsLoX = new System.Windows.Forms.ToolStrip();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tstbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.tsbSearch = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.tsbHelp = new System.Windows.Forms.ToolStripButton();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.openDlg = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSaveFn = new System.Windows.Forms.Label();
            this.tbGameXML = new System.Windows.Forms.TextBox();
            this.tsbEditDlg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLoX.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsLoX
            // 
            this.tsLoX.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbOpen,
            this.tsbSave,
            this.toolStripLabel1,
            this.tstbSearch,
            this.tsbSearch,
            this.tsbEditDlg,
            this.toolStripSeparator1,
            this.tsbAbout,
            this.tsbHelp,
            this.tsbExit});
            this.tsLoX.Location = new System.Drawing.Point(0, 0);
            this.tsLoX.Name = "tsLoX";
            this.tsLoX.Size = new System.Drawing.Size(784, 25);
            this.tsLoX.TabIndex = 0;
            this.tsLoX.Text = "LoX ToolStrip";
            // 
            // tsbOpen
            // 
            this.tsbOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(23, 22);
            this.tsbOpen.Text = "Open";
            this.tsbOpen.ToolTipText = "Open LoX Saved Game";
            this.tsbOpen.Click += new System.EventHandler(this.TsbOpen_Click);
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.Enabled = false;
            this.tsbSave.Image = ((System.Drawing.Image)(resources.GetObject("tsbSave.Image")));
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(23, 22);
            this.tsbSave.Text = "Save";
            this.tsbSave.ToolTipText = "Save Edited LoX Game";
            this.tsbSave.Click += new System.EventHandler(this.TsbSave_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(65, 22);
            this.toolStripLabel1.Text = "&Search For:";
            // 
            // tstbSearch
            // 
            this.tstbSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tstbSearch.Name = "tstbSearch";
            this.tstbSearch.Size = new System.Drawing.Size(150, 25);
            this.tstbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TstbSearch_KeyDown);
            this.tstbSearch.TextChanged += new System.EventHandler(this.TstbSearch_TextChanged);
            // 
            // tsbSearch
            // 
            this.tsbSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSearch.Image = ((System.Drawing.Image)(resources.GetObject("tsbSearch.Image")));
            this.tsbSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearch.Name = "tsbSearch";
            this.tsbSearch.Size = new System.Drawing.Size(23, 22);
            this.tsbSearch.Text = "Search Next";
            this.tsbSearch.Click += new System.EventHandler(this.TsbSearch_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsbAbout.Image")));
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(23, 22);
            this.tsbAbout.Text = "About";
            this.tsbAbout.Click += new System.EventHandler(this.TsbAbout_Click);
            // 
            // tsbHelp
            // 
            this.tsbHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbHelp.Image = ((System.Drawing.Image)(resources.GetObject("tsbHelp.Image")));
            this.tsbHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbHelp.Name = "tsbHelp";
            this.tsbHelp.Size = new System.Drawing.Size(23, 22);
            this.tsbHelp.Text = "Help";
            this.tsbHelp.Click += new System.EventHandler(this.TsbHelp_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(23, 22);
            this.tsbExit.Text = "Exit";
            this.tsbExit.ToolTipText = "Exit Application";
            this.tsbExit.Click += new System.EventHandler(this.TsbExit_Click);
            // 
            // openDlg
            // 
            this.openDlg.Filter = "Saved Game|*.jxsav|Any File|*.*";
            this.openDlg.Title = "Open LoX Saved Game";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Save:";
            this.label1.UseMnemonic = false;
            // 
            // lblSaveFn
            // 
            this.lblSaveFn.AutoSize = true;
            this.lblSaveFn.Location = new System.Drawing.Point(82, 34);
            this.lblSaveFn.Name = "lblSaveFn";
            this.lblSaveFn.Size = new System.Drawing.Size(19, 13);
            this.lblSaveFn.TabIndex = 2;
            this.lblSaveFn.Text = "<>";
            this.lblSaveFn.UseMnemonic = false;
            // 
            // tbGameXML
            // 
            this.tbGameXML.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGameXML.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGameXML.Location = new System.Drawing.Point(12, 50);
            this.tbGameXML.MaxLength = 0;
            this.tbGameXML.Multiline = true;
            this.tbGameXML.Name = "tbGameXML";
            this.tbGameXML.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbGameXML.Size = new System.Drawing.Size(760, 439);
            this.tbGameXML.TabIndex = 3;
            this.tbGameXML.WordWrap = false;
            this.tbGameXML.TextChanged += new System.EventHandler(this.TbGameXML_TextChanged);
            // 
            // tsbEditDlg
            // 
            this.tsbEditDlg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbEditDlg.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditDlg.Image")));
            this.tsbEditDlg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbEditDlg.Name = "tsbEditDlg";
            this.tsbEditDlg.Size = new System.Drawing.Size(23, 22);
            this.tsbEditDlg.Text = "Edit Chars";
            this.tsbEditDlg.ToolTipText = "Edit Characters Using Dialog";
            this.tsbEditDlg.Click += new System.EventHandler(this.TsbEditDlg_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 501);
            this.Controls.Add(this.tbGameXML);
            this.Controls.Add(this.lblSaveFn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tsLoX);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 270);
            this.Name = "MainWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoX Save Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
            this.Load += new System.EventHandler(this.MainWin_Load);
            this.tsLoX.ResumeLayout(false);
            this.tsLoX.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsLoX;
        private System.Windows.Forms.ToolStripButton tsbOpen;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.OpenFileDialog openDlg;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.ToolStripButton tsbHelp;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSaveFn;
        private System.Windows.Forms.TextBox tbGameXML;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox tstbSearch;
        private System.Windows.Forms.ToolStripButton tsbSearch;
        private System.Windows.Forms.ToolStripButton tsbEditDlg;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

