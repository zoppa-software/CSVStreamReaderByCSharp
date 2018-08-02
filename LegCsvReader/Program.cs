using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FZZ01_LocalUtility.Common.Csv;

namespace LegCsvReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            using (var sr = new FZZ01_0200_CsvStreamReader("KEN_ALL.CSV", System.Text.Encoding.Default)) {
                while (!sr.EndOfStream) {
                    var ans = sr.ReadGroup();
                    //for (int i = 0; i < ans.Length; ++i) {
                    //    Console.Write($"{ans[i].StringValue},");
                    //}
                    //Console.WriteLine();
                };
            }
            sw.Stop();
            Console.WriteLine("{0}s.", sw.Elapsed.TotalSeconds);
        }
    }
}
