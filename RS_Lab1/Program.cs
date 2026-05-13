using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParallelComputingTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Завдання 1. Продуктивність конвеєра");
            Task1();

            Console.WriteLine("\nЗавдання 2. Граф функціональних пристроїв");
            Task2();

            Console.WriteLine("\nЗавдання 3. Закон Амдала");
            Task3();

            Console.ReadLine();
        }

        static void Task1()
        {
            int[] stages = { 6, 7, 5, 10, 6, 3 };
            int initTime = 3;
            int n = 3000;

            int tSeq1 = stages.Sum(); 
            int tau = stages.Max();   

            int tSeq = n * tSeq1;
            int tPipe = initTime + tSeq1 + (n - 1) * tau;

            Console.WriteLine($"a) Тактів послідовно: {tSeq}");
            Console.WriteLine($"   Тактів конвеєрно: {tPipe}");

            double pSeqPeak = (1.0 / tSeq1) * 1000;
            double pPipePeak = (1000.0 / tau); 

            Console.WriteLine($"b) Пікова продуктивність (послідовно): {pSeqPeak:F2} MOPS");
            Console.WriteLine($"   Пікова продуктивність (конвеєр): {pPipePeak:F2} MOPS");

            double sMax = (double)tSeq1 / tau;
            double targetS = 0.85 * sMax;
            int requiredN = 1;

            while (true)
            {
                double currentS = (double)(requiredN * tSeq1) / (initTime + tSeq1 + (requiredN - 1) * tau);
                if (currentS >= targetS) break;
                requiredN++;
            }
            Console.WriteLine($"c) Мінімальна кількість операцій для 85% прискорення: {requiredN}");
        }

        static void Task2()
        {
            double[] pi = { 7, 5, 4, 7, 5, 8, 7, 12, 5, 5, 14, 8, 10, 8, 11 };
            double[] flow = new double[15];

            double f1 = 5.0;
            flow[0] = f1; flow[1] = f1;
            flow[2] = f1 / 2; flow[3] = f1 / 2; 
            flow[4] = f1; flow[5] = f1;

            double f2 = 5.0;
            flow[9] = f2;
            flow[6] = f2 / 2; flow[7] = f2 / 2;
            flow[10] = flow[6] / 3; flow[8] = flow[6] / 3;
            flow[7] += flow[6] / 3; 
            flow[8] += flow[7];     

            double f3 = 8.0;
            flow[13] = f3;
            flow[14] = f3 / 3; flow[12] = f3 / 3; flow[11] = f3 / 3;
            flow[12] += flow[14]; 

            Console.WriteLine("a) Завантаженість пристроїв (K_z_i):");
            double[] kz = new double[15];
            for (int i = 0; i < 15; i++)
            {
                kz[i] = flow[i] / pi[i];
                Console.WriteLine($"   Пристрій {i}: {kz[i]:P1} (Потік: {flow[i]:F2} / Макс: {pi[i]})");
            }

            double kzSys = kz.Average();
            Console.WriteLine($"b) Завантаженість системи: {kzSys:P2}");

            double realPerformance = f1 + f2 + f3;
            Console.WriteLine($"c) Реальна продуктивність системи: {realPerformance}");

            double seqTime = pi.Sum(p => 1.0 / p);
            double speedup = realPerformance * seqTime;
            Console.WriteLine($"d) Орієнтовне прискорення системи: {speedup:F2}");
        }

        static void Task3()
        {
            double f = 0.80;
            double s = 0.20;

            int l = 10;
            double s10 = 1.0 / (s + f / l);
            Console.WriteLine($"a) Макс. прискорення для 10 процесорів: {s10:F2}");

            double sMaxLimit = 1.0 / s; 
            double s50 = 0.50 * sMaxLimit; 
            double s80 = 0.80 * sMaxLimit; 

            int pMin = (int)Math.Ceiling(f / ((1.0 / s50) - s));
            int pMax = (int)Math.Ceiling(f / ((1.0 / s80) - s));

            Console.WriteLine($"b) Теоретичне максимальне прискорення: {sMaxLimit:F2}");
            Console.WriteLine($"   Кількість процесорів для досягнення від 50% до 80% прискорення: від {pMin} до {pMax}");
        }
    }
}