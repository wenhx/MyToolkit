using System;
using System.IO;

namespace FtpUploader.Commands
{
    class PutCommand : Command
    {
        public PutCommand()
            : base(CommandName.Put)
        {

        }

        public override void Execute(Options options, System.Net.FtpClient.FtpClient client)
        {
            if (options == null)
                throw new ArgumentNullException("options");
            if (client == null)
                throw new ArgumentNullException("client");

            foreach (var f in options.File.Split(','))
            {
                var fileName = Path.GetFileName(f);
                if (File.Exists(f))
                {

                    using (Stream ostream = client.OpenWrite(fileName))
                    {
                        try
                        {
                            using (var source = File.OpenRead(f))
                            {
                                source.CopyTo(ostream);
                            }
                        }
                        finally
                        {
                            ostream.Close();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("file [{0}] not exits.", fileName);
                }
            }
        }
    }
}
