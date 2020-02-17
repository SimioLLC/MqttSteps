using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    class AlertStepDefinition : IStepDefinition
    {
        #region IStepDefinition Members

        public AlertBox Alert { get; set; }

        /// <summary>
        /// Property returning the full name for this type of step. The name should contain no spaces.
        /// </summary>
        public string Name
        {
            get { return "Alert"; }
        }

        /// <summary>
        /// Property returning a short description of what the step does.
        /// </summary>
        public string Description
        {
            get { return "Simply popup a messagebox with a message."; }
        }

        /// <summary>
        /// Property returning an icon to display for the step in the UI.
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Property returning a unique static GUID for the step.
        /// </summary>
        public Guid UniqueID
        {
            get { return MY_ID; }
        }
        static readonly Guid MY_ID = new Guid("{a40a2add-ceab-4e0c-bea1-acb941a15281}");

        /// <summary>
        /// Property returning the number of exits out of the step. Can return either 1 or 2.
        /// </summary>
        public int NumberOfExits
        {
            get { return 2; }
        }

        /// <summary>
        /// Method called that defines the property schema for the step.
        /// </summary>
        public void DefineSchema(IPropertyDefinitions schema)
        {
            // Example of how to add a property definition to the step.
            IPropertyDefinition pd;

            pd = schema.AddStringProperty("Message", "No message");
            pd.DisplayName = "Message";
            pd.Description = "The message to display";
            pd.Required = true;
        }

        /// <summary>
        /// Method called to create a new instance of this step type to place in a process.
        /// Returns an instance of the class implementing the IStep interface.
        /// </summary>
        public IStep CreateStep(IPropertyReaders properties)
        {
            return new AlertStep(properties);
        }

        #endregion
    }

    class AlertStep : IStep
    {
        IPropertyReaders _propReaders;

        IPropertyReader prMessage;

        public AlertStep(IPropertyReaders properties)
        {
            _propReaders = properties;

            prMessage = _propReaders.GetProperty("Message");

        }

        #region IStep Members

        /// <summary>
        /// Method called when a process token executes the step.
        /// </summary>
        public ExitType Execute(IStepExecutionContext context)
        {
            // Example of how to get the value of a step property.
            string message = prMessage.GetStringValue(context);

            DialogResult result = AlertBox.ShowMessage(message);

            if (result == DialogResult.OK)
                return ExitType.FirstExit;
            else
                return ExitType.AlternateExit;

        }

        #endregion
    }
}
