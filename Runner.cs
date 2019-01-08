using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPU_GPU_Tester
{
    class Runner
    {
        private readonly static object GPULock = new object();
        private readonly static object WriteLock = new object();


        private List<Image[]> imgs;
        private double percent = 0;
        private int cores = 1;
        List<string> symbols;

        OpenCLTemplate.CLCalc.Program.Kernel VectorSum;

        public Runner(List<Image[]> imgs, List<string> symbols, double percent, int cores)
        {
            this.imgs = imgs;
            this.percent = percent;
            this.symbols = symbols;
            this.cores = cores;

            string checker = @"
                    #pragma OPENCL EXTENSION cl_khr_global_int32_base_atomics : enable

                     __kernel void
                    VectorCheck(__global double * alls1,
                                __global double * mains1,
                                __global int * size,
                                __global int * result)
                    {
                        // Vector element index
                        int i = get_global_id(0);

                        ////// First Neuron Layer
                        double buf = fabs(mains1[i % size[0]] - alls1[i]);

                        ////// Second Neuron Layer
                        if (buf <= 0.7) atom_inc(&result[i / size[0]]); 
                    }";
            ////

            //Compiles the source codes. The source is a string array because the user may want
            //to split the source into many strings.
            OpenCLTemplate.CLCalc.Program.Compile(new string[] { checker });

            //Gets host access to the OpenCL floatVectorSum kernel
            OpenCLTemplate.CLCalc.Program.Kernel VectorSum = new OpenCLTemplate.CLCalc.Program.Kernel("VectorCheck");

            this.VectorSum = VectorSum;
        }

        public List<string[]> Run()
        {

            int size = 0;
            // Initialize result symbol list and similarity coefficients
            List<string[]> res_symb = new List<string[]>();
            for (int i = 1; i < imgs.Count(); i++)
            {
                res_symb.Add(new string[imgs[i].GetLength(0)]);
                size += imgs[i].GetLength(0);
            }


            // Buffer data
            string[] buf_res = new string[size];
            double[] similarity = new double[size];


            var Iterate = new Action<int>(process =>
            {
                List<double[][]> alls = new List<double[][]>();

                double[][] mains = Resizer.ResizeImage(imgs[0][process], imgs[0][process].Width, imgs[0][process].Height);

                for (int i = 1; i < imgs.Count(); i++)
                {
                    for (int j = 0; j < imgs[i].GetLength(0); j++)
                    {
                        alls.Add(Resizer.ResizeImage(imgs[i][j], mains.GetLength(0), mains[0].GetLength(0)));
                    }
                }

                // Preparation
                List<double> mains1 = new List<double>();
                for (int i = 0; i < mains.GetLength(0); i++)
                {
                    for (int j = 0; j < mains[i].GetLength(0); j++)
                    {
                        mains1.Add(mains[i][j]);
                    }
                }

                List<double> alls1 = new List<double>();
                foreach (double[][] element in alls)
                {
                    for (int i = 0; i < element.GetLength(0); i++)
                    {
                        for (int j = 0; j < element[i].GetLength(0); j++)
                        {
                            alls1.Add(element[i][j]);
                        }
                    }
                }

                int[] ress = new int[alls.Count()];
                for (int i = 0; i < alls.Count(); i++) { ress[i] = 0; }

                var CPUCalc = new Action(() =>
                {
                    Parallel.For(0, alls1.Count(), i =>
                    {
                        ////// First Neuron Layer
                        double buf = Math.Abs(mains1[i % mains1.Count()] - alls1[i]);

                        ////// Second Neuron Layer
                        if (buf <= 0.7) Interlocked.Increment(ref ress[i / mains1.Count()]);
                    });
                });

                var GPUCalc = new Action(() =>
                {
                    OpenCLTemplate.CLCalc.Program.Variable varV1 = new OpenCLTemplate.CLCalc.Program.Variable(alls1.ToArray());
                    OpenCLTemplate.CLCalc.Program.Variable varV2 = new OpenCLTemplate.CLCalc.Program.Variable(mains1.ToArray());
                    OpenCLTemplate.CLCalc.Program.Variable varV3 = new OpenCLTemplate.CLCalc.Program.Variable(new int[1] { mains1.Count() });
                    OpenCLTemplate.CLCalc.Program.Variable varV4 = new OpenCLTemplate.CLCalc.Program.Variable(ress);

                    OpenCLTemplate.CLCalc.Program.Variable[] args = new OpenCLTemplate.CLCalc.Program.Variable[] { varV1, varV2, varV3, varV4 };
                    int[] workers = new int[1] { alls1.Count() };

                    // Cannot execute GPU from different threads at the same time
                    //lock(GPULock)
                    //{
                    VectorSum.Execute(args, workers);
                    varV4.ReadFromDeviceTo(ress);
                    //}
                });

                if (process < symbols.Count() * percent)
                {
                    CPUCalc();
                }
                else
                {
                    GPUCalc();
                }

                Parallel.For(0, alls.Count(), i =>
                {
                    lock (WriteLock)
                    {
                        ////// Third Neuron Layer
                        if ((double)ress[i] / mains1.Count() > similarity[i])
                        {
                            similarity[i] = (double)ress[i] / mains1.Count();
                            buf_res[i] = symbols[process];
                        };
                    }
                });

            });

            var GoCPU = new Action(() =>
            {
                Parallel.For(0, (int)((double)symbols.Count() * percent), new ParallelOptions { MaxDegreeOfParallelism = cores }, Iterate);
            });

            var GoGPU = new Action(() =>
            {
                Parallel.For((int)((double)symbols.Count() * percent), symbols.Count(), new ParallelOptions { MaxDegreeOfParallelism = cores }, Iterate);
            });

            Parallel.Invoke(() => GoCPU(), () => GoGPU());

            // Final
            int count = 0;
            for (int i = 0; i < res_symb.Count(); i++)
            {
                for (int j = 0; j < res_symb[i].GetLength(0); j++)
                {
                    res_symb[i][j] = buf_res[count];
                    count++;
                }
            }

            return res_symb;
        }

    }
}
