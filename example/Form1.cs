using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            byte[,] image_matr; // матрица полутонового изображения
            int[] hist; //гистограмма
            double[] ver; //вероятностная гистограмма
            double[,] obr_image; // матрица обработанного изображения
            double[] hist_new; //гистограмма
            int gmax, gmin; //желаемое максимальное и минимальное значение яркости
            int fmax, fmin; //максимальное и минимальное значение яркости изображения
            int w_b, h_b; //ширина и высота изображения


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
            ofd.Title = "Выберите файл изображения";

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(@ofd.FileName);
            }

            Bitmap ish_bitmap = (Bitmap)pictureBox1.Image;

            w_b = ish_bitmap.Width;  //Ширина изображения
            h_b = ish_bitmap.Height; //Высота изображения

            image_matr = new byte[w_b, h_b];  //матрица изображения 
            obr_image = new double[w_b, h_b];  //матрица изображения 

            hist = new int[256];
            hist_new = new double[256];

            fmin = 1000;
            fmax = -100;

            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    System.Drawing.Color c = ish_bitmap.GetPixel(x, y);//получаем цвет указанной точки
                    int r = Convert.ToInt32(c.R);
                    int b = Convert.ToInt32(c.B);
                    int g = Convert.ToInt32(c.G);
                    int brit = Convert.ToInt32(0.299 * r + 0.587 * g + 0.114 * b); //Перевод из RGB в полутон

                    image_matr[x, y] = Convert.ToByte(brit);
                }
            }

            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    hist[image_matr[x, y]]++;

                    if (image_matr[x, y] > fmax)
                        fmax = image_matr[x, y];
                    if (image_matr[x, y] < fmin)
                        fmin = image_matr[x, y];
                }
            }


            label1.Text = Convert.ToString(fmax); //Максимальная яркость
            label2.Text = Convert.ToString(fmin); //Минимальная яркость


            //Отображение гистограммы на компоненте chart
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart1.Series[0].Points.AddXY(i, hist[i]);
            }

            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    if (obr_image[x, y] > 255)
                        obr_image[x, y] = 255;
                    if (obr_image[x, y] < 0)
                        obr_image[x, y] = 0;
                }
            }

            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    obr_image[x, y] = (double)(255 - image_matr[x, y]);
                }
            }

            Bitmap obr = (Bitmap)pictureBox1.Image;
            for (int x = 0; x < w_b; x++)
            {
                for (int y = 0; y < h_b; y++)
                {
                    byte briteness = Convert.ToByte(obr_image[x, y]);
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(briteness, briteness, briteness);
                    obr.SetPixel(x, y, c);
                }
            }

        }
        //private void OpenButton_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog ofd = new OpenFileDialog();
        //    ofd.Filter = "Image Files (*.BMP; *.JPG; *.GIF; *.PNG) | *.BMP; *.JPG; *.GIF; *.PNG | All Files(*.*)|*.*";

        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        try
        //        {
        //            pictureBox1.Image = new Bitmap(ofd.FileName);
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }


            }
        }
