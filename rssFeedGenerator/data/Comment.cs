using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rssFeedGenerator
{
    public class Comment
    {
        public int id { get; set; }
        public int parent { get; set; }
        public string content { get; set; }
        public int created { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public double confidence { get; set; }
        public string name { get; set; }
        public int mark { get; set; }
    }
}
