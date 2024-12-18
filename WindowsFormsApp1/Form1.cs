using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Stitching;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV.Util;
using Emgu.CV.UI;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<string> selectedImagePaths;

        public Form1()
        {
            InitializeComponent();

            selectedImagePaths = new List<string>();

            openDialog();
        }

        private void openDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Title = "Select images",
                Multiselect = true 
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePaths.AddRange(openFileDialog.FileNames);

                LoadImagesToPictureBoxes();
            }
            else
            {
                MessageBox.Show("No images selected. Closing the application");
                button1.Visible = false;
            }
        }
         
        private void LoadImagesToPictureBoxes()
        {
            while (selectedImagePaths.Count < 3)
            {
                MessageBox.Show("Please select at least 3 images");
                selectedImagePaths.Clear();
                openDialog();
               
            }

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Image = Image.FromFile(selectedImagePaths[0]);

            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Image = Image.FromFile(selectedImagePaths[1]);

            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.Image = Image.FromFile(selectedImagePaths[2]);
        }

    private void StitchingImages(List<Mat> imgs)
        {
            foreach (Mat img in imgs)
            {
                if (img.IsEmpty)
                {
                    Console.WriteLine("Can't read an empty image");
                    return;
                }
            }

            Stitcher.Mode mode = Stitcher.Mode.Panorama;
            Mat pano = new Mat();
            //Stitcher stitcher = Stitcher.Create(mode);
            Stitcher stitcher = new Stitcher(mode);

            VectorOfMat vectorOfMat = new VectorOfMat(imgs.ToArray());
            Stitcher.Status status = stitcher.Stitch(vectorOfMat, pano);

            if (status != Stitcher.Status.Ok)
            {
                Console.WriteLine("Can't stitch images");
                label1.Text = "Can't stitch this images";
                return;
            }
            Mat stitchMat=new Mat();

            CvInvoke.Resize(pano, stitchMat, new System.Drawing.Size(500, 300), 0, 0, Inter.Linear);

            CvInvoke.Imwrite("result.jpg", pano);
            CvInvoke.Imshow("stitchingImage", stitchMat);
            CvInvoke.WaitKey(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Mat> imgs = new List<Mat>();


            foreach (string path in selectedImagePaths)
            {
                Mat mat = CvInvoke.Imread(path, ImreadModes.Color);
                imgs.Add(mat);
            }

            StitchingImages(imgs);
        }

    }
}
