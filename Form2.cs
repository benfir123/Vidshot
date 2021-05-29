using System;
using System.Drawing;
using System.Windows.Forms;
using VisioForge.Types.OutputFormat;

namespace Vidshot
{
    public partial class Form2 : Form
    {
        public Form2()
        {

            InitializeComponent();
            this.Opacity = .7D;
            Console.WriteLine(window.X);

        }

        bool mouseDown = false;
        bool PointExists = false;
        Point mouseDownPoint = Point.Empty;
        Point mousePoint = Point.Empty;
        Rectangle window;


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
                //ToolStrip.Visible = false;

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

                ToolStrip.Location = new Point(window.X + window.Width - 111, window.Y + window.Height);












            }




        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            ToolStrip.Visible = true;
        }

        private void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            string itemName = e.ClickedItem.Name;

            switch (itemName)
            {
                case "toolStripButton1":

                    this.Close();
                    MessageBox.Show("Clicked");

                    videoCapture1.Screen_Capture_Source = new VisioForge.Types.Sources.ScreenCaptureSourceSettings { FullScreen = true };
                    videoCapture1.Audio_PlayAudio = videoCapture1.Audio_RecordAudio = false;
                    videoCapture1.Output_Filename = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\output.mp4";
                    videoCapture1.Output_Format = new VFMP4v8v10Output();
                    videoCapture1.Mode = VisioForge.Types.VFVideoCaptureMode.ScreenCapture;

                    videoCapture1.Start();


                    break;

                case "SaveBtn":

                    videoCapture1.Stop();

                    SaveFileDialog sfd = new();
                    sfd.CheckPathExists = true;
                    sfd.FileName = "Capture";
                    sfd.Filter = "PNG Image(*.png)|*.png|JPG Image(*.jpg)|*.jpg|BMP Image(*.bmp)|*.bmp";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        //pdCapture.Image.Save(sfd.FileName);
                    }

                    break;
            }

        }
    }
}
