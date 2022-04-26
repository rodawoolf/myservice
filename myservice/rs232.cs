using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace myservice
{
    class rs232
    {
        bool _continue;

        public bool is_open { get; set; }
        public string data { get; set; }

        SerialPort _serialPort;

        public static rs232 getInstance()
        {
            return new rs232();
        }

        public void initPort(string PortName, string BaudRate, Config.Config conf, string NewLine)
        {
            Thread thread1 = new Thread(() => Init(PortName, BaudRate, conf, NewLine));
            thread1.Start();
          
        }

        public void Init(string defaultPortName, string BaudRate, Config.Config conf, string NewLine)
        {

            _serialPort = new SerialPort();
            _serialPort.PortName = defaultPortName;
            _serialPort.BaudRate = int.Parse(BaudRate);
            _serialPort.ReadTimeout = 200;
            _serialPort.WriteTimeout = 200;
            _serialPort.NewLine = NewLine;
            try
            {
                _serialPort.Open();
                is_open = true;
                _continue = true;
                Thread readThread = new Thread(() => Read(_serialPort, conf, ref _continue));
                readThread.Start();


            }
            catch (Exception ex)
            {

                is_open = false;
            }

        }

        public static void Read(SerialPort _serialPort, Config.Config conf, ref bool _continue)
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    conf.rs232[_serialPort.PortName].data = message;
                    
                }
                catch (TimeoutException) { }
            }
        }

       

        public void sendData(string data_)
        {
            try
            {
                _serialPort.WriteLine(data_);
            }
            catch (TimeoutException) { }

        }

        public void Open(Config.Config conf)
        {
            _continue = false;
            Thread.Sleep(500);
            _serialPort.Close();
            _serialPort.Open();

            Thread readThread = new Thread(() => Read(_serialPort, conf, ref _continue));
      
            _continue = true;
            is_open = true;
            readThread.Start();
          
        }

        public void Close()
        {
            _continue = false;
            Thread.Sleep(500);
            _serialPort.Close();
            is_open = false;
            _serialPort.Dispose();
        }

    }
}
