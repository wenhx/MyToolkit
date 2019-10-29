using System;

namespace FtpUploader.Commands
{
    static class CommandFactory
    {
        public static Command Create(CommandName cmd)
        {
            switch (cmd)
            {
                case CommandName.Put:
                    return new PutCommand();
                case CommandName.Cd:
                    return new CdCommand();
                default:
                    throw new NotSupportedException("不支持的命令。");
            }
        }
    }
}
