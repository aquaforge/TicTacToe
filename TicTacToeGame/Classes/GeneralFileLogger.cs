using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToeGame
{
    public static class GeneralFileLogger
    {
        public const string filePath = @"c:\temp\application.log";

        private static readonly object _lock = new object();

        public static void Log(string logText, bool clearLog = false)
        {
            try
            {
                lock (_lock)
                {
                    string text = $"<br>{string.Format("{0:dd.MM.yyyy HH:mm:ss}", DateTime.Now)} {logText}{Environment.NewLine}";
                    //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "_error_.log");
                    if (clearLog)
                        File.WriteAllText(filePath, text);
                    else
                        File.AppendAllText(filePath, text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GeneralFileLogger.Log: {ex.Message}");
            }
        }
    }

}
