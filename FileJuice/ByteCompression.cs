using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileJuice
{
    public class ByteCompression
    {
        byte[] originalBytes;
        long byteIndex = 0;
        long trueByteIndex = 0;
        long filecount = 0;
        List<Color> ColorValues = new List<Color>();
        public void readFileBytes(FileInfo byteFile)
        {
            if (!byteFile.Exists)
                return;
            originalBytes = File.ReadAllBytes(byteFile.FullName);
            long modifiedLen = originalBytes.Length;
            Console.WriteLine(modifiedLen.ToString());
            filecount = modifiedLen / (255 * 255 * 255);
            long originalLen = modifiedLen;
            for (long i = 0; i <= filecount || (filecount == 0 && i == 0); i++)
            {
                long curLen = (modifiedLen > 255 * 255 * 255) ? 255 * 255 * 255 : modifiedLen;
                modifiedLen -= curLen;
            //    ColorValues = new List<Color>();
            //    long red = curLen / (255 * 255);
            //    curLen -= red * (255 * 255);
            //    long green = curLen / 255;
            //    curLen -= green * 255;
            //    long blue = curLen % 255;
            //    Color fileSize = Color.FromArgb((int)i, (int)red, (int)green, (int)blue);

                byteIndex = 0;
                //ColorValues.Add(fileSize);
                while (byteIndex < curLen)
                {
                    if (nextFourMatch())
                    {
                        long unbroken = getUnbrokenCount();
                        while (unbroken > 0) {
                            long patLen = unbroken > 255 * 255 ? 255 * 255 : unbroken;
                            unbroken -= patLen;
                            long a = 254;
                            long r = patLen / 255;
                            long g = patLen % 255;
                            long b = originalBytes[trueByteIndex];
                            byteIndex += patLen;
                            trueByteIndex += patLen;
                            ColorValues.Add(Color.FromArgb((int)a, (int)r, (int)g, (int)b));
                        }
                    }
                    else
                    {
                        long r = (trueByteIndex < originalBytes.Length) ? originalBytes[trueByteIndex] : 0;
                        long g = (trueByteIndex + 1 < originalBytes.Length) ? originalBytes[trueByteIndex + 1] : 0;
                        long b = (trueByteIndex + 2 < originalBytes.Length) ? originalBytes[trueByteIndex + 2] : 0;
                        ColorValues.Add(Color.FromArgb((int)r, (int)g, (int)b));
                        byteIndex += 3;
                        trueByteIndex += 3;
                        //pBarConversion.Value = (byteIndex / fullSize) * 100;
                    }
                }
                if(i == filecount)
                    ColorValues.Add(Color.FromArgb(0, 0, 0, 0));
                writeOutput(i);
                ColorValues.Clear();
            }
        }
        public void writeOutput(long modifier)
        {
            long fullSize = (int)ColorValues.Count;
            long sizex = (int)Math.Ceiling(Math.Sqrt(fullSize));
            long sizey = sizex;
            Bitmap output = new Bitmap((int)sizex, (int)sizey);
            long x = 0, y = 0;
            foreach(Color col in ColorValues) {
                //Console.WriteLine("x: "+ x + " y: " + y + " color: " +col.ToString());
                if (x >= sizex)
                {
                    x = 0;
                    y++;
                    output.SetPixel((int)x, (int)y, col);
                    x++;
                }
                else
                {
                    output.SetPixel((int)x, (int)y, col);
                    x++;
                }
                
                //if (x >= sizex-1)
                //{
                //    x = 0;
                //    y++;
                //}
                //else
                //{
                //    x++;
                //}
            }
            ColorValues.Clear();
            output.Save("tmpimg" + (modifier > 0 ? "-" + modifier : "") + ".png");
            output.Dispose();
            main.compressToImage(originalBytes);
        }
        public bool nextFourMatch()
        {
            if (trueByteIndex + 3 >= originalBytes.Length)
                return false;
            for (long i = trueByteIndex; i < trueByteIndex + 3; i++)
            {
                if (originalBytes[i] != originalBytes[i + 1])
                    return false;
            }
            return true;
        }
        public long getUnbrokenCount()
        {
            long count = 0;
            if (!nextFourMatch())
                return 0;
            while (trueByteIndex + count < originalBytes.Length && originalBytes[trueByteIndex + count] == originalBytes[trueByteIndex])
                count++;
            return count;
        }
        public void decompress(String filename, String extension)
        {

            List<Bitmap> images = new List<Bitmap>();
            int j = 0;
            while (new FileInfo(filename + (j > 0 ? "-" + j : "") + ".png").Exists)
            {
                images.Add((Bitmap)Bitmap.FromFile(filename + (j > 0 ? "-" + j : "") + ".png"));
                j++;
            }
            //Color firstColor = image.GetPixel(0, 0);
            //long originalLen = firstColor.R * (255 * 255);
            //originalLen += firstColor.G * (255);
            //originalLen += firstColor.B;
            //filecount = originalLen / (255 * 255 * 255);
            //originalLen += 25;
            List<byte> outBytes = new List<byte>();
            foreach(Bitmap image in images) {
                bool cont = true;
                for (long y = 0; y < image.Height && cont; y++)
                {
                    for (long x = 0; x < image.Width && cont; x++)
                    {
                        //if (y == 0 && x == 0)
                        //{
                        //    continue;
                        //}
                        Color curColor = image.GetPixel((int)x, (int)y);
                        if (curColor.A == 0)
                        {
                            cont = false;
                            continue;
                        }
                        if (curColor.A == 254)
                        {
                            long repeats = (curColor.R * 255) + curColor.G;
                            byte pattern = curColor.B;
                            for (long i = 0; i < repeats; i++)
                            {
                                outBytes.Add(pattern);
                            }
                        }
                        else
                        {
                                outBytes.Add(curColor.R);
                                outBytes.Add(curColor.G);
                                outBytes.Add(curColor.B);
                        }
                    }
                }
            Console.WriteLine("MB: " + (double)outBytes.Count / 1048576.0);
            Console.WriteLine("KB: " + (double)outBytes.Count / 1024.0);
            }
            File.WriteAllBytes(filename + "." + extension, outBytes.ToArray());
        }
    }
}
