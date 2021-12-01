using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using OPC001.Test.Tool;

namespace OPC001.Test
{
    [TestClass]
    public class Test
    {
        private static readonly List<int> _scores = new List<int>();
        private static readonly Dictionary<string, string> _outputs = new Dictionary<string, string>();
        private static readonly string[] _inputFiles = new string[]
        {
            @"input/0000.txt",
            @"input/0001.txt",
            @"input/0002.txt",
            @"input/0003.txt",
            @"input/0004.txt",
            @"input/0005.txt",
            @"input/0006.txt",
            @"input/0007.txt",
            @"input/0008.txt",
            @"input/0009.txt",
        };

        [TestMethod]
        public void Run()
        {
            foreach (var file in _inputFiles)
            {
                RunImpl(file);
            }
        }

        private static void RunImpl(string inputFilePath)
        {
            var input = File.ReadAllText(inputFilePath);

            var sb = new StringBuilder();
            using (var reader = new StringReader(input))
            using (var writer = new StringWriter(sb))
            {
                Console.SetIn(reader);
                Console.SetOut(writer);

                // 実装した処理を実行します
                Program.Main();
            }

            var output = sb.ToString();

            var r = ScoreCalculator.Calc(input, output);

            Debug.WriteLine(string.Format("T:{0} Score:{1}", r.T, r.Score));
            _scores.Add(r.Score);
            _outputs.Add(Path.GetFileName(inputFilePath), output);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // 出力結果とスコアの計算結果をデスクトップに出力します
            var folderName = string.Format("result_{0: yyMMddHHmmss}", DateTime.Now);
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var dir = Path.Combine(desktop, folderName);
            Directory.CreateDirectory(dir);

            var path = Path.Combine(dir, "_score.txt");
            File.WriteAllText(path, GetScoreList());

            foreach (var output in _outputs)
            {
                File.WriteAllText(Path.Combine(dir, output.Key), output.Value);
            }
        }

        private static string GetScoreList()
        {
            var sb = new StringBuilder();
            foreach (var item in _scores)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}
