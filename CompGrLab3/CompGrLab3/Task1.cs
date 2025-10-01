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
    public partial class Task1 : Form
    {
        private Bitmap _canvas;
        private Graphics _g;
        private Point _lastPoint;

        private Color _drawColor = Color.Black;
        private Color _fillColor = Color.Blue;
        private Color _borderColor = Color.Black;
        private Bitmap _textureBitmap;

        private enum Mode { Draw, FillColor, FillTexture, TraceBoundary };
        private Mode _currentMode = Mode.Draw;

        public Task1()
        {
            InitializeComponent();
        }

        private void Task1_Load(object sender, EventArgs e)
        {
            _canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _g = Graphics.FromImage(_canvas);
            _g.Clear(Color.White);
            pictureBox1.Image = _canvas;

            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            this.Text = "Растровые алгоритмы: Режим Рисования";
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            _currentMode = Mode.Draw;
            this.Text = "Растровые алгоритмы: Режим Рисования (ЛКМ для рисования)";
        }

        private void btnFillColor_Click(object sender, EventArgs e)
        {
            _currentMode = Mode.FillColor;
            this.Text = $"Растровые алгоритмы: Режим Заливки Цветом ({_fillColor.Name}). Кликните внутри области.";
        }

        private void btnFillTexture_Click(object sender, EventArgs e)
        {
            if (_textureBitmap == null)
            {
                MessageBox.Show("Сначала загрузите текстуру с помощью кнопки 'Загрузить текстуру'.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _currentMode = Mode.FillTexture;
            this.Text = "Растровые алгоритмы: Режим Заливки Текстурой. Кликните внутри области.";
        }

        private void btnTraceBoundary_Click(object sender, EventArgs e)
        {
            _currentMode = Mode.TraceBoundary;
            this.Text = $"Растровые алгоритмы: Режим Обхода Границы. Граница цвета ({_borderColor.Name}). Кликните по границе.";
        }

        private void btnChooseFillColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                _fillColor = colorDialog1.Color;
                if (_currentMode == Mode.FillColor)
                {
                    this.Text = $"Растровые алгоритмы: Режим Заливки Цветом ({_fillColor.Name}). Кликните внутри области.";
                }
            }
        }

        private void btnLoadTexture_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Графические файлы (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|Все файлы (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _textureBitmap?.Dispose();
                        _textureBitmap = new Bitmap(ofd.FileName);
                        MessageBox.Show($"Текстура '{System.IO.Path.GetFileName(ofd.FileName)}' загружена ({_textureBitmap.Width}x{_textureBitmap.Height}). При заливке будет использована циклическая (плиточная) заливка.", "Текстура загружена", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        btnFillTexture_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка загрузки текстуры: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_currentMode == Mode.Draw && e.Button == MouseButtons.Left)
                _lastPoint = e.Location;
            else if (e.Button == MouseButtons.Left)
            {
                Point startPoint = e.Location;

                Bitmap tempBmp = (Bitmap)_canvas.Clone();
                bool modified = false;

                if (_currentMode == Mode.FillColor)
                {
                    RasterAlgorithms.FillAreaScanline(tempBmp, startPoint, _fillColor);
                    modified = true;
                }
                else if (_currentMode == Mode.FillTexture && _textureBitmap != null)
                {
                    RasterAlgorithms.FillAreaWithTextureScanline(tempBmp, startPoint, _textureBitmap);
                    modified = true;
                }
                else if (_currentMode == Mode.TraceBoundary)
                {
                    List<Point> boundary = RasterAlgorithms.TraceBoundary(tempBmp, startPoint, _borderColor);

                    if (boundary.Count > 0)
                    {
                        RasterAlgorithms.DrawBoundary(tempBmp, boundary, Color.Red);
                        MessageBox.Show($"Найдено {boundary.Count} точек границы. Граница выделена красным цветом.", "Обход завершен", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show("Не удалось найти замкнутую границу. Убедитесь, что начальная точка находится на пикселе цвета границы.", "Ошибка обхода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    modified = true;
                }

                if (modified)
                {
                    _canvas.Dispose();
                    _canvas = tempBmp;
                    _g.Dispose();
                    _g = Graphics.FromImage(_canvas);
                    pictureBox1.Image = _canvas;
                    pictureBox1.Refresh();
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_currentMode == Mode.Draw && e.Button == MouseButtons.Left)
            {
                _g.DrawLine(new Pen(_drawColor, 1.5f), _lastPoint, e.Location);
                _lastPoint = e.Location;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) { }
    }
}
