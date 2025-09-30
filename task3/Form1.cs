using System;
using System.Drawing;
using System.Windows.Forms;

namespace task3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.ClientSize = new Size(600, 400);
            this.Text = "Градиент по барицентрическим координатам";
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);

            PointF v0 = new PointF(100, 300);
            PointF v1 = new PointF(500, 250);
            PointF v2 = new PointF(250, 50);

            Color c0 = Color.Red;
            Color c1 = Color.Green;
            Color c2 = Color.Blue;

            RasterizeTriangle_Barycentric(bmp, v0, v1, v2, c0, c1, c2);

            e.Graphics.DrawImage(bmp, 0, 0);
        }

        private void RasterizeTriangle_Barycentric(Bitmap bmp, PointF v0, PointF v1, PointF v2, Color c0, Color c1, Color c2)
        {
            // Ограничивающий прямоугольник
            int minX = (int)Math.Max(0, Math.Min(v0.X, Math.Min(v1.X, v2.X)));
            int maxX = (int)Math.Min(bmp.Width - 1, Math.Max(v0.X, Math.Max(v1.X, v2.X)));
            int minY = (int)Math.Max(0, Math.Min(v0.Y, Math.Min(v1.Y, v2.Y)));
            int maxY = (int)Math.Min(bmp.Height - 1, Math.Max(v0.Y, Math.Max(v1.Y, v2.Y)));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var bc = Barycentric(x + 0.5f, y + 0.5f, v0, v1, v2);
                    float a = bc.Item1, b = bc.Item2, c = bc.Item3;

                    if (a >= 0 && b >= 0 && c >= 0) // точка внутри треугольника
                    {
                        int r = Clamp((int)(a * c0.R + b * c1.R + c * c2.R));
                        int g = Clamp((int)(a * c0.G + b * c1.G + c * c2.G));
                        int bcol = Clamp((int)(a * c0.B + b * c1.B + c * c2.B));

                        bmp.SetPixel(x, y, Color.FromArgb(r, g, bcol));
                    }
                }
            }
        }

        private Tuple<float, float, float> Barycentric(float px, float py, PointF v0, PointF v1, PointF v2)
        {
            float x0 = v0.X, y0 = v0.Y;
            float x1 = v1.X, y1 = v1.Y;
            float x2 = v2.X, y2 = v2.Y;

            float abcS = (y1 - y2) * (x0 - x2) + (x2 - x1) * (y0 - y2);
            if (Math.Abs(abcS) < 1e-5)
                return Tuple.Create(-1f, -1f, -1f);

            float a = ((y1 - y2) * (px - x2) + (x2 - x1) * (py - y2)) / abcS;
            float b = ((y2 - y0) * (px - x2) + (x0 - x2) * (py - y2)) / abcS;
            float c = 1 - a - b;

            return Tuple.Create(a, b, c);
        }

        private int Clamp(int val)
        {
            if (val < 0)
                return 0;
            else if (val > 255)
                return 255;
            else
                return val;
        }

    }
}
