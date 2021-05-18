using System;
using System.Drawing;
using System.Windows.Forms;

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
           if(PointExists == false)
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
            Rectangle window;
            


            if (mouseDown)
            {
               this.DoubleBuffered = true;
               this.BackColor = Color.White;
               this.TransparencyKey = Color.White;
               toolStrip1.Visible = false;

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

                toolStrip1.Location = new Point(window.X + window.Width - 111, window.Y + window.Height);
                
                










            }    
        
        


        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            toolStrip1.Visible = true;
        }
    }
}
