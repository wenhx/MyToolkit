using System;
using System.Collections.Generic;
using System.Text;

namespace DuplicateEverywhere
{
    class Filter
    {
        public List<string> HighlightPaths { get; } = new List<string>();
        public List<string> IgnorePaths { get; } = new List<string>();
        public bool OnlyShow1ResultRows { get; set; }
        public bool NotShowResultsContainHighlightPaths { get; set; }
        public bool SameSize { get; set; }

        public static Filter Current { get; } = new Filter();
        
    }
}
