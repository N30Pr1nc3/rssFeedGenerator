using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace rssFeedGenerator
{
    public class Item
    {
        private List<Tag> _tags = new List<Tag>();
        private List<Comment> _comment = new List<Comment>();
        private bool _loadet = false;

        public bool loadet { get { return this._loadet; } set { this._loadet = value; } }

        public List<Tag> tags { get { return this._tags; } set { this._tags = value; } }
        public List<Comment> comments { get { return this._comment; } set { this._comment = value; } }
        public int ts { get; set; }
        public string cache { get; set; }
        public int rt { get; set; }
        public int qc { get; set; }

        public int id { get; set; }
        public int promoted { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public int created { get; set; }
        public string image { get; set; }
        public string thumb { get; set; }
        public string fullsize { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool audio { get; set; }
        public string source { get; set; }
        public int flags { get; set; }
        public string user { get; set; }
        public int mark { get; set; }

        internal bool matchesFilter(string filter)
        {
            if (this.image.EndsWith("jpg"))
            {
                return false;
            }
            if (Program.config.minUp < this.up)
            {
                return true;
            }
            if (this.user == filter)
            {
                return true;
            }
            foreach(Tag tag in this.tags)
            {
                if (tag.tag == filter)
                {
                    return true;
                }
            }
            //foreach (Comment comment in this.comments)
            //{
            //    if (comment.content.Contains(filter))
            //    {
            //        return true;
            //    }
            //}
            return false;
        }
    }
}
