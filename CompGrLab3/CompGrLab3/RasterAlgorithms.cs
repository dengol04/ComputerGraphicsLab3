using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace CompGrLab3
{
    internal class RasterAlgorithms
    {
        public static void FillAreaScanline(Bitmap bmp, Point startPoint, Color fillColor)
        {
            if (startPoint.X < 0 || startPoint.Y < 0 || startPoint.X >= bmp.Width || startPoint.Y >= bmp.Height)
                return;

            Color targetColor = bmp.GetPixel(startPoint.X, startPoint.Y);

            if (targetColor.ToArgb() == fillColor.ToArgb())
                return;

            Stack<Point> stack = new Stack<Point>();
            stack.Push(startPoint);

            int w = bmp.Width;
            int h = bmp.Height;

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int y = p.Y;
                int x = p.X;

                if (x < 0 || x >= w || y < 0 || y >= h) continue;

                if (bmp.GetPixel(x, y).ToArgb() != targetColor.ToArgb()) continue;

                int x_left = x;
                while (x_left >= 0 && bmp.GetPixel(x_left, y).ToArgb() == targetColor.ToArgb())
                    x_left--;
                x_left++;

                int x_right = x;
                while (x_right < w && bmp.GetPixel(x_right, y).ToArgb() == targetColor.ToArgb())
                    x_right++;
                x_right--;

                for (int i = x_left; i <= x_right; i++)
                    bmp.SetPixel(i, y, fillColor);

                if (y > 0)
                    CheckAndPushNeighbors(bmp, x_left, x_right, y - 1, targetColor, stack);

                if (y < h - 1)
                    CheckAndPushNeighbors(bmp, x_left, x_right, y + 1, targetColor, stack);
            }
        }

        private static void CheckAndPushNeighbors(Bitmap bmp, int x_start, int x_end, int y_neighbor, Color targetColor, Stack<Point> stack)
        {
            bool inTargetArea = false;
            for (int x = x_start; x <= x_end; x++)
            {
                bool isTarget = (bmp.GetPixel(x, y_neighbor).ToArgb() == targetColor.ToArgb());

                if (isTarget && !inTargetArea)
                {
                    stack.Push(new Point(x, y_neighbor));
                    inTargetArea = true;
                }
                else if (!isTarget)
                    inTargetArea = false;
            }
        }

        public static void FillAreaWithTextureScanline(Bitmap bmp, Point startPoint, Bitmap textureBitmap)
        {
            if (textureBitmap == null) return;
            if (startPoint.X < 0 || startPoint.Y < 0 || startPoint.X >= bmp.Width || startPoint.Y >= bmp.Height)
                return;

            Color targetColor = bmp.GetPixel(startPoint.X, startPoint.Y);

            Stack<Point> stack = new Stack<Point>();
            stack.Push(startPoint);

            int w = bmp.Width;
            int h = bmp.Height;
            int texW = textureBitmap.Width;
            int texH = textureBitmap.Height;

            if (texW <= 0 || texH <= 0) return;

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int y = p.Y;
                int x = p.X;

                if (x < 0 || x >= w || y < 0 || y >= h) continue;
                if (bmp.GetPixel(x, y).ToArgb() != targetColor.ToArgb()) continue;

                int x_left = x;
                while (x_left >= 0 && bmp.GetPixel(x_left, y).ToArgb() == targetColor.ToArgb()) x_left--;
                x_left++;

                int x_right = x;
                while (x_right < w && bmp.GetPixel(x_right, y).ToArgb() == targetColor.ToArgb()) x_right++;
                x_right--;

                for (int i = x_left; i <= x_right; i++)
                {
                    int offsetX = i - startPoint.X;
                    int offsetY = y - startPoint.Y;

                    int texX = (offsetX % texW + texW) % texW;
                    int texY = (offsetY % texH + texH) % texH;

                    Color fillColor = textureBitmap.GetPixel(texX, texY);

                    bmp.SetPixel(i, y, fillColor);
                }

                if (y > 0)
                    CheckAndPushNeighbors(bmp, x_left, x_right, y - 1, targetColor, stack);

                if (y < h - 1)
                    CheckAndPushNeighbors(bmp, x_left, x_right, y + 1, targetColor, stack);
            }
        }

        public static List<Point> TraceBoundary(Bitmap bmp, Point startPoint, Color borderColor)
        {
            List<Point> boundaryPoints = new List<Point>();

            int[] dx = { 1, 1, 0, -1, -1, -1, 0, 1 }; // R, DR, D, DL, L, UL, U, UR
            int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };

            int w = bmp.Width;
            int h = bmp.Height;

            if (startPoint.X < 0 || startPoint.Y < 0 || startPoint.X >= w || startPoint.Y >= h ||
                bmp.GetPixel(startPoint.X, startPoint.Y).ToArgb() != borderColor.ToArgb())
                return boundaryPoints;

            boundaryPoints.Add(startPoint);

            int prevDir = 4;
            Point currentPoint = startPoint;

            bool foundP1 = false;
            for (int i = 0; i < 8; i++)
            {
                int checkDir = (prevDir + 1 + i) % 8;

                int nextX = currentPoint.X + dx[checkDir];
                int nextY = currentPoint.Y + dy[checkDir];

                if (nextX < 0 || nextX >= w || nextY < 0 || nextY >= h)
                    continue;

                if (bmp.GetPixel(nextX, nextY).ToArgb() == borderColor.ToArgb())
                {
                    currentPoint = new Point(nextX, nextY);
                    boundaryPoints.Add(currentPoint);

                    prevDir = (checkDir + 4) % 8;
                    foundP1 = true;
                    break;
                }
            }

            if (!foundP1)
                return new List<Point>();

            while (currentPoint != startPoint)
            {
                int startDir = (prevDir + 1) % 8;

                bool foundNext = false;
                for (int i = 0; i < 8; i++)
                {
                    int checkDir = (startDir + i) % 8;

                    int nextX = currentPoint.X + dx[checkDir];
                    int nextY = currentPoint.Y + dy[checkDir];

                    if (nextX < 0 || nextX >= w || nextY < 0 || nextY >= h)
                        continue;

                    if (bmp.GetPixel(nextX, nextY).ToArgb() == borderColor.ToArgb())
                    {
                        currentPoint = new Point(nextX, nextY);
                        boundaryPoints.Add(currentPoint);

                        prevDir = (checkDir + 4) % 8;
                        foundNext = true;
                        break;
                    }
                }

                if (!foundNext)
                    break;
            }

            return boundaryPoints;
        }

        public static void DrawBoundary(Bitmap bmp, List<Point> boundaryPoints, Color drawColor)
        {
            if (boundaryPoints.Count < 1) return;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Pen p = new Pen(drawColor, 1.5f))
                {
                    for (int i = 0; i < boundaryPoints.Count - 1; i++)
                        g.DrawLine(p, boundaryPoints[i], boundaryPoints[i + 1]);

                    if (boundaryPoints.Count > 1)
                        g.DrawLine(p, boundaryPoints[boundaryPoints.Count - 1], boundaryPoints[0]);
                }
            }
        }
    }
}
