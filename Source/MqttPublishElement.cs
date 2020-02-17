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


    /// <summary>
    /// The element defines the url of the MQTT Server (broker) and the port.
    /// </summary>
    class MqttPublishElementDefinition : IElementDefinition
    {
        #region IElementDefinition Members

        /// <summary>
        /// Property returning the full name for this type of element. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "MqttPublish"; }
        }

        /// <summary>
        /// Property returning a short description of what the element does.
        /// </summary>
        public string Description
        {
            get { return "Defines a Publish connection to a MQTT Server (e.g. Mosquitto). Used by the MQTT Publish step"; }
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
        public static readonly Guid MY_ID = new Guid("{487add29-b4d1-4453-8dd3-1ec2376d283d}");

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
            return new MqttPublishElement(data);
        }

        #endregion
    }


    /// <summary>
    /// Defines what happens when the runtime starts
    /// </summary>
    class MqttPublishElement : IElement
    {
        IElementData _data;

        //MqttConnectSingleton MqttConnector { get; set; }


        public IMqttClient PublishClient { get; set; }

        string ServerUrl { get; set; }
        int ServerPort { get; set; }

        /// <summary>
        /// Constructor. The argument "data" includes run-time info, including ExecutionContext,
        /// I.e. data has members of Events, that can be indexed with the name
        /// and also Properties and ExecutionContext.
        /// </summary>
        /// <param name="data"></param>
        public MqttPublishElement(IElementData data)
        {
            _data = data;
        }

        #region IElement Members


        /// <summary>
        /// Method called when the simulation run starts.
        /// Called after the constructor.
        /// </summary>
        public void Initialize()
        {
            var mqttEvent = _data.Events["MqttEvent"];

            IPropertyReader prUrl = _data.Properties.GetProperty("ServerUrl");
            ServerUrl = prUrl.GetStringValue(_data.ExecutionContext);

            IPropertyReader prPort = _data.Properties.GetProperty("ServerPort");
            ServerPort = int.Parse( prPort.GetStringValue(_data.ExecutionContext));

            PublishClient = new MqttFactory().CreateMqttClient();
            var t = Task.Run( () => MqttHelpers.ConnectClient(PublishClient, ServerUrl, ServerPort));
            t.Wait();

            var info = _data.ExecutionContext.ExecutionInformation;
            string payload = $"Project:{info.ProjectFolder} {info.ProjectName} Model={info.ModelName} Scenario={info.ScenarioName} Replication={info.ReplicationNumber}";

            MqttHelpers.MqttPublish(PublishClient, "simio/start", payload);

            _data.ExecutionContext.ExecutionInformation.TraceInformation($"Connecting to Server: Url={ServerUrl} Port={ServerPort}");


        }

        /// <summary>
        /// Method called when the simulation run is terminating.
        /// </summary>
        public void Shutdown()
        {
            var task = Task.Run(() => PublishClient.DisconnectAsync());
            task.Wait();

            _data.ExecutionContext.ExecutionInformation.TraceInformation("Shutdown.");

        }

        #endregion
    }
}
