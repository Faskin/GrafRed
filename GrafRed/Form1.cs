using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafRed
{
    public partial class Form1 : Form
    {
        bool drawing;
        GraphicsPath graphicsPath;
        Point oldlocation;
        Pen currentpen;
        Color historyColor;
        List<Image> History;
        int historyCounter;
        bool style = false;
        Form2 f2 = new Form2();
        

        public Form1()
        {
            InitializeComponent();
            History = new List<Image>();

            drawing = false;
            currentpen = new Pen(Color.Black);
            currentpen.Width = trackBar.Value;
            
            
        }
        


        private void picDrawingSurface_Click(object sender, EventArgs e)
        {

        }

        private void picDrawingSurface_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDrawingSurface.Image == null)
            {
                MessageBox.Show("Create a new file first", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldlocation = e.Location;
                graphicsPath = new GraphicsPath();
                historyColor = currentpen.Color;


            }
            else if (e.Button == MouseButtons.Right)
            {
               
                graphicsPath = new GraphicsPath();
                oldlocation = e.Location;
                drawing = true;
                currentpen.Color = Color.White;
                



            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog SaveDlg = new SaveFileDialog();
            SaveDlg.Filter = "JPEG Image|*.jpeg| Bitmap Image| .bmp| GIF Image| *.gif| PNG Image| *.png";
            SaveDlg.Title = "Save an Image File";
            SaveDlg.FilterIndex = 4;
            SaveDlg.ShowDialog();

            if (SaveDlg.FileName != "")
            {
                System.IO.FileStream fileStream =
                    (System.IO.FileStream)SaveDlg.OpenFile();
                switch (SaveDlg.FilterIndex)
                {
                    case 1:
                        this.picDrawingSurface.Image.Save(fileStream, ImageFormat.Jpeg);
                        break;
                    case 2:
                        this.picDrawingSurface.Image.Save(fileStream, ImageFormat.Bmp);
                        break;
                    case 3:
                        this.picDrawingSurface.Image.Save(fileStream, ImageFormat.Gif);
                        break;
                    case 4:
                        this.picDrawingSurface.Image.Save(fileStream, ImageFormat.Png);
                        break;

                }
                fileStream.Close();




            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            History.Clear();
            historyCounter = 0;
            Bitmap pic = new Bitmap(750, 500);
            picDrawingSurface.Image = pic;
            History.Add(new Bitmap(picDrawingSurface.Image));


            Graphics g = Graphics.FromImage(picDrawingSurface.Image);
            g.Clear(Color.White);
            g.DrawImage(picDrawingSurface.Image, 0, 0, 750, 500);

            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save the current image before creating a new drawing?", "Warning", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: saveToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel: return;
                }
                
            }
            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPEG Image|*.jpeg| Bitmap Image| .bmp| GIF Image| *.gif| PNG Image| *.png";
            openFileDialog.Title = "Open an Image File";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                picDrawingSurface.Load(openFileDialog.FileName);
            }
            picDrawingSurface.SizeMode = PictureBoxSizeMode.AutoSize;
            

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void picDrawingSurface_MouseUp(object sender, MouseEventArgs e)
        {
            History.RemoveRange(historyCounter + 1, History.Count - historyCounter - 1);
            History.Add(new Bitmap(picDrawingSurface.Image));
            if (historyCounter + 1 < 10) historyCounter++;
            if (History.Count - 1 == 10) History.RemoveAt(0);

            currentpen.Color = historyColor;
            drawing = false;
            try
            {
                
                graphicsPath.Dispose();
            }
            catch { };
           
        }

        private void picDrawingSurface_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = e.X.ToString() + ", " + e.Y.ToString();
            if (drawing)
            {
                
             
                    Graphics g = Graphics.FromImage(picDrawingSurface.Image);
                    graphicsPath.AddLine(oldlocation, e.Location);
                    g.DrawPath(currentpen, graphicsPath);
                    oldlocation = e.Location;
                    g.Dispose();
                    picDrawingSurface.Invalidate();
            }
           
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
             currentpen.Width = trackBar.Value;

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (History.Count != 0 && historyCounter != 0)
            {
                picDrawingSurface.Image = new Bitmap(History[--historyCounter]);

            }
            else MessageBox.Show("History is empty.");

        }

        private void renoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (historyCounter < History.Count - 1)
            {
                picDrawingSurface.Image = new Bitmap(History[++historyCounter]);
            }
            else MessageBox.Show("History is empty.");

        }

        private void solidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentpen.DashStyle = DashStyle.Solid;

            
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            f2.ShowDialog();
        }
    }
}

