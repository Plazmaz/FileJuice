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

            for (long i = filecount; i >= 0; i--)
            {
                long curLen = (modifiedLen > 255 * 255 * 255) ? 255 * 255 * 255 : modifiedLen;
                modifiedLen -= curLen;
                //ColorValues = new List<Color>();
                //long red = curLen / (255 * 255);
                //curLen -= red * (255 * 255);
                //long green = curLen / 255;
                //curLen -= green * 255;
                //long blue = curLen % 255;
                //Color fileSize = Color.FromArgb((int)i, (int)red, (int)green, (int)blue);


                //ColorValues.Add(fileSize);
                while (byteIndex + modifiedLen < curLen)
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
                            long b = originalBytes[byteIndex];
                            byteIndex += patLen;
                            ColorValues.Add(Color.FromArgb((int)a, (int)r, (int)g, (int)b));
                        }
                    }
                    else
                    {
                        long r = (byteIndex < originalBytes.Length) ? originalBytes[byteIndex] : 0;
                        long g = (byteIndex + 1 < originalBytes.Length) ? originalBytes[byteIndex + 1] : 0;
                        long b = (byteIndex + 2 < originalBytes.Length) ? originalBytes[byteIndex + 2] : 0;
                        ColorValues.Add(Color.FromArgb((int)r, (int)g, (int)b));
                        byteIndex += 3;
                        //pBarConversion.Value = (byteIndex / fullSize) * 100;
                    }
                }
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
            output.Save("tmpimg-" + modifier + ".png");
            
            main.compressToImage(originalBytes);
        }
        public bool nextFourMatch()
        {
            if (byteIndex + 3 >= originalBytes.Length)
                return false;
            for (long i = byteIndex; i < byteIndex + 3; i++)
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
            while (byteIndex + count < originalBytes.Length && originalBytes[byteIndex + count] == originalBytes[byteIndex])
                count++;
            Console.WriteLine();
            return count;
        }
        public void decompress(Bitmap image)
        {
            Color firstColor = image.GetPixel(0, 0);
            long originalLen = firstColor.R * (255 * 255);
            originalLen += firstColor.G * (255);
            originalLen += firstColor.B;
            //originalLen += 25;
            byte[] outBytes = new byte[originalLen];
            Console.WriteLine("Original File Size: ");
            Console.WriteLine("MB: " + (double)originalLen / 1048576.0);
            Console.WriteLine("KB: " + (double)originalLen / 1024.0);
            long byteIndex = 0;
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
                            outBytes[byteIndex + i] = pattern;
                        }
                        byteIndex += repeats;
                    }
                    else
                    {
                        if (byteIndex < outBytes.Length)
                        {
                            outBytes[byteIndex] = curColor.R;
                            byteIndex++;
                        }
                        if (byteIndex < outBytes.Length)
                        {
                            outBytes[byteIndex] = curColor.G;
                            byteIndex++;
                        }
                        if (byteIndex < outBytes.Length)
                        {
                            outBytes[byteIndex] = curColor.B;
                            byteIndex++;
                        }
                    }
                }
            }
            File.WriteAllBytes("outfile.exe", outBytes);
        }
    }
}
