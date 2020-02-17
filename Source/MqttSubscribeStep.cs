using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    class MqttSubscribeStepDefinition : IStepDefinition
    {
        #region IStepDefinition Members

        /// <summary>
        /// Property returning the full name for this type of step. The name should contain no spaces.
        /// </summary>
        public string Name => "MqttSubscribe";

        /// <summary>
        /// Property returning a short description of what the step does.
        /// </summary>
        public string Description => "Subscribe to a MQTT Topic";


        /// <summary>
        /// Property returning an icon to display for the step in the UI.
        /// </summary>
        public System.Drawing.Image Icon => null;

        /// <summary>
        /// Property returning a unique static GUID for the step.
        /// </summary>
        public Guid UniqueID => MY_ID;

        static readonly Guid MY_ID = new Guid("{70167773-5C6E-41E3-9939-B3788125E13B}");  //dth 13Feb2020

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

            pd = schema.AddStringProperty("EventToFire", "0.0");
            pd.DisplayName = "Event";
            pd.Description = "The Simio Event to Fire when the topic is received.";
            pd.Required = true;

            // Example of how to add an element property definition to the step.
            pd = schema.AddElementProperty("MqttServer", MqttPublishElementDefinition.MY_ID);
            pd.DisplayName = "MQTT Server";
            pd.Description = "The MQTT URL and port of the MQTT server(broker). For example, localhost:8033. Default port is 1883.";
            pd.Required = true;
        }

        /// <summary>
        /// Method called to create a new instance of this step type to place in a process.
        /// Returns an instance of the class implementing the IStep interface.
        /// </summary>
        public IStep CreateStep(IPropertyReaders properties)
        {
            return new MqttSubscribeStep(properties);
        }

        #endregion
    }

    /// <summary>
    /// The step instance.
    /// Retrive the run-time information, such as:
    /// MQTT Element that holds server information
    /// Properties including the MQTT Topic to subscribe to, and the Event to fire.
    /// </summary>
    class MqttSubscribeStep : IStep
    {
        IPropertyReaders _props;

        IElementProperty prServerElement;
        IPropertyReader prTopic;
        IPropertyReader prEvent;

        IMqttClient SubscribeClient = null;


        /// <summary>
        /// Constructor. Create property readers
        /// Called when this Step is inserted into a Process.
        /// </summary>
        /// <param name="properties"></param>
        public MqttSubscribeStep(IPropertyReaders properties)
        {
            _props = properties;

            prServerElement = (IElementProperty)_props.GetProperty("MqttServer");
            

            prTopic = (IPropertyReader)_props.GetProperty("MqttTopic");
            prEvent = (IPropertyReader)_props.GetProperty("MqttEvent");

            SubscribeClient = new MqttFactory().CreateMqttClient();

        }

        #region IStep Members

        /// <summary>
        /// Method called when a process token executes the step.
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {

            // Example of how to get the value of a step property.
            string simioEvent = prEvent.GetStringValue(context);
            string topic = prTopic.GetStringValue(context);

            // Example of how to get an element reference specified in an element property of the step.
            MqttPublishElement mqttElement = (MqttPublishElement)prServerElement.GetElement(context);

            // The referenced element has events can can be fired,
            // as well as 
            if ( SubscribeClient == null )
            {
                return ExitType.AlternateExit;
            }

            //await MqttHelpers.ConnectClient(SubscribeClient, url, port);

            //MqttHelpers.MqttSubscribeInitialize(SubscribeClient, HandleReceive);
            ////var result = Task.Run(() => ConnectClient(publishClient, url, port)).Result;

            //if (SubscribeClient.IsConnected)
            //{
            //    MqttHelpers.MqttSubscribeInitialize(SubScribeClient, Action<
            //    MqttHelpers.MqttSubscribe(client, topic, cancellationToken);
            //}
            //else
            //    context.ExecutionInformation.TraceInformation($"Execute. Step= .Client not connected. Topic={topic} Payload={payload}.");

            // Show the topic and payload

            return ExitType.FirstExit;
        }

        #endregion
    }
}
