using System;
using System.IO;

namespace PromptDiff.Utils
{
    public static class LogWriter
    {
        private const string LogFileName = "log.txt";

        public static void Write(string text)
        {
            // Ensure directory exists
            Directory.CreateDirectory(AppPaths.LocalDataDirectory);

            var logPath = Path.Combine(AppPaths.LocalDataDirectory, LogFileName);

            using TextWriter tw = new StreamWriter(logPath, true);
            tw.WriteLine(string.Empty);
            tw.WriteLine("----------");
            tw.WriteLine($"TimeStamp: {DateTime.Now}");
            tw.WriteLine(text);
        }
    }
}