using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Superb_Shortcuts
{
    public partial class Shortcuts : Form
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        private const byte VK_LWIN = 0x5B;  // Left Windows-Key
        private const byte M = 0x4D;        // M-Key
        private const byte VK_TAB = 0x09;   // Tab-Key
        private const byte VK_RETURN = 0x0D;// Enter
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public enum DropType
        {
            PictureBox,
            Picture,
            App,
            Invalid
        }

        DropType dropType = DropType.Invalid;
        string? dropFilepath;

        Paths paths;
        // ToDo: Dictionary overkill here?
        Dictionary<String, String> appPaths;

        int startupCounter = 0;
        PictureBox[] startupSequence;

        int? selectedIndex;        
        int pbMinWidth;
        int pbMaxWidth;


        public Shortcuts()
        {
            paths = new Paths();
            appPaths = paths.LoadAppPaths();
            InitializeComponents(paths);
            RegisterInStartup();

            pbMinWidth = A0.Width;
            pbMaxWidth = A0.Width + selDifWidth;

            foreach (PictureBox pb in tiles)
            {
                pb.Visible = false;
            }
            startupSequence = [A0, A1, A2, A5, A4, A3, A6, A7, A8];
            StartupAnimationTimer.Start();

            ControllerHandler controller = new ControllerHandler(MoveSelection, StartApp, HomeButtonPressed);
            controllerTimer.Tick += (s, e) => controller.Update(Form.ActiveForm == this);
            controllerTimer.Start();
        }

        private void Pb_DoubleClick(object sender, EventArgs e)
        {
            StartApp(sender as PictureBox);
        }

        private void Pb_Click(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (e.Button == MouseButtons.Right)
            {
                if (ApplicationDialog.ShowDialog() == DialogResult.OK)
                {
                    if (PictureDialog.ShowDialog() == DialogResult.OK) ProcessAppFile(pb, ApplicationDialog.FileName, PictureDialog.FileName);
                    else ProcessAppFile(pb, ApplicationDialog.FileName);
                }
            }
        }

        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                PictureBox pb = sender as PictureBox;
                pb.DoDragDrop(pb, DragDropEffects.Move);
            }
        }

        private void Pb_MouseEnter(object sender, EventArgs e)
        {
            SelectPb(sender as PictureBox);
        }

        private void Pb_MouseLeave(object sender, EventArgs e)
        {
            UnselectPb(sender as PictureBox);
        }


        private void Pb_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PictureBox)))
            {
                dropType = DropType.PictureBox;
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                dropType = FileHandling.GetDropTypeAndPath(e, out dropFilepath);
                if (dropType != DropType.Invalid) e.Effect = DragDropEffects.Copy;
            }
            else dropType = DropType.Invalid;
        }


        private void Pb_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            switch (dropType)
            {
                case DropType.PictureBox:
                    SwitchPbs(pb, e.Data.GetData(typeof(PictureBox)) as PictureBox);
                    break;
                case DropType.Picture:
                    UpdatePb(pb, dropFilepath);
                    break;
                case DropType.App:
                    ProcessAppFile(pb, dropFilepath);
                    break;
                default:
                    break;
            }
            dropType = DropType.Invalid;
        }

        private void StartApp()
        {
            if (selectedIndex != null)
            {
                StartApp(tiles[(int)selectedIndex]);
            }
        }

        private void StartApp(PictureBox pb)
        {
            string? ap;
            if (appPaths.TryGetValue(pb.Name, out ap))
            {
                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = ap;
                    myProcess.Start();
                }
            }
        }

        private void MoveSelection(int moveDir)
        {
            if (selectedIndex == null) selectedIndex = 4;
            int newIndex = (int)selectedIndex + moveDir;
            if (newIndex < 0 || newIndex >= tiles.Length) return;
            UnselectPb(tiles[(int)selectedIndex]);
            SelectPb(tiles[newIndex]);
        }

        private void HomeButtonPressed(int times)
        {
            if (times <= 2)
            {
                MinimizeWindows();

                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = "SoundSwitch.CLI.exe";
                    if (times == 1) myProcess.StartInfo.Arguments = "profile --name \"Sofa\"";
                    else myProcess.StartInfo.Arguments = "profile --name \"Headset\"";
                    myProcess.Start();
                }

                using (Process myProcess = new Process())
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = "DisplaySwitch";
                    myProcess.StartInfo.Arguments = "/internal";
                    myProcess.Start();
                }
                // this.Show();
            }
        }

        private void MinimizeWindows()
        {
            keybd_event(VK_LWIN, 0, 0, IntPtr.Zero);
            keybd_event(M, 0, 0, IntPtr.Zero);
            keybd_event(M, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
            keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }


        private void SelectPb(PictureBox pb)
        {
            if (selectedIndex != null) UnselectPb(tiles[(int)selectedIndex]);
            if (pb.Width == pbMaxWidth) return;
            selectedIndex = pb.Name[1] - '0';
            SuspendLayout();
            pb.Width = pbMaxWidth;
            pb.Height = pb.Height + selDifHeight;
            pb.Left = pb.Left - selDifWidth / 2;
            pb.Top = pb.Top - selDifHeight / 2;
            ResumeLayout(false);
        }

        private void UnselectPb(PictureBox pb)
        {
            if (pb.Width == pbMinWidth) return;
            selectedIndex = null;
            SuspendLayout();
            pb.Width = pbMinWidth;
            pb.Height = pb.Height - selDifHeight;
            pb.Left = pb.Left + selDifWidth / 2;
            pb.Top = pb.Top + selDifHeight / 2;
            ResumeLayout(false);
        }

        private void SwitchPbs(PictureBox pb1, PictureBox pb2)
        {
            if (pb1.Equals(pb2)) return;
            paths.SwitchTiles(pb1.Name, pb2.Name);
            appPaths = paths.LoadAppPaths();
            ReloadImage(pb1);
            ReloadImage(pb2);
        }

        private void UpdatePb(PictureBox pb, string picturePath, string? applicationPath = null)
        {
            UpdatePb(pb, new Bitmap(picturePath), applicationPath);
        }

        private void UpdatePb(PictureBox pb, Bitmap pic, string? applicationPath = null)
        {
            if (applicationPath != null) paths.AddOrUpdateTile(pb.Name, pic, applicationPath);
            else paths.SavePicture(pb.Name, pic);
            appPaths = paths.LoadAppPaths();
            ReloadImage(pb);
        }

        private void ReloadImage(PictureBox pb)
        {
            pb.Image = paths.LoadPicture(pb.Name);
        }

        private void ProcessAppFile(PictureBox pb, string dropFilepath, string? picturePath = null)
        {
            Bitmap? pic;
            string? appPath;
            if (FileHandling.GetPicAndPath(out pic, out appPath, dropFilepath, picturePath)) UpdatePb(pb, pic, appPath);
        }

        private void StartupAnimationTimer_Tick(object sender, EventArgs e)
        {
            int startupLength = startupSequence.Length;
            if (startupCounter < startupLength) startupSequence[startupCounter].Visible = true;
            else if (startupCounter < startupLength + 4)
            {
                if (startupCounter < startupLength + 3)
                {
                    SelectPb(tiles[startupCounter % 9]);
                    selectedIndex = null;
                    SelectPb(tiles[startupCounter % 9 + 3]);
                    selectedIndex = null;
                    SelectPb(tiles[startupCounter % 9 + 6]);
                    selectedIndex = null;
                }
                if (startupCounter > startupLength)
                {
                    UnselectPb(tiles[startupCounter % 9 - 1]);
                    UnselectPb(tiles[startupCounter % 9 + 2]);
                    UnselectPb(tiles[startupCounter % 9 + 5]);
                }
            }
            else StartupAnimationTimer.Stop();
            startupCounter += 1;
        }

        private void RegisterInStartup()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey.SetValue("SuperbShortcuts", Application.ExecutablePath);
        }
    }
}
