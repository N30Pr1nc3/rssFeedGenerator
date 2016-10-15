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
        public static string savefile = @".\data.txt";
        static void Main(string[] args)
        {
            RootObject objekt;
            if (File.Exists(savefile))
            {
                string text = System.IO.File.ReadAllText(Program.savefile);
                objekt = JsonConvert.DeserializeObject<RootObject>(text);
            }
            else
            {
                objekt = new RootObject();
            }
            
            Random rnd = new Random();
            while (true)
            {
                objekt.update();                
                string json = JsonConvert.SerializeObject(objekt);
                System.IO.File.WriteAllText(Program.savefile, json);
                Thread.Sleep(rnd.Next(10, 30) * 1000);
            }            
        }

        public static void log(string _msg)
        {
            Console.WriteLine(_msg);
        }
    }
}
