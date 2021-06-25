using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Types.OutputFormat;
using VisioForge.Controls.VideoCapture;
using VisioForge.Controls.UI.WinForms;


namespace Vidshot
{
    public partial class Form2 : Form
    {

        public Form2()
        {

            InitializeComponent();
            this.Opacity = .5D;
        }
        
        bool mouseDown = false;
        bool PointExists = false;
        Point mouseDownPoint = Point.Empty;
        Point mousePoint = Point.Empty;
        Rectangle window;
        VideoCaptureCore VideoCapture1 = new();
        private void Form2_Load(object sender, EventArgs e)
        {
            ToolStrip.Visible = true;
        }

        private void Form2_Click(object sender, EventArgs e)
        {
            ToolStrip.Visible = true;

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mouseDown = true;
            mousePoint = mouseDownPoint = e.Location;
            PointExists = false;

        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mouseDown = false;
            PointExists = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (PointExists == false)
            {
                base.OnMouseMove(e);
                mousePoint = e.Location;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Region r = new();
            if (mouseDown)
            {
                this.DoubleBuffered = true;
                this.BackColor = Color.White;
                this.TransparencyKey = Color.White;
                ToolStrip.Visible = false;

                window = new(
                    Math.Min(mouseDownPoint.X, mousePoint.X),
                    Math.Min(mouseDownPoint.Y, mousePoint.Y),
                    Math.Abs(mouseDownPoint.X - mousePoint.X),
                    Math.Abs(mouseDownPoint.Y - mousePoint.Y));
                r.Xor(window);

                e.Graphics.FillRegion(Brushes.Black, r);

                Console.WriteLine("Painted: " + window);

                ControlPaint.DrawReversibleFrame(window, Color.FromArgb(80, 120, 120, 120), FrameStyle.Dashed);
            }

            if (PointExists)
            {

                window = new(
                          Math.Min(mouseDownPoint.X, mousePoint.X),
                          Math.Min(mouseDownPoint.Y, mousePoint.Y),
                          Math.Abs(mouseDownPoint.X - mousePoint.X),
                          Math.Abs(mouseDownPoint.Y - mousePoint.Y));
                r.Xor(window);
                e.Graphics.FillRegion(Brushes.Black, r);

                ControlPaint.DrawReversibleFrame(window, Color.FromArgb(80, 120, 120, 120), FrameStyle.Dashed);
                ToolStrip.Location = new Point(window.X + window.Width - 125, window.Y + window.Height);
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            ToolStrip.Visible = true;
            mousePoint = e.Location;
        }

        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string itemName = e.ClickedItem.Name;
            bool chkFullScreen = window.Top == 0 ? true : false;
            if (chkFullScreen)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            switch (itemName)
            {    
                case "btnRecord":
                    VideoCapture1.Screen_Capture_Source = new VisioForge.Types.Sources.ScreenCaptureSourceSettings()
                    {

                        FullScreen = chkFullScreen,
                        Top = window.Top,
                        Bottom = window.Bottom,
                        Right = window.Right,
                        Left = window.Left,
                    };
                    VideoCapture1.Audio_PlayAudio = VideoCapture1.Audio_RecordAudio = false;
                    VideoCapture1.Output_Format = new VFMP4v11Output();
                    VideoCapture1.Output_Filename = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\output.mp4";
                    VideoCapture1.Mode = VisioForge.Types.VFVideoCaptureMode.ScreenCapture;
                    VideoCapture1.Start();
                    break;
                case "btnStop":
                    VideoCapture1.Stop();
                    this.Close();
                    break;
            }
        }
    }
}
