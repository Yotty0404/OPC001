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
            foreach (var n in L) L_Origin.Add(n + 1);
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
            for (int i = 0; i < N / groupNum; i++)
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

            var random = new Random(4);
            var LIMIT_TIME = new TimeSpan(0, 0, 0, 1, 900);
            var tempScoreTime = Calc(N, F, E, EN, L_Origin, L_Output);

            var cnt = 0;
            while (sw.Elapsed < LIMIT_TIME)
            {
                cnt++;
                var index = random.Next(N);
                var nextScoreTime = Calc(N, F, E, EN, L_Origin, L_Output);
                var tempFloor = L_Output[index];
                var nextFloor = random.Next(E) + 1;
                if (tempFloor == nextFloor)
                {
                    continue;
                }

                L_Output[index] = nextFloor;
                nextScoreTime = Calc(N, F, E, EN, L_Origin, L_Output);
                if (nextScoreTime < tempScoreTime)
                {
                    tempScoreTime = nextScoreTime;
                }
                else
                {
                    L_Output[index] = tempFloor;
                }
            }


            Console.WriteLine(String.Join(" ", L_Output));
        }

        public static int Calc(int N, int X, int Y, int Z, List<int> a, List<int> b)
        {
            // 各エレベーターの前に並ぶ社員の行列を作成する
            var queues = Enumerable.Repeat(0, Y).Select(x => new Queue<int>()).ToArray();
            for (int i = 0; i < N; i++)
            {
                // index に変換
                var elevatorIndex = b[i] - 1;
                queues[elevatorIndex].Enqueue(a[i]);
            }

            var elevators = queues.Select(q => new Elevator(Z, q)).ToList();

            var t = 0;
            while (!elevators.All(x => x.Finished))
            {
                t++;
                foreach (var elevator in elevators)
                {
                    elevator.Do();
                }
            }

            return t;
        }

        internal class Elevator
        {
            private readonly int Z;
            private readonly Queue<int> _q;

            private int _currentFloor = 1;
            private List<int> _shanins = new List<int>();

            public bool Finished { get; internal set; }

            public Elevator(int z, Queue<int> q)
            {
                Z = z;
                _q = q;
            }

            internal void Do()
            {
                if (Finished) return;

                if (_shanins.Any())
                {
                    if (_shanins.Any(x => x == _currentFloor))
                    {
                        _shanins.Remove(_currentFloor);
                    }
                    else
                    {
                        _currentFloor++;
                    }
                }
                else
                {
                    if (_currentFloor == 1)
                    {
                        if (_q.Any())
                        {
                            for (int i = 0; i < Z; i++)
                            {
                                _shanins.Add(_q.Dequeue());
                                if (!_q.Any()) break;
                            }
                        }
                        else
                        {
                            // 動作を停止する
                            Finished = true;
                        }
                    }
                    else
                    {
                        _currentFloor--;
                    }
                }

                // 終了判定
                if (!_q.Any() && !_shanins.Any())
                {
                    Finished = true;
                }
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
