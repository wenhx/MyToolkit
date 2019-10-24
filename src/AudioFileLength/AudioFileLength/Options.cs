namespace AudioLengthCounter
{
    class Options
    {
        public string Directory { get; set; }

        public bool IncludeSubdirectories { get; set; }

        public bool Verbose { get; set; }

        internal string Error { get; set; }
    }
}
