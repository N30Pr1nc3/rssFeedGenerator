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
        public static string configPath = @"config.txt";
        private static RootObject RootObjekt;
        public static Config config = new Config();

        static void Main(string[] args)
        {   
            if (File.Exists(Program.configPath))
            {
                string text = System.IO.File.ReadAllText(Program.configPath);
                Program.config = JsonConvert.DeserializeObject<Config>(text);

                if (File.Exists(config.datapath))
                {
                    text = System.IO.File.ReadAllText(config.datapath);
                    Program.RootObjekt = JsonConvert.DeserializeObject<RootObject>(text);
                }
                else
                {
                    Program.RootObjekt = new RootObject();
                }

                Random rnd = new Random();
                while (true)
                {
                    Program.RootObjekt.update();
                    string json = JsonConvert.SerializeObject(Program.RootObjekt);
                    System.IO.File.WriteAllText(config.datapath, json);
                    byte[] bytes = Encoding.Default.GetBytes(Program.RootObjekt.createFeed(Program.config.searchTags.ToArray()));
                    System.IO.File.WriteAllText(config.programm, Encoding.UTF8.GetString(bytes));
                    int inter = Program.config.RefreshIntervall;
                    int variants= Program.config.RefreshIntervall;
                    Thread.Sleep(rnd.Next(inter-(int)(variants*0.5)*1000, inter + (int)(variants * 0.5) * 1000));
                }
            }
            else
            {
                Program.log("Fehler config nicht gefunden");
            }           
        }

        public static void log(string _msg)
        {
            Console.WriteLine(_msg);
        }     
    }
}
