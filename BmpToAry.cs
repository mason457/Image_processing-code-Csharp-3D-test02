using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace test
{
    class BmpToAry
    {
        static public int[,] Transfer(Bitmap bmp)
        {
            int[,] img = new int[bmp.Height, bmp.Width];    //output (y, x)

            BitmapData bmData = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height)), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Console.WriteLine(bmp.PixelFormat.ToString());

            if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                int stride = bmData.Stride;
                IntPtr Scan0 = bmData.Scan0;
                int offset = stride - bmp.Width;

                unsafe
                {
                    byte* ptrP = (byte*)(void*)Scan0;

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            img[y, x] = ptrP[0];
                            ptrP++;
                        }
                        ptrP += offset;
                    }
                }

                bmp.UnlockBits(bmData);
            }
            else if(bmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                int stride = bmData.Stride;
                IntPtr Scan0 = bmData.Scan0;
                int offset = stride - bmp.Width * 3;

                unsafe
                {
                    byte* ptrP = (byte*)(void*)Scan0;

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            img[y, x] = (byte)(ptrP[0] * 0.299 + ptrP[1] * 0.587 + ptrP[2] * 0.114);
                            ptrP += 3;
                        }
                        ptrP += offset;
                    }
                }

                bmp.UnlockBits(bmData);
            }
            else if (bmp.PixelFormat == PixelFormat.Format32bppArgb || bmp.PixelFormat == PixelFormat.Format32bppRgb)
            {
                int stride = bmData.Stride;
                IntPtr Scan0 = bmData.Scan0;
                int offset = stride - bmp.Width * 4;

                unsafe
                {
                    byte* ptrP = (byte*)(void*)Scan0;

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            img[y, x] = (byte)(ptrP[1] * 0.299 + ptrP[2] * 0.587 + ptrP[3] * 0.114);
                            ptrP += 4;
                        }
                        ptrP += offset;
                    }
                }

                bmp.UnlockBits(bmData);
            }

            return img;
        }

        static public Bitmap Invert(int[,] img)
        {
            //GetLength(1) == width, GetLenght(0) == height
            Bitmap bmp = new Bitmap(img.GetLength(1), img.GetLength(0));

            BitmapData bmData = bmp.LockBits(new Rectangle(new Point(0, 0), new Size(bmp.Width, bmp.Height)), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;
            int offset = stride - bmp.Width * 3;

            unsafe
            {
                byte* ptrP = (byte*)(void*)Scan0;

                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        ptrP[0] = (byte)img[y, x];
                        ptrP[1] = (byte)img[y, x];
                        ptrP[2] = (byte)img[y, x];
                        ptrP += 3;
                    }
                    ptrP += offset;
                }
            }

            bmp.UnlockBits(bmData);

            return bmp;
        }
    }
}
