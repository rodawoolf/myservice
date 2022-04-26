using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myservice.Config
{
    class Config
    {
        public Dictionary<string, rs232> rs232 = new Dictionary<string, rs232>();
        
        public string url { get; set; }
        public string ff { get; set; }

        public string ipweight { get; set; }


        string Text;

        string fileName =  @"c:\Program Files (x86)\IMCWeightService\confimc";
        
        public Config()
        {
            url = "http://localhost:5050/";
            ipweight = "";
            ff = fileName;
            if (File.Exists(fileName))
            {
                try
                {
                    using (var sr = new StreamReader(fileName))
                    {
                        Text =  sr.ReadToEnd();
                        if (Text!="")
                        {
                            url = Text;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                  
                }
            }
           

        }
    }
}
