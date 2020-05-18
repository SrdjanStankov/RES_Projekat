using Common;
using DumpingBufferLib;
using HistoricalLib;
using ReaderLib;
using System;
using System.Globalization;
using System.Threading;
using WritterLib;

namespace MainApp
{
    internal class Program
    {
        private static Writter writter = new Writter();
        private static DumpingBufferr buffer = new DumpingBufferr();
        private static Historical historical = new Historical();
        private static Reader reader = new Reader();

        private static void Main(string[] args)
        {

            reader.GetChangesForInterval(DateTime.MinValue, DateTime.MaxValue);

            //DumpingBufferLib.Class1 w = new DumpingBufferLib.Class1();
            var thread = new Thread(WriteEvery2Sec)
            {
                IsBackground = true
            };
            thread.Start();

            while (true)
            {
                //dal zeli manuelno ili da citam
                Meni();


            }

        }

        private static void WriteEvery2Sec()
        {
            while (true)
            {
                var ret = writter.WriteToDumpingBuffer(new RandomImplement());

                var rec = buffer.ReceiveValues(ret);
                if (rec != null)
                {
                    historical.ReceiveValue(rec);
                }

                Thread.Sleep(2000);
            }
        }

        private static void Meni()
        {
            Console.WriteLine("Enter number for action: ");
            Console.WriteLine("1.Manual Write");
            Console.WriteLine("2.Read from data base");
            var izbor = Int32.Parse(Console.ReadLine());
            if (izbor == 1)
                writter.ManualWriteToHistory();
            else if (izbor == 2)
                MeniForReadFromDataBase();
            else
                Console.WriteLine("Entered number is wrong");
        }

        private static void MeniForReadFromDataBase()
        {
            Console.WriteLine("Choose interval for reading from base: ");
            Console.WriteLine("1.Last houre");
            Console.WriteLine("2.Last day");
            Console.WriteLine("3.Last week");
            Console.WriteLine("4.Choose manual intevral ");
            var izbor = Int32.Parse(Console.ReadLine());
            if (izbor == 1)
                reader.WriteDatabase(DateTime.Now.AddHours(-1), DateTime.Now);
            else if (izbor == 2)
                reader.WriteDatabase(DateTime.Now.AddHours(-24), DateTime.Now);
            else if (izbor == 3)
                reader.WriteDatabase(DateTime.Now.AddDays(-7), DateTime.Now);
            else if (izbor == 4)
            {
                ManualEnterTime();
            }
            else
                Console.WriteLine("Entered number is wrong ");
        }

        private static void ManualEnterTime()
        {
            DateTime d1 = new DateTime();
            DateTime d2 = new DateTime();

            Console.Write("Enter start date and hour (for example 2/6/2012 11:25 AM): ");
            string unos1 = Console.ReadLine();
            try
            {
                d1 = DateTime.ParseExact(unos1, "d/M/yyyy HH:mm tt", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Bad time format");
            }

            Console.Write("Enter end date and hour (for example 2/6/2012 11:25 AM): ");
            string unos2 = Console.ReadLine();

            try
            {
                d2 = DateTime.ParseExact(unos2, "d/M/yyyy HH:mm tt", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                Console.WriteLine("Bad time format");
            }


            reader.WriteDatabase(d1, d2);
        }
    }
}
