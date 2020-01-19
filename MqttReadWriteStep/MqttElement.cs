using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    class MqttElementDefinition : IElementDefinition
    {
        #region IElementDefinition Members

        /// <summary>
        /// Property returning the full name for this type of element. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "MqttServer"; }
        }

        /// <summary>
        /// Property returning a short description of what the element does.
        /// </summary>
        public string Description
        {
            get { return "Defines the connection to a MQTT Server (e.g. Mosquitto). Used by the MQTT Publish and RPC steps"; }
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
        /// This is called from Simio when you select "User Defined" on the ??? tab.
        /// </summary>
        public void DefineSchema(IElementSchema schema)
        {
            IPropertyDefinition pd = schema.PropertyDefinitions.AddStringProperty("ServerUrl", string.Empty);
            pd.DisplayName = "MQTT Server URL";
            pd.Description = "The address of the MQTT Server (e.g. Mostquitto server)";
            pd.Required = true;

            pd = schema.PropertyDefinitions.AddStringProperty("ServerPort", "1883");
            pd.DisplayName = "MQTT Port";
            pd.Description = "Internet port number used by MQTT Server. Default is 1883";
            pd.Required = true;

            // Example of how to add an event definition to the element.
            IEventDefinition ed;
            ed = schema.EventDefinitions.AddEvent("MqttCommunicationsEvent");
            ed.Description = "An event owned by this element";
            

        }

        /// <summary>
        /// Method called to add a new instance of this element type to a model.
        /// Returns an instance of the class implementing the IElement interface.
        /// Called as the Simulation Run begins.
        /// </summary>
        public IElement CreateElement(IElementData data)
        {
            return new MqttElement(data);
        }

        #endregion
    }


    /// <summary>
    /// Defines what happens when the RUn starts
    /// </summary>
    class MqttElement : IElement
    {
        IElementData _data;

        /// <summary>
        /// Data includes run-time info, including context.
        /// </summary>
        /// <param name="data"></param>
        public MqttElement(IElementData data)
        {
            _data = data;
        }

        #region IElement Members

        /// <summary>
        /// Method called when the simulation run starts.
        /// </summary>
        public void Initialize()
        {
            string xx = "";
            var events = _data.Events;
            var mqttEvent = events["MqttCommunicationsEvent"];

            mqttEvent.Fire();
            //?? put a test connection here.

            // So, for example, you can initiate the event with: mqttEvent.Fire();
        }

        /// <summary>
        /// Method called when the simulation run is terminating.
        /// </summary>
        public void Shutdown()
        {
            _data.ExecutionContext.ExecutionInformation.TraceInformation("Shutdown.");
        }

        #endregion
    }
}
