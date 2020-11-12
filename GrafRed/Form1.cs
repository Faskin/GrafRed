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
        public Form1()
        {
            InitializeComponent();

            Bitmap pic = new Bitmap(750, 500);
            picDrawingSurface.Image = pic;

            drawing = false;
            currentpen = new Pen(Color.Black);
            

            
        }



        private void picDrawingSurface_Click(object sender, EventArgs e)
        {

        }

        private void picDrawingSurface_MouseDown(object sender, MouseEventArgs e)
        {
            if (picDrawingSurface.Image == null)
            {
                MessageBox.Show("Create a new file first");
                return;
            }
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                oldlocation = e.Location;
                graphicsPath = new GraphicsPath();

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
            if (picDrawingSurface.Image != null)
            {
                var result = MessageBox.Show("Save the current image before creating a new drawing?", "Warning", MessageBoxButtons.YesNoCancel);
                switch (result)
                {
                    case DialogResult.No: break;
                    case DialogResult.Yes: newToolStripMenuItem_Click(sender, e); break;
                    case DialogResult.Cancel:
                        return;
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
                picDrawingSurface.Load(openFileDialog.FileName);
            picDrawingSurface.SizeMode = PictureBoxSizeMode.AutoSize;
            

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void picDrawingSurface_MouseUp(object sender, MouseEventArgs e)
        {
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
                Graphics graphics = Graphics.FromImage(picDrawingSurface.Image);
                graphicsPath.AddLine(oldlocation, e.Location);
                graphics.DrawPath(currentpen, graphicsPath);
                oldlocation = e.Location;
                graphics.Dispose();
                picDrawingSurface.Invalidate();


               
            }
        }

        
    }
}

