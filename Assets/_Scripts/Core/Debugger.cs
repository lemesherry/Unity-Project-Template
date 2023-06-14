using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core {

    public static class Debugger {

        private const string LogsFileName = "gameLogs.sherry";
        private static readonly string LogsFilesPath = Path.Combine( Application.persistentDataPath, LogsFileName );

        private static readonly List<LogsData> LogsList = new();

        /// <summary>Logs any event of any severity.</summary>
        /// <param name="value">event string that you want to log.</param>
        /// <param name="severity">severity of log.</param>
        public static void Log( string value, LogSeverity severity = LogSeverity.Low ) {

            switch( severity ) {

                case LogSeverity.Low:
                    Debug.Log( $"<color=#B5B2B2>{value}</color>" );

                    break;
                case LogSeverity.Medium:
                    Debug.Log( $"<color=#666DFF>{value}</color>" );

                    break;
                case LogSeverity.High:
                    Debug.Log( $"<color=#FF66E2>{value}</color>" );

                    break;
                case LogSeverity.Critical:
                    Debug.Log( $"<color=#FF0000>{value}</color>" );

                    break;
                default:
                    Debug.Log( value );

                    break;
            }

            LogMessage( value );
        }

        /// <summary>Creates new log entry and saves the logs file to the path.</summary>
        private static void LogMessage( string message ) {

            var logData = new LogsData( message, DateTime.UtcNow.ToString( CultureInfo.CurrentCulture ) );

            LogsList.Add( logData );

            SaveLogsToJson();
        }

        /// <summary>Saves the logs file to the path.</summary>
        private static void SaveLogsToJson() {
            
            var existingLogs = new List<LogsData>();

            if( File.Exists( LogsFilesPath ) ) {
                
                var existingJson = File.ReadAllText( LogsFilesPath );
                existingLogs = JsonConvert.DeserializeObject<List<LogsData>>( existingJson );
            }

            existingLogs.AddRange( LogsList );

            var json = JsonConvert.SerializeObject( existingLogs, Formatting.Indented );

            File.WriteAllText( LogsFilesPath, json );
        }
    }

    public enum LogSeverity {

        Low,
        Medium,
        High,
        Critical

    }

    public struct LogsData {

        public string logMessage;
        public string logTimestamp;

        public LogsData( string message, string timestamp ) {
            logMessage = message;
            logTimestamp = timestamp;
        }

    }

}
