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
            PictureDialog = new OpenFileDialog();
            ApplicationDialog = new OpenFileDialog();
            SuspendLayout();
            // 
            // PictureDialog
            // 
            PictureDialog.Filter = "Bilder| *.png; *.jpg; *.jpeg";
            PictureDialog.Title = "Choose Picture";
            // 
            // ApplicationDialog
            // 
            ApplicationDialog.Filter = "Executables (*.exe)|*.exe";
            ApplicationDialog.Title = "Choose Application";
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
        }

        private void InitializePictureBoxes(Paths paths)
        {
            SuspendLayout();
            var tilePaths = paths.LoadTilePaths();
            tiles = new PictureBox[] { A0, A1, A2, A3, A4, A5, A6, A7, A8 };
            positions = InitializePositions();
            for (int i = 0; i < tiles.Length; i++)
            {
                PictureBox pb = new PictureBox();
                tiles[i] = pb;
                ((System.ComponentModel.ISupportInitialize)pb).BeginInit();

                // Properties
                pb.Location = new Point(positions[i, 0], positions[i, 1]);
                pb.Margin = new Padding(10, 9, 10, 9);
                pb.Size = new Size(713, 370);
                pb.SizeMode = PictureBoxSizeMode.Zoom;
                pb.Name = "A" + i.ToString();
                TilePaths? tp;
                if (tilePaths.TryGetValue(pb.Name, out tp)) pb.Image = Image.FromFile(tp.PicturePath);
                else pb.Image = Image.FromFile(paths.GetBasicPicPath());
                // pb.TabIndex = ... -> TabStop = true; (Default Wert)
                pb.TabStop = false;
                pb.AllowDrop = true;

                // Actions
                pb.DoubleClick += new EventHandler(Pb_DoubleClick);
                pb.MouseClick += new MouseEventHandler(Pb_Click);
                pb.MouseDown += new MouseEventHandler(Pb_MouseDown);
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
        private PictureBox A0;
        private PictureBox A1;
        private PictureBox A2;
        private PictureBox A3;
        private PictureBox A4;
        private PictureBox A5;
        private PictureBox A6;
        private PictureBox A7;
        private PictureBox A8;
        PictureBox[] tiles;
        OpenFileDialog PictureDialog;
        OpenFileDialog ApplicationDialog;
    }
}
