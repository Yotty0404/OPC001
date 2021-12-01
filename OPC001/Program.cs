using System;
using System.Collections.Generic;
using System.Linq;

namespace OPC001
{
    public class Program
    {
        public static void Main()
        {
            var input1 = Console.ReadLine().Split(' ');
            var input2 = Console.ReadLine().Split(' ');

            var N = int.Parse(input1[0]);
            var E = int.Parse(input1[1]);
            var F = int.Parse(input1[2]);
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
            Console.WriteLine(output);


            for (int i = 0; i < E; i++)
            {
                elevator.Add(groups[i]);
            }


            foreach (var item in elevator)
            {
                output = string.Join(" ", item);
                Console.WriteLine(output);
            }

            var dic = new Dictionary<int, int>();

            for (int i = 0; i < E; i++)
            {
                dic.Add(i, elevator[i].Sum());
            }
        }
    }
}
