//using CsvStream;
using SplitFormts.Csv;
using SplitFormts.Values;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CsvReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var v1 = new ValueObject("-2147483648");
            Console.WriteLine(v1.IntegerValue);

            var sw = new Stopwatch();
            sw.Start();
            //var t = new SplitFormts.Values.ValueLazyObject("0.123e2");
            //Console.WriteLine(t.DoubleValue);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using (var sr = new CsvStreamReader("KEN_ALL.CSV", CsvReadMode.UseEscape)) {
                foreach (var ans in sr.Enumerator) {
                    for (int i = 0; i < ans.Count; ++i) {
                        Console.Write($"{ans[i].ToString()},");
                    }
                    Console.WriteLine();
                }
            }

            /*
            using (var sr = CsvStreamReader.FromString(@"""1
2""""4"", 5, 678")) {
                Console.WriteLine(sr.ReadLine());
            }
            */

            sw.Stop();
            Console.WriteLine("{0}s.", sw.Elapsed.TotalSeconds);
        }
    }
}
