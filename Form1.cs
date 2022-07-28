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
        IniFile MyIni = new IniFile(@"./setting.ini");
        
        

        public Form1()
        {
            InitializeComponent();

            //panel1.HorizontalScroll.Maximum = 0;
            //panel1.AutoScroll = false;
            //panel1.VerticalScroll.Visible = false;

            ToolStripMenuItem changeText = new ToolStripMenuItem("Выбор шрифта");        //добавление строчек в меню
            ToolStripMenuItem colorText = new ToolStripMenuItem("Цвет шрифта");
            ToolStripMenuItem bgcolor = new ToolStripMenuItem("Цвет фона");
            ToolStripMenuItem bgimage_selector = new ToolStripMenuItem("Картинка на фон");
            ToolStripMenuItem bgimage = new ToolStripMenuItem("Выбрать картинку");
            ToolStripMenuItem del_bgimage = new ToolStripMenuItem("Удалить картинку");
            ToolStripMenuItem quitItem = new ToolStripMenuItem("Выход");
                                                                                         
            bgimage_selector.DropDownItems.AddRange(new[] { bgimage, del_bgimage });          //добавление элементов в меню
            contextMenuStrip1.Items.AddRange(new[] { changeText, colorText, bgcolor, bgimage_selector, quitItem });
 
            pictureBox1.ContextMenuStrip = contextMenuStrip1;                                 //ассоциируем контекстное меню с текстовым полем
            panel1.ContextMenuStrip = contextMenuStrip1;

                                                                                              //устанавливаем обработчики событий для меню
            changeText.Click += changeText_Click;                                             //шрифт
            colorText.Click += colorText_Click;                                               //цвет текста
            bgcolor.Click += bgcolor_Click;                                                   //цвет пикчербокса
            bgimage.Click += bgimage_Click;                                                   //каринка пикчербокса
            del_bgimage.Click += del_bgmimage_Click;                                          //удаление картинки пикчербокса
            quitItem.Click += quitItem_Click;                                                 //выход

            this.WindowState = FormWindowState.Maximized;                                     //Окно на весь экран

            var watcher = new FileSystemWatcher(filePath);                                    //путь файлвотчера
            watcher.EnableRaisingEvents = true;                                               //ивенты файлвотчера
            watcher.SynchronizingObject = this;                                               //синхронизация с обьектом
            watcher.NotifyFilter = NotifyFilters.LastWrite |                                  //фильтры оповещений
                                   NotifyFilters.FileName; 

            watcher.Changed += OnChanged;                                                     //проверки на действия
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            watcher.Filter = "*.txt";                                                         //фильтр имени

            SetSize();
            LoadParams();
            CheckFile();
        }

        private int ImageNumber = 1;

        private void LoadNextImages()                                                         //грузим фотки по таймеру
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

        private void CheckFile()                                                              //провер_очка файла
        {
            fileName = Directory.GetFiles(filePath, "*.txt").OrderBy(f => new FileInfo(f).CreationTime).ToArray();
            if (fileName.Length != 0)                                                                        //если файл сущесвует
            {
                 ReadFile();
            }
            else                                                                                             //если файл отсутствует
            {
                lines.Clear();                                                                               //чистим строки и текст
                label1.Text = "";
                panel1.Visible = false;                                                                   //скрываем инфу
            }
        }

        private void ReadFile()                                                               //чтение файла
        {
            label1.Text = null;
            string[] line = File.ReadAllLines(fileName.Last());                                              //читаем строки
            int count = File.ReadAllLines(fileName.Last()).Length;                                           //кол-во строк

            foreach (string s in line)                                                                       //строки в список
            {
                lines.Add(s);
            }

            for (int i = 0; i < count; i++)                                                                  //вывод в текст
            {
                label1.Text += lines[i] + "\n";
            }

            if(label1.Text=="")
                 panel1.Visible = false;                                                                  //показываем инфу
            if(label1.Text!="")
                 panel1.Visible = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)                          //проверка  изменения  
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            lines.Clear();
            ReadFile();
        }

        private void OnCreated(object sender, FileSystemEventArgs e)                          //проверка создания
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
            {
                return;
            }
            CheckFile();

        }

        private void OnDeleted(object sender, FileSystemEventArgs e)                          //проверка удаления
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted)
            {
                return;
            }
            fileName = null;
            CheckFile();
        }

        private void OnRenamed(object sender, RenamedEventArgs e)                             //проверка удаления
        {
            fileName = null;
            CheckFile();
        }

        private void timer1_Tick(object sender, EventArgs e)                                  //если таймер кончился
        {
            LoadNextImages();
        }

        private void SetSize()                                                                //подгон под размер экрана
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

        private void pictureBox1_MouseMove(object sender, EventArgs e)                        //активация верхней менюшки
        {

        }

        private void changeText_Click(object sender, EventArgs e)                             //выбор шрифта(поменшбе)
        {
            fontDialog1.ShowDialog();
            label1.Font = fontDialog1.Font;
            
        }   
       
        private void colorText_Click(object sender, EventArgs e)                              //выбор цвета текста
        {
            colorDialog2.ShowDialog();
            label1.ForeColor = colorDialog2.Color;
          
        }

        private void bgcolor_Click(object sender, EventArgs e)                                //выбор цвета подложки(групбокс)
        {
            colorDialog1.ShowDialog();
            panel1.BackColor = colorDialog1.Color;
           
        }

        public Image ImageOpen(string filename)                                               //открытие картинки
        {
            if (File.Exists(filename))
            {
                return Image.FromFile(filename);
            }
            else return null;
            
        }
        private void bgimage_Click(object sender, EventArgs e)                                //выбор пожилой картинки
        {
            openFileDialog1.InitialDirectory = filePath;
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowDialog();
            panel1.BackgroundImage = ImageOpen(openFileDialog1.FileName);
        }

        private void del_bgmimage_Click(object sender, EventArgs e)                           //удаляем пожилую картинку подложки(групбокс)
        {
            panel1.BackgroundImage = null;
            MyIni.DeleteKey("image");
        }

        private void LoadParams()                                                             //загрузка параметров(бом бом)
        {
            if (MyIni.KeyExists("font"))                                                                      //загружаем шрифт из .ini
            {
                float x = float.Parse(MyIni.Read("fontSize"));

                label1.Font = new Font(MyIni.Read("font"), x);
                
            }
            else
            {
                label1.Font = new Font("Consolas", 12);
            }

            if (MyIni.KeyExists("textColor"))                                                                 //загружаем цвет шрифта из .ini
            {
                label1.ForeColor = ColorTranslator.FromHtml(MyIni.Read("textColor"));
            }
            else
            {
                label1.ForeColor = Color.Black;
            }

            if (MyIni.KeyExists("bgmColor"))                                                                  //загружаем цвет подложки(групбокса) из .ini
            {
                panel1.BackColor = ColorTranslator.FromHtml(MyIni.Read("bgmColor"));
            }
            else
            {
                panel1.BackColor = Color.White;
            }
            
            if (MyIni.KeyExists("image")&&MyIni.Read("image")!= "openFileDialog1")                            //загружаем картинку подложки(групбокса) из .ini
            {
                panel1.BackgroundImage = ImageOpen(MyIni.Read("image"));
            }
            else
            {
                panel1.BackgroundImage = null;
            }
        }
        
        private void quitItem_Click(object sender, EventArgs e)                               //выход из приложения(пожилого)
        {
            MyIni.Write("font", label1.Font.FontFamily.Name);
            MyIni.Write("fontSize", label1.Font.Size.ToString());
            MyIni.Write("textColor", ColorTranslator.ToHtml(label1.ForeColor).ToString());
            MyIni.Write("bgmColor", ColorTranslator.ToHtml(panel1.BackColor).ToString());
            MyIni.Write("image", openFileDialog1.FileName);
            File.SetAttributes(@"./setting.ini", FileAttributes.ReadOnly);

            this.Close();
        }   
    }
 
}
