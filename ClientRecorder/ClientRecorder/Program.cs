using System;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace ClientRecorder
{
    class Program
    {
        
        private static MQTTConnectionManager client;
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;
        
        static void Main(string[] args)
        {   
            
            ShowWindow(ThisConsole, MINIMIZE);
            
            JObject config = JObject.Parse(File.ReadAllText(".\\clientConfig.json"));
            Boolean setup = (Boolean)config["Setup"] ;
            string  MasterIP = (string)config["MasterIP"];
            int MasterPort = (int)config["MasterPort"];
            int ActiveDevice = (int)config["ActiveDevice#"];
            
            if(setup){               
                
                Console.WriteLine("");
                Console.WriteLine(" Listed below are the Devices that support audio recording.");
                Console.WriteLine("");
                new Recorder(0).showDevices();
                Console.WriteLine("");
                Console.WriteLine(" ********************************************************SETUP********************************************************");
                Console.WriteLine("");
                Console.WriteLine(" Step 0 : Open the clientConfig.json file in the Client Recorder directory.");
                Console.WriteLine(" Step 1 : Change the Active Device # field to the appropriate device desired.");
                Console.WriteLine(" Step 2 : Change the MasterIP field to the appropriate IP Address. This is the Master Control machine's IP.");
                Console.WriteLine(" Step 3 : Change the MasterPort field to the appropriate Port # listed in the Master Control machine's config.json.");
                Console.WriteLine(" Step 4 : Change the Setup field from 'true' to 'false'.");
                Console.WriteLine(" Step 5 : Close this window and relaunch the program.");
                Console.WriteLine("");
                Console.WriteLine(" *********************************************************************************************************************");
                Console.WriteLine("");
                Console.WriteLine(" To see this information again, change the Setup field in config.json file to 'true' and relaunch the program.");
                Console.WriteLine("");
                Console.WriteLine(" Happy Recording!");
                Console.WriteLine("");
                Console.WriteLine(" Press Enter to close this window.");
                Console.ReadLine();
            } else {

                client = new MQTTConnectionManager(MasterIP,MasterPort,ActiveDevice);
                try
                {
                    client.connect().Wait(); 
                    Console.ReadLine();
                                
                }
                catch(Exception e){ 
                
                    Console.WriteLine("There was an error on the Client Side");
                    Console.WriteLine(e);
                }
            }       
        }       
       
    }
}

    
