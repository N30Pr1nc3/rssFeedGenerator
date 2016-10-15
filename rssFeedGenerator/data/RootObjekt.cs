using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace rssFeedGenerator
{
    public class RootObject
    {


        public bool atEnd { get; set; }
        public bool atStart { get; set; }
        public object error { get; set; }
        public List<Item> items { get; set; }
        public int ts { get; set; }
        public string cache { get; set; }
        public int rt { get; set; }
        public int qc { get; set; }

        private DateTime lastUpdate = new DateTime(1,1,1);

        public void update()
        {
            if (DateTime.Now.Subtract(this.lastUpdate).TotalMinutes > 20)
            {
                Program.log("update Root");
                this.lastUpdate = DateTime.Now;
                this.updateRoot();
                return;
            }
            foreach(Item item in this.items)
            {
                Program.log("update Item");
                if (!item.loadet)
                {
                    this.updateItem(item);
                    return;
                }
            }
            Program.log("Reset Loadet");
            foreach (Item item in this.items)
            {
                item.loadet = false;
            }            
        }

        private void updateRoot()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://pr0gramm.com/api/items/get?flags=1");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*;q=0.8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }
            JsonConvert.PopulateObject(json, this);
        }

        internal string createFeed(string[] _filter)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<rss xmlns:atom=\"http://www.w3.org/2005/Atom\" version = \"2.0\">\r\n");
            sb.Append("<channel>\r\n");
            sb.Append("<title>Pr0gramm</title>\r\n");
            sb.Append("<link>http://pr0gramm.com</link>\r\n");
            sb.Append("<description>Pr0gramm</description >\r\n");
            sb.Append("<language > de - de </language >\r\n");
            sb.Append("<webMaster ></webMaster >\r\n");
            //sb.Append("< atom:link href = "http://me-studium.de/rss.php" rel = "self" type = "application/rss+xml" />

            foreach (Item tItem in this.items)
            {
                foreach (string filter in _filter)
                {
                    if (tItem.matchesFilter(filter))
                    {
                        sb.Append("<item>\r\n");
                        sb.Append("<title>\r\n");
                        foreach (Tag tag in tItem.tags)
                        {
                            sb.Append(tag.tag);
                            sb.Append(" ");
                        }
                        sb.Append("</title>\r\n");
                        sb.Append("<link>\r\n");
                        sb.Append("http://pr0gramm.com/new/");
                        sb.Append(tItem.id);
                        sb.Append("</link>\r\n");
                        sb.Append("<pubDate >");
                        sb.Append(tItem.created);
                        sb.Append("</ pubDate >");
                        sb.Append("<guid>\r\n");
                        sb.Append(tItem.id);
                        sb.Append("</guid>\r\n");
                        sb.Append("<description >\r\n");
                        sb.Append("<![CDATA[");
                        sb.Append("<img border = 0 src = \"http://img.pr0gramm.com/");
                        sb.Append(tItem.image);
                        sb.Append("\" ></ img >");
                        sb.Append("]]>\r\n");
                        sb.Append("</description>\r\n");
                        sb.Append("</item>\r\n");
                        break;
                    }
                }
            }
            sb.Append("</channel></rss>");
            return sb.ToString();
        }

        private void updateItem(Item item)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat("http://pr0gramm.com/api/items/info?itemId=", item.id.ToString()));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*;q=0.8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            RootObject objekt = new RootObject();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                JsonConvert.PopulateObject(reader.ReadToEnd(), item);
                item.loadet = true;
            }
        }
    }
}
