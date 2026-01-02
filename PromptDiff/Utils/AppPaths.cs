using System;
using System.IO;

namespace PromptDiff.Utils
{
    public static class AppPaths
    {
        public static string BaseDirectory => AppContext.BaseDirectory;

        public static string LocalDataDirectory => Path.Combine(BaseDirectory, "local_data");
    }
}