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
            controllerTimer = new System.Windows.Forms.Timer(components);
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
            // controllerTimer
            // 
            controllerTimer.Interval = 50;
            // 
            // Shortcuts
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            ClientSize = new Size(640, 360);
            BackColor = Color.White;
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
            InitializeComponent();
            InitializePictureBoxes(paths);
            ScaleAndPositionWindow();
        }

        private void InitializePictureBoxes(Paths paths)
        {
            SuspendLayout();
            tiles = [A0, A1, A2, A3, A4, A5, A6, A7, A8];
            for (int i = 0; i < tiles.Length; i++)
            {
                PictureBox pb = tiles[i];
                ((System.ComponentModel.ISupportInitialize)pb).BeginInit();

                // Properties
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
            int leftMargin = (int)Math.Ceiling(0.01 * Size.Width); // ToDo: alt name "widthMargin"?
            int topMargin = (int)Math.Ceiling(0.01 * Size.Height);// ToDo: alt name "heightMargin"?
            for (int i = 0; i < tiles.Length; i++)
            {
                positions[i, 0] = leftMargin + Size.Width * (i % 3) / 3;
                positions[i, 1] = topMargin + Size.Height * ((i / 3) % 3) / 3;
            }
            return positions;
        }

        private void ScaleAndPositionWindow() // ToDo: alt name PositionAndScaleEverything?
        {
            SuspendLayout();
            Size = new Size((int)(Screen.PrimaryScreen.WorkingArea.Width / 2.5), (int)(Screen.PrimaryScreen.WorkingArea.Height / 2.5));
            Top = (int)Math.Ceiling(0.02 * Size.Height);
            Left = Screen.PrimaryScreen.WorkingArea.Width - Size.Width - Top;

            selDifWidth = (int)(Size.Width * 0.015);
            selDifHeight = (int)(Size.Height * 0.015);

            positions = InitializePositions();
            for (int i = 0; i < tiles.Length; i++)
            {
                PictureBox pb = tiles[i];
                pb.Size = new Size(Size.Width / 3 - (int)Math.Ceiling(0.01 * Size.Width) * 3, Size.Height / 3 - (int)Math.Ceiling(0.01 * Size.Height) * 3);
                pb.Location = new Point(positions[i, 0], positions[i, 1]);
            }

            pbMinWidth = A0.Width;
            pbMaxWidth = A0.Width + selDifWidth;
            ResumeLayout(false);
        }

        // ToDo: positions als Array vom Typ Point?
        int[,] positions;
        int selDifWidth;
        int selDifHeight;
        int pbMinWidth;
        int pbMaxWidth;
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
        private System.Windows.Forms.Timer controllerTimer;
    }
}
