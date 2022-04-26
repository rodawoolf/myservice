using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myservice
{
    class resultresponse
    {
        public string result { get; set; }
        public string command { get; set; }
        public string port { get; set; }
        public int status { get; set; }


        public static resultresponse getInstance()
        {
            resultresponse res = new resultresponse();
            res.status = 0;
            res.command = "undefined";
            res.port = "undefined";
            res.result = "";
            return res;
        }
    }
}
