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
                bmp = new Bitmap(openFileDialog1.FileName);                //圖片像素資料存於變數bmp
                pictureBox1.Image = bmp;                                   //顯示於pictureBox1.
                img = test.BmpToAry.Transfer(bmp);                         //將相速資料置入test.BmpToAry.Transfer函式，輸出陣列img            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            { 
                newbmp.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);   //圖檔newbmp輸出
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
            // Step 1: 利用 Bitmap 將 bmp 包起來
            Bitmap bimage = new Bitmap(bmp);
            int Height = bimage.Height;
            int Width = bimage.Width;
            int[,,]rgbData=new int[Width,Height,3];
            // Step 2: 取得像點顏色資訊
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
           Rectangle rect, // 指定要鎖住的影像範圍
           ImageLockMode flags, // 指定記憶體的拴鎖模式
           PixelFormat format // 指定這個 Bitmap 的資料格式 
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
                 
            //取得像點資料的起始位址
            System.IntPtr Scan0 = bmData.Scan0;         // 計算每行的像點所佔據的byte 總數 
            int ByteNumber_Width = bimage.Width * 3;    // 計算每一行後面幾個 Padding bytes 
            int ByteOfSkip = stride - ByteNumber_Width; 

            // 直接利用指標, 更改圖檔的內容 
            int Height = bimage.Height;
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < ByteNumber_Width; x++)
                    {
                        p[0] = (byte)(255 - p[0]); // 彩色資料反轉 
                        ++p;
                    }
                    p += ByteOfSkip; // 跳過剩下的 Padding bytes 
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