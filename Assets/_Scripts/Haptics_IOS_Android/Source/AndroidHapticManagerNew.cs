//AndroidHapticManagerNew

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class AndroidHapticManagerNew {

    // Component Parameters
    public static LogLevel logLevel = LogLevel.Disabled;

    // Vibrator References
    private static AndroidJavaObject _vibrator;
    private static AndroidJavaClass _vibrationEffectClass;
    private static int _defaultAmplitude = 255;

    // Api Level
    private static int _apiLevel = 1;
    private static bool DoesSupportVibrationEffect() => _apiLevel >= 26; // available only from Api >= 26
    private static bool DoesSupportPredefinedEffect() => _apiLevel >= 29; // available only from Api >= 29

#region Initialization

    private static bool _isInitialized;

    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
    [SuppressMessage( "Code quality", "IDE0051", Justification = "Called on scene load" )]
    private static void Initialize() {
        // Add APP VIBRATION PERMISSION to the Manifest
    #if UNITY_ANDROID
        if( Application.isConsolePlatform ) {
            Handheld.Vibrate();
        }
    #endif

        // load references safely
        if( _isInitialized || Application.platform != RuntimePlatform.Android ) return;

        // Get Api Level
        using( var androidVersionClass = new AndroidJavaClass( "android.os.Build$VERSION" ) ) {
            _apiLevel = androidVersionClass.GetStatic<int>( "SDK_INT" );
        }

        // Get UnityPlayer and CurrentActivity
        using( var unityPlayer = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" ) )
        using( var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>( "currentActivity" ) ) {
            if( currentActivity != null ) {
                _vibrator = currentActivity.Call<AndroidJavaObject>( "getSystemService", "vibrator" );

                // if device supports vibration effects, get corresponding class
                if( DoesSupportVibrationEffect() ) {
                    _vibrationEffectClass = new AndroidJavaClass( "android.os.VibrationEffect" );
                    _defaultAmplitude = Mathf.Clamp( _vibrationEffectClass.GetStatic<int>( "DEFAULT_AMPLITUDE" ), 1,
                        255 );
                }

                // if device supports predefined effects, get their IDs
                if( DoesSupportPredefinedEffect() ) {
                    PredefinedEffect.effectClick = _vibrationEffectClass.GetStatic<int>( "EFFECT_CLICK" );
                    PredefinedEffect.effectDoubleClick = _vibrationEffectClass.GetStatic<int>( "EFFECT_DOUBLE_CLICK" );
                    PredefinedEffect.effectHeavyClick = _vibrationEffectClass.GetStatic<int>( "EFFECT_HEAVY_CLICK" );
                    PredefinedEffect.effectTick = _vibrationEffectClass.GetStatic<int>( "EFFECT_TICK" );
                }
            }
        }

        LogAuto( "Vibration component initialized", LogLevel.Info );
        _isInitialized = true;
    }

#endregion

#region Vibrate Public

    /// <summary>
    /// Vibrate for Milliseconds, with Amplitude (if available).
    /// If amplitude is -1, amplitude is Disabled. If -1, device DefaultAmplitude is used. Otherwise, values between 1-255 are allowed.
    /// If 'cancel' is true, Cancel() will be called automatically.
    /// </summary>
    public static void Vibrate( long milliseconds, int amplitude = -1, bool cancel = false ) {
        try {
            // string FuncToStr() => $"Vibrate ({milliseconds}, {amplitude}, {cancel})";

            Initialize(); // make sure script is initialized

            if( !_isInitialized ) {
                //logAuto(funcToStr() + ": Not initialized", logLevel.Warning);
            } else if( !HasVibrator() ) {
                //logAuto(funcToStr() + ": Device doesn't have Vibrator", logLevel.Warning);
            } else {
                if( cancel ) Cancel();

                if( DoesSupportVibrationEffect() ) {
                    // validate amplitude
                    amplitude = Mathf.Clamp( amplitude, -1, 255 );
                    if( amplitude == -1 ) amplitude = 255; // if -1, disable amplitude (use maximum amplitude)

                    if( amplitude != 255 && HasAmplitudeControl() == false ) {
                        // if amplitude was set, but not supported, notify developer
                        //logAuto(funcToStr() + ": Device doesn't have Amplitude Control, but Amplitude was set", logLevel.Warning);
                    }

                    if( amplitude == 0 ) amplitude = _defaultAmplitude; // if 0, use device DefaultAmplitude

                    // if amplitude is not supported, use 255; if amplitude is -1, use systems DefaultAmplitude. Otherwise use user-defined value.
                    amplitude = HasAmplitudeControl() == false? 255 : amplitude;
                    VibrateEffect( milliseconds, amplitude );

                    //logAuto(funcToStr() + ": Effect called", logLevel.Info);
                } else {
                    VibrateLegacy( milliseconds );

                    //logAuto(funcToStr() + ": Legacy called", logLevel.Info);
                }
            }
        } catch( Exception ex ) {
            Debug.Log( ex );
        }
    }

    /// <summary>
    /// Vibrate Pattern (pattern of durations, with format Off-On-Off-On and so on).
    /// Amplitudes can be Null (for default) or array of Pattern array length with values between 1-255.
    /// To cause the pattern to repeat, pass the index into the pattern array at which to start the repeat, or -1 to disable repeating.
    /// If 'cancel' is true, Cancel() will be called automatically.
    /// </summary>
    public static void Vibrate( long[] pattern, int[] amplitudes = null, int repeat = -1, bool cancel = false ) {
        string FuncToStr() => $"Vibrate (({ArrToStr( pattern )}), ({ArrToStr( amplitudes )}), {repeat}, {cancel})";

        Initialize(); // make sure script is initialized

        if( _isInitialized == false ) { LogAuto( FuncToStr() + ": Not initialized", LogLevel.Warning ); } else if
            ( HasVibrator() == false ) {
            LogAuto( FuncToStr() + ": Device doesn't have Vibrator", LogLevel.Warning );
        } else {
            // check Amplitudes array length
            if( amplitudes != null && amplitudes.Length != pattern.Length ) {
                LogAuto(
                    FuncToStr() +
                    ": Length of Amplitudes array is not equal to Pattern array. Amplitudes will be ignored.",
                    LogLevel.Warning );
                amplitudes = null;
            }

            // limit amplitudes between 1 and 255
            if( amplitudes != null ) { ClampAmplitudesArray( amplitudes ); }

            // vibrate
            if( cancel ) Cancel();

            if( DoesSupportVibrationEffect() ) {
                if( amplitudes != null && HasAmplitudeControl() == false ) {
                    LogAuto( FuncToStr() + ": Device doesn't have Amplitude Control, but Amplitudes was set",
                        LogLevel.Warning );
                    amplitudes = null;
                }

                if( amplitudes != null ) {
                    VibrateEffect( pattern, amplitudes, repeat );
                    LogAuto( FuncToStr() + ": Effect with amplitudes called", LogLevel.Info );
                } else {
                    VibrateEffect( pattern, repeat );
                    LogAuto( FuncToStr() + ": Effect called", LogLevel.Info );
                }
            } else {
                VibrateLegacy( pattern, repeat );

                //logAuto(funcToStr() + ": Legacy called", logLevel.Info);
            }
        }
    }

    /// <summary>
    /// Vibrate predefined effect (described in Vibration.PredefinedEffect). Available from Api Level >= 29.
    /// If 'cancel' is true, Cancel() will be called automatically.
    /// </summary>
    public static void VibratePredefined( int effectId, bool cancel = false ) {
        // string FuncToStr() => $"VibratePredefined ({effectId})";

        Initialize(); // make sure script is initialized

        if( !_isInitialized ) {
            //logAuto(funcToStr() + ": Not initialized", logLevel.Warning);
        } else if( !HasVibrator() ) {
            //logAuto(funcToStr() + ": Device doesn't have Vibrator", logLevel.Warning);
        } else if( DoesSupportPredefinedEffect() == false ) {
            //logAuto(funcToStr() + ": Device doesn't support Predefined Effects (Api Level >= 29)", logLevel.Warning);
        } else {
            if( cancel ) Cancel();
            VibrateEffectPredefined( effectId );

            //logAuto(funcToStr() + ": Predefined effect called", logLevel.Info);
        }
    }

#endregion

#region Public Properties & Controls

    public static long[] ParsePattern( string pattern ) {
        if( pattern == null ) return Array.Empty<long>();

        pattern = pattern.Trim();
        var split = pattern.Split( ',' );
        var timings = new long[split.Length];

        for( var i = 0; i < split.Length; i++ ) {
            if( int.TryParse( split[i].Trim(), out var duration ) ) { timings[i] = duration < 0? 0 : duration; } else {
                timings[i] = 0;
            }
        }

        return timings;
    }

    /// <summary>
    /// Returns Android Api Level
    /// </summary>
    public static int GetApiLevel() => _apiLevel;

    /// <summary>
    /// Returns Default Amplitude of device, or 0.
    /// </summary>
    public static int GetDefaultAmplitude() => _defaultAmplitude;

    /// <summary>
    /// Returns true if device has vibrator
    /// </summary>
    public static bool HasVibrator() { return _vibrator != null && _vibrator.Call<bool>( "hasVibrator" ); }

    /// <summary>
    /// Return true if device supports amplitude control
    /// </summary>
    public static bool HasAmplitudeControl() {
        if( HasVibrator() && DoesSupportVibrationEffect() ) {
            return _vibrator.Call<bool>( "hasAmplitudeControl" ); // API 26+ specific
        } else {
            return false; // no amplitude control below API level 26
        }
    }

    /// <summary>
    /// Tries to cancel current vibration
    /// </summary>
    public static void Cancel() {
        if( HasVibrator() ) {
            _vibrator.Call( "cancel" );

            //logAuto("Cancel (): Called", logLevel.Info);
        }
    }

#endregion

#region Vibrate Internal

#region Vibration Callers

    private static void VibrateEffect( long milliseconds, int amplitude ) {
        using var effect = createEffect_OneShot( milliseconds, amplitude );
        _vibrator.Call( "vibrate", effect );
    }

    private static void VibrateLegacy( long milliseconds ) { _vibrator.Call( "vibrate", milliseconds ); }

    private static void VibrateEffect( long[] pattern, int repeat ) {
        using var effect = createEffect_Waveform( pattern, repeat );
        _vibrator.Call( "vibrate", effect );
    }

    private static void VibrateLegacy( IEnumerable pattern, int repeat ) =>
        _vibrator.Call( "vibrate", pattern, repeat );

    private static void VibrateEffect( long[] pattern, int[] amplitudes, int repeat ) {
        using var effect = createEffect_Waveform( pattern, amplitudes, repeat );
        _vibrator.Call( "vibrate", effect );
    }

    private static void VibrateEffectPredefined( int effectId ) {
        using var effect = createEffect_Predefined( effectId );
        _vibrator.Call( "vibrate", effect );
    }

#endregion

#region Vibration Effect

    /// <summary>
    /// Wrapper for public static VibrationEffect createOneShot (long milliseconds, int amplitude). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_OneShot( long milliseconds, int amplitude ) =>
        _vibrationEffectClass.CallStatic<AndroidJavaObject>( "createOneShot", milliseconds, amplitude );

    /// <summary>
    /// Wrapper for public static VibrationEffect createPredefined (int effectId). API >= 29
    /// </summary>
    private static AndroidJavaObject createEffect_Predefined( int effectId ) =>
        _vibrationEffectClass.CallStatic<AndroidJavaObject>( "createPredefined", effectId );

    /// <summary>
    /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int[] amplitudes, int repeat). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_Waveform( IEnumerable timings, IEnumerable amplitudes, int repeat ) =>
        _vibrationEffectClass.CallStatic<AndroidJavaObject>( "createWaveform", timings, amplitudes, repeat );

    /// <summary>
    /// Wrapper for public static VibrationEffect createWaveform (long[] timings, int repeat). API >= 26
    /// </summary>
    private static AndroidJavaObject createEffect_Waveform( IEnumerable timings, int repeat ) =>
        _vibrationEffectClass.CallStatic<AndroidJavaObject>( "createWaveform", timings, repeat );

#endregion

#endregion

#region Internal

    private static void LogAuto( string text, LogLevel level ) {
        if( level == LogLevel.Disabled ) level = LogLevel.Info;

        if( text == null ) return;

        switch( level ) {
            case LogLevel.Warning when logLevel == LogLevel.Warning:
                Debug.LogWarning( text );

                break;
            case LogLevel.Info when logLevel >= LogLevel.Info:
                Debug.Log( text );

                break;
        }
    }

    private static string ArrToStr( long[] array ) => array == null? "null" : string.Join( ", ", array );
    private static string ArrToStr( int[] array ) => array == null? "null" : string.Join( ", ", array );

    private static void ClampAmplitudesArray( int[] amplitudes ) {
        for( var i = 0; i < amplitudes.Length; i++ ) { amplitudes[i] = Mathf.Clamp( amplitudes[i], 1, 255 ); }
    }

#endregion

    public static class PredefinedEffect {

        public static int effectClick; // public static final int EFFECT_CLICK
        public static int effectDoubleClick; // public static final int EFFECT_DOUBLE_CLICK
        public static int effectHeavyClick; // public static final int EFFECT_HEAVY_CLICK
        public static int effectTick; // public static final int EFFECT_TICK

    }

    public enum LogLevel {

        Disabled,
        Info,
        Warning,

    }

}
