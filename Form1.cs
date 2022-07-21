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
            watcher.NotifyFilter = NotifyFilters.LastWrite| NotifyFilters.FileName;
            //watcher.NotifyFilter = NotifyFilters.FileName;
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Filter = fileName;
            
            InitializeComponent();
            CheckFile();
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
            //labels = this.Controls.OfType<Label>().ToList();
            if (File.Exists(filePath + @"\" + fileName)) //если файл сущесвует
            {
                label1.Visible = true;
                label2.Visible = true;
                groupBox1.Visible = true;
                ReadFile();              
            }
            else    //если файл отсутствует
            {
                lines.Clear();
                label1.Text = "";

                label1.Visible = false;
                label2.Visible = false;
                groupBox1.Visible = false;
            }
        }

        private void ReadFile()
        {
            label1.Text = "";
            labels = this.Controls.OfType<Label>().ToList();
            string[] line = File.ReadAllLines(fileName);
            int count = File.ReadAllLines(fileName).Length;

            foreach (string s in line)
            {
                lines.Add(s);
            }

            for (int i=0; i < count; i++) //вывод в текст
            {   
                label1.Text += lines[i].Substring(lines[i].IndexOf(':') + 1)+"\n";
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

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }
            ReadFile();
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                return;
            }
            lines.Clear();
            CheckFile();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImages();
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
