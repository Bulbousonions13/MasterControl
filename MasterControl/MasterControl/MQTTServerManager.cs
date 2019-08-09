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

namespace MasterControl
{
    public class MQTTServerManager{ 

        private IMqttServer server;

        public MQTTServerManager(){ 
            server = new MqttFactory().CreateMqttServer();
        }
        public async Task StartServer(){ 
            
            server.UseClientConnectedHandler(c =>                
            {
                Console.WriteLine("Client Connected !");
                Console.WriteLine("ID : " + c.ClientId);                
            }           
            );
            
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithConnectionBacklog(100)
                .WithDefaultEndpointPort(4503);
            
            await server.StartAsync(optionsBuilder.Build());
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await server.StopAsync();           
       }     

        public async void publish(string messageText, string topic){ 
            
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

