using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CPU_GPU_Tester
{
    public partial class Form1 : Form
    {
 
        public List<string> symbols()
        {
            List<string> symbs = new List<string>();

            int unic = 1040;
            for (int i = 0; i < 66; i++)
            {
                if (i == 6 || i == 39)
                {
                    symbs.Add("");
                }
                else
                {
                    symbs.Add(Char.ToString((char)unic));
                    unic++;
                }
            }

            symbs[6] = "Ё";
            symbs[39] = "ё";

            int d = 66;
            for (int i = 0; i < 10; i++)
            {
                symbs.Add("" + i);
                d++;
            }

            symbs.Add(" ");

            return symbs;
        }

        public List<Image[]> Split (string source)
        {

            //Densities
            double linesDensity = 0.4; // gray % to split lines
            double symbDensity = 0.5; // gray % to split symbols
            double faultDensity = 0.4; // gray % to split close symbols
            double clearDensity = 0.2; // gray % to clean symbols

            // Maximum pixels for splitting close letters
            int maxfault = 2;

            // Minimum height of line
            int minlinepx = 8;

            // Maximum interference on line when cleaning symbols
            int intf = 2;

            // Pixels % to consider space
            double space = 0.5;

            Image img1 = Image.FromFile(source);
            Bitmap b1 = new Bitmap(img1);

            List<Bitmap> lines = new List<Bitmap>();

            Image memory = Image.FromFile("..\\..\\symbols.png");
            Bitmap meme = new Bitmap(memory);
            lines.Add(meme);

            // Get List of Lines
            int st = -1;
            int fin = -1;
            for(int i = 0; i < b1.Height; i++)
            {
                for(int j = 0; j < b1.Width; j++)
                {
                    if(Resizer.inGray(b1.GetPixel(j, i).R, b1.GetPixel(j, i).G, b1.GetPixel(j, i).B) >= linesDensity)
                    {
                        if (st == -1)
                        {
                            st = i;
                        }

                        break;
                    }
                    if (j == b1.Width - 1 && st > -1 && i - minlinepx >= st)
                    {
                        fin = i - 1;
                        //Console.WriteLine(st + " " + fin);

                        Rectangle cloneRect = new Rectangle(0, st, b1.Width, fin - st + 1);
                        System.Drawing.Imaging.PixelFormat format = b1.PixelFormat;

                        Bitmap cloned = b1.Clone(cloneRect, format);

                        lines.Add(cloned);

                        st = -1;
                        fin = -1;
                    }
                    else if (j == b1.Width - 1 && st > -1 && i - minlinepx < st)
                    {
                        st = -1;
                    }
                }
            }

            // Get Symbols
            List<Image[]> images = new List<Image[]>();

            foreach (Bitmap line in lines)
            {

                st = -1;
                fin = -1;
                List<Bitmap> im1 = new List<Bitmap>();

                int spacepixels = 0;
                // Get symbol list
                for (int i = 0; i < line.Width; i++)
                {
                    int fault = 0;

                    for (int j = 0; j < line.Height; j++)
                    {
                        if (Resizer.inGray(line.GetPixel(i, j).R, line.GetPixel(i, j).G, line.GetPixel(i, j).B) >= symbDensity)
                        {
                            if (st == -1)
                            {
                                st = i;
                            }

                            break;
                        } else if (Resizer.inGray(line.GetPixel(i, j).R, line.GetPixel(i, j).G, line.GetPixel(i, j).B) >= faultDensity)
                        {
                            fault++;
                            if (fault > maxfault)
                            {
                                if (st == -1)
                                {
                                    st = i;
                                }

                                break;
                            }
                        }

                        if (j == line.Height - 1 && st > -1)
                        {
                            fin = i - 1;

                            //Console.WriteLine("spc " + spacepixels + " " + (fin - st + 1) * space);
                            if ((spacepixels > (fin - st + 1) * space) && im1.Count() > 0 && !line.Equals(lines[0]))
                            {
                                Image img2 = Image.FromFile("..\\..\\space.png");
                                Bitmap spc = new Bitmap(img2);
                                im1.Add(spc);
                                //Console.WriteLine("Add " + fin);
                            }

                            Rectangle cloneRect = new Rectangle(st, 0, fin - st + 1, line.Height);
                            System.Drawing.Imaging.PixelFormat format = line.PixelFormat;

                            //Console.WriteLine(st + " " + fin);
                            Bitmap cloned = line.Clone(cloneRect, format);

                            im1.Add(cloned);


                            st = -1;
                            fin = -1;
                            spacepixels = 0;
                            break;
                        }

                        //Check spaces
                        if (j == line.Height - 1)
                        {
                            
                            spacepixels++;
                        }
                    }
                }


                Image[] imgs = new Image[im1.Count()];
                //Clear symbols
                for (int i = 0; i < im1.Count(); i++)
                {
                    Bitmap symb = im1[i];
                    int sth = -1;
                    int finh = -1;

                    for (int k = 0; k < symb.Height; k++)
                    {
                        int count = 0;
                        for (int m = 0; m < symb.Width; m++)
                        {

                            if (Resizer.inGray(symb.GetPixel(m, k).R, symb.GetPixel(m, k).G, symb.GetPixel(m, k).B) >= clearDensity)
                            {
                                count++;
                                if (sth == -1 && count > intf)
                                {
                                    sth = k;
                                }

                                if (count > intf) {
                                    finh = k;
                                }
                            }
                        }

                    }

                    /// Very bad fix, need to change
                    if (sth == -1)
                    {
                        imgs[i] = Image.FromFile("..\\..\\space.png");
                    }
                    ///
                    else
                    {
                        Rectangle cloneRect = new Rectangle(0, sth, symb.Width, finh - sth + 1);
                        System.Drawing.Imaging.PixelFormat format = symb.PixelFormat;

                        //Console.WriteLine(st + " " + fin);
                        Bitmap cloned = symb.Clone(cloneRect, format);
                        imgs[i] = (Image)cloned;
                    }
                        
                }

                images.Add(imgs);

            }

            return images;

        }

        public Form1()
        {
            InitializeComponent();
            textBox2.AppendText("..\\..\\image1.png");
            txPar.AppendText("1");

            //Initializes OpenCL Platforms and Devices and sets everything up
            // 4 parameter: 0 - GPU, 1 - CPU
            OpenCLTemplate.CLCalc.InitCL();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Test(int par)
        {
            List<Image[]> list = Split(textBox2.Text);

            List<string> symbs = symbols();
            txn.Clear();
            foreach (string symb in symbs)
            {
                txn.AppendText(symb);
            }

            List<string[]> res = null;
            int time = int.MaxValue;

            for (int i = 10; i >= 0; i--)
            {
                // Timer
                Stopwatch sw = new Stopwatch();
                sw.Start();

                Runner runner = new Runner(list, symbs, ((double)i / 10), par);
                List<string[]> buf_res = runner.Run();

                sw.Stop();

                tx1.AppendText("CPU " + (int)(i * 10) + "/" + (int)(100 - i * 10) + " GPU: " + (int) sw.Elapsed.TotalMilliseconds + " ms\r\n");

                if(sw.Elapsed.TotalMilliseconds < time)
                {
                    time = (int) sw.Elapsed.TotalMilliseconds;
                    res = buf_res;
                }
            }
       

            for (int i = 0; i < res.Count(); i++)
            {
                for (int j = 0; j < res[i].GetLength(0); j++)
                {
                    textBox1.AppendText(res[i][j]);
                }
                textBox1.AppendText("\r\n");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tx1.Clear();
            textBox1.Clear();

            int par = 1;
            Int32.TryParse(txPar.Text, out par); 
            // Testing: 0 - consinstently, else - parallel
            Test(par);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
