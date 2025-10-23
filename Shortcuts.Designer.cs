using static Paths;

namespace Superb_Shortcuts
{
    partial class Shortcuts
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            PictureDialog = new OpenFileDialog();
            ApplicationDialog = new OpenFileDialog();
            StartupAnimationTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // PictureDialog
            // 
            PictureDialog.Filter = "Bilder| *.png; *.jpg; *.jpeg; *.bmp";
            PictureDialog.Title = "Choose Picture";
            // 
            // ApplicationDialog
            // 
            ApplicationDialog.Filter = "Applications | *.exe; *.lnk; *.url";
            ApplicationDialog.Title = "Choose Application";
            // 
            // StartupAnimationTimer
            // 
            StartupAnimationTimer.Interval = 200;
            StartupAnimationTimer.Tick += StartupAnimationTimer_Tick;
            // 
            // Shortcuts
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = Color.White;
            ClientSize = new Size(1940, 1100);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(1);
            Name = "Shortcuts";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Shortcuts";
            TransparencyKey = Color.White;
            ResumeLayout(false);
        }

        #endregion

        private void InitializeComponents(Paths paths)
        {
            InitializePictureBoxes(paths);
            InitializeComponent();
            AutoScaleDimensions = new SizeF(19F, 37F);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width / 3, Screen.PrimaryScreen.Bounds.Height / 3);
            Top = (int)Math.Ceiling(0.01 * Size.Height);
            Left = Screen.PrimaryScreen.Bounds.Width - Size.Width - (int)Math.Ceiling(0.01 * Size.Width);
            selDifWidth = (int)(Size.Width * 0.02);
            selDifHeight = (int)(Size.Height * 0.02);
        }

        private void InitializePictureBoxes(Paths paths)
        {
            SuspendLayout();
            tiles = [A0, A3, A6, A1, A4, A7, A2, A5, A8];
            positions = InitializePositions();
            for (int i = 0; i < tiles.Length; i++)
            {
                PictureBox pb = tiles[i];
                ((System.ComponentModel.ISupportInitialize)pb).BeginInit();

                // Properties
                pb.Location = new Point(positions[i, 0], positions[i, 1]);
                pb.Margin = new Padding(10, 9, 10, 9);
                pb.Size = new Size(713, 370);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Name = "A" + i.ToString();
                pb.Image = paths.LoadPicture(pb.Name);
                // pb.TabIndex = ... -> TabStop = true; (Default Wert)
                pb.TabStop = false;
                pb.AllowDrop = true;

                // Actions
                pb.DoubleClick += new EventHandler(Pb_DoubleClick);
                pb.MouseClick += new MouseEventHandler(Pb_Click);
                pb.MouseDown += new MouseEventHandler(Pb_MouseDown);
                pb.MouseEnter += new EventHandler(Pb_MouseEnter);
                pb.MouseLeave += new EventHandler(Pb_MouseLeave);
                pb.DragDrop += new DragEventHandler(Pb_DragDrop);
                pb.DragEnter += new DragEventHandler(Pb_DragEnter);
                // pb.MouseLeave += new EventHandler(Pb_MouseLeave);
                // pb.MouseHover += new EventHandler(Pb_MouseHover);

                Controls.Add(pb);

                ((System.ComponentModel.ISupportInitialize)pb).EndInit();
            }
            ResumeLayout(false);
        }

        private int[,] InitializePositions()
        {
            int[,] positions = new int[9, 2];
            // ToDo: positions "automatisch" befüllen?
            positions[0, 0] = 19;
            positions[0, 1] = 14;
            positions[1, 0] = 19;
            positions[1, 1] = 421;
            positions[2, 0] = 19;
            positions[2, 1] = 840;
            positions[3, 0] = 779;
            positions[3, 1] = 14;
            positions[4, 0] = 779;
            positions[4, 1] = 421;
            positions[5, 0] = 779;
            positions[5, 1] = 840;
            positions[6, 0] = 1536;
            positions[6, 1] = 14;
            positions[7, 0] = 1536;
            positions[7, 1] = 421;
            positions[8, 0] = 1536;
            positions[8, 1] = 840;
            return positions;
        }

        // ToDo: positions als Array vom Typ Point?
        int[,] positions;
        int selDifWidth;
        int selDifHeight;
        PictureBox A0 = new PictureBox();
        PictureBox A1 = new PictureBox();
        PictureBox A2 = new PictureBox();
        PictureBox A3 = new PictureBox();
        PictureBox A4 = new PictureBox();
        PictureBox A5 = new PictureBox();
        PictureBox A6 = new PictureBox();
        PictureBox A7 = new PictureBox();
        PictureBox A8 = new PictureBox();
        PictureBox[] tiles;
        OpenFileDialog PictureDialog;
        OpenFileDialog ApplicationDialog;
        private System.Windows.Forms.Timer StartupAnimationTimer;
    }
}
