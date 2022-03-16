using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicateEverywhere
{
    class EverythingSearchedResult
    {
        public string Keyword { get; set; }
        public string OriginalFileSize { get; set; }
        public int Total { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long Size { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public string TotalString
        {
            get
            {
                if (Total > 0)
                    return Total.ToString();
                return string.Empty;
            }
        }
    }
}
