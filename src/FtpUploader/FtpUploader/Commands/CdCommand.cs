using System;
using System.Net.FtpClient;

namespace FtpUploader.Commands
{
    class CdCommand : Command
    {
        public CdCommand()
            : base(CommandName.Cd)
        {

        }

        public override void Execute(Options options, FtpClient client)
        {
            if (options == null)
                throw new ArgumentNullException("options");
            if (client == null)
                throw new ArgumentNullException("client");

            if (string.IsNullOrWhiteSpace(options.Destination))
                return;

            options.Destination = options.Destination.Trim();
            if (options.Destination == "/")
                return;

            if (!client.DirectoryExists(options.Destination))
            {
                client.CreateDirectory(options.Destination, true);
            }

            client.SetWorkingDirectory(options.Destination);
        }
    }
}
