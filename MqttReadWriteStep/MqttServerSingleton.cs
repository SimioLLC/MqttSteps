using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
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
    public class MqttServerSingleton
    {
        private static MqttServerSingleton _instance;

        public bool IsInitialized { get; set; }
        public string ServerUrl { get; set; }
        public int ServerPort { get; set; }

        /// <summary>
        /// The publisher client can public information for a given topic.
        /// </summary>
        IMqttClient publishClient { get; set; }

        /// <summary>
        /// The subscriber client that receives information about topics.
        /// </summary>
        IMqttClient receiveClient { get; set; }


        /// <summary>
        /// The singleton constructor
        /// </summary>
        private MqttServerSingleton()
        {
            IsInitialized = false;
        }

        /// <summary>
        /// The singleton pattern implemented.
        /// </summary>
        public static MqttServerSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MqttServerSingleton();
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
        public bool Initialize (string url, int port)
        {
            if (IsInitialized)
                return true;

            try
            {
                publishClient = new MqttFactory().CreateMqttClient();
                var result = Task.Run(() => ConnectClient(publishClient, url, port)).Result;

                IsInitialized = true;
                return true;
            }
            catch (Exception ex)
            {

                return false;
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
