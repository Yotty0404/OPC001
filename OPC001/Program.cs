using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OPC001
{
    public class Program
    {
        public static void Main()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            var input1 = Console.ReadLine().Split(' ');
            var input2 = Console.ReadLine().Split(' ');

            var N = int.Parse(input1[0]);
            var F = int.Parse(input1[1]);
            var E = int.Parse(input1[2]);
            var EN = int.Parse(input1[3]);
            var L = input2.Select(x => int.Parse(x) - 1).ToList();
            var L_Output = new List<int>();
            //Output用のリストは同じサイズ分0で初期化しておく
            foreach (var n in L) L_Output.Add(0);

            var groupNum = E * EN;
            var timeElevator = new List<int>();
            //エレベータの時間初期化
            for (int i = 0; i < E; i++)
            {
                timeElevator.Add(0);
            }

            var chunkSize = EN;
            //最後のグループだけ全探索するため、ここでは除外
            for (int i = 0; i < (N - groupNum) / (groupNum); i++)
            {
                var l = L.GetRange(i * groupNum, groupNum);
                l.Sort();
                foreach (var group in l.Chunks(EN))
                {
                    //現状最小時間のところに加算
                    var ele_num = timeElevator.FindIndex(x => x == timeElevator.Min());
                    timeElevator[ele_num] += group.Max() * 2 + EN;

                    foreach (var n in group)
                    {
                        var index = L.FindIndex(x => x == n);
                        //乗るエレベーターを更新
                        L_Output[index] = ele_num + 1;
                        //すでにエレベーターに乗った人は0で更新
                        L[index] = 0;
                    }
                }
            }

            for (int i = 0; i < Math.Pow(6, 4) * 5 + 1; i++)
            {
                var bit0 = (int)(i / Math.Pow(6, 4)) % 6;
                var bit1 = (int)(i / Math.Pow(6, 3)) % 6;
                var bit2 = (int)(i / Math.Pow(6, 2)) % 6;
                var bit3 = (int)(i / Math.Pow(6, 1)) % 6;
                var bit4 = (int)(i / Math.Pow(6, 0)) % 6;

                if (bit0 + bit1 + bit2 + bit3 + bit4  != 5)
                {
                    continue;
                }
                var test = 1;
            }
        }
    }

    public static class Extensions
    {
        // 指定サイズでのチャンクに分割する拡張メソッド
        public static IEnumerable<IEnumerable<T>> Chunks<T>(this IEnumerable<T> list, int size)
        {
            while (list.Any())
            {
                yield return list.Take(size);
                list = list.Skip(size);
            }
        }
    }
}
