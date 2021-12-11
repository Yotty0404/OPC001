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
            var L_Origin = new List<int>();
            foreach (var n in L) L_Origin.Add(n);
            L.Sort();
            L.Reverse();
            var output = "";

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
                var tempGroup = new List<int>();
                foreach (var n in groups[i]) tempGroup.Add(n);
                elevator.Add(tempGroup);
            }


            var dic = new Dictionary<int, int>();

            for (int i = 0; i < E; i++)
            {
                dic.Add(i, elevator[i].Max());
            }

            var index = E;
            while (index < N / EN)
            {
                //現状最小のエレベーターの番号を取得
                var e_min = dic.OrderBy(x => x.Value).FirstOrDefault().Key;
                elevator[e_min].AddRange(groups[index]);

                //dicを最新の情報で更新
                //往復分＋人数
                dic[e_min] += groups[index].Max() * 2 + EN;
                index++;
            }

            Func<int> CalcTime = () =>
            {
                int rtnTime = 0;
                int tempTime = 0;

                foreach (var item in elevator)
                {
                    tempTime = 0;
                    item.Sort();
                    var amari = item.Count % EN;

                    if (amari == 0)
                    {
                        amari = 5;
                    }

                    for (int i = 0; i < item.Count; i++)
                    {
                        if (i < item.Count - amari)
                        {
                            tempTime += item[i] * 2 + EN;
                        }
                        else
                        {
                            tempTime += item[i];
                        }
                    }

                    rtnTime = System.Math.Max(rtnTime,tempTime);
                }

                return rtnTime;
            };


            var tempTimeScore = CalcTime();
            var nextTimeScore = 0;
            var tempElevatorIndex = 0;
            var nextElevatorIndex = 0;
            var changeIndex = 0;

            var random = new Random(4);
            var LIMIT_TIME = new TimeSpan(0, 0, 0, 1, 900);


            cnt = 0;
            while (sw.Elapsed < LIMIT_TIME)
            {
                cnt++;
                tempElevatorIndex = random.Next(E);
                nextElevatorIndex = random.Next(E);
                if (tempElevatorIndex == nextElevatorIndex)
                {
                    continue;
                }

                changeIndex = random.Next(elevator[tempElevatorIndex].Count);
                elevator[nextElevatorIndex].Add(elevator[tempElevatorIndex][changeIndex]);
                elevator[tempElevatorIndex].RemoveAt(changeIndex);

                nextTimeScore = CalcTime();

                //if (nextScoreTime > tempScoreTime)
                //{
                //    //元に戻す
                //    outputL[index] = tempFloor;
                //}
                //else
                //{
                //    tempScoreTime = nextScoreTime;
                //}
            }

            var test = cnt;
            sw.Stop();


            //前後に空白を追加
            output = " " + string.Join(" ", L_Origin) + " ";
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

            output = output.Substring(1, output.Length - 2);

            var outputL = output.Split(' ').Select(x => int.Parse(x)).ToList();
            Console.WriteLine(String.Join(" ", outputL));
        }
    }

}
