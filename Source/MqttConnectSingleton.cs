using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using SimioAPI;
using SimioAPI.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqttSteps
{
    /// <summary>
    /// A singleton class to hold our data.
    /// It can be renamed and modified as necessary. It demonstrates Jon Skeet's singleton pattern
    /// by providing a thread-safe dictionary of objects with a string key.
    /// </summary>
    public class MqttConnectSingleton
    {
        private static MqttConnectSingleton _instance;

        public bool IsInitialized { get; set; }
        public string ServerUrl { get; set; }
        public int ServerPort { get; set; }

        public IStepExecutionContext ExecutionContext { get; set; }

        public IEvent MyEvent { get; set; }

        /// <summary>
        /// The publisher client can public information for a given topic.
        /// </summary>
        public IMqttClient PublishClient { get; set; }

        /// <summary>
        /// The subscriber client that receives information about topics.
        /// </summary>
        public IMqttClient SubscribeClient { get; set; }


        /// <summary>
        /// The singleton constructor
        /// </summary>
        private MqttConnectSingleton()
        {
            IsInitialized = false;
        }

        /// <summary>
        /// The singleton pattern implemented.
        /// </summary>
        public static MqttConnectSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MqttConnectSingleton();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Initialize, but only once.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public async Task<bool> Initialize (string url, int port, string topic)
        {
            if (IsInitialized)
                return true;

            try
            {
                PublishClient = new MqttFactory().CreateMqttClient();
                await MqttHelpers.ConnectClient(PublishClient, url, port);

                SubscribeClient = new MqttFactory().CreateMqttClient();
                await MqttHelpers.ConnectClient(SubscribeClient, url, port);

                MqttHelpers.MqttSubscribeSetHandler(SubscribeClient, HandleReceive);
                //var result = Task.Run(() => ConnectClient(publishClient, url, port)).Result;

                CancellationToken cancelToken = new CancellationToken();
                MqttHelpers.MqttSubscribe(SubscribeClient, topic, cancelToken);

                IsInitialized = true;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The handling of the subscribed payload
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="payload"></param>
        private void HandleReceive(string topic, string payload)
        {
            // Parse topic and look at the top level
            string[] tokens = topic.Split('/');
            if (tokens.Length == 0)
                return;

            try
            {
                switch (tokens[0])
                {
                    default:
                        {
                            StringBuilder sb = new StringBuilder("Message:");

                            if (ExecutionContext != null)
                            {
                                ExecutionContext.Calendar.ScheduleCurrentEvent(null, (obj) =>
                                {
                                    MyEvent.Fire();
                                });
                            }
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Handler for received messages. Topic={topic}. Err={ex}");
            }
        }

            private async Task<bool> ConnectClient(IMqttClient client, string serverAddress, int port)
        {
            try
            {
                var connectOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(serverAddress, port)
                    .Build();

                CancellationToken cancellationToken;
                await client.ConnectAsync(connectOptions, cancellationToken);

                await Task.Delay(200);
                return true;
            }
            catch (Exception ex)
            {
                Alert($"Start Client: Err={ex}");
                return false;
            }
        }

        private void Alert(string message)
        {

        }

        /// <summary>
        /// Store or replace runtime info
        /// </summary>
        /// <param name="key"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool PutRuntimeInfo(string key, object info)
        {

            return true;
        }
    }
}
