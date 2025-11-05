using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SimioAPI;
using SimioAPI.Extensions;

namespace MqttSteps
{
    class TestBuilder : IDesignAddIn
    {
        #region IDesignAddIn Members

        /// <summary>
        /// Property returning the name of the add-in. This name may contain any characters and is used as the display name for the add-in in the UI.
        /// </summary>
        public string Name
        {
            get { return "AlertStep"; }
        }

        /// <summary>
        /// Property returning a short description of what the add-in does.
        /// </summary>
        public string Description
        {
            get { return "Description text for the 'AlertStep' add-in."; }
        }

        /// <summary>
        /// Property returning an icon to display for the add-in in the UI.
        /// </summary>
        public System.Drawing.Image Icon
        {
            get { return null; }
        }

        /// <summary>
        /// Method called when the add-in is run.
        /// </summary>
        public void Execute(IDesignContext context)
        {
            // This example code places some new objects from the Standard Library into the active model of the project.
            if (context.ActiveModel != null)
            {
                // Example of how to place some new fixed objects into the active model.
                // This example code places three new fixed objects: a Source, a Server, and a Sink.
                IIntelligentObjects intelligentObjects = context.ActiveModel.Facility.IntelligentObjects;
                IFixedObject sourceObject = intelligentObjects.CreateObject("Source", new FacilityLocation(-10, 0, -10)) as IFixedObject;
                IFixedObject serverObject = intelligentObjects.CreateObject("Server", new FacilityLocation(0, 0, 0)) as IFixedObject;
                IFixedObject sinkObject = intelligentObjects.CreateObject("Sink", new FacilityLocation(10, 0, 10)) as IFixedObject;

                // Example of how to place some new link objects into the active model (to add network paths between nodes).
                // This example code places two new link objects: a Path connecting the Source 'output' node to the Server 'input' node,
                // and a Path connecting the Server 'output' node to the Sink 'input' node.
                INodeObject sourceOutputNode = sourceObject.Nodes[0];
                INodeObject serverInputNode = serverObject.Nodes[0];
                INodeObject serverOutputNode = serverObject.Nodes[1];
                INodeObject sinkInputNode = sinkObject.Nodes[0];
                ILinkObject pathObject1 = intelligentObjects.CreateLink("Path", sourceOutputNode, serverInputNode, null) as ILinkObject;
                ILinkObject pathObject2 = intelligentObjects.CreateLink("Path", serverOutputNode, sinkInputNode, null) as ILinkObject;

                // Example of how to edit the property of an object.
                // This example code edits the 'ProcessingTime' property of the added Server object.
                serverObject.Properties["ProcessingTime"].Value = "100";
            }
        }

        #endregion
    }
}
