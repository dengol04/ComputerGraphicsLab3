using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompGrLab3
{
    public partial class Form2 : Form
    {
        private Bitmap bmp;
        private Point startPoint;
        private bool isFirstPointSet = false;

        private const int GridSize = 10;

        private enum DrawingAlgorithm
        {
            None,
            Bresenham,
            Wu
        }

        private DrawingAlgorithm currentAlgorithm = DrawingAlgorithm.None;

        public Form2()
        {
            InitializeComponent();

            this.Load += Form2_Load;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                DrawGrid(g);
            }
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isFirstPointSet)
            {
                startPoint = e.Location;
                isFirstPointSet = true;
            }
            else
            {
                Point endPoint = e.Location;

                switch (currentAlgorithm)
                {
                    case DrawingAlgorithm.Bresenham:
                        Bresenham(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, Color.Black);
                        break;
                    case DrawingAlgorithm.Wu:
                        Wu(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y, Color.Blue);
                        break;
                }

                isFirstPointSet = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentAlgorithm = DrawingAlgorithm.Bresenham;
            isFirstPointSet = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentAlgorithm = DrawingAlgorithm.Wu;
            isFirstPointSet = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                DrawGrid(g);
            }
            pictureBox1.Invalidate();
            isFirstPointSet = false;
        }

        // МЕТОДЫ РИСОВАНИЯ

        private void DrawGrid(Graphics g)
        {
            using (Pen gridPen = new Pen(Color.LightGray))
            {
                for (int x = 0; x < bmp.Width; x += GridSize)
                {
                    g.DrawLine(gridPen, x, 0, x, bmp.Height);
                }

                for (int y = 0; y < bmp.Height; y += GridSize)
                {
                    g.DrawLine(gridPen, 0, y, bmp.Width, y);
                }
            }
        }


        private void Bresenham(int x0, int y0, int x1, int y1, Color color)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = (x0 < x1) ? 1 : -1;
            int sy = (y0 < y1) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                if (x0 >= 0 && y0 >= 0 && x0 < bmp.Width && y0 < bmp.Height)
                    bmp.SetPixel(x0, y0, color);

                if ((x0 == x1) && (y0 == y1)) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            pictureBox1.Invalidate();
        }

        private void Wu(float x0, float y0, float x1, float y1, Color color)
        {
            bool vertically = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (vertically)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            float dx = x1 - x0;
            float dy = y1 - y0;
            float gradient = dy / dx;

            int x_start = (int)Math.Round(x0);
            float y = y0 + gradient * (x_start - x0);

            for (int x = x_start; x <= (int)x1; x++)
            {
                if (vertically)
                {
                    DrawPixel((int)y, x, 1 - (y - (int)y), color);
                    DrawPixel((int)y + 1, x, y - (int)y, color);
                }
                else
                {
                    DrawPixel(x, (int)y, 1 - (y - (int)y), color);
                    DrawPixel(x, (int)y + 1, y - (int)y, color);
                }
                y += gradient;
            }

            pictureBox1.Invalidate();
        }

        private void DrawPixel(int x, int y, float intensity, Color color)
        {
            if (x < 0 || y < 0 || x >= bmp.Width || y >= bmp.Height) return;
            intensity = Math.Max(0, Math.Min(1, intensity));
            int alpha = (int)(intensity * 255);
            bmp.SetPixel(x, y, Color.FromArgb(alpha, color));
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
