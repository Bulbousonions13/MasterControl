using Msa.Interfaces.Bus.NetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using MQTTnet;
using Newtonsoft.Json.Linq;

namespace MasterControl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            JObject config = JObject.Parse(File.ReadAllText(".\\masterConfig.json"));
            
            Boolean setup = (Boolean)config["Setup"] ;
            string  MasterIP = (string)config["MasterIP"];
            int MasterPort = (int)config["MasterPort"];
            
            if(setup){
                Console.WriteLine("");
                Console.WriteLine(" ********************************************************SETUP********************************************************");
                Console.WriteLine("");
                Console.WriteLine(" Step 0 : Open the masterConfig.json file in the Master Control directory.");               
                Console.WriteLine(" Step 1 : Change the MasterPort field to the appropriate Port # listed in the Master Control machine's config.json.");
                Console.WriteLine(" Step 2 : Change the Setup field from 'true' to 'false'.");
                Console.WriteLine(" Step 3 : Close this window and relaunch the program.");
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

                Console.WriteLine("Starting Up mqttserver");          
            
                MQTTServerManager serverManager = new MQTTServerManager();
                Task.Run(async() =>                
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1(serverManager));
                    }
                );
                try{ 
            
                    serverManager.StartServer().Wait();
                }
                catch(Exception e){ 
                
                    Console.WriteLine("There was an error on the Server.");
                    Console.WriteLine(e);
                }   

            }            
        }
    }
}
