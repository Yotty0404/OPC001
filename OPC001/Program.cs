using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace OPC001
{
    public class Group
    {
        public List<int> Members;
    }

    [Serializable]
    public class Elevator
    {
        public List<Group> HavingGroup;
        public int TotalTime;
        public int EN;

        public Elevator(int en)
        {
            HavingGroup = new List<Group>();
            TotalTime = 0;
            EN = en;
        }

        public void Add(Group g)
        {
            HavingGroup.Add(g);
            if (TotalTime == 0)
            {
                TotalTime = g.Members.Max();
            }
            else
            {
                TotalTime += g.Members.Max() * 2 + EN;
            }

        }

        public Group RemoveAt(int groupIndex)
        {
            var rtn = new Group();
            rtn = HavingGroup[groupIndex];
            HavingGroup.RemoveAt(groupIndex);
            TotalTime -= rtn.Members.Max();

            return rtn;
        }
    }

    public class Elevators
    {
        public List<Elevator> Elevator;

        public Elevators()
        {
            Elevator = new List<Elevator>();
        }

        public int GetElevatorIndexMinTotalTime()
        {
            return Elevator.FindIndex(x => x.TotalTime == Elevator.Min(y => y.TotalTime));
        }

        public Elevators ShallowCopy()
        {
            return (Elevators)MemberwiseClone();
        }

        public Elevators Deepcopy()
        {
            var es = ShallowCopy();
            var rtn = new Elevators();

            foreach (var itemE in es.Elevator)
            {
                var e = new Elevator(es.Elevator.FirstOrDefault().EN);
                foreach (var itemG in itemE.HavingGroup)
                {
                    var g = new Group();
                    g.Members = new List<int>();
                    foreach (var itemM in itemG.Members)
                    {
                        g.Members.Add(itemM);
                    }
                    e.Add(g);
                }
                rtn.Elevator.Add(e);
            }

            return rtn;
        }
    }


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

            var groups = new List<Group>();
            var elevators = new Elevators();

            //エレベーターの収容人数ごとにグループ分け
            for (int i = 0; i < N / EN; i++)
            {
                groups.Add(new Group { Members = L.GetRange(i * EN, EN) });
            }


            //最初の10グループを各エレベーターに追加
            for (int i = 0; i < E; i++)
            {
                var e = new Elevator(EN);
                e.Add(groups[i]);
                elevators.Elevator.Add(e);
            }

            var index = E;
            while (index < N / EN)
            {
                //現状トータルタイムが最小のエレベーターにグループを追加
                elevators.Elevator[elevators.GetElevatorIndexMinTotalTime()].Add(groups[index]);
                index++;
            }

            var random = new Random(4);
            var LIMIT_TIME = new TimeSpan(0, 0, 0, 1, 900);
            var tempTime = 0;
            var elevatorIndex = 0;
            var nextelevatorIndex = 0;
            var groupIndex = 0;
            var nextgroupIndex = 0;

            var cnt = 0;
            while (true)
            //while (sw.Elapsed < LIMIT_TIME)
            {
                cnt++;
                var tempElavators = elevators.Deepcopy();
                tempTime = elevators.Elevator.Max(x => x.TotalTime);
                elevatorIndex = random.Next(E);
                nextelevatorIndex = random.Next(E);
                if (elevatorIndex == nextelevatorIndex)
                {
                    continue;
                }

                //インデックス0を避けて、グループインデックスを生成
                groupIndex = random.Next(elevators.Elevator[elevatorIndex].HavingGroup.Count() - 1) + 1;
                nextgroupIndex = random.Next(elevators.Elevator[nextelevatorIndex].HavingGroup.Count() - 1) + 1;

                //グループごと交換
                var g = new Group();
                g = elevators.Elevator[elevatorIndex].RemoveAt(groupIndex);
                elevators.Elevator[nextelevatorIndex].Add(g);

                g = new Group();
                g = elevators.Elevator[nextelevatorIndex].RemoveAt(nextgroupIndex);
                elevators.Elevator[elevatorIndex].Add(g);

                if (elevators.Elevator.Max(x => x.TotalTime) >= tempTime)
                {
                    elevators = tempElavators;
                }
                else
                {
                    var fjkdasklfj = 1;
                }
            }

            //前後に空白を追加
            output = " " + string.Join(" ", L_Origin) + " ";
            //Console.WriteLine(output);
            var e_num = 1;
            foreach (var e in elevators.Elevator)
            {
                foreach (var g in e.HavingGroup)
                {
                    foreach (var n in g.Members)
                    {
                        //空白を挟んで置換することで、数字単位で置換
                        var re = new Regex(" " + n.ToString() + " ");
                        output = re.Replace(output, " " + e_num.ToString() + " ", 1);
                    }
                }

                e_num++;
            }

            //前後の空白を削除
            output = output.Substring(1, output.Length - 2);
            var outputL = output.Split(' ').Select(x => int.Parse(x)).ToList();
            //var tempScoreTime = CalcTime(outputL);
            //var nextScoreTime = 0;
            //var tempFloor = 0;
            //var nextFloor = 0;

            //var random = new Random(4);
            //var LIMIT_TIME = new TimeSpan(0, 0, 0, 1, 900);


            //cnt = 0;
            //while (sw.Elapsed < LIMIT_TIME)
            //{
            //    cnt++;
            //    //最後のグループは固定
            //    index = random.Next(N-E*EN);
            //    tempFloor = outputL[index];
            //    nextFloor = random.Next(E) + 1;
            //    if (tempFloor == nextFloor)
            //    {
            //        continue;
            //    }

            //    outputL[index] = nextFloor;
            //    nextScoreTime = CalcTime(outputL);
            //    if (nextScoreTime >= tempScoreTime)
            //    {
            //        outputL[index] = tempFloor;
            //    }
            //    else
            //    {
            //        tempScoreTime = nextScoreTime;
            //    }
            //}

            //var test = cnt;
            sw.Stop();
            Console.WriteLine(String.Join(" ", outputL));
        }
    }

}
