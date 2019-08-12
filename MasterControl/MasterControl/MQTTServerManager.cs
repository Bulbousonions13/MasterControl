using MQTTnet;

using System.Threading.Tasks;
using System.Text;
using MQTTnet.Client;
using System.Threading;
using System;
using MQTTnet.Server;
using System.Globalization;

namespace MasterControl
{
    public class MQTTServerManager{ 

        private IMqttServer server;
        string MasterIP;
        int MasterPort;

        long timeSent;

        public MQTTServerManager(string masterIP, int masterPort){
            
            MasterIP = masterIP;
            MasterPort = masterPort;
            
            server = new MqttFactory().CreateMqttServer();

            server.UseApplicationMessageReceivedHandler( e=> 
            {   
                
                
                if(e.ApplicationMessage.Topic == "master/response"){
                    long timeArrived = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    Console.WriteLine("\nMaster Control is receiving a message on " +e.ApplicationMessage.Topic+" channel @ " + timeArrived);
                    Console.WriteLine("\nMassage received from client!");
                    Console.WriteLine("ID : " + e.ClientId);
                    Console.WriteLine("Payload : " + Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                    Console.WriteLine("Roundtrip latency was ~" + (timeArrived - timeSent) + " milliseconds");
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
                .WithDefaultEndpointPort(MasterPort);

            Console.WriteLine("Starting Up MQTT Broker on " + MasterIP + ":" + MasterPort);       
            
            await server.StartAsync(optionsBuilder.Build());
            
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
            await server.StopAsync();           
       }     

        public async void publish(string messageText, string topic){ 
            
            timeSent = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("\nMaster Control is sending a message on the " +topic+" channel @ " + timeSent);

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

