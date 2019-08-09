using MQTTnet;

using System.Threading.Tasks;
using System.Linq;
using System.Text;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using System.Threading;
using MQTTnet.Extensions.ManagedClient;
using System;
using MQTTnet.Server;
using System.Globalization;

namespace MasterControl
{
    public class MQTTServerManager{ 

        private IMqttServer server;

        float timeSent;

        public MQTTServerManager(){ 
            
            server = new MqttFactory().CreateMqttServer();

            server.UseApplicationMessageReceivedHandler( e=> 
            {
                if(e.ApplicationMessage.Topic == "master/response"){
                    Console.WriteLine("\nMassage received from client!");
                    Console.WriteLine("ID : " + e.ClientId);
                    Console.WriteLine("Payload : " + Encoding.UTF8.GetString(e.ApplicationMessage.Payload)); 
                }
            });                
            

            server.UseClientConnectedHandler(c =>                
            {
                Console.WriteLine("\nClient Connected !");
                Console.WriteLine("ID : " + c.ClientId);                
            }           
            );
        }
        public async Task StartServer(){ 
            
          
            
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(4503);
            
            await server.StartAsync(optionsBuilder.Build());
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await server.StopAsync();           
       }     

        public async void publish(string messageText, string topic){ 
            
            
            Console.WriteLine("\nMaster Control is sending a message @ " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture));

            timeSent = DateTime.Now.Millisecond;
            
            var message = new MqttApplicationMessageBuilder()            
                .WithTopic(topic)
                .WithPayload(messageText)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
                          
            await server.PublishAsync(message, CancellationToken.None);
        }        
    } 
}

