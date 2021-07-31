using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace _Dtest
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        Bitmap newbmp;
        int[,] img;
        int a;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog(); 
            if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                bmp = new Bitmap(openFileDialog1.FileName);                //�Ϥ�������Ʀs���ܼ�bmp
                pictureBox1.Image = bmp;                                   //��ܩ�pictureBox1.
                img = test.BmpToAry.Transfer(bmp);                         //�N�۳t��Ƹm�Jtest.BmpToAry.Transfer�禡�A��X�}�Cimg            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                newbmp.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);   //����newbmp��X
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int HEIGHT = img.GetLength(0);
            int WIDTH = img.GetLength(1);
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    if (img[i, j] < 28)
                    {
                        img[i, j] = 0;
                    }
                    else
                    {
                        img[i, j] = 255;
                    }
                }
            }
            newbmp = test.BmpToAry.Invert(img);
            pictureBox2.Image = newbmp;
        }

        public int[,,] getRGBData() 
        {
            // Step 1: �Q�� Bitmap �N bmp �]�_��
            Bitmap bimage = new Bitmap(bmp);
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[,,]rgbData=new int[Width,Height,3];
            // Step 2: ���o���I�C���T
            for(int y=0;y<Height;y++)
            {
                for(int x=0;x<Width;x++)
                {
                    Color color=bimage.GetPixel(x,y);
                    rgbData[x, y, 0] = color.R;
                    rgbData[x, y, 1] = color.G;
                    rgbData[x, y, 2] = color.B;
                }
            }
            return rgbData;
        }

       /* public BitmapData LockBits(
           Rectangle rect, // ���w�n����v���d��
           ImageLockMode flags, // ���w�O���骺�C��Ҧ�
           PixelFormat format // ���w�o�� Bitmap ����Ʈ榡 
       );*/
         

        private void button4_Click(object sender, EventArgs e)
        {
            int HEIGHT = img.GetLength(0);
            int WIDTH = img.GetLength(1);
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    
                     img[i, j] = (255-img[i,j]);
                }
            }
            newbmp = test.BmpToAry.Invert(img);
            pictureBox2.Image = newbmp;
        }
        public static bool Invert(Bitmap bimage)
        {
            BitmapData bmData = bimage.LockBits(new Rectangle(0, 0, bimage.Width, bimage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bmData.Stride;
                 
            //���o���I��ƪ��_�l��}
            System.IntPtr Scan0 = bmData.Scan0;         // �p��C�檺���I�Ҧ��ڪ�byte �`�� 
            int ByteNumber_Width = bimage.Width * 3;    // �p��C�@��᭱�X�� Padding bytes 
            int ByteOfSkip = stride - ByteNumber_Width; 

            // �����Q�Ϋ���, �����ɪ����e 
            int Height = bimage.Height;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < ByteNumber_Width; x++)
                    {
                        p[0] = (byte)(255 - p[0]); // �m���Ƥ��� 
                        ++p;
                    }
                    p += ByteOfSkip; // ���L�ѤU�� Padding bytes 
                }
            }
            bimage.UnlockBits(bmData);
            return true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
             
        }
    }
}