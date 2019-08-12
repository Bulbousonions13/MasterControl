using MQTTnet;

using System.Threading.Tasks;
using System.Text;
using MQTTnet.Client.Options;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using System;
using System.Net;
using System.Globalization;

namespace ClientRecorder
{
    public class MQTTConnectionManager{ 

        private Recorder _recorder;
        private IManagedMqttClient client;
        private int MasterPort;
        private int ActiveDevice;
        private string MasterIP;
        public Boolean recording = false;
        public Boolean connected = false;

        public MQTTConnectionManager(string masterIP, int masterPort, int activeDevice){ 
            
            
            
            var factory = new MqttFactory();            
            client = factory.CreateManagedMqttClient();
            
            MasterIP = masterIP;
            MasterPort = masterPort;
            ActiveDevice = activeDevice;
            _recorder = new Recorder(ActiveDevice);

            Console.WriteLine("MQTT Client Initializing");

            Setup();
        }
        public async Task connect(){

            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(new MqttClientOptionsBuilder()
                    .WithClientId("Client "+Dns.GetHostName())
                    .WithTcpServer(MasterIP,MasterPort)                   
                    .WithCleanSession()
                    .Build())
                .Build();
            
            await client.StartAsync(options);

            Console.WriteLine("MQTT Client Running");           

            await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("master/start").Build());
            await client.SubscribeAsync(new TopicFilterBuilder().WithTopic("master/stop").Build());
            
            Console.ReadLine();
        }
        
        public async void publish(string messageText, string topic){ 
            
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(messageText)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await client.PublishAsync(message);
        }
        
        public void Setup(){ 

              client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("");
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
                // Task.Run(() => client.PublishAsync("hello/world"));               
                
                if(e.ApplicationMessage.Topic == "master/start" ){
                    if (!recording) {
                        recording=true;
                        publish("\nStart Signal received @ "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture),"master/response");
                        _recorder.record();
                    }                    
                }
                else if(e.ApplicationMessage.Topic == "master/stop" ){ 
                    if(recording){ 
                       recording = false;
                       publish("\nStop Signal received @ "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture),"master/response");
                       _recorder.stop();
                    }                    
                }               
            });

            client.UseDisconnectedHandler(e=>{
                Console.WriteLine("Attempting to Connect to " + MasterIP);
                Console.WriteLine("\nDisconnected from mqtt broker !");
                connected = false;
            } );

            client.UseConnectedHandler(c=>{
                Console.WriteLine("\nConnected to mqtt broker!");
                connected = true;
            });

            
        }         
    } 
}

