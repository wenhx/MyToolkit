using System;
using System.IO;
using System.Text;

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
                    switch (arg)
                    {
                        case "-s":
                            rtn.IncludeSubdirectories = true;
                            break;
                        case "-v":
                            rtn.Verbose = true;
                            break;
                        case "-h":
                            rtn.DisplayHelp = true;
                            break;
                        default:
                            break;
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
                rtn.Message = "path is not exists.";
            }

            if (rtn.DisplayHelp)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("afl {0}", rtn.GetType().Assembly.GetName().Version.ToString());
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Usage: afl [options] [path]");
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("Options:");
                sb.AppendLine("-h   Display help.");
                sb.AppendLine("-s   Include subfolders.");
                sb.AppendLine("-v   Display verbose information.");
                sb.AppendLine();
                sb.AppendLine("path:");
                sb.AppendLine("  The folder to count, if empty use current folder.");

                rtn.Message = sb.ToString();
            }
        }
    }
}
