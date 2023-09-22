using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core {

    public static class Debug {

        private const string LogsFileName = "gameLogs.sherry";
        private static readonly string LogsFilesPath = Path.Combine( Application.persistentDataPath, LogsFileName );

        private static readonly List<LogsData> LogsList = new();

        public static void ListenLogEvents() => Application.logMessageReceived += OnLogMessageReceived;
        public static void UnListenLogEvents() => Application.logMessageReceived -= OnLogMessageReceived;

        private static Type _callingClass;
        private static string _debugColor;
        private static string _logMessage;

        /// <summary>Logs the messages sent in the console via unity.</summary>
        private static void OnLogMessageReceived( string message, string stacktrace, LogType type ) {

            var newMessage = $"Log type: \n{type}\n Log: \n{message}\n Stack: \n{stacktrace}";
            LogMessageToFile( newMessage );
        }

        /// <summary>Logs any event of any severity.</summary>
        /// <param name="message">event string that you want to log.</param>
        /// <param name="severity">severity of log.</param>
        /// <param name="doLogInFile">save logs into file?</param>
        public static void Log( string message, LogSeverity severity = LogSeverity.Low, bool doLogInFile = false ) {

        // #if UNITY_EDITOR
        //
        //     try {
        //
        //         var stackTrace = new StackTrace();
        //
        //         StackFrame frame = stackTrace.GetFrame( 1 );
        //
        //         _callingClass = frame.GetMethod().DeclaringType;
        //
        //     } catch( Exception exception ) {
        //
        //         UnityEngine.Debug.Log( $"<color=#B5B2B2>{exception} Couldn't stackTrace</color>" );
        //     }
        // _logMessage = _callingClass != null? $"{_callingClass}: <color=#{_debugColor}>{message}</color>" : $"<color=#{_debugColor}>{message}</color>";
        // #endif

            _debugColor = severity switch {

                LogSeverity.Low => "B5B2B2",
                LogSeverity.Medium => "666DFF",
                LogSeverity.High => "FF66E2",
                LogSeverity.Critical => "FF0000",
                LogSeverity.Ads => "66FFFA",
                _ => ""
            };

            _logMessage = $"<color=#{_debugColor}>{message}</color>";

            UnityEngine.Debug.Log( _logMessage );

            if( !doLogInFile ) return;

            LogMessageToFile( message );
        }

        /// <summary>Creates new log entry and saves the logs file to the path.</summary>
        private static void LogMessageToFile( string message ) {

            if( DataManager.gameData.LogsSessionsCount >= 10 ) {
                DataManager.gameData.LogsSessionsCount = 0;
                Log( "Sessions exceeded length of 10, Deleting previous logs", LogSeverity.High, false );
                File.Delete( LogsFilesPath );
            }

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
        Critical,
        Ads

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
