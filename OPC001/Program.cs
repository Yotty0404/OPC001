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
            var input1 = Console.ReadLine().Split(' ');
            var input2 = Console.ReadLine().Split(' ');

            var N = int.Parse(input1[0]);
            var F = int.Parse(input1[1]);
            var E = int.Parse(input1[2]);
            var EN = int.Parse(input1[3]);
            var L = input2.Select(x => int.Parse(x)).ToList();
            var L_Origin = new List<int>();
            foreach (var n in L) L_Origin.Add(n);
            L.Sort();
            var output = "";
            //L.Reverse();

            //var output = string.Join(" ", Enumerable.Repeat("1", N));

            var groups = new List<List<int>>();
            var group = new List<int>();
            var cnt = 0;
            var elevator = new List<List<int>>();

            //エレベーターの人数ごとにグループ分け
            foreach (var n in L)
            {
                if (cnt == 0)
                {
                    group = new List<int>();
                }
                group.Add(n);

                cnt++;

                if (cnt == EN)
                {
                    groups.Add(group);
                    cnt = 0;
                }
            }

            output = string.Join(" ", L);
            //Console.WriteLine(output);

            //最初の10グループをエレベーターに追加
            for (int i = 0; i < E; i++)
            {
                elevator.Add(groups[i]);
            }


            var dic = new Dictionary<int, int>();

            for (int i = 0; i < E; i++)
            {
                dic.Add(i, elevator[i].Sum());
            }



            //for (int i = 0; i < E; i++)
            //{
            //    output = string.Join(" ", elevator[i]);
            //    Console.WriteLine(i + ": sum " + elevator[i].Sum() + " " + output);
            //}
            //Console.WriteLine();

            var index = E;
            while (index <= N / EN - 1)
            {
                foreach (var d in dic.OrderByDescending(x => x.Value))
                {
                    elevator[d.Key].AddRange(groups[index]);
                    index++;
                }

                //for (int i = 0; i < E; i++)
                //{
                //    output = string.Join(" ", elevator[i]);
                //    Console.WriteLine(i + ": sum " + elevator[i].Sum() + " " + output);
                //}
                //Console.WriteLine();

                //dicを最新の情報で更新
                dic = new Dictionary<int, int>();
                for (int i = 0; i < E; i++)
                {
                    dic.Add(i, elevator[i].Sum());
                }
            }


            //前後に空白を追加
            output = " " + string.Join(" ", L_Origin) + " ";
            //Console.WriteLine(output);
            var e_num = 1;
            foreach (var e in elevator)
            {
                foreach (var n in e)
                {
                    //空白を挟んで置換することで、数字単位で置換
                    var re = new Regex(" " + n.ToString() + " ");
                    output = re.Replace(output, " " + e_num.ToString() + " ", 1);
                }

                e_num++;
            }

            //前後の空白を削除して出力
            Console.WriteLine(output.Substring(1, output.Length - 2));
        }
    }
}
