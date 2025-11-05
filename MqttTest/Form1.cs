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
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Extensions.ManagedClient;
using Newtonsoft.Json;
using System.Threading;
using static System.Windows.Forms.Design.AxImporter;


namespace MqttTest
{
    public partial class FormMain : Form
    {
        private IManagedMqttClient _mqttClient; // <-- MOVED HERE


        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Enabled = true;

            // --- Client created and handlers attached here ---
            _mqttClient = new MqttFactory().CreateManagedMqttClient();

            _mqttClient.ConnectedAsync += OnConnected;
            _mqttClient.DisconnectedAsync += OnDisconnected;

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
        private async Task EduardoCode(string topic, string brokerUrl, int brokerPort, int autoConnectDelaySeconds,
            int publishIntervalInMilliseconds, int nbrToPublish,
            Action<int> onPublished)
        {
            // 1. Build the regular client options (where to connect)
            var clientOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(brokerUrl, brokerPort)
                .Build();

            // 2. Build the managed client options (how to auto-reconnect)
            var options = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(autoConnectDelaySeconds))
                .WithClientOptions(clientOptions)
                .Build();

            // Client creation and handlers are REMOVED from here (moved to Form_Load)

            // Starts a connection with the Broker
            try
            {

                await _mqttClient.StartAsync(options);
            }
            catch (MqttCommunicationException ex)
            {
                // This replaces 'OnConnectingFailed'
                Log.LogIt("E", $"Connection failed: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Log.LogIt("E", $"An error occurred: {ex.Message}");
                return;
            }

            // Send a new message to the broker every send interval
            // Do until total is reached.
            int publishCount = 0;
            while (publishCount < nbrToPublish)
            {
                // ... inside while loop ...
                string json = JsonConvert.SerializeObject(new { message = "Heyo :)", sent = DateTimeOffset.UtcNow });

                // Build a v4 message
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic($"{topic}/topic/json")
                    .WithPayload(json)
                    .Build();

                // Use 'EnqueueAsync' instead of 'PublishAsync'
                await _mqttClient.EnqueueAsync(message);

                publishCount++;
                onPublished(publishCount);

                // Use 'await' to prevent UI freeze
                await Task.Delay(publishIntervalInMilliseconds);
            }
        }

    private void buttonPublish_Click(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

        }


        public static Task OnConnected(MqttClientConnectedEventArgs obj)
        {
            Log.LogIt("I", "Successfully connected.");
            return Task.CompletedTask; // <-- Must return Task
        }

        public static void OnConnectingFailed(ManagedProcessFailedEventArgs obj)
        {
            // This method is no longer used by the client.
            // Logic is now in the 'try-catch' block.
            Log.LogIt("W", "Couldn't connect to broker.");
        }

        public static Task OnDisconnected(MqttClientDisconnectedEventArgs obj)
        {
            Log.LogIt("I", "Successfully disconnected.");
            return Task.CompletedTask; // <-- Must return Task
        }

        private void OnPublished(int nn)
        {
            labelNumberPublished.Text = $"Published={nn}";
            return;
        }

        private async void buttonBlaster_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textConnectPort.Text, out int port))
                return;

            int publishInterval = (int)updnPublishInterval.Value;
            int nbrToPublish = (int)updnNumberToPublish.Value;

            Action<int> increment = OnPublished;
            await EduardoCode(textPublishTopic.Text, textConnectHosturl.Text, port, 30, publishInterval, nbrToPublish, increment);
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
