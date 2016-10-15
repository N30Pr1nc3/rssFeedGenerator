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
    class Program
    {
        public static string savefile = @"data.txt";
        private static RootObject RootObjekt;

        static void Main(string[] args)
        {   
            if (File.Exists(savefile))
            {
                string text = System.IO.File.ReadAllText(Program.savefile);
                Program.RootObjekt = JsonConvert.DeserializeObject<RootObject>(text);
            }
            else
            {
                Program.RootObjekt = new RootObject();
            }

            SimpleHTTPServer webServer = new SimpleHTTPServer("127.0.0.1", 12345);
            Random rnd = new Random();
            while (true)
            {
                Program.RootObjekt.update();                
                string json = JsonConvert.SerializeObject(Program.RootObjekt);
                System.IO.File.WriteAllText(Program.savefile, json);
                Thread.Sleep(rnd.Next(10, 30) * 1000);
            }            
        }

        public static void log(string _msg)
        {
            Console.WriteLine(_msg);
        }

        internal static string generateRssFeed(string feed)
        {
            if (feed.StartsWith("/Pr0gramm/"))
            {
                string filterList = feed.Substring(10);
                String[] filter = filterList.Split('&');
                return  Program.RootObjekt.createFeed(filter);
            }
            return "feed not found";
        }
    }
}
