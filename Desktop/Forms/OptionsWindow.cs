using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jammit
{
  public partial class OptionsWindow : Form
  {
    public OptionsWindow()
    {
      InitializeComponent();
      contentFolderTextBox.Text = Properties.Settings.Default.TrackPath;
      serverTextBox.Text = Properties.Settings.Default.Server;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.TrackPath = contentFolderTextBox.Text;
      Properties.Settings.Default.Server = serverTextBox.Text;
      Properties.Settings.Default.Save();
      DialogResult = DialogResult.OK;
      Close();
    }

    private void folderSelectButton_Click(object sender, EventArgs e)
    {
      var fbd = new FolderBrowserDialog
      {
        SelectedPath = Properties.Settings.Default.TrackPath,
        ShowNewFolderButton = false
      };
      if (fbd.ShowDialog() == DialogResult.OK)
      {
        contentFolderTextBox.Text = fbd.SelectedPath;
      }
    }
  }
}
