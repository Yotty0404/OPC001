using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OPC001.InputGenerator
{
    internal class Program
    {
        private static readonly int _N = 1000; // 社員数
        private static readonly int _X = 20;   // ビルの高さ
        private static readonly int _Y = 5;    // エレベーターの台数
        private static readonly int _Z = 10;   // 1台のエレベーターに乗れる人数

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                var seed = i;
                Generate(seed);
            }
        }

        private static void Generate(int seed)
        {
            var rnd = new Random(seed);

            var sigma = rnd.NextDouble() * 10;
            var ave = 2 + rnd.NextDouble() * 18;
            Console.WriteLine(string.Format("seed = {0} 標準偏差 = {1} 平均 {2}", seed, sigma, ave));

            var a = new List<int>();
            while (a.Count < _N)
            {
                var x = rnd.NextDouble();
                var y = rnd.NextDouble();

                var z = sigma * Math.Sqrt(-2.0 * Math.Log(x)) * Math.Cos(2.0 * Math.PI * y) + ave;
                var ai = (int)z;
                if (1 < ai && ai <= _X)
                {
                    a.Add(ai);
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} {1} {2} {3}", _N, _X, _Y, _Z));
            sb.AppendLine(string.Join(" ", a));

            var fileName = string.Format("{0:0000}.txt", seed);
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}
