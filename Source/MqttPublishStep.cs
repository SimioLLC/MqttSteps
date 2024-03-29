﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MQTTnet.Client;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    //=========================================================================
    // Step Definition

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
        public System.Drawing.Image Icon => Properties.Resources.Mqtt;

        /// <summary>
        /// Property returning a unique static GUID for the step.
        /// </summary>
        public Guid UniqueID => MY_ID;

        /// <summary>
        /// Changed 1Jul2020/dth
        /// </summary>
        static readonly Guid MY_ID = new Guid("{D82FFCE4-221C-4772-95AC-4CF3556B19BA}");

        /// <summary>
        /// Property returning the number of exits out of the step. Can return either 1 or 2.
        /// </summary>
        public int NumberOfExits => 2;

        /// <summary>
        /// Method called that defines the property schema for the step.
        /// </summary>
        public void DefineSchema(IPropertyDefinitions schema)
        {
            // property definition that will be added to the schema of this step
            IPropertyDefinition pd;

            // Add the connector element property definition to the step.
            pd = schema.AddElementProperty("MqttServer", MqttPublishConnectorDefinition.MY_ID);
            pd.DisplayName = "MQTT Server";
            pd.Description = "The MQTT URL and port of the MQTT server(broker). For example, localhost:1883. Default port is 1883.";
            pd.Required = true;

            pd = schema.AddStringProperty("MqttTopic", "MqttSample/MyTopic");
            pd.DisplayName = "The MQTT Topic";
            pd.Description = "The MQTT topic. Case sensitive. By convention, hierarchical using slashes. Example: PlantTopeka/Machine1/State/Speed";
            pd.Required = true;

            pd = schema.AddExpressionProperty("MqttPayload", "0.0");
            pd.DisplayName = "Payload";
            pd.Description = "The payload data to send with the Topic";
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

    //=========================================================================
    // Step Instance

    /// <summary>
    /// The step instance
    /// </summary>
    class MqttPublishExpressionStep : IStep
    {
        readonly IPropertyReaders _props;
        readonly IElementProperty prServerElement;
        readonly IPropertyReader prTopic;
        readonly IPropertyReader prPayload;

        /// <summary>
        /// Constructor. Create property readers
        /// Called when this Step is inserted into a Process.
        /// Create all of the needed Property Readers.
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
        /// Does an MQTT publish to the MqttServer with MqttTopic. 
        /// The data that is published is MqttPayload
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {
            string topic = "";

            try
            {
                // Example of how to get the value of a step property.
                Logit(context, $"Getting value of Payload Property.");
                var eprPayload = (IExpressionPropertyReader) prPayload;
                var payload = eprPayload.GetExpressionValue(context).ToString();

                topic = prTopic.GetStringValue(context);

                // Get the MQTT connector which holds the MQTT client information
                MqttPublishConnector mqttConnector = (MqttPublishConnector)prServerElement.GetElement(context);

                // The referenced element has a MQTT client
                IMqttClient client = (IMqttClient) mqttConnector.PublishClient;

                if (client.IsConnected)
                {
                    MqttHelpers.MqttPublish(client, topic, payload); // payload);
                    Logit(context, $"Info: Topic={topic} Payload={payload}");
                    return ExitType.FirstExit;
                }
                else
                {
                    Logit(context, $"Execute. Step Topic={topic} .Client not connected. Topic={topic} Payload={payload}.");
                    return ExitType.AlternateExit;
                }

            }
            catch (Exception ex)
            {
                Logit(context, $"Execute Topic={topic} Error={ex.Message}");
                return ExitType.AlternateExit;
            }
        }

        private void Logit(IStepExecutionContext context, string msg)
        {
            context.ExecutionInformation.TraceInformation($"MqttPublishStep: {msg}");
        }
        #endregion
    }
}
