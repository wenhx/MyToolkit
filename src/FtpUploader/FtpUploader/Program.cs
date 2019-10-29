using CommandLine;
using System.Linq;

namespace FtpUploader
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args);
            if (!result.Errors.Any())
            {
                Ftp.Execute(result.Value);
            }
        }
    }
}