using UnityEngine;

namespace Core {

    public static class Debugger {

        /// <summary>This function is used to log any event of any severity.</summary>
        /// <param name="value">event string that you want to log.</param>
        /// <param name="logLevel">level/severity of log.</param>
        public static void Log( string value, LogLevel logLevel = LogLevel.Low ) {

        #if UNITY_EDITOR
            switch( logLevel ) {

                case LogLevel.Low:
                    Debug.Log( $"<color=#B5B2B2>{value}</color>" );
                    break;
                case LogLevel.Medium:
                    Debug.Log( $"<color=#666DFF>{value}</color>" );
                    break;
                case LogLevel.High:
                    Debug.Log( $"<color=#FF66E2>{value}</color>" );
                    break;
                case LogLevel.Critical:
                    Debug.Log( $"<color=#FF0000>{value}</color>" );
                    break;
                default:
                    Debug.Log( value );
                    break;
            }
        #endif
            
        }
    }

    public enum LogLevel {

        Low,
        Medium,
        High,
        Critical
    }

}
