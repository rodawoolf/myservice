using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace myservice
{
    class http
    {
        public static void Listener(Config.Config conf)
        {


            if (!HttpListener.IsSupported)
            {
                 return;
            }

            socketw socketwu = new socketw();

              // Create a listener.
              HttpListener listener = new HttpListener();
            // Add the prefixes.

            listener.Prefixes.Add(conf.url);

            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;

           

            while (true)
            {

                listener.Start();
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Stream body = request.InputStream;
                // Display the URL used by the client.

                string[] subs = request.RawUrl.Split('/');
                resultresponse responseob = resultresponse.getInstance();
                responseob.command = subs[1];

                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                // response.ContentType = "Application/json";
                // Construct a response.

                string responseString = "{result:undefined}";
                string portName ="";
                string speed = "";
                string jsonBody;
                string data = "";
                string newLine = "\n";

                using (StreamReader readStream = new StreamReader(body, Encoding.UTF8))
                {
                    try {
                        jsonBody = readStream.ReadToEnd();
                        dynamic sett = JsonConvert.DeserializeObject(jsonBody);
                        portName = sett.portName;
                        speed = sett.speed;
                        data = sett.data;
                        newLine = sett.newLine;

                        responseob.port = portName;
                        responseob.result = jsonBody;

                        if (stringComparer.Equals(subs[1], "open") && request.HasEntityBody)
                        {
                            if (!conf.rs232.ContainsKey(portName))
                            {
                                rs232 con1;
                                con1 = rs232.getInstance();
                                conf.rs232.Add(portName, con1);
                                newLine = sett.newLine;
                                con1.initPort(portName, speed, conf, newLine);
                                responseob.result = "open";
                                responseob.status = 1;
                            }
                            else
                            {
                                responseob.result = "reopen";
                                conf.rs232[portName].Open(conf);
                                responseob.status = 1;
                            }
                        }

                        if (stringComparer.Equals(subs[1], "close") && request.HasEntityBody)
                        {
                            if (conf.rs232.ContainsKey(portName))
                            {
                                rs232 con1 = conf.rs232[portName];
                                con1.Close();
                                responseob.result = "close ok";
                                responseob.status = 1;

                            }

                        }

                        if (stringComparer.Equals(subs[1], "send") && request.HasEntityBody)
                        {

                            if (conf.rs232.ContainsKey(portName))
                            {
                                rs232 con1 = conf.rs232[portName];
                                con1.sendData(data);
                                responseob.result = "send ok";
                                responseob.status = 1;
                            }
                            else
                            {
                                responseob.result = String.Format("port: {0} not open", portName);
                            }



                        }

                        if (stringComparer.Equals(subs[1], "read") && request.HasEntityBody)
                        {


                            if (conf.rs232.ContainsKey(portName))
                            {

                                responseob.result = conf.rs232[portName].data;
                                responseob.status = 1;
                            }
                            else
                            {
                                responseob.result = String.Format("port: {0} not open", portName);
                            }

                        }

                        if (stringComparer.Equals(subs[1], "getipweight") && request.HasEntityBody)
                        {
                            if (stringComparer.Equals(newLine, "udp"))
                            {
                                try
                                {
                                    socketwu.initUDP();
                                }
                                catch
                                {

                                }
                                 responseob.result = socketwu.getWeightUDP(portName, speed, data);
                               // responseob.result = @"udp";
                            }
                            else
                            {
                                 responseob.result = socketw.getWeight(portName, speed, data);
                            }
                          

                            responseob.status = 1;
                            if (responseob.result == @"terminal unavailable")
                            { responseob.status = 0; }



                        }

                        if (stringComparer.Equals(subs[1], "get"))
                        {

                            responseob.result = JsonConvert.SerializeObject(conf);
                            responseob.status = 1;

                        }

                    }
                    catch { }
                   
                }


                responseString = JsonConvert.SerializeObject(responseob);

                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                
                output.Close();
               
            }
        }

    }
}
