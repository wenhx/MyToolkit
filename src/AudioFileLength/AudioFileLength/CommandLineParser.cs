using System;
using System.IO;

namespace AudioLengthCounter
{
    class CommandLineParser
    {
        public static Options Parse(string[] args)
        {
            var rtn = new Options { Directory = Environment.CurrentDirectory };
            if (args == null || args.Length == 0)
                return rtn;

            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                    continue;

                if (arg.StartsWith('-'))
                {
                    if (arg == "-s")
                    {
                        rtn.IncludeSubdirectories = true;
                    }

                    if (arg == "-v")
                    {
                        rtn.Verbose = true;
                    }

                    continue;
                }

                rtn.Directory = arg;
            }

            CheckInput(rtn);
            return rtn;
        }

        static void CheckInput(Options rtn)
        {
            if (!Directory.Exists(rtn.Directory))
            {
                rtn.Error = "path is not exists.";
            }
        }
    }
}
