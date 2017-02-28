namespace Jammit
{
  partial class OptionsWindow
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
      this.label1 = new System.Windows.Forms.Label();
      this.contentFolderTextBox = new System.Windows.Forms.TextBox();
      this.folderSelectButton = new System.Windows.Forms.Button();
      this.saveButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 15);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(79, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Content Folder:";
      // 
      // contentFolderTextBox
      // 
      this.contentFolderTextBox.Location = new System.Drawing.Point(97, 12);
      this.contentFolderTextBox.Name = "contentFolderTextBox";
      this.contentFolderTextBox.Size = new System.Drawing.Size(183, 20);
      this.contentFolderTextBox.TabIndex = 1;
      // 
      // folderSelectButton
      // 
      this.folderSelectButton.Location = new System.Drawing.Point(286, 10);
      this.folderSelectButton.Name = "folderSelectButton";
      this.folderSelectButton.Size = new System.Drawing.Size(95, 23);
      this.folderSelectButton.TabIndex = 2;
      this.folderSelectButton.Text = "Select Folder";
      this.folderSelectButton.UseVisualStyleBackColor = true;
      this.folderSelectButton.Click += new System.EventHandler(this.folderSelectButton_Click);
      // 
      // saveButton
      // 
      this.saveButton.Location = new System.Drawing.Point(226, 150);
      this.saveButton.Name = "saveButton";
      this.saveButton.Size = new System.Drawing.Size(75, 23);
      this.saveButton.TabIndex = 3;
      this.saveButton.Text = "Save";
      this.saveButton.UseVisualStyleBackColor = true;
      this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Location = new System.Drawing.Point(78, 150);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 4;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(94, 35);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(263, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "This folder should contain your content folders or .zips.";
      // 
      // OptionsWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(393, 185);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.saveButton);
      this.Controls.Add(this.folderSelectButton);
      this.Controls.Add(this.contentFolderTextBox);
      this.Controls.Add(this.label1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OptionsWindow";
      this.ShowIcon = false;
      this.Text = "Options";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox contentFolderTextBox;
    private System.Windows.Forms.Button folderSelectButton;
    private System.Windows.Forms.Button saveButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label label2;
  }
}