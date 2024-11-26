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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Mat mat = CvInvoke.Imread("C:\\Users\\The user\\Downloads\\IMG20241125150920.jpg", ImreadModes.Color);
            Mat mat2 = CvInvoke.Imread("C:\\Users\\The user\\Downloads\\IMG20241125150917.jpg", ImreadModes.Color);
            Mat mat3 = CvInvoke.Imread("C:\\Users\\The user\\Downloads\\IMG20241125150915.jpg", ImreadModes.Color);

            List<Mat> imgs = new List<Mat>();
            imgs.Add(mat);
            imgs.Add(mat2);
            imgs.Add(mat3);

            StitchingImages(imgs);

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
                return;
            }

            CvInvoke.Imwrite("result.jpg", pano);
            CvInvoke.Imshow("stitchingImage", pano);
            CvInvoke.WaitKey(0);
        }


    }
}
