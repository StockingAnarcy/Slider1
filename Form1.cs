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
        string imagePath = @"./Images/";                    //путь папки с картиночками
        string filePath = @"./";                            //путь папки с exe
        string fileName = "demo.txt";                       //имя файла для чтения

        List<string> lines = new List<string>();
        

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;           //Окно на весь экран
           
            var watcher = new FileSystemWatcher(filePath);          //путь файлвотчера
            watcher.EnableRaisingEvents = true;                     //ивенты файлвотчера
            watcher.SynchronizingObject = this;                     //синхронизация с обьектом
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName; //фильтры оповещений

            watcher.Changed += OnChanged;                   //проверки на действия
            watcher.Created += OnCreated;                   
            watcher.Deleted += OnDeleted;                   

            watcher.Filter = fileName;                      //фильтр имени

            InitializeComponent();
            CheckFile();
        }
        private int ImageNumber = 1;
        private void LoadNextImages()                       //грузим фотки по таймеру
        {
            timer1.Start();
            ImageNumber++;
            if(ImageNumber > 5)
            {
                ImageNumber = 1;
            }
            pictureBox1.ImageLocation = string.Format(imagePath + ImageNumber+".jpg");
        }

        private void CheckFile()                            //проверка файла
        {
            
            if (File.Exists(filePath + @"\" + fileName))    //если файл сущесвует
            {                    
                groupBox1.Visible = true;                   //показываем инфу
                ReadFile();              
            }
            else                                            //если файл отсутствует
            {
                lines.Clear();                              //чистим строки и текст
                label1.Text = "";

                groupBox1.Visible = false;                  //скрываем инфу
            }
        }

        private void ReadFile()                             //чтение файла
        {
            label1.Text = "";
            string[] line = File.ReadAllLines(fileName);    //читаем строки
            int count = File.ReadAllLines(fileName).Length; //кол-во строк

            int[] sum = new int[count];                     //сумма

            foreach (string s in line)                      //строки в список
            {
                lines.Add(s);
            }

            for (int i=0; i < count; i++)                   //вывод в текст
            {
                label1.Text += lines[i].Substring(0, lines[i].IndexOf(':'))
                + " " + lines[i].Substring(lines[i].IndexOf(':') + 1) + "\n\n";
                sum[i] = int.Parse(lines[i].Substring(lines[i].IndexOf(':') + 1));
                
            }
            label1.Text += String.Format("{0,70}", "Итого: " + sum.Sum().ToString());            //сумма            //String.Format("{0,10}", sum.Sum().ToString()); справа????
        }

        private void OnChanged(object sender, FileSystemEventArgs e)           //проверка изменения  
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
              return;
            }
            lines.Clear();
            ReadFile();
        }      

        private void OnCreated(object sender, FileSystemEventArgs e)          //проверка создания
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }
            CheckFile();
        }      

        private void OnDeleted(object sender, FileSystemEventArgs e)          //проверка удаления
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                return;
            }
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

    }
}
