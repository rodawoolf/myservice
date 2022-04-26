using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace myservice
{
    public partial class MyService : ServiceBase
    {

        static Config.Config conf;
        

        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            conf = new Config.Config();
            Thread thread1 = new Thread(new ThreadStart(httpserv));
            thread1.Start();



        }

        protected override void OnStop()
        {

        }

        static void httpserv()
        {

            http.Listener(conf);
        }
    }
}
