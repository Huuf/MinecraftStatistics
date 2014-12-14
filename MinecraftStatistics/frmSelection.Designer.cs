namespace MinecraftStatistics
{
  partial class frmSelection
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
      this.lblLevelDir = new System.Windows.Forms.Label();
      this.btnBrowseDir = new System.Windows.Forms.Button();
      this.cobStatistic = new System.Windows.Forms.ComboBox();
      this.lblStatistic = new System.Windows.Forms.Label();
      this.lblBlockTypes = new System.Windows.Forms.Label();
      this.cobBlockTypes = new System.Windows.Forms.ComboBox();
      this.lblProgress = new System.Windows.Forms.Label();
      this.btnGenerate = new System.Windows.Forms.Button();
      this.pgbProgress = new System.Windows.Forms.ProgressBar();
      this.lblResult = new System.Windows.Forms.Label();
      this.txtResult = new System.Windows.Forms.TextBox();
      this.txtSeed = new System.Windows.Forms.TextBox();
      this.lblSeed = new System.Windows.Forms.Label();
      this.lblBiomes = new System.Windows.Forms.Label();
      this.cobBiomes = new System.Windows.Forms.ComboBox();
      this.cobLevelDirectory = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // lblLevelDir
      // 
      this.lblLevelDir.AutoSize = true;
      this.lblLevelDir.Location = new System.Drawing.Point(12, 17);
      this.lblLevelDir.Name = "lblLevelDir";
      this.lblLevelDir.Size = new System.Drawing.Size(78, 13);
      this.lblLevelDir.TabIndex = 0;
      this.lblLevelDir.Text = "&Level Directory";
      // 
      // btnBrowseDir
      // 
      this.btnBrowseDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnBrowseDir.Location = new System.Drawing.Point(471, 12);
      this.btnBrowseDir.Name = "btnBrowseDir";
      this.btnBrowseDir.Size = new System.Drawing.Size(75, 23);
      this.btnBrowseDir.TabIndex = 2;
      this.btnBrowseDir.Text = "&Browse";
      this.btnBrowseDir.UseVisualStyleBackColor = true;
      this.btnBrowseDir.Click += new System.EventHandler(this.btnBrowseDir_Click);
      // 
      // cobStatistic
      // 
      this.cobStatistic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cobStatistic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cobStatistic.FormattingEnabled = true;
      this.cobStatistic.Location = new System.Drawing.Point(96, 67);
      this.cobStatistic.Name = "cobStatistic";
      this.cobStatistic.Size = new System.Drawing.Size(450, 21);
      this.cobStatistic.TabIndex = 6;
      this.cobStatistic.SelectedIndexChanged += new System.EventHandler(this.cobStatistic_SelectedIndexChanged);
      // 
      // lblStatistic
      // 
      this.lblStatistic.AutoSize = true;
      this.lblStatistic.Location = new System.Drawing.Point(12, 70);
      this.lblStatistic.Name = "lblStatistic";
      this.lblStatistic.Size = new System.Drawing.Size(44, 13);
      this.lblStatistic.TabIndex = 5;
      this.lblStatistic.Text = "S&tatistic";
      // 
      // lblBlockTypes
      // 
      this.lblBlockTypes.AutoSize = true;
      this.lblBlockTypes.Enabled = false;
      this.lblBlockTypes.Location = new System.Drawing.Point(12, 97);
      this.lblBlockTypes.Name = "lblBlockTypes";
      this.lblBlockTypes.Size = new System.Drawing.Size(60, 13);
      this.lblBlockTypes.TabIndex = 7;
      this.lblBlockTypes.Text = "&Block type:";
      // 
      // cobBlockTypes
      // 
      this.cobBlockTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cobBlockTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cobBlockTypes.Enabled = false;
      this.cobBlockTypes.FormattingEnabled = true;
      this.cobBlockTypes.Location = new System.Drawing.Point(96, 94);
      this.cobBlockTypes.Name = "cobBlockTypes";
      this.cobBlockTypes.Size = new System.Drawing.Size(450, 21);
      this.cobBlockTypes.TabIndex = 8;
      // 
      // lblProgress
      // 
      this.lblProgress.AutoSize = true;
      this.lblProgress.Location = new System.Drawing.Point(12, 153);
      this.lblProgress.Name = "lblProgress";
      this.lblProgress.Size = new System.Drawing.Size(48, 13);
      this.lblProgress.TabIndex = 11;
      this.lblProgress.Text = "Progress";
      // 
      // btnGenerate
      // 
      this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnGenerate.Location = new System.Drawing.Point(471, 148);
      this.btnGenerate.Name = "btnGenerate";
      this.btnGenerate.Size = new System.Drawing.Size(75, 23);
      this.btnGenerate.TabIndex = 13;
      this.btnGenerate.Text = "&Generate";
      this.btnGenerate.UseVisualStyleBackColor = true;
      this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
      // 
      // pgbProgress
      // 
      this.pgbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pgbProgress.Location = new System.Drawing.Point(96, 148);
      this.pgbProgress.Name = "pgbProgress";
      this.pgbProgress.Size = new System.Drawing.Size(369, 23);
      this.pgbProgress.TabIndex = 12;
      // 
      // lblResult
      // 
      this.lblResult.AutoSize = true;
      this.lblResult.Location = new System.Drawing.Point(12, 174);
      this.lblResult.Name = "lblResult";
      this.lblResult.Size = new System.Drawing.Size(40, 13);
      this.lblResult.TabIndex = 14;
      this.lblResult.Text = "&Result:";
      // 
      // txtResult
      // 
      this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtResult.Location = new System.Drawing.Point(15, 190);
      this.txtResult.Multiline = true;
      this.txtResult.Name = "txtResult";
      this.txtResult.Size = new System.Drawing.Size(531, 222);
      this.txtResult.TabIndex = 15;
      // 
      // txtSeed
      // 
      this.txtSeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSeed.Location = new System.Drawing.Point(96, 41);
      this.txtSeed.Name = "txtSeed";
      this.txtSeed.ReadOnly = true;
      this.txtSeed.Size = new System.Drawing.Size(450, 20);
      this.txtSeed.TabIndex = 4;
      // 
      // lblSeed
      // 
      this.lblSeed.AutoSize = true;
      this.lblSeed.Location = new System.Drawing.Point(12, 44);
      this.lblSeed.Name = "lblSeed";
      this.lblSeed.Size = new System.Drawing.Size(32, 13);
      this.lblSeed.TabIndex = 3;
      this.lblSeed.Text = "&Seed";
      // 
      // lblBiomes
      // 
      this.lblBiomes.AutoSize = true;
      this.lblBiomes.Enabled = false;
      this.lblBiomes.Location = new System.Drawing.Point(12, 124);
      this.lblBiomes.Name = "lblBiomes";
      this.lblBiomes.Size = new System.Drawing.Size(62, 13);
      this.lblBiomes.TabIndex = 9;
      this.lblBiomes.Text = "B&iome type:";
      // 
      // cobBiomes
      // 
      this.cobBiomes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cobBiomes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cobBiomes.Enabled = false;
      this.cobBiomes.FormattingEnabled = true;
      this.cobBiomes.Location = new System.Drawing.Point(96, 121);
      this.cobBiomes.Name = "cobBiomes";
      this.cobBiomes.Size = new System.Drawing.Size(450, 21);
      this.cobBiomes.TabIndex = 10;
      // 
      // cobLevelDirectory
      // 
      this.cobLevelDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cobLevelDirectory.FormattingEnabled = true;
      this.cobLevelDirectory.Location = new System.Drawing.Point(96, 14);
      this.cobLevelDirectory.Name = "cobLevelDirectory";
      this.cobLevelDirectory.Size = new System.Drawing.Size(369, 21);
      this.cobLevelDirectory.TabIndex = 1;
      // 
      // frmSelection
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(558, 424);
      this.Controls.Add(this.cobLevelDirectory);
      this.Controls.Add(this.lblBiomes);
      this.Controls.Add(this.cobBiomes);
      this.Controls.Add(this.txtSeed);
      this.Controls.Add(this.lblSeed);
      this.Controls.Add(this.txtResult);
      this.Controls.Add(this.lblResult);
      this.Controls.Add(this.pgbProgress);
      this.Controls.Add(this.btnGenerate);
      this.Controls.Add(this.lblProgress);
      this.Controls.Add(this.lblBlockTypes);
      this.Controls.Add(this.cobBlockTypes);
      this.Controls.Add(this.lblStatistic);
      this.Controls.Add(this.cobStatistic);
      this.Controls.Add(this.btnBrowseDir);
      this.Controls.Add(this.lblLevelDir);
      this.Name = "frmSelection";
      this.Text = "Minecraft Statistics";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblLevelDir;
    private System.Windows.Forms.Button btnBrowseDir;
    private System.Windows.Forms.ComboBox cobStatistic;
    private System.Windows.Forms.Label lblStatistic;
    private System.Windows.Forms.Label lblBlockTypes;
    private System.Windows.Forms.ComboBox cobBlockTypes;
    private System.Windows.Forms.Label lblProgress;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.ProgressBar pgbProgress;
    private System.Windows.Forms.Label lblResult;
    private System.Windows.Forms.TextBox txtResult;
    private System.Windows.Forms.TextBox txtSeed;
    private System.Windows.Forms.Label lblSeed;
    private System.Windows.Forms.Label lblBiomes;
    private System.Windows.Forms.ComboBox cobBiomes;
    private System.Windows.Forms.ComboBox cobLevelDirectory;
  }
}

