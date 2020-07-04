using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    class MqttSubscriberElementDefinition : IElementDefinition
    {
        #region IElementDefinition Members

        /// <summary>
        /// Property returning the full name for this type of element. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "MqttSubscriberElement"; }
        }

        /// <summary>
        /// Property returning a short description of what the element does.
        /// </summary>
        public string Description
        {
            get { return "An element that connects to a MQTT Server and fires an Event when the give topic is received."; }
        }

        /// <summary>
        /// Property returning an icon to display for the element in the UI.
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Property returning a unique static GUID for the element.
        /// </summary>
        public Guid UniqueID
        {
            get { return MY_ID; }
        }

        /// <summary>
        /// Changed 1Jul2020/dth
        /// </summary>
        public static readonly Guid MY_ID = new Guid("{8D75B9E4-E992-4B06-9B1F-630A77EAE31A}");

        /// <summary>
        /// Method called that defines the property, state, and event schema for the element.
        /// </summary>
        public void DefineSchema(IElementSchema schema)
        {
            // Example of how to add a property definition to the element.
            IPropertyDefinition pd;

            pd = schema.PropertyDefinitions.AddStringProperty("ServerUrl", "localhost");
            pd.DisplayName = "Server Url";
            pd.Description = "Url for the MQTT Server (broker)";
            pd.Required = true;

            pd = schema.PropertyDefinitions.AddStringProperty("ServerPort", "1883");
            pd.DisplayName = "Server Port";
            pd.Description = "Port for the MQTT Server (broker)";
            pd.Required = true;

            // The MQTT Topic, which looks like this: foo/bar
            pd = schema.PropertyDefinitions.AddStringProperty("Topic", "MqttSample/Actions");
            pd.DisplayName = "Mqtt Topic";
            pd.Description = "The MQTT Topic we are subscribing to (e.g. 'MqttSample/Actions'";
            pd.Required = true;
            
            // Example of how to add a state definition to the element.
            IStateDefinition sd;
            sd = schema.StateDefinitions.AddStringState("ReceivedPayload");
            sd.DisplayName = "Mqtt Payload";
            sd.Description = "Where the subscribed MQTT's topic's payload data is placed";

            // Create a Simio Event that we will fire when an MQTT message is received
            IEventDefinition ed;
            ed = schema.EventDefinitions.AddEvent("OnMqttMessageReceived");
            ed.Description = "Event that will be Fired upon receipt of MQTT Topic message";

        }

        /// <summary>
        /// Method called to add a new instance of this element type to a model.
        /// Returns an instance of the class implementing the IElement interface.
        /// </summary>
        public IElement CreateElement(IElementData data)
        {
            return new MqttSubscriberElement(data);
        }

        #endregion
    }

    /// <summary>
    /// This Element subscribes to a MQTT topic.
    /// When the topic is received, it fires an event.
    /// </summary>
    class MqttSubscriberElement : IElement
    {
        IElementData _data;

        IMqttClient SubscriberClient { get; set; }

        IPropertyReaders _props;

        string ServerUrl { get; set; }
        int ServerPort { get; set; }
        string Topic { get; set; }

        List<IEvent> TopicEventList { get; set; }


        /// <summary>
        /// The constructor for the element.
        /// Called at runtime whenever the element is first referenced.
        /// </summary>
        /// <param name="data"></param>
        public MqttSubscriberElement(IElementData data)
        {
            _data = data;

            _props = _data.Properties;

            //var prConnector = _data.Properties.GetProperty("MqttConnect");
            // Get the Property Readers
            IPropertyReader prServerUrl = _data.Properties.GetProperty("ServerUrl");
            IPropertyReader prServerPort = _data.Properties.GetProperty("ServerPort");
            IPropertyReader prTopic = _data.Properties.GetProperty("Topic");

            // Get a reference to our event
            TopicEventList = new List<IEvent>();
            TopicEventList.Add(_data.Events["OnMqttMessageReceived"]);

            //var myEvent = prServerEventRef.GetStringValue(_data.ExecutionContext);
            ServerUrl = prServerUrl.GetStringValue(_data.ExecutionContext);
            string port = prServerPort.GetStringValue(_data.ExecutionContext);
            if (int.TryParse(port, out int portNumber))
                ServerPort = portNumber;

            Topic = prTopic.GetStringValue(_data.ExecutionContext);
            
        }

        #region IElement Members

        /// <summary>
        /// Method called when the simulation run is initialized.
        /// </summary>
        public void Initialize()
        {
            try
            {
                // Subscribe to the MQTT server and topic
                SubscriberClient = new MqttFactory().CreateMqttClient();
                var task = Task.Run(() => MqttHelpers.ConnectClient(SubscriberClient, ServerUrl, ServerPort));
                task.Wait();

                // Get ready to receive
                MqttHelpers.MqttSubscribeSetHandler(SubscriberClient, HandlePayload);

                // Subscribe to the MQTT's Server/Broker for a topic
                CancellationToken cancelToken = new CancellationToken();
                MqttHelpers.MqttSubscribe(SubscriberClient, Topic, cancelToken);

                LogIt($"Info: Subscribed to Client={SubscriberClient}. Server={ServerUrl}");

            }
            catch (Exception ex)
            {
                Alert($"Initialize. Subscribing to MQTT:{ex.Message}");
            }

        }

        /// <summary>
        /// The handling of the subscribed payload.
        /// Convention of this payload is keyword, then a number, separated by colon.
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="payload"></param>
        private void HandlePayload(string topic, string payload)
        {
            if ( string.IsNullOrEmpty(topic))
            {
                LogIt($"Received Null Topic.");
                return;
            }

            if (string.IsNullOrEmpty(payload))
            {
                LogIt($"Received empty payloaod.");
                return;
            }

            // Parse the payload and look at the top level
            string[] tokens = payload.Trim().ToUpper().Split(':');
            if (tokens.Length != 2)
            {
                return;
            }

            if ( !int.TryParse(tokens[1], out int eventNumber) || eventNumber < 0 || eventNumber > TopicEventList.Count()-1 )
            {
                return;
            }

            try
            {
                // Based on the payload we can take different actions.
                // Here, we'll just always fire our defined Simio Event.
                switch (tokens[0])
                {
                    case "EVENT":
                        {
                            // Use the calendar to fire the event since we may be on a different thread
                            _data.ExecutionContext.Calendar.ScheduleCurrentEvent(null, (obj) =>
                            {
                                TopicEventList[eventNumber].Fire();
                            });

                            LogIt($"Info: MQTT Event Msg. Topic={topic} Payload={payload}");
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                Alert($"Handler for received messages. Topic={topic}. Err={ex.Message}");
            }

        }

        /// <summary>
        /// Method called when the simulation run is terminating.
        /// </summary>
        public void Shutdown()
        {
            var task = Task.Run( () => SubscriberClient.DisconnectAsync());
            task.Wait();
        }

        private void LogIt(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.TraceInformation($"MqttSubscribeElement::{msg}");
        }
        private void Alert(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.ReportError($"MqttSubscribeElement::{msg}");
        }

        #endregion
    }
}
