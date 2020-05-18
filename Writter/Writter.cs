using Common;
using LoggerLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WritterLib
{
    public class Writter
    {
        private List<string> areas = new List<string>()
        {
            "Novi Sad",
            "Beograd",
            "Nis",
            "Kragujevac",
            "Bor",
            "Babin zub",
            "Uzice",
            "Zrenjanin"
        };

        public Tuple<Code, Value> WriteToDumpingBuffer(IRandom r)
        {
            var code = Enum.GetValues(typeof(Code)).Cast<Code>().ToList();
            var choosedCode = code[r.Next(code.Count - 1)];

            var value = new Value(new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day), areas[r.Next(areas.Count - 1)], r.Next(1000));

            Logger.Instance.Log($"Automatic write:   Code: {choosedCode}  Value: {value}");
            return new Tuple<Code, Value>(choosedCode, value);
        }

        public Tuple<Code, Value> ManualWriteToHistory()
        {
            Console.WriteLine("Input Code");
            var inputCode = Console.ReadLine();

            if(!Enum.TryParse(inputCode, out Code code))
            {
                Console.WriteLine("Wrong code");
                return null;
            }

            Console.WriteLine("Input area");
            var inputArea = Console.ReadLine();

            Console.WriteLine("Input consumption");
           if(!Int32.TryParse(Console.ReadLine(), out int consumption))
           {
                Console.WriteLine("Consumption must be number");
                return null;
           }

            if (consumption < 0)
            {
                return null;
            }

            var value = new Value(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), inputArea, consumption);

            Logger.Instance.Log($"Manual write:   Code: {inputCode}  Value: {value}");
            return new Tuple<Code, Value>(code, value);
        }
    }
}
