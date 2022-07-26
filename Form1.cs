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
        string[] fileName;                                  //имя файла для чтения
        string[] image;                                     //имя  картинки для чтения

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

            watcher.Filter = "*.txt";                      //фильтр имени

            InitializeComponent();
            SetSize();
            CheckFile();

            //элементы меню
            ToolStripMenuItem changeText = new ToolStripMenuItem("Настройка шрифтов");
            ToolStripMenuItem quitItem = new ToolStripMenuItem("Выход");

            //добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { changeText, quitItem });

            //ассоциируем контекстное меню с текстовым полем
            pictureBox1.ContextMenuStrip = contextMenuStrip1;
            groupBox1.ContextMenuStrip = contextMenuStrip1;

            //устанавливаем обработчики событий для меню
            changeText.Click += changeText_Click;
            quitItem.Click += quitItem_Click;
        }

        private int ImageNumber = 1;

        private void LoadNextImages()                       //грузим фотки по таймеру
        {
            image = Directory.GetFiles(imagePath, "*.jpg");

            timer1.Start();
            ImageNumber++;
            if (ImageNumber == image.Length)
            {
                ImageNumber = 1;
            }
            for (int i = 0; i <= ImageNumber; i++)
            {
                pictureBox1.ImageLocation = image[i];
            }
        }

        private void CheckFile()                            //проверка файла
        {
            fileName = Directory.GetFiles(filePath, "*.txt").OrderBy(f => new FileInfo(f).CreationTime).ToArray();
            if (fileName.Length != 0)    //если файл сущесвует
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
            label1.Text = null;
            string[] line = File.ReadAllLines(fileName.Last());    //читаем строки
            int count = File.ReadAllLines(fileName.Last()).Length; //кол-во строк

            foreach (string s in line)                      //строки в список
            {
                lines.Add(s);
            }

            for (int i = 0; i < count; i++)                   //вывод в текст
            {
                label1.Text += lines[i] + "\n\n";//.Substring(0, lines[i].IndexOf(':'))
                //+ " " + lines[i].Substring(lines[i].IndexOf(':') + 1) + "\n\n";

            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)           //проверка  изменения  
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
            fileName = null;
            CheckFile();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadNextImages();
        }

        private void SetSize()
        {
            Screen[] screens = Screen.AllScreens;
            if (screens.Length <= 1)
            {
                pictureBox1.Width = Screen.PrimaryScreen.Bounds.Width;
                pictureBox1.Height = Screen.PrimaryScreen.Bounds.Height;
            }
            else
            {
                pictureBox1.Width = Screen.AllScreens[1].Bounds.Width;
                pictureBox1.Height = Screen.AllScreens[1].Bounds.Height;
            }
            

        }

        //вызов меню смены текста
        private void changeText_Click(object sender, EventArgs e)
        {
            fontDialog1.ShowDialog();
            label1.Font = fontDialog1.Font;
        }

        //выход из приложения(пожилого)
        private void quitItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
