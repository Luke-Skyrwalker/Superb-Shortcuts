using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Superb_Shortcuts
{
    public partial class Shortcuts : Form
    {
        protected bool validData;
        string path;

        Paths paths;
        Dictionary<String, Paths.TilePaths> tilePaths;


        public Shortcuts()
        {
            paths = new Paths();
            tilePaths = paths.LoadTilePaths();
            InitializeComponents(paths);
        }

        private void Pb_DoubleClick(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            Paths.TilePaths? tp;
            if (tilePaths.TryGetValue(pb.Name, out tp))
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = false; // ToDo: Needed?
                    myProcess.StartInfo.FileName = tp.ApplicationPath;
                    myProcess.Start();
                }
            }
        }

        private void Pb_Click(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (e.Button == MouseButtons.Right)
            {
                if (ApplicationDialog.ShowDialog() == DialogResult.OK && PictureDialog.ShowDialog() == DialogResult.OK)
                {
                    UpdatePb(pb, PictureDialog.FileName, ApplicationDialog.FileName);
                }
            }
        }

        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                PictureBox pb = sender as PictureBox;
                pb.DoDragDrop(pb, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void Pb_DragEnter(object sender, DragEventArgs e)
        {
            string filename;
            validData = GetFilename(out filename, e);
            if (validData)
            {
                path = filename;
                e.Effect = DragDropEffects.Copy;
            }
            else if (e.Data.GetDataPresent(typeof(PictureBox))) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private void Pb_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (validData) UpdatePb(pb, path);
            else if (e.Data.GetDataPresent(typeof(PictureBox))) SwitchPbs(pb, e.Data.GetData(typeof(PictureBox)) as PictureBox);
        }

        private void SwitchPbs(PictureBox pb1, PictureBox pb2)
        {
            paths.SwitchPbs(pb1.Name, pb2.Name);
            tilePaths = paths.LoadTilePaths();
            ReloadImage(pb1);
            ReloadImage(pb2);
        }

        private void UpdatePb(PictureBox pb, string picturePath, string? applicationPath = null)
        {
            if (applicationPath != null) paths.AddOrUpdateTilePath(pb.Name, picturePath, applicationPath);
            else paths.UpdatePicturePath(pb.Name, picturePath);
            tilePaths = paths.LoadTilePaths();
            ReloadImage(pb);
        }

        private void ReloadImage(PictureBox pb)
        {
            Paths.TilePaths? tp;
            if (tilePaths.TryGetValue(pb.Name, out tp))
            {
                pb.ImageLocation = tp.PicturePath;
            }
            else MessageBox.Show("You need to set the path to an Application first!");
        }

        private bool GetFilename(out string filename, DragEventArgs e)
        {
            bool ret = false;
            filename = String.Empty;
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileDrop") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                    {
                        filename = ((string[])data)[0];
                        string ext = Path.GetExtension(filename).ToLower();
                        if ((ext == ".jpg") || (ext == ".png") || (ext == ".bmp") || (ext == ".lnk") || (ext == ".exe"))
                        {
                            ret = true;
                        }
                    }
                }
            }
            return ret;
        }

    }
}
