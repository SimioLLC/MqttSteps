using SimioAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttSteps
{
    public static class LogHelpers
    {
        /// <summary>
        /// General logs to be written to the Simio ecosystem.
        /// These currently (Feb2021) end of in SimioActions.Log file.
        /// </summary>
        /// <param name="message"></param>
        public static void LogIt(string message)
        {
            System.Diagnostics.Trace.TraceError(message);
        }

        public static void LogIt(IExecutionContext context, string message)
        {
            context.ExecutionInformation.TraceInformation($"{message}");
            LogIt(message);
        }

        public static void Alert(string message)
        {
            LogIt(message);
        }

        public static void Alert(IExecutionContext context, string message)
        {
            context.ExecutionInformation.ReportError(message);
            Alert(message);
        }

    }
}
