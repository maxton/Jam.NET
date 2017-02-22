namespace Jammit.Controls
{
  partial class Fader
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
      this.label1 = new System.Windows.Forms.Label();
      this.muteButton = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      this.SuspendLayout();
      // 
      // trackBar1
      // 
      this.trackBar1.Location = new System.Drawing.Point(3, 8);
      this.trackBar1.Maximum = 100;
      this.trackBar1.Minimum = 1;
      this.trackBar1.Name = "trackBar1";
      this.trackBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
      this.trackBar1.Size = new System.Drawing.Size(45, 104);
      this.trackBar1.TabIndex = 0;
      this.trackBar1.TickFrequency = 10;
      this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
      this.trackBar1.Value = 75;
      // 
      // label1
      // 
      this.label1.Dock = System.Windows.Forms.DockStyle.Top;
      this.label1.Location = new System.Drawing.Point(0, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(51, 14);
      this.label1.TabIndex = 1;
      this.label1.Text = "label1";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // muteButton
      // 
      this.muteButton.Appearance = System.Windows.Forms.Appearance.Button;
      this.muteButton.ForeColor = System.Drawing.Color.Black;
      this.muteButton.Location = new System.Drawing.Point(15, 107);
      this.muteButton.Name = "muteButton";
      this.muteButton.Size = new System.Drawing.Size(20, 19);
      this.muteButton.TabIndex = 2;
      this.muteButton.Text = "M";
      this.muteButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.muteButton.UseVisualStyleBackColor = true;
      // 
      // Fader
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.muteButton);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.trackBar1);
      this.Name = "Fader";
      this.Size = new System.Drawing.Size(51, 129);
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TrackBar trackBar1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox muteButton;
  }
}
