using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using SimioAPI;
using SimioAPI.Extensions;

// There are two classes in this file:
// 1. MqttServerElementDefinition: The definition of the element
// 2. MqttServerElement: The instantiation of the element itself

namespace MqttSteps
{

    //===================================================================================
    // The definition of a Publish connector

    /// <summary>
    /// The element defines the url of the MQTT Server (broker) and the port.
    /// </summary>
    internal class MqttPublishConnectorDefinition : IElementDefinition
    {
        #region IElementDefinition Members

        /// <summary>
        /// Property returning the full name for this type of element. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "MqttPublishConnector"; }
        }

        /// <summary>
        /// Property returning a short description of what the element does.
        /// </summary>
        public string Description
        {
            get { return "Defines a connection to a MQTT Server/Broker (e.g. Mosquitto). Used by the MQTT Publish steps"; }
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
        public static readonly Guid MY_ID = new Guid("{26EEFAED-6ED4-4C6F-B273-7C84BC3EF6AF}");

        /// <summary>
        /// Method called that defines the property, state, and event schema for the element.
        /// This is called from Simio when you select "User Defined" on the ??? tab.,
        /// or when you have these elements already defined in the project.
        /// </summary>
        public void DefineSchema(IElementSchema schema)
        {
            IPropertyDefinition pd = schema.PropertyDefinitions.AddStringProperty("ServerUrl", string.Empty);
            pd.DisplayName = "MQTT Server URL";
            pd.Description = "The address of the MQTT Server (e.g. localhost for a local server, such as Mosquitto)";
            pd.Required = true;

            pd = schema.PropertyDefinitions.AddStringProperty("ServerPort", "1883");
            pd.DisplayName = "MQTT Server Port";
            pd.Description = "Internet port number used by MQTT Server. Default is 1883";
            pd.Required = true;

            // An optional Simio Event can be added to this Mqtt Connection element
            IEventDefinition ed;
            ed = schema.EventDefinitions.AddEvent("MqttEvent");
            ed.Description = "An event owned by this element";

            IStateDefinition sd = schema.StateDefinitions.AddStringState("Topic");
            sd.DisplayName = "MQTT Topic";
            sd.Description = "The received MQTT topic";
            sd.AutoResetWhenStatisticsCleared = false;

            sd = schema.StateDefinitions.AddStringState("Payload");
            sd.DisplayName = "MQTT Payload";
            sd.Description = "The received MQTT Payload";
            sd.AutoResetWhenStatisticsCleared = false;


        }

        /// <summary>
        /// Method called to add a new instance of this element type to a model.
        /// Returns an instance of the class implementing the IElement interface.
        /// Called as the Simulation Run begins.
        /// </summary>
        public IElement CreateElement(IElementData data)
        {
            return new MqttPublishConnector(data);
        }

        #endregion
    }

    //===================================================================================
    // An instance of a public connector.
    // In the constructor an MQTT connection is made.

    /// <summary>
    /// Defines what happens when the runtime starts
    /// </summary>
    class MqttPublishConnector : IElement
    {
        readonly IElementData _data;

        public IMqttClient PublishClient { get; set; }

        /// <summary>
        /// The referenced server/broker
        /// </summary>
        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        /// <summary>
        /// A status topic that is published when the connector is created.
        /// </summary>
        string StatusTopic { get; set; }

        /// <summary>
        /// Constructor. The argument "data" includes run-time info, including ExecutionContext,
        /// I.e. data has members of Events, that can be indexed with the name
        /// and also Properties and ExecutionContext.
        /// </summary>
        /// <param name="data"></param>
        public MqttPublishConnector(IElementData data)
        {
            _data = data;
            try
            {
                IPropertyReader prUrl = _data.Properties.GetProperty("ServerUrl");
                ServerUrl = prUrl.GetStringValue(_data.ExecutionContext);

                IPropertyReader prPort = _data.Properties.GetProperty("ServerPort");
                ServerPort = int.Parse(prPort.GetStringValue(_data.ExecutionContext));

                IPropertyReader prStatusTopic = _data.Properties.GetProperty("StatusTopic");
                StatusTopic = prUrl.GetStringValue(_data.ExecutionContext);

            }
            catch (Exception ex)
            {
                LogIt($"Err={ex.Message}");
            }
        }

        #region IElement Members


        /// <summary>
        /// Method called when the simulation run starts.
        /// Called after the constructor.
        /// </summary>
        public void Initialize()
        {
            try
            {
                ////var mqttEvent = _data.Events["MqttEvent"];

                PublishClient = new MqttFactory().CreateMqttClient();
                var t = Task.Run(() => MqttHelpers.ConnectClient(PublishClient, ServerUrl, ServerPort));
                //Todo: This will throw an error if there are no MQTT Servers.
                t.Wait();

                var info = _data.ExecutionContext.ExecutionInformation;
                string payload = $"Project:{info.ProjectFolder} {info.ProjectName} Model={info.ModelName} Scenario={info.ScenarioName} Replication={info.ReplicationNumber}";

                // Publish a topic to indicate our presence
                //MqttHelpers.MqttPublish(PublishClient, StatusTopic, payload);

                LogIt($"Info: Connecting to Server: Url={ServerUrl} Port={ServerPort} StatusTopic={StatusTopic}");

            }
            catch (Exception ex)
            {
                Alert($"Cannot Initalize. Err={ex.Message}");
            }
        }

        /// <summary>
        /// Method called when the simulation run is terminating.
        /// </summary>
        public void Shutdown()
        {
            var task = Task.Run(() => PublishClient.DisconnectAsync());
            task.Wait();

            LogIt("Shutdown");

        }

        private void LogIt(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.TraceInformation($"MqttPublishConnector::{msg}");
        }
        private void Alert(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.ReportError($"MqttPublishConnector::{msg}");
        }
        #endregion
    }
}
