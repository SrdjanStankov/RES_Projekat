using System;
using System.IO;

namespace LoggerLib
{
    public class Logger
    {
        public static Logger Instance { get; set; } = new Logger();

        private Logger() { }

        public void Log(string message, string fileName = "Log.txt")
        {
            try
            {
                using (var sw = new StreamWriter(fileName, append: true))
                {
                    sw.WriteLine(message);
                }
            }
            catch (UnauthorizedAccessException)
            {
                fileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.None)}\\Log.txt";
                using (var sw = new StreamWriter(fileName, append: true))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
