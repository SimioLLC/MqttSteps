using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
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
    // The definition of a Subscribe connector

    /// <summary>
    /// The element defines the url of the MQTT Server (broker) and the port.
    /// </summary>
    internal class MqttSubscribeConnectorDefinition : IElementDefinition
    {
        #region IElementDefinition Members

        /// <summary>
        /// Property returning the full name for this type of element. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "MqttSubscribeConnector"; }
        }

        /// <summary>
        /// Property returning a short description of what the element does.
        /// </summary>
        public string Description
        {
            get { return "Defines a connection to a MQTT Server/Broker (e.g. Mosquitto). Used by the MQTT Subscribe elements"; }
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
        /// Changed 22Oct2020/dth
        /// </summary>
        public static readonly Guid MY_ID = new Guid("{EA80B3EB-5810-4458-9E55-60F906748403}");

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

            pd = schema.PropertyDefinitions.AddStringProperty("StatusTopic", "MqttSample1/Status");
            pd.DisplayName = "MQTT Status Topic";
            pd.Description = "This is the topic published to for Status, such as Start";
            pd.Required = true;


        }

        /// <summary>
        /// Method called to add a new instance of this element type to a model.
        /// Returns an instance of the class implementing the IElement interface.
        /// Called as the Simulation Run begins.
        /// </summary>
        public IElement CreateElement(IElementData data)
        {
            return new MqttSubscribeConnector(data);
        }

        #endregion
    }

    //===================================================================================
    // An instance of a public connector.
    // In the constructor an MQTT connection is tried.

    /// <summary>
    /// Defines what happens when the runtime starts
    /// </summary>
    class MqttSubscribeConnector : IElement
    {
        readonly IElementData _data;

        /// <summary>
        /// The referenced server/broker.
        /// </summary>
        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public IMqttClient SubscribeClient { get; set; }

        /// <summary>
        /// Constructor. The argument "data" includes run-time info, including ExecutionContext,
        /// I.e. data has members of Events, that can be indexed with the name
        /// and also Properties and ExecutionContext.
        /// </summary>
        /// <param name="data"></param>
        public MqttSubscribeConnector(IElementData data)
        {
            _data = data;

            try
            {
                IPropertyReader prUrl = _data.Properties.GetProperty("ServerUrl");
                ServerUrl = prUrl.GetStringValue(_data.ExecutionContext);

                IPropertyReader prPort = _data.Properties.GetProperty("ServerPort");
                ServerPort = int.Parse(prPort.GetStringValue(_data.ExecutionContext));

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
                using (SubscribeClient = new MqttFactory().CreateMqttClient())
                {
                    var t = Task.Run(() => MqttHelpers.ConnectClient(SubscribeClient, ServerUrl, ServerPort));
                    //Todo: This will throw an error if there are no MQTT Servers.
                    t.Wait(2000);

                    var info = _data.ExecutionContext.ExecutionInformation;
                    string payload = $"Project:{info.ProjectFolder} {info.ProjectName} Model={info.ModelName} Scenario={info.ScenarioName} Replication={info.ReplicationNumber}";
                }

                LogIt($"Info: Connecting to Server: Url={ServerUrl} Port={ServerPort}");
            }
            catch (Exception ex)
            {
                Alert($"MQTT Subscribe. Cannot Initalize. Err={ex.Message}");
            }
        }

        /// <summary>
        /// Method called when the simulation run is terminating.
        /// </summary>
        public void Shutdown()
        {
            try
            {

                var task = Task.Run(() => SubscribeClient.DisconnectAsync());
                task.Wait(2000);

                LogIt("Shutdown MQTT Subscribe Connector");

            }
            catch (Exception ex)
            {
                LogIt($"Shutdown MQTT Subscribe Error={ex}");
            }

        }

        private void LogIt(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.TraceInformation($"MqttSubscribeConnector::{msg}");
        }
        private void Alert(string msg)
        {
            _data.ExecutionContext.ExecutionInformation.ReportError($"MqttSubscribeConnector::{msg}");
        }
        #endregion

    }
}
