using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Extensions.Rpc;
using SimioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MqttSteps
{
    public static class MqttHelpers
    {

        /// <summary>
        /// Publish some data for all the subscribers to see.
        /// Takes an already connected client and a topic string, plus payload.
        /// </summary>
        public static async void MqttPublish(IMqttClient publishClient, string topic, string payload)
        {
            if (publishClient == null)
                throw new ApplicationException("MqttPublish::PublishClient is null");

            if (topic == null)
                throw new ApplicationException("MqttPublish::Topic is null.");

            try
            {
                var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

                CancellationToken cancellationToken;
                await publishClient.PublishAsync(message, cancellationToken);

            }
            catch (Exception ex)
            {
                string payload80 = payload.Length > 80 ? (payload.Substring(80) + "...") : payload;
                throw new ApplicationException($"MqttPublish:: Topic={topic} Payload={payload80} Err-{ex}");
            }
        }

        /// <summary>
        /// Set up the Action to be taken when messages are received.
        /// </summary>
        /// <param name="subscriberClient"></param>
        /// <param name="ProcessMessage">Action delegate to return Topic and Payload received</param>
        public static async void MqttSubscribeSetHandler(IMqttClient subscriberClient, Action<string, string> ProcessMessage)
        {
            if (subscriberClient == null)
                throw new ApplicationException($"Null Subscriber Client");

            try
            {
                subscriberClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(msg =>
                {
                    string payload = "";
                    if (msg.ApplicationMessage.Payload != null)
                        payload = Encoding.UTF8.GetString(msg.ApplicationMessage.Payload);

                    ProcessMessage(msg.ApplicationMessage.Topic, payload );
                });

                await Task.Delay(20);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Initializing Subscriber. Err={ex}");
            }
        }

        /// <summary>
        /// Subscribe the given client to a topic.
        /// </summary>
        /// <param name="subscriberClient"></param>
        /// <param name="topic"></param>
        public static async void MqttSubscribe(IMqttClient subscriberClient, string topic, CancellationToken cancellationToken)
        {
            if (subscriberClient == null)
                throw new ApplicationException($"Null Subscriber Client");

            try
            {
                var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                    .WithTopicFilter(topic)
                    .Build();

                await subscriberClient.SubscribeAsync(subscribeOptions, cancellationToken);

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Subscribing: Topic={topic} Err={ex}");
            }

        }

        /// <summary>
        /// Establish a connection with a TCP client at the given address and port.
        /// This connection is made with *no* KeepAlive timeout.
        /// There is a short delay (100 ms) after the connection.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static async Task ConnectClient(IMqttClient client, string serverAddress, int port)
        {
            try
            {
                var connectOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer(serverAddress, port)
                    .WithNoKeepAlive()
                    .Build();

                CancellationToken cancellationToken;
                await client.ConnectAsync(connectOptions, cancellationToken);

                await Task.Delay(100);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Start Client: Server={serverAddress}:{port} Err={ex.Message}");
            }
        }



    }
}
