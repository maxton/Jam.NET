namespace Jammit
{
  partial class SongWindow
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
      this.button1 = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.mixerFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
      this.albumArtwork = new System.Windows.Forms.PictureBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.score1 = new Jammit.Controls.Score();
      this.waveform1 = new Jammit.Controls.Waveform();
      this.seekBar1 = new Jammit.Controls.SeekBar();
      this.leftSeekBtn = new System.Windows.Forms.Button();
      this.rightSeekBtn = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.albumArtwork)).BeginInit();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.button1.BackColor = System.Drawing.Color.Transparent;
      this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.button1.FlatAppearance.BorderSize = 0;
      this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.button1.ForeColor = System.Drawing.Color.White;
      this.button1.Location = new System.Drawing.Point(638, 524);
      this.button1.Margin = new System.Windows.Forms.Padding(0);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(58, 28);
      this.button1.TabIndex = 5;
      this.button1.Text = "Play";
      this.button1.UseVisualStyleBackColor = false;
      this.button1.Click += new System.EventHandler(this.playPauseButton_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox1.Controls.Add(this.mixerFlowPanel);
      this.groupBox1.ForeColor = System.Drawing.Color.White;
      this.groupBox1.Location = new System.Drawing.Point(165, 396);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(322, 153);
      this.groupBox1.TabIndex = 14;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Mixer";
      // 
      // mixerFlowPanel
      // 
      this.mixerFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.mixerFlowPanel.Location = new System.Drawing.Point(3, 16);
      this.mixerFlowPanel.Name = "mixerFlowPanel";
      this.mixerFlowPanel.Size = new System.Drawing.Size(316, 134);
      this.mixerFlowPanel.TabIndex = 0;
      // 
      // albumArtwork
      // 
      this.albumArtwork.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.albumArtwork.Location = new System.Drawing.Point(12, 402);
      this.albumArtwork.Name = "albumArtwork";
      this.albumArtwork.Size = new System.Drawing.Size(147, 147);
      this.albumArtwork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.albumArtwork.TabIndex = 8;
      this.albumArtwork.TabStop = false;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(900, 24);
      this.menuStrip1.TabIndex = 19;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
      this.closeToolStripMenuItem.Text = "Close song";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // score1
      // 
      this.score1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.score1.BackColor = System.Drawing.Color.White;
      this.score1.Location = new System.Drawing.Point(9, 36);
      this.score1.Name = "score1";
      this.score1.SamplePosition = ((long)(0));
      this.score1.Samples = ((long)(0));
      this.score1.Size = new System.Drawing.Size(879, 354);
      this.score1.TabIndex = 20;
      // 
      // waveform1
      // 
      this.waveform1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.waveform1.BackColor = System.Drawing.Color.Black;
      this.waveform1.ForeColor = System.Drawing.Color.Green;
      this.waveform1.Location = new System.Drawing.Point(503, 424);
      this.waveform1.Name = "waveform1";
      this.waveform1.SamplePosition = ((long)(0));
      this.waveform1.Samples = ((long)(0));
      this.waveform1.Size = new System.Drawing.Size(385, 94);
      this.waveform1.TabIndex = 18;
      this.waveform1.TabStop = false;
      // 
      // seekBar1
      // 
      this.seekBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.seekBar1.Location = new System.Drawing.Point(503, 398);
      this.seekBar1.Name = "seekBar1";
      this.seekBar1.SamplePosition = ((long)(0));
      this.seekBar1.Samples = ((long)(0));
      this.seekBar1.Size = new System.Drawing.Size(385, 27);
      this.seekBar1.TabIndex = 21;
      // 
      // leftSeekBtn
      // 
      this.leftSeekBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.leftSeekBtn.BackColor = System.Drawing.Color.Transparent;
      this.leftSeekBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.leftSeekBtn.FlatAppearance.BorderSize = 0;
      this.leftSeekBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.leftSeekBtn.ForeColor = System.Drawing.Color.White;
      this.leftSeekBtn.Location = new System.Drawing.Point(604, 524);
      this.leftSeekBtn.Margin = new System.Windows.Forms.Padding(0);
      this.leftSeekBtn.Name = "leftSeekBtn";
      this.leftSeekBtn.Size = new System.Drawing.Size(34, 28);
      this.leftSeekBtn.TabIndex = 22;
      this.leftSeekBtn.Text = "<<";
      this.leftSeekBtn.UseVisualStyleBackColor = false;
      this.leftSeekBtn.Click += new System.EventHandler(this.leftSeekBtn_Click);
      // 
      // rightSeekBtn
      // 
      this.rightSeekBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.rightSeekBtn.BackColor = System.Drawing.Color.Transparent;
      this.rightSeekBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.rightSeekBtn.FlatAppearance.BorderSize = 0;
      this.rightSeekBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.rightSeekBtn.ForeColor = System.Drawing.Color.White;
      this.rightSeekBtn.Location = new System.Drawing.Point(696, 524);
      this.rightSeekBtn.Margin = new System.Windows.Forms.Padding(0);
      this.rightSeekBtn.Name = "rightSeekBtn";
      this.rightSeekBtn.Size = new System.Drawing.Size(34, 28);
      this.rightSeekBtn.TabIndex = 23;
      this.rightSeekBtn.Text = ">>";
      this.rightSeekBtn.UseVisualStyleBackColor = false;
      this.rightSeekBtn.Click += new System.EventHandler(this.rightSeekBtn_Click);
      // 
      // button2
      // 
      this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.button2.BackColor = System.Drawing.Color.Transparent;
      this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
      this.button2.FlatAppearance.BorderSize = 0;
      this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.button2.ForeColor = System.Drawing.Color.White;
      this.button2.Location = new System.Drawing.Point(730, 524);
      this.button2.Margin = new System.Windows.Forms.Padding(0);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(75, 28);
      this.button2.TabIndex = 24;
      this.button2.Text = "Loop";
      this.button2.UseVisualStyleBackColor = false;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // SongWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
      this.ClientSize = new System.Drawing.Size(900, 561);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.rightSeekBtn);
      this.Controls.Add(this.leftSeekBtn);
      this.Controls.Add(this.score1);
      this.Controls.Add(this.waveform1);
      this.Controls.Add(this.albumArtwork);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.menuStrip1);
      this.Controls.Add(this.seekBar1);
      this.DoubleBuffered = true;
      this.MainMenuStrip = this.menuStrip1;
      this.MinimumSize = new System.Drawing.Size(916, 600);
      this.Name = "SongWindow";
      this.ShowIcon = false;
      this.Text = "Score";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SongWindow_FormClosing);
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.albumArtwork)).EndInit();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.PictureBox albumArtwork;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.FlowLayoutPanel mixerFlowPanel;
    private Controls.Waveform waveform1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private Controls.Score score1;
    private Controls.SeekBar seekBar1;
    private System.Windows.Forms.Button leftSeekBtn;
    private System.Windows.Forms.Button rightSeekBtn;
    private System.Windows.Forms.Button button2;
  }
}