using FtpUploader.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.FtpClient;

namespace FtpUploader
{
    class Ftp
    {
        internal static void Execute(Options options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            if (options.Verbose)
            {
                FtpTrace.AddListener(new System.Diagnostics.ConsoleTraceListener());
            }
            using (FtpClient client = new FtpClient())
            {
                client.Host = options.Host;
                client.Port = options.Port;
                if (options.User != null)
                {
                    client.Credentials = new NetworkCredential(options.User, options.Password);
                }
                client.DataConnectionType = FtpDataConnectionType.PASV; //TODO: 参数化
                client.EncryptionMode = FtpEncryptionMode.Explicit;
                client.ValidateCertificate += new FtpSslValidation(OnValidateCertificate);
                client.DataConnectionConnectTimeout = options.Timeout * 1000;
                client.Connect();
                var commands = CreateCommands();
                foreach (var cmd in commands)
                {
                    cmd.Execute(options, client);
                }
            }
        }

        static IEnumerable<Command> CreateCommands()
        {
            var cd = CommandFactory.Create(CommandName.Cd);
            yield return cd;
            var put = CommandFactory.Create(CommandName.Put);
            yield return put;
        }

        static void OnValidateCertificate(FtpClient control, FtpSslValidationEventArgs e)
        {
            if (e.PolicyErrors != System.Net.Security.SslPolicyErrors.None)
            {
                e.Accept = true;
            }
        }
    }
}
