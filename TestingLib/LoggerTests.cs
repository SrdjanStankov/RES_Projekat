using NUnit.Framework;
using LoggerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerLib.Tests
{
    [TestFixture()]
    public class LoggerTests
    {
        [Test()]
        [TestCase("Message", "C:\\Users")]
         public void Logger_BadFailName(string message, string fileName)
        {
            Logger.Instance.Log(message, fileName);
        }

        [Test()]
        [TestCase("Message", "C:\\Log.txt")]
        public void Logger_GoodTest(string message, string fileName)
        {
            Logger.Instance.Log(message, fileName);
        }
    }
}