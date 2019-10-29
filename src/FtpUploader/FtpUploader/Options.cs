using CommandLine;

namespace FtpUploader
{
    class Options
    {
        [Option('h', "host", Required = true, HelpText = "Specifies the host name or IP address of the remote host to connect to.")]
        public string Host { get; set; }

        [Option('p', "port", DefaultValue = 21, HelpText = "The port used by remote host to connect to.")]
        public int Port { get; set; }

        [Option('u', "user", HelpText = "The user name authenticate by remote host.")]
        public string User { get; set; }

        [Option("pw", HelpText = "The password authenticate by remote host.")]
        public string Password { get; set; }

        [Option('t', "timeout", HelpText = "The length of time in seconds for a data connection to be established before giving up.", DefaultValue = 60)]
        public int Timeout { get; set; }

        [Option('a', "aotoaccept", HelpText = "Auto accept remote certificate.", DefaultValue = true)]
        public bool AcceptRemoteCertificate { get; set; }

        [Option('f', "file", HelpText = "Input files to be transactions.")]
        public string File { get; set; }

        [Option('v', "verbose", HelpText = "Display verbose information.")]
        public bool Verbose { get; set; }

        [Option('d', "destination", HelpText = "The folder will be store input files in remote host.")]
        public string Destination { get; set; }
    }
}
