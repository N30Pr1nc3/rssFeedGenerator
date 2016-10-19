using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rssFeedGenerator
{
    public class Config
    {
        public string datapath { get; set; }
        public string programm { get; set; }
        public int minUp { get; set; }
        public int rootIntervall { get; set; }
        public int RefreshIntervall { get; set; }
        public int RefreshVariants { get; set; }
        public List<string> searchTags { get; set; }
    }
}
