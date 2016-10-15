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
                this.lastUpdate = DateTime.Now;
                this.updateRoot();
                return;
            }
            foreach(Item item in this.items)
            {
                if (!item.loadet)
                {
                    this.updateItem(item);
                    return;
                }
            }
            foreach(Item item in this.items)
            {
                item.loadet = false;
            }            
        }

        private void updateRoot()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://pr0gramm.com/api/items/get?flags=1");
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                json = reader.ReadToEnd();
            }
            JsonConvert.PopulateObject(json, this);
        }

        private void updateItem(Item item)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Concat("http://pr0gramm.com/api/items/info?itemId=", item.id.ToString()));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

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
