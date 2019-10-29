using System;
using System.Net.FtpClient;

namespace FtpUploader.Commands
{
    abstract class Command
    {
        public CommandName Name { get; private set; }

        public Command(CommandName name)
        {
            Name = name;
        }

        public abstract void Execute(Options options, FtpClient client);
    }
}
