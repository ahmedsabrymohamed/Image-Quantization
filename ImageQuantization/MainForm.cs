using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageQuantization
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();

        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
           
            //----------------------------------------
            

            //MST2 tree = new MST2(c.count);
            //tree.cal_weight(c.colors);
            // MessageBox.Show(tree.printMinCostEdges().ToString());
            //---------------------------------------

            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            int K=Convert.ToInt32( textBox1.Text.ToString());
            distinct_colors c = new distinct_colors();
            c.get_colors(ImageMatrix);
            MessageBox.Show(c.count.ToString());
            MST tree = new MST(c.count, c.colors);
            tree.MST_Construct();
            tree.total_cost();
            double d = tree.cost;
            MessageBox.Show(d.ToString());
            Cluster cc = new Cluster(tree.L, c.colors, K);
            cc.function();
            cc.Color_Pallette();
            cc.Quantize_the_image(ImageMatrix);
            //----------------------------------------------------------
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            //----------------------------------------------------------
            //-------------------------------------------------
            var fd = new SaveFileDialog();
            fd.Filter = "Bmp(*.BMP;)|*.BMP;| Jpg(*Jpg)|*.jpg";
            fd.AddExtension = true;
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (System.IO.Path.GetExtension(fd.FileName).ToUpper())
                {
                    case ".BMP":
                        pictureBox2.Image.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case ".JPG":
                        pictureBox2.Image.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".PNG":
                        pictureBox2.Image.Save(fd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    default:
                        break;
                }
            }
            //-------------------------------------------------
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}