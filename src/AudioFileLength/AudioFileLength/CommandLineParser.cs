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

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
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
                        case "-n":
                            try
                            {
                                rtn.FilesNumber = int.Parse(args[++i]);
                            }
                            catch
                            {

                            }
                            break;
                        case "-k":
                            try
                            {
                                rtn.SkipFilesNumber = int.Parse(args[++i]);
                            }
                            catch
                            {

                            }
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
                sb.AppendLine("-h           Display help.");
                sb.AppendLine("-s           Include subfolders.");
                sb.AppendLine("-v           Display verbose information.");
                sb.AppendLine("-n number    The max number files to count.");
                sb.AppendLine("-k number    Skip number files before counting.");
                sb.AppendLine();
                sb.AppendLine("path:");
                sb.AppendLine("  The folder to count, if empty use current folder.");

                rtn.Message = sb.ToString();
            }
        }
    }
}
