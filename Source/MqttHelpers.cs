using MQTTnet;
using MQTTnet.Client;
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

                CancellationToken cancellationToken = CancellationToken.None;
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
        public static void MqttSubscribeSetHandler(IMqttClient subscriberClient, Action<string, string> ProcessMessage)
        {
            if (subscriberClient == null)
                throw new ApplicationException($"Null Subscriber Client");

            try
            {
                // Use the += operator to subscribe to the event
                subscriberClient.ApplicationMessageReceivedAsync += msg =>
                {
                    // CRITICAL: Add a try...catch INSIDE the handler
                    try
                    {
                        string payload = "";
                        // The payload is now a "PayloadSegment" for performance.
                        // Check its Count property instead of checking for null.
                        if (msg.ApplicationMessage.PayloadSegment.Count > 0)
                        {
                            payload = Encoding.UTF8.GetString(msg.ApplicationMessage.PayloadSegment);
                        }

                        ProcessMessage(msg.ApplicationMessage.Topic, payload);
                    }
                    catch (Exception ex)
                    {
                        // Properly report the error. This is vital for debugging.
                        // You might want to log this to a file or the Simio trace.
                        // Re-throwing here is an option if you have a global handler.
                        Console.WriteLine($"Error processing MQTT message: {ex}");
                    }

                    // Because the new handler is async, you must return a Task.
                    return Task.CompletedTask;
                };

                // This Task.Delay(20) is likely unnecessary and can be removed.
                // The event subscription (+=) is synchronous.
                // await Task.Delay(20); 
            }
            catch (Exception ex)
            {
                // This only catches errors while *assigning* the handler,
                // not errors *inside* the handler.
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
        /// Establish a connection with a TCP client to the given broker address and port.
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

                CancellationToken cancellationToken = CancellationToken.None;
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
