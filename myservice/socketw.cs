using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace myservice
{
    class socketw
    {
        public UdpClient udpClient { get; set; }

        public static socketw getInstance()
        {
          
            return new socketw();
        }
        public static  string getWeight(string adress, string port, string command)
        {
            // Data buffer for incoming data.  
            byte[] data = new byte[1024];

            string result = @"no data";

            // Connect to a remote device.
            if (true)
            {
                try
                {
                    TcpClient tcpClient = new TcpClient();

                    var res = tcpClient.BeginConnect(adress, Int32.Parse(port), null, null);
                    var success = res.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));
                    if (!success)
                    {
                        throw new Exception("Failed to connect.");
                    }
                    tcpClient.EndConnect(res);
                    // tcpClient.Connect(adress, Int32.Parse(port));
                    NetworkStream netStream = tcpClient.GetStream();
                    // byte[] msg = Encoding.ASCII.GetBytes("ALL\r");
                    byte[] msg = Encoding.ASCII.GetBytes(command);
                    
                    netStream.Write(msg, 0, msg.Length);
                   
                    int bytes = netStream.Read(data, 0, data.Length);

                    result = Encoding.ASCII.GetString(data, 0, bytes);

                    netStream.Close();
                    tcpClient.Close();


                }
                catch {
                     result = @"terminal unavailable";
                }
            }
           

            return result;
        }

        public  string getWeightUDP(string adress, string port, string command)
        {
           
            // Data buffer for incoming data.  
            byte[] data = new byte[1024];

            string result = @"no data";

            // Connect to a remote device.
            if (true)
            {
                try
                {

                    this.udpClient.Connect(adress, Int32.Parse(port));
                    byte[] msg = Encoding.ASCII.GetBytes(command);

                    this.udpClient.Send(msg, msg.Length);

                    IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    result = returnData.ToString();

                    this.udpClient.Close();
 
                }
                catch(Exception e)
                {
                     //result = @"terminal unavailable";
                    result = e.ToString();
                }
            }
           

            return result;
        }

        public socketw initUDP()
        {
            
            try
            {
                this.udpClient = new UdpClient(12345);
                this.udpClient.Client.SendTimeout = 1000;
                this.udpClient.Client.ReceiveTimeout = 1000;
            }
            catch (Exception e)
            {

            }

            return this;
          
        }


    }
}
