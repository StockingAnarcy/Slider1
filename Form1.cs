using System;
using System.IO;
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
        string imagePath = @"./Images/";
        string filePath = @"./";
        string fileName = "demo.txt";

        List<string> lines = new List<string>();
        List<Label> labels = new List<Label>();
        

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
           
            var watcher = new FileSystemWatcher(filePath);
            watcher.EnableRaisingEvents = true;
            watcher.SynchronizingObject = this;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += OnChanged;
            watcher.Filter = "demo.txt";
            
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
            pictureBox1.ImageLocation = string.Format(imagePath + ImageNumber+".jpg");
        }

       /* private void LoadPreviousImages()
        {
            timer1.Start();
            ImageNumber--;
            if (ImageNumber > 1)
            {
                ImageNumber = 5;
            }
            pictureBox1.ImageLocation = string.Format(imagePath + ImageNumber + ".jpg");
        }*/

        public void CheckFile()
        {
            timer2.Start();

            if (File.Exists(filePath + @"\" + fileName)) //если файл сущесвует
            {
                ReadFile();              
            }
            else    //если файл отсутствует
            {
                lines.Clear();
                for(int i = 0; i < labels.Count; i++) 
                {
                    labels[i].Text = "";
                }
            }
        }

        private void ReadFile()
        {
            labels = this.Controls.OfType<Label>().ToList();
            string[] line = File.ReadAllLines(fileName);

            foreach (string s in line)
            {
                lines.Add(s);
            }

            for (int i = 0; i < lines.Count; i++) //вывод в текст
            {
                label1.Text += i;// lines[i];//.Substring(lines[i].IndexOf(':') + 1)+"\n";
                if (i == lines.Count) return;
                //labels[i].Text = lines[i].Substring(lines[i].IndexOf(':') + 1);
            }
            
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
              return;
            }
            
            lines.Clear();
            ReadFile();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImages();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            CheckFile();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
