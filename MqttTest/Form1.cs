using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;


namespace MqttTest
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        /// <summary>
        /// Eduardo's code: https://dev.to/eduardojuliao/basic-mqtt-with-c-1f88
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="brokerUrl"></param>
        /// <param name="brokerPort"></param>
        /// <param name="autoConnectDelaySeconds"></param>
        /// <param name="publishIntervalInMilliseconds"></param>
        /// <param name="nbrToPublish"></param>
        private void EduardoCode(string topic, string brokerUrl, int brokerPort, int autoConnectDelaySeconds, 
            int publishIntervalInMilliseconds, int nbrToPublish,
            Action<int> onPublished)
        {

            // Creates a new client
            MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                    .WithClientId(topic)
                                                    .WithTcpServer(brokerUrl, brokerPort);

            // Create client options objects
            ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(autoConnectDelaySeconds))
                                    .WithClientOptions(builder.Build())
                                    .Build();

            // Creates the client object
            IManagedMqttClient _mqttClient = new MqttFactory().CreateManagedMqttClient();

            // Set up handlers
            _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
            _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
            _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);

            // Starts a connection with the Broker
            _mqttClient.StartAsync(options).GetAwaiter().GetResult();

            // Send a new message to the broker every send interval
            // Do until total is reached.
            int publishCount = 0;
            while (publishCount < nbrToPublish)
            {
                string json = JsonConvert.SerializeObject(new { message = "Heyo :)", sent = DateTimeOffset.UtcNow });
                _mqttClient.PublishAsync($"{topic}/topic/json", json);
                publishCount++;
                onPublished(publishCount);

                Task.Delay(publishIntervalInMilliseconds).GetAwaiter().GetResult();
            }
        }
    private void buttonPublish_Click(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

        }


        public static void OnConnected(MqttClientConnectedEventArgs obj)
        {
            Log.LogIt("I", "Successfully connected.");
            
        }

        public static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            Log.LogIt("W", "Couldn't connect to broker.");
        }

        public static void OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            Log.LogIt("I", "Successfully disconnected.");
        }

        private void OnPublished(int nn)
        {
            labelNumberPublished.Text = $"Published={nn}";
            return;
        }

        private void buttonBlaster_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textConnectPort.Text, out int port))
                return;

            int publishInterval = (int)updnPublishInterval.Value;
            int nbrToPublish = (int)updnNumberToPublish.Value;

            Action<int> increment = OnPublished;
            EduardoCode(textPublishTopic.Text, textConnectHosturl.Text, port, 30, publishInterval, nbrToPublish, increment);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( Log.IsDirty )
            {
                textLogs.Text = Log.BuildLog();
                Log.IsDirty = false;
            }
        }

    }
}
