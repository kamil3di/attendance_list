using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp56
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
        }
        //These are for moving the program
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //"Train your face" button's clicked part
        private async void btnEgit_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    if (!recognition.SaveTrainingData(pictureBox2.Image, txtFaceName.Text)) MessageBox.Show("Error", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Thread.Sleep(100);
                    lblEgitilenAdet.Text = (i + 1) + " times photo taken.";
                }

                recognition = null;
                train = null;

                recognition = new BusinessRecognition("D:\\", "Faces", "yuz.xml");
                train = new Classifier_Train("D:\\", "Faces", "yuz.xml");
            });
        }
        BusinessRecognition recognition = new BusinessRecognition("D:\\", "Faces", "yuz.xml");
        Classifier_Train train = new Classifier_Train("D:\\", "Faces", "yuz.xml");

        private void Form1_Load(object sender, EventArgs e)
        {
            webcamProcess();
        }
        //The pictureBox1 runs the webcam process
        private void webcamProcess()
        {
            List<string> nameList = new List<string>();
            Capture capture = new Capture();
            capture.Start();
            capture.ImageGrabbed += (a, b) =>
            {
                var image = capture.RetrieveBgrFrame();
                var grayimage = image.Convert<Gray, byte>();
                HaarCascade haaryuz = new HaarCascade("haarcascade_frontalface_alt2.xml");
                MCvAvgComp[][] Yuzler = grayimage.DetectHaarCascade(haaryuz, 1.2, 5, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(15, 15));
                MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, 0.5);
                foreach (MCvAvgComp yuz in Yuzler[0])
                {
                    var sadeyuz = grayimage.Copy(yuz.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    pictureBox2.Image = sadeyuz.ToBitmap();
                    if (train != null)
                        if (train.IsTrained)
                        {
                            string name = train.Recognise(sadeyuz);
                            if (!nameList.Contains(name) && name != "Undefined")
                            {
                                this.Invoke((MethodInvoker)delegate ()
                                {
                                    //Make the name uppercase
                                    string newName = char.ToUpper(name[0]) + name.Substring(1);
                                    //Calls the method to transfer data to attendance list
                                    populateItems(newName, pictureBox2.Image);
                                });
                            }
                            nameList.Add(name);
                            txtWho.Text = name;
                            int match_value = (int)train.Get_Eigen_Distance;
                            image.Draw(name + " ", ref font, new Point(yuz.rect.X - 2, yuz.rect.Y - 2), new Bgr(Color.LightGreen));
                        }
                    image.Draw(yuz.rect, new Bgr(Color.Red), 2);
                }
                pictureBox1.Image = image.ToBitmap();
            };
        }
        //This function for adding new items to the right hand side that is attendance list / It takes the name and pictureBox2 image to add attendance list
        private void populateItems(string TitleName, Image IconName)
        {
            ListItem[] listItems = new ListItem[1];
            for (int i = 0; i < listItems.Length; i++)
            {
                listItems[i] = new ListItem();
                listItems[i].Title = TitleName + " is here!";
                listItems[i].Icon = IconName;
                listItems[i].Message = "Enterance date: " + Convert.ToString(DateTime.Now);
                flowLayoutPanel1.Controls.Add(listItems[i]);
                WriteLogin(TitleName);
            }
        }
        //This function creates a file which is named as "Login.txt" in main file and records the logins from attendance list
        private void WriteLogin(string TitleName)
        {
            StreamWriter sw = new StreamWriter("../../../Login.txt", true);
            sw.WriteLine(TitleName + " has entered the class at " + Convert.ToString(DateTime.Now));
            sw.Close();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        private void txtFaceName_TextChanged(object sender, EventArgs e)
        {

        }
        //To minimize the window - button
        private void button1_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        //To minimize and maximize window - button
        private void button2_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
        //Exit button
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //To move the application
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        //Help us part in menu strip
        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prepared by Kamil Dinleyici,\n         ID:2016513019 \nÇukurova University EEMB");
        }
        //Restart application button in menu strip
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
        //Delete button to delete all trained faces
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(@"D:\Faces");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}

