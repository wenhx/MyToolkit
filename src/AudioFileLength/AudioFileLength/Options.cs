namespace AudioLengthCounter
{
    class Options
    {
        public string Directory { get; set; }

        public bool IncludeSubdirectories { get; set; }

        public bool Verbose { get; set; }

        public bool DisplayHelp { get; set; }

        internal string Message { get; set; }
    }
}
