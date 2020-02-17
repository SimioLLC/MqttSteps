using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MQTTnet.Client;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    class MqttPublishExpressionStepDefinition : IStepDefinition
    {
        #region IStepDefinition Members

        /// <summary>
        /// Property returning the full name for this type of step. The name should contain no spaces.
        /// </summary>
        public string Name => "MqttPublish";

        /// <summary>
        /// Property returning a short description of what the step does.
        /// </summary>
        public string Description => "Publishes the payload to the given topic"; 
        

        /// <summary>
        /// Property returning an icon to display for the step in the UI.
        /// </summary>
        public System.Drawing.Image Icon => null;

        /// <summary>
        /// Property returning a unique static GUID for the step.
        /// </summary>
        public Guid UniqueID => MY_ID;

        static readonly Guid MY_ID = new Guid("{a263d985-0a3d-45b2-a844-c40adb3d3c5c}");

        /// <summary>
        /// Property returning the number of exits out of the step. Can return either 1 or 2.
        /// </summary>
        public int NumberOfExits => 2;

        /// <summary>
        /// Method called that defines the property schema for the step.
        /// </summary>
        public void DefineSchema(IPropertyDefinitions schema)
        {
            // Example of how to add a property definition to the step.
            IPropertyDefinition pd;

            pd = schema.AddStringProperty("MqttTopic", "Test/Topic");
            pd.DisplayName = "The MQTT Topic";
            pd.Description = "The MQTT topic. Often hierarchical e.g. Machine1/State/Speed";
            pd.Required = true;

            pd = schema.AddExpressionProperty("MqttPayload", "0.0");
            pd.DisplayName = "Payload";
            pd.Description = "The payload expression to send with the Topic";
            pd.Required = true;

            // Example of how to add an element property definition to the step.
            pd = schema.AddElementProperty("MqttServer", MqttPublishElementDefinition.MY_ID);
            pd.DisplayName = "MQTT Server";
            pd.Description = "The MQTT URL and port of the MQTT server(broker). For example, localhost:1883. Default port is 1883.";
            pd.Required = true;
        }

        /// <summary>
        /// Method called to create a new instance of this step type to place in a process.
        /// Returns an instance of the class implementing the IStep interface.
        /// </summary>
        public IStep CreateStep(IPropertyReaders properties)
        {
            return new MqttPublishExpressionStep(properties);
        }

        #endregion
    }

    /// <summary>
    /// The step instance
    /// </summary>
    class MqttPublishExpressionStep : IStep
    {
        IPropertyReaders _props;

        IElementProperty prServerElement;
        IPropertyReader prTopic;
        IPropertyReader prPayload;
        

        /// <summary>
        /// Constructor. Create property readers
        /// Called when this Step is inserted into a Process.
        /// </summary>
        /// <param name="properties"></param>
        public MqttPublishExpressionStep(IPropertyReaders properties)
        {
            _props = properties;

            prServerElement = (IElementProperty)_props.GetProperty("MqttServer");

            prTopic = (IPropertyReader) _props.GetProperty("MqttTopic");
            prPayload = (IPropertyReader)_props.GetProperty("MqttPayload");
            
        }

        #region IStep Members

        /// <summary>
        /// Method called when a process token executes the step.
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {
            
            // Example of how to get the value of a step property.
            var epReader = (IExpressionPropertyReader) prPayload;
            var payload = epReader.GetExpressionValue((IExecutionContext)context).ToString();
            string topic = prTopic.GetStringValue(context);

            // Example of how to get an element reference specified in an element property of the step.
            MqttPublishElement mqttElement = (MqttPublishElement) prServerElement.GetElement(context);

            // The referenced element has a MQTT client
            IMqttClient client = (IMqttClient) mqttElement.PublishClient;

            if (client.IsConnected)
            {
                MqttHelpers.MqttPublish(client, topic, payload); // payload);
            }
            else
                context.ExecutionInformation.TraceInformation($"Execute. Step= .Client not connected. Topic={topic} Payload={payload}.");

            return ExitType.FirstExit;
        }

        #endregion
    }
}
