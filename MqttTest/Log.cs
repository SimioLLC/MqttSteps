using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttTest
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } 

        public string Code { get; set; }

        public LogEntry(string code, string msg)
        {
            Code = code; 
            Timestamp = DateTime.Now;
            Message = msg;
        }
        public LogEntry(string msg)
        {
            Code = "I"; // Informational
            Timestamp = DateTime.Now;
            Message = msg;
        }
    }

    internal static class Log
    {
        public static Dictionary<DateTime, LogEntry> LogEntries { get; set; } 
            = new Dictionary<DateTime, LogEntry>();

        /// <summary>
        /// Set true when LogEntries changes
        /// </summary>
        public static bool IsDirty {get;set;}


        internal static void AddEntry(LogEntry logEntry)
        {
            if (!LogEntries.ContainsKey(logEntry.Timestamp))
            {
                LogEntries.Add(logEntry.Timestamp, logEntry);
                IsDirty = true;
            }
        }

        public static void LogIt(string code, string message)
        {
            LogEntry logEntry = new LogEntry(code, message);
            AddEntry(logEntry);
        }

        public static void LogIt(string message)
        {
            LogEntry logEntry = new LogEntry(message);
            AddEntry(logEntry);
        }

        /// <summary>
        /// Construct a multi-line string ordered by time, with newest first.
        /// </summary>
        /// <returns></returns>
        public static string BuildLog()
        {
            StringBuilder sb = new StringBuilder();
            foreach ( KeyValuePair<DateTime,LogEntry> kvp in LogEntries.OrderByDescending(rr => rr.Key) )
            {
                LogEntry le = kvp.Value;
                if ( le != null)
                    sb.AppendLine($"{le.Timestamp:HH:mm:ss.ff}:{le.Code}: {le.Message}");
            }
            return sb.ToString();
        }
    }
}
