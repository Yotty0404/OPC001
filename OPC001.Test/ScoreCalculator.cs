using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace OPC001.Test.Tool
{
    public class ScoreCalculator
    {
        public static CalcResult Calc(string input, string output)
        {
            var l = input.Split('\n').ToArray();
            var input1 = l[0].Split();

            var N = int.Parse(input1[0]);
            var X = int.Parse(input1[1]);
            var Y = int.Parse(input1[2]);
            var Z = int.Parse(input1[3]);
            var a = l[1].Split(' ').Select(x => int.Parse(x)).ToList();

            var b = output.Split(' ').Select(x => int.Parse(x)).ToList();

            return Calc(N, X, Y, Z, a, b);
        }

        public static CalcResult Calc(int N, int X, int Y, int Z, List<int> a, List<int> b)
        {
            if (b.Count != N) throw new Exception("出力結果の個数が不正です");
            if (!b.All(bi => 1 <= bi && bi <= Y)) throw new Exception("出力結果の範囲が不正です");

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

            return new CalcResult
            {
                T = t,
                Score = (int)Math.Round(1000D * N / t),
            };
        }
    }

    public class CalcResult
    {
        public int T { get; set; }
        public int Score { get; set; }
    }

    [DebuggerDisplay("CURRENT:{_currentFloor}")]
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
