using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace mypaint
{
    public partial class Form1 : Form
    {
        private bool checkmouse = false;
        private class ArPoint
        {
            private int index = 0;
            private Point[] points;
            public ArPoint(int size)
            {
                if (size <= 0)
                {
                    size = 2;
                }
                points = new Point[size];
            }

            public void setpoint(int x, int y)
            {
                if (index >= points.Length)
                {
                    index = 0;
                }
                points[index] = new Point(x, y);
                index++;

            }
            public void resetpoint()
            {
                index = 0;
            }
            public int getCountspoint()
            {
                return index;
            }
            public Point[] GetPoints()
            {
                return points;
            }
        }
        public Form1()
        {
            
            InitializeComponent();
            SetSize();
           
        }


        private ArPoint arraypoints = new ArPoint(2);
        Rectangle rectangle = Screen.PrimaryScreen.Bounds;
      
        Bitmap map;
        Graphics graphics;

        Pen penfill=new Pen(Color.Red);
        Pen pen = new Pen(Color.Black, 3f);
        bool line = false;
        bool rec = false;
        bool oval = false;
        bool checkrez = false;
        bool checktrinager = false;
        int Xclick = 0, Yclick = 0;
        bool check_fill = false;
        Point end;
        Point f;
        Point m;
       
      

        public void Fill(int x, int y)
        {

            
            if (pictureBox1.Image == null)
            {
                return;
            }
            if (x >= pictureBox1.Width - 1 || x <= 1 || y >= pictureBox1.Height - 1 || y <= 1) { return; }

            Bitmap b = (Bitmap)pictureBox1.Image;
            graphics.DrawLine(penfill, x, y, x + 1, y + 0.5f);

            

                if (b.GetPixel(x, y).ToArgb() != pen.Color.ToArgb() && b.GetPixel(x + 1, y).ToArgb() != penfill.Color.ToArgb())
                {
                    Fill(x + 1, y);
                }
                if (b.GetPixel(x - 1, y).ToArgb() != pen.Color.ToArgb() && b.GetPixel(x - 1, y).ToArgb() != penfill.Color.ToArgb())
                {
                    Fill(x - 1, y);
                }
                if (b.GetPixel(x, y + 1).ToArgb() != pen.Color.ToArgb() && b.GetPixel(x, y + 1).ToArgb() != penfill.Color.ToArgb())
                {
                    Fill(x, y + 1);
                }
                if (b.GetPixel(x, y - 1).ToArgb() != pen.Color.ToArgb() && b.GetPixel(x, y - 1).ToArgb() != penfill.Color.ToArgb())
                {
                    Fill(x, y - 1);
                }
            





        }
        private void SetSize()
        {
            Rectangle rectangle = Screen.PrimaryScreen.Bounds;
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            Color black = Color.Black;
            color.BackColor = black;


        }

        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            checkmouse = true;
            Xclick = e.X;
            Yclick = e.Y;

            if (e.Button == MouseButtons.Right)
            {
                Fill(e.X, e.Y);
                pictureBox1.Invalidate();
                pictureBox1.Image = map;

            }

          
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            if (rec)
            {
                graphics.DrawRectangle(pen, Xclick, Yclick, e.X - Xclick, e.Y - Yclick);
            }
            if (oval)
            {
                graphics.DrawEllipse(pen, Xclick, Yclick, e.X - Xclick, e.Y - Yclick);
            }
            if (line)
            {
                graphics.DrawLine(pen, Xclick, Yclick, e.X - Xclick, e.Y - Yclick);
            }
            if (checkrez)
            {
                var ma = new Bitmap(rectangle.Width, rectangle.Height);

                checkrez = false;
                var a = (Bitmap)pictureBox1.Image;
                ma = a.Clone(new Rectangle(Xclick, Yclick, e.X, e.Y), PixelFormat.DontCare);
                graphics.Clear(pictureBox1.BackColor);
                graphics.DrawImage(ma, 0, 0);
                pictureBox1.Image = ma;
                 
            }
            if (checktrinager)
            {
                graphics.DrawLine(pen, f, m);
                graphics.DrawLine(pen, f, end);
                graphics.DrawLine(pen, end, m);

            }
            arraypoints.resetpoint();
            rec = false;
            oval = false;
            checkmouse = false;
            line = false;
            checkrez = false;
            checktrinager = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            button1.BackColor = penfill.Color;
            if (checkmouse == false)
            {
                return;
            }



            if (e.Button == MouseButtons.Left)
            {
                Bitmap map1 = new Bitmap(rectangle.Width, rectangle.Height);
                Graphics graphics1 = Graphics.FromImage(map1);


                

                if (rec)
                {

                    graphics1.Clear(pictureBox1.BackColor);
                    graphics1.DrawRectangle(pen, Xclick, Yclick, e.X - Xclick, e.Y - Yclick);
                    graphics1.DrawImage(map, 0, 0);
                    pictureBox1.Image = map1;


                }
                else if (checktrinager)
                {
                    graphics1.Clear(pictureBox1.BackColor);
                     end = PointToClient(MousePosition);
                    double XMid = (Xclick + end.X) / 2;
                     f = new Point(Xclick, end.Y);
                     m = new Point((int)XMid, Yclick);
                    graphics1.DrawLine(pen, f, m);
                    graphics1.DrawLine(pen, f, end);
                    graphics1.DrawLine(pen, end, m);
                    graphics1.DrawImage(map, 0, 0);
                    pictureBox1.Image = map1;


                }
                else if (checkrez)
                {
                    graphics1.Clear(pictureBox1.BackColor);
                }
                else if (oval)
                {

                    graphics1.Clear(pictureBox1.BackColor);
                    graphics1.DrawEllipse(pen, Xclick, Yclick, Math.Abs(e.X - Xclick), Math.Abs(e.Y - Yclick));
                    graphics1.DrawImage(map, 0, 0);
                    pictureBox1.Image = map1;

                }
                
                else if (line)
                {

                    graphics1.Clear(pictureBox1.BackColor);
                    graphics1.DrawLine(pen, Xclick, Yclick, e.X - Xclick, e.Y - Yclick);
                    graphics1.DrawImage(map, 0, 0);
                    pictureBox1.Image = map1;
                }
                else
                {

                    arraypoints.setpoint(e.X, e.Y);//просто рисуем работает
                    if (arraypoints.getCountspoint() >= 2)
                    {
                        graphics.DrawLines(pen, arraypoints.GetPoints());
                        arraypoints.setpoint(e.X, e.Y);
                        pictureBox1.Image = map;
                    }

                

                }
                
            }













        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void color_Click(object sender, EventArgs e)
        {
            if (check_fill == true)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {

                    penfill.Color = colorDialog1.Color;
                    

                }
                check_fill = false;
            }
            else
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {

                    pen.Color = colorDialog1.Color;
                    ((Button)sender).BackColor = colorDialog1.Color;

                }
                check_fill = false;
            }
        }



        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;
        }


      

       

       

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                pen.Color = pictureBox1.BackColor;

            }
            else
            {
                pen.Color = colorDialog1.Color;
            }
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FlowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JPG(*.JPG)|*.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void clearToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            map = new Bitmap(rectangle.Width, rectangle.Height);
            graphics = Graphics.FromImage(map);
            pictureBox1.Image = map;
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var bmp = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName);
                graphics.DrawImage(bmp, 0, 0, bmp.Width, bmp.Height);
                pictureBox1.Image = map;
               
                
                
            }
           

        
        }

       

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += printDocument1_PrintPage;
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDialog1.Document.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
               
        }

        private void button4_Click(object sender, EventArgs e)
        {
            checkrez = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;

            }
            
           Bitmap bitmap_input= new Bitmap(pictureBox1.Image);
            Bitmap bitmap_output = new Bitmap(map.Width,map.Height);
            for (int j = 0; j < pictureBox1.Height; j++)
            {
                for (int i = 0; i < pictureBox1.Width; i++)
                {
              

                    if (map.GetPixel(i, j) != pictureBox1.BackColor)
                    {UInt32 pixel = (UInt32)(bitmap_input.GetPixel(i, j).ToArgb());
                        float R = (float)((pixel & 0x00FF0000) >> 16);
                        float G = (float)((pixel & 0x0000FF00) >> 8);
                        float B = (float)(pixel & 0x000000FF);
                        R = G = B = (R + G + B) / 3.0f;
                        UInt32 p = 0xFF000000 | ((UInt32)R << 16) | ((UInt32)G << 8) | ((UInt32)B);
                        bitmap_output.SetPixel(i, j, Color.FromArgb((int)p));

                    }
                    

                }
            }
            pictureBox1.Image = bitmap_output;
            map = bitmap_output;
           
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            line = true;
        }

        private void кругToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oval = true;
        }

        private void треугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            checktrinager = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_fill = true;
            

        }

        private void прямоугольникToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rec = true;
        }

     

        


    }
}
