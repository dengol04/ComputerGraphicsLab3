using System;
using System.Drawing;
using System.Windows.Forms;

namespace task3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.ClientSize = new Size(800, 400);
            this.Text = "Градиент по барицентрическим координатам";
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);

            //Левый треугольник (метод площадей)
            PointF v0 = new PointF(100, 300);
            PointF v1 = new PointF(300, 250);
            PointF v2 = new PointF(150, 50);

            //Правый треугольник (метод уравнений)
            PointF u0 = new PointF(450, 300);
            PointF u1 = new PointF(650, 250);
            PointF u2 = new PointF(500, 50);

            Color c0 = Color.Red;
            Color c1 = Color.Green;
            Color c2 = Color.Blue;

            RasterizeTriangle_Barycentric_Area(bmp, v0, v1, v2, c0, c1, c2);
            RasterizeTriangle_Barycentric_Equation(bmp, u0, u1, u2, c0, c1, c2);

            e.Graphics.DrawImage(bmp, 0, 0);
            e.Graphics.DrawString("Метод 1: площади", this.Font, Brushes.Black, 100, 10);
            e.Graphics.DrawString("Метод 2: уравнения", this.Font, Brushes.Black, 500, 10);
        }

        //через площади
        private void RasterizeTriangle_Barycentric_Area(Bitmap bmp, PointF v0, PointF v1, PointF v2, Color c0, Color c1, Color c2)
        {
            int minX = (int)Math.Max(0, Math.Min(v0.X, Math.Min(v1.X, v2.X)));
            int maxX = (int)Math.Min(bmp.Width - 1, Math.Max(v0.X, Math.Max(v1.X, v2.X)));
            int minY = (int)Math.Max(0, Math.Min(v0.Y, Math.Min(v1.Y, v2.Y)));
            int maxY = (int)Math.Min(bmp.Height - 1, Math.Max(v0.Y, Math.Max(v1.Y, v2.Y)));

            float abcS = TriangleArea(v0, v1, v2);
            if (Math.Abs(abcS) < 1e-6f) 
                return;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    PointF p = new PointF(x + 0.5f, y + 0.5f);

                    float a = TriangleArea(p, v1, v2) / abcS;
                    float b = TriangleArea(p, v2, v0) / abcS;
                    float c = TriangleArea(p, v0, v1) / abcS;

                    if (a >= 0 && b >= 0 && c >= 0)
                    {
                        int r = Clamp((int)(a * c0.R + b * c1.R + c * c2.R));
                        int g = Clamp((int)(a * c0.G + b * c1.G + c * c2.G));
                        int bcol = Clamp((int)(a * c0.B + b * c1.B + c * c2.B));

                        bmp.SetPixel(x, y, Color.FromArgb(r, g, bcol));
                    }
                }
            }
        }

        private float TriangleArea(PointF A, PointF B, PointF C)
        {
            return (B.X - A.X) * (C.Y - A.Y) - (C.X - A.X) * (B.Y - A.Y);
        }

        //Через уравнение
        private void RasterizeTriangle_Barycentric_Equation(Bitmap bmp, PointF v0, PointF v1, PointF v2, Color c0, Color c1, Color c2)
        {
            int minX = (int)Math.Max(0, Math.Min(v0.X, Math.Min(v1.X, v2.X)));
            int maxX = (int)Math.Min(bmp.Width - 1, Math.Max(v0.X, Math.Max(v1.X, v2.X)));
            int minY = (int)Math.Max(0, Math.Min(v0.Y, Math.Min(v1.Y, v2.Y)));
            int maxY = (int)Math.Min(bmp.Height - 1, Math.Max(v0.Y, Math.Max(v1.Y, v2.Y)));

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var bc = BarycentricEquation(x + 0.5f, y + 0.5f, v0, v1, v2);
                    float a = bc.Item1, b = bc.Item2, c = bc.Item3;

                    if (a >= 0 && b >= 0 && c >= 0)
                    {
                        int r = Clamp((int)(a * c0.R + b * c1.R + c * c2.R));
                        int g = Clamp((int)(a * c0.G + b * c1.G + c * c2.G));
                        int bcol = Clamp((int)(a * c0.B + b * c1.B + c * c2.B));

                        bmp.SetPixel(x, y, Color.FromArgb(r, g, bcol));
                    }
                }
            }
        }

        private Tuple<float, float, float> BarycentricEquation(float px, float py, PointF v0, PointF v1, PointF v2)
        {
            float denom = (v1.Y - v2.Y) * (v0.X - v2.X) + (v2.X - v1.X) * (v0.Y - v2.Y);
            if (Math.Abs(denom) < 1e-6f)
                return Tuple.Create(-1f, -1f, -1f);

            float a = ((v1.Y - v2.Y) * (px - v2.X) + (v2.X - v1.X) * (py - v2.Y)) / denom;
            float b = ((v2.Y - v0.Y) * (px - v2.X) + (v0.X - v2.X) * (py - v2.Y)) / denom;
            float c = 1 - a - b;

            return Tuple.Create(a, b, c);
        }

        private int Clamp(int val)
        {
            if (val < 0) 
                return 0;
            if (val > 255) 
                return 255;
            return val;
        }
    }
}
