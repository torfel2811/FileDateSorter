using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace FileDateSorter
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }
    string m_Statustext;

    public string Statustext
    {
      get { return m_Statustext; }
      set { m_Statustext = value; }
    }

    private void Form1_Load(object sender, EventArgs e)
    {
    }

    private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        OFD1.Filter = "Image(*.jpg)|*.jpg|Image(*.jpeg)|*.jpeg|Movie(*.mp4)|*.mp4|Movie(*.3gp)|*.3gp|AVI(*.avi)|*.avi|MOV(*.mov)|*.mov|Nikon RAW(*.nef)|*.nef|HiEff(*.heic)|*.heic";
        OFD1.Multiselect = true;
      if (OFD1.ShowDialog() == DialogResult.OK)
      {
          Thread CopyThread = new Thread(CopyFiles);
          CopyThread.Start();
      }
    }

    private void CopyFiles()
    {
      String[] strFilnames;
      DateTime dtDateTime;
      strFilnames = new string[OFD1.SafeFileNames.Length];
      strFilnames = OFD1.FileNames;
      DirectoryInfo DirInfo = new DirectoryInfo(OFD1.FileName);
      string sText;
      int FileIndex = 1;
     //menuStrip1.Enabled = false;
      foreach (string index in strFilnames)
      {
        
        dtDateTime = File.GetLastWriteTime(index);
        FileInfo FI = new FileInfo(index);
        sText = dtDateTime.Year + ".";
        if (dtDateTime.Month < 10)
          sText = sText + "0" + dtDateTime.Month + ".";
        else
          sText = sText + dtDateTime.Month + ".";
        if (dtDateTime.Day < 10)
            sText = sText + "0" + dtDateTime.Day + "\\Camera\\";
        else
          sText = sText + dtDateTime.Day + "\\Camera\\";
        //Bitmap bmp1 = (Bitmap)Bitmap.FromFile(index);
        //Bitmap bmp2 = new Bitmap(bmp1, (int)(bmp1.Width * 0.2), (int)(bmp1.Height * 0.2));
        //pbImage.Image = bmp2;
        
        try
        {
          if (!Directory.Exists(FI.DirectoryName + "\\" + sText))
            Directory.CreateDirectory(FI.DirectoryName + "\\" + sText);
        }
        catch (IOException ex)
        {
          MessageBox.Show("Ordner konnte nicht erstellt werden!\n"+ex.ToString());
        }
        try
        {
          pbImage.Image = null;
          File.Move(index, FI.DirectoryName + "\\" + sText + "\\" + FI.Name);
        }
        catch (IOException ex)
        {
          MessageBox.Show("Datei " + index + " konte nicht kopiert werden!\n" + ex.ToString());
        }
        Statustext = Convert.ToString((FileIndex * 100) / OFD1.FileNames.Length ) + " % fertig!";
        StatusBar.Invoke(new MethodInvoker (SetStatus));
        FileIndex++;
      }
      
      
      MessageBox.Show("Fertig!");

    }
    private void SetStatus()
    {
      toolStripStatusLabel1.Text = Statustext;
    }
  }
}
