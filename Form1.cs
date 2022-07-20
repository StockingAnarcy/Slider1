using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Slider1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int ImageNumber = 1;
        private void LoadNextImages()
        {
            timer1.Start();
            ImageNumber++;
            if(ImageNumber > 5)
            {
                ImageNumber = 1;
            }
            pictureBox1.ImageLocation = string.Format(@"C:\Project\Slider1\Images\"+ImageNumber+".jpg");
        }

        private void LoadPreviousImages()
        {
            timer1.Start();
            ImageNumber--;
            if (ImageNumber > 1)
            {
                ImageNumber = 5;
            }
            pictureBox1.ImageLocation = string.Format(@"C:\Project\Slider1\Images\" + ImageNumber + ".jpg");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImages();
        }
    }
}
