using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileJuice
{
    public partial class main : Form
    {
        Boolean decompressing = false;
        Bitmap output;
        Bitmap inputImage;
        ByteCompression compress = new ByteCompression();
        byte[] outputBytes;
        byte[] preBytes;
        private FileInfo curFile;
        public main()
        {
            InitializeComponent();
        }

        private void compress_Click(object sender, EventArgs e)
        {

            if (!decompressing && preBytes != null)
                compress.readFileBytes(curFile);
            else if (inputImage != null)
                //decompress();
                compress.decompress(inputImage);
            else
                MessageBox.Show("Please select a valid file!");
        }

        private void decompress()
        {

            int inputWidth = inputImage.Width;
            int inputHeight = inputImage.Height;
            int counter = 0;
            int byteSize = 0;
            Color scolor = inputImage.GetPixel(0, 0);
            byteSize += scolor.R * 65536;
            byteSize += scolor.G * 256;
            byteSize += scolor.B;
            //byteSize *= 3;
            outputBytes = new Byte[byteSize];
            try
            {
                for (int y = 0; ((y < inputImage.Height)); y++) // && (counter < byteSize)
                {
                    for (int x = 0; ((x < inputImage.Width)); x++) //  && (counter < byteSize))
                    {
                        if (x == 0 && y == 0)
                            continue;
                        //if (y >= inputHeight) 
                        //    goto finished;
                        //if (counter >= byteSize)
                        //    goto finished;
                        //if (x >= inputWidth)
                        //    x=0;

                        int xpix = x;
                        int ypix = y;
                        Color color = inputImage.GetPixel(xpix, ypix);
                        outputBytes[counter] = color.R;
                        if (counter < byteSize - 1)
                            outputBytes[counter + 1] = color.G;
                        if (counter < byteSize - 2)
                            outputBytes[counter + 2] = color.B;
                        counter += 3;
                        // pBarConversion.Value = (int)((double)counter / (double)byteSize) * 100;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error decompressing selected file: " + ex.Message);
            }
            File.WriteAllBytes("tmpexe.exe", outputBytes);

        }

        public static void compressToImage(byte[] preBytes)
        {
            int fullSize = preBytes.Length;
            Console.WriteLine("MB: " + (double)preBytes.Length / 1048576.0);
            Console.WriteLine("KB: " + (double)preBytes.Length / 1024.0);
           // return;
            int sizex = (int)Math.Ceiling(Math.Sqrt(preBytes.Length));
            int p65k = (int)Math.Floor(fullSize * 1.0 / 65536); //converting to short value?
            int remainder = fullSize % 65536;
            int p256 = (int)Math.Floor(remainder * 1.0 / 256); //converting remainder to short value?
            remainder %= 256;

            //size = size / 3;
            int sizey = sizex / 3; //Only y is divided by three, so as to avoid dividing by 3^2, or 9. 
                                   //Now you're thinking in Portals!
            Bitmap output = new Bitmap((int)sizex, (int)sizey);
            int counter = 0;
            for (int y = 0; y < sizey && counter < fullSize; y++)
            {
                for (int x = 0; x < sizex && counter < fullSize; x++)
                {
                    Color color = new Color();
                    //if (x == 0 && y == 0)
                    //{
                    //    color = Color.FromArgb(p65k, p256, remainder);
                    //}
                    //else
                    //{
                        int r = (counter < fullSize) ? preBytes[counter] : 0;
                        int g = (counter + 1 < fullSize) ? preBytes[counter + 1] : 0;
                        int b = (counter + 2 < fullSize) ? preBytes[counter + 2] : 0;
                        color = Color.FromArgb(r, g, b);
                        counter += 3;
                        //pBarConversion.Value = (counter / fullSize) * 100;
                    //}
                        output.SetPixel(x, y, color);
                        //Console.WriteLine("original x: " + x + " y: " + y + " color: " + color.ToString());
                }
            }
            //pBoxEncoded.Image = output;
           
            
            output.Save("tmpimg-orig.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        private void fileChooser_Click(object sender, EventArgs e)
        {
            InitializeFileSelector();
        }
        
        public void InitializeFileSelector()
        {
            if (fileselector.ShowDialog() == DialogResult.OK)
            {
                if (!decompressing)
                {
                    try
                    {
                        preBytes = File.ReadAllBytes(fileselector.FileName);
                        String ext = fileselector.FileName.Substring(fileselector.FileName.IndexOf("."));
                        curFile = new FileInfo(fileselector.FileName);
                        // File.WriteAllBytes("tmp" + ext, preBytes);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error parsing selected file: " + ex.Message);
                    }
                }
                else
                {
                    inputImage = new Bitmap(fileselector.FileName);
                }
            }
        }

        private void cbDecompressing_CheckedChanged(object sender, EventArgs e)
        {
            decompressing = !decompressing;
            if (decompressing)
            {
                fileselector.Filter = "Image Files(*.png, *.jpg, *.bmp)|*.png; *.jpg; *.bmp";
            }
            else
            {
                fileselector.Filter = "All files *.*|*.*";
            }
        }

        private void saveData_Click(object sender, EventArgs e)
        {

        }
    }
}
