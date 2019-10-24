using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.IO;

namespace AudioLengthCounter
{
    class Program
    {
        static readonly string _TimeSpanFormat = "hh\\:mm\\:ss";
        static bool _Verbose = false;
        static int _SubfoldersNumber = 0;

        static void Main(string[] args)
        {
            var options = CommandLineParser.Parse(args);
            if (options.Error != null)
            {
                Console.WriteLine(options.Error);
                return;
            }

            _Verbose = options.Verbose;

            var duration = SumFilesDuration(options.Directory);
            if (options.IncludeSubdirectories)
            {
                duration += SumFoldersDuration(options.Directory);
            }

            if (_SubfoldersNumber == 0)
            {
                Console.WriteLine("The duration of folder \"{0}\" is: {1}.", options.Directory, duration.ToString(_TimeSpanFormat));
            }
            else
            {
                Console.WriteLine("The duration of folder \"{0}\"(with {2} subfolders) is: {1}.", options.Directory, duration.ToString(_TimeSpanFormat), _SubfoldersNumber);
            }
        }

        static TimeSpan SumFoldersDuration(string directory)
        {
            var total = TimeSpan.Zero;
            foreach (var folder in Directory.GetDirectories(directory))
            {
                total += SumFilesDuration(folder);
                total += SumFoldersDuration(folder);
                _SubfoldersNumber++;
            }
            return total;
        }

        static TimeSpan SumFilesDuration(string directory)
        {
            var totalDuration = TimeSpan.Zero;
            var files = Directory.GetFiles(directory);
            foreach (var file in files)
            {
                ShellFile so = ShellFile.FromFilePath(file);
                double.TryParse(so.Properties.System.Media.Duration.Value.ToString(),
                    out double nanoseconds);
                if (nanoseconds > 0)
                {
                    var duration = TimeSpan.FromMilliseconds(Convert100NanosecondsToMilliseconds(nanoseconds));
                    if (_Verbose)
                    {
                        Console.WriteLine("[{0}]: {1}.", file, duration.ToString(_TimeSpanFormat));
                    }
                    totalDuration += duration;
                }
            }
            if (_Verbose)
            {
                Console.WriteLine("The duration of folder \"{0}\" is: {1}.", directory, totalDuration.ToString(_TimeSpanFormat));
            }
            return totalDuration;
        }

        public static double Convert100NanosecondsToMilliseconds(double nanoseconds)
        {
            // One million nanoseconds in 1 millisecond, 
            // but we are passing in 100ns units...
            return nanoseconds * 0.0001;
        }
    }
}
