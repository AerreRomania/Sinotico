using System;
using System.IO;

namespace CsvLibExpress
{
    public class Log
    {
        /// <summary>
        /// Writes the data output.
        /// </summary>
        /// <param name="message">The message.</param>
        public void WriteLog(string message)
        {
            StreamWriter sw = null;

            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Log.txt", true);
            sw.WriteLine(DateTime.Now + ": " + message);
            sw.Flush();
            sw.Close();
        }
    }
}