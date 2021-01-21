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

        List<IEventDefinition> EventDefList { get; set; } = new List<IEventDefinition>();

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

            // Add the connector element property definition to this Element
            pd = schema.PropertyDefinitions.AddElementProperty("MqttServer", MqttSubscribeConnectorDefinition.MY_ID);
            pd.DisplayName = "MQTT Server";
            pd.Description = "The MQTT address of the server(broker). For example, localhost:1883. Default port is 1883.";
            pd.Required = true;

            // The MQTT Topic, which looks like this: foo/bar
            pd = schema.PropertyDefinitions.AddStringProperty("Topic", "MqttSample1/MySubscribedActions");
            pd.DisplayName = "MQTT Topic";
            pd.Description = "The MQTT Topic we are subscribing to (e.g. 'MqttSample1/MySubscribedActions'";
            pd.Required = true;

            // Example of how to add a state definition to the element.
            IStateDefinition sd;
            sd = schema.StateDefinitions.AddStringState("Payload");
            sd.DisplayName = "MQTT Payload";
            sd.Description = "Where the subscribed MQTT's topic's payload data is placed";

            // Create Simio Events that can be fired when an MQTT message is received. 
            // For this implementation, there are up to three of these.
            IEventDefinition ed;
            ed = schema.EventDefinitions.AddEvent("OnMqttReceivedEvent1");
            ed.Description = "Event1 that will be Fired upon receipt of MQTT Topic message";
            EventDefList.Add(ed);

            ed = schema.EventDefinitions.AddEvent("OnMqttReceivedEvent2");
            ed.Description = "Event2 that will be Fired upon receipt of MQTT Topic message";
            EventDefList.Add(ed);

            ed = schema.EventDefinitions.AddEvent("OnMqttReceivedEvent3");
            ed.Description = "Event3 that will be Fired upon receipt of MQTT Topic message";
            EventDefList.Add(ed);

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

        private readonly IElementProperty prServerElement;

        IPropertyReaders _props;

        MqttSubscribeConnector MqttConnector { get; set; }

        string SubscribeTopic { get; set; }

        /// <summary>
        /// A lookup list of simio events
        /// </summary>
        List<IEvent> TopicEventList { get; set; }

        /// <summary>
        /// The constructor for the element.
        /// Called at runtime when the element is first referenced.
        /// </summary>
        /// <param name="data"></param>
        public MqttSubscriberElement(IElementData data)
        {
            _data = data;

            _props = _data.Properties;

            // The IElementData contains a reference to the execution context
            var context = _data.ExecutionContext;

            prServerElement = (IElementProperty)_props.GetProperty("MqttServer");
            MqttConnector = (MqttSubscribeConnector)prServerElement.GetElement(context);

            // Get the Property Readers
            IPropertyReader prTopic = _data.Properties.GetProperty("Topic");

            // Place simio events into a lookup array that will be used at runtime.
            TopicEventList = new List<IEvent>();

            foreach ( var eventDef in _data.Events )
            {
                TopicEventList.Add(eventDef);
            }

            LogIt($"SubscriberElement Start: Url={MqttConnector.ServerUrl} Port={MqttConnector.ServerPort} ");

            SubscribeTopic = prTopic.GetStringValue(_data.ExecutionContext);
            
        }

        #region IElement Members

        /// <summary>
        /// Method called when the simulation run is initialized.
        /// (after Run is specified but before the simulation actually begins)
        /// </summary>
        public void Initialize()
        {
            try
            {
                if (MqttConnector.ServerUrl == null)
                    return;

                // Subscribe to the MQTT server and topic
                SubscriberClient = new MqttFactory().CreateMqttClient();
                var task = Task.Run(() => MqttHelpers.ConnectClient(SubscriberClient, MqttConnector.ServerUrl, MqttConnector.ServerPort));
                task.Wait();

                // Get ready to receive
                MqttHelpers.MqttSubscribeSetHandler(SubscriberClient, HandlePayload);

                // Subscribe to the MQTT's Server/Broker for a topic
                CancellationToken cancelToken = new CancellationToken();
                MqttHelpers.MqttSubscribe(SubscriberClient, SubscribeTopic, cancelToken);

            }
            catch (Exception ex)
            {
                Alert($"Initialize. Subscribing to MQTT:{ex.Message}");
            }

        }

        /// <summary>
        /// The handling of the subscribed payload.
        /// By convention, this payload is an action, followed by a colon, and then any other info needed.
        /// For example, A new order is created with ORDER:ADD:order-name
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

            // Parse the payload. The format is SIMIOEVENT={event#}:PAYLOAD={string}
            // There should be 2 tokens. The simioevent, and the payload data.
            string[] tokens = payload.Trim().Split(':');
            if (tokens.Length != 2)
            {
                LogIt($"Payload should be 2 colon-delimited tokens. Found={payload}");
                return;
            }


            try
            {
                string[] eventsTokens = tokens[0].ToLower().Split('=');
                if ( eventsTokens.Length != 2 || eventsTokens[0] != "simioevent"  )
                {
                    LogIt($"Event Token should be of the form SimioEvent=myEventNumber. Found={tokens[0]}");
                    return;
                }

                if ( !Int32.TryParse(eventsTokens[1], out int eventNumber) || (eventNumber < 1 || eventNumber > 3))
                {
                    LogIt($"Event Number must be in integer in the range of 1..3. Found={eventsTokens[1]}");
                    return;
                }

                // Use the calendar to fire the event since we may be on a different thread
                string simioEventName = $"OnMqttReceivedEvent{eventNumber}";

                string[] payloadTokens = tokens[1].ToLower().Split('=');
                if (payloadTokens.Length != 2 || payloadTokens[0] != "payload")
                {
                    LogIt($"Payload Token should be of the form Payload=myPayloadString. Found={tokens[1]}");
                    return;
                }

                string payloadData = payloadTokens[1];

                _data.ExecutionContext.Calendar.ScheduleCurrentEvent(null, (obj) =>
                {
                    var simEvent = TopicEventList.SingleOrDefault(rr => rr.Name.ToLower() == simioEventName.ToLower());
                    if (simEvent != null)
                    {
                        // Set the Simio State variable Payload to the payload
                        var srPayload = ((IStringState)_data.States["Payload"]);
                        srPayload.Value = payloadData;

                        simEvent.Fire();
                    }
                    else
                        LogIt($"No such SimioEvent={simioEventName}");
                });

                LogIt($"Info: MQTT Event Msg. Topic={topic} SimioEvent={simioEventName} Payload={payload}");

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
            if (SubscriberClient == null)
                return;

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
