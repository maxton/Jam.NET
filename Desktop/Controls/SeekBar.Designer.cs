namespace Jammit.Controls
{
  partial class SeekBar
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      this.timeRemain = new System.Windows.Forms.Label();
      this.timePos = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      this.SuspendLayout();
      // 
      // trackBar1
      // 
      this.trackBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.trackBar1.Location = new System.Drawing.Point(29, 3);
      this.trackBar1.Name = "trackBar1";
      this.trackBar1.Size = new System.Drawing.Size(409, 45);
      this.trackBar1.TabIndex = 18;
      this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
      // 
      // timeRemain
      // 
      this.timeRemain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.timeRemain.AutoSize = true;
      this.timeRemain.BackColor = System.Drawing.Color.Transparent;
      this.timeRemain.ForeColor = System.Drawing.Color.White;
      this.timeRemain.Location = new System.Drawing.Point(432, 6);
      this.timeRemain.Name = "timeRemain";
      this.timeRemain.Size = new System.Drawing.Size(37, 13);
      this.timeRemain.TabIndex = 17;
      this.timeRemain.Text = "-00:00";
      // 
      // timePos
      // 
      this.timePos.AutoSize = true;
      this.timePos.BackColor = System.Drawing.Color.Transparent;
      this.timePos.ForeColor = System.Drawing.Color.White;
      this.timePos.Location = new System.Drawing.Point(-1, 6);
      this.timePos.Name = "timePos";
      this.timePos.Size = new System.Drawing.Size(34, 13);
      this.timePos.TabIndex = 16;
      this.timePos.Text = "00:00";
      // 
      // SeekBar
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.trackBar1);
      this.Controls.Add(this.timeRemain);
      this.Controls.Add(this.timePos);
      this.Name = "SeekBar";
      this.Size = new System.Drawing.Size(469, 27);
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TrackBar trackBar1;
    private System.Windows.Forms.Label timeRemain;
    private System.Windows.Forms.Label timePos;
  }
}
