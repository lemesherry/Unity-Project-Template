using UnityEngine;

public class HapticsManager: MonoBehaviour {

    public static HapticsManager instance;

    private static bool IsVibrationOn => PlayerPrefs.GetInt( "Vibration", 1 ) == 1;

    private void Awake() {
        if( instance != null && instance != this ) {
            Destroy( gameObject );
        } else {
            instance = this;
            DontDestroyOnLoad( gameObject );
        }
    }

#region Haptics

    public static void DoHaptics_Short() {
        if( !IsVibrationOn ) return;

    #if UNITY_ANDROID
        AndroidHapticManagerNew.Vibrate( 35 );
    # elif UNITY_IOS || UNITY_IPHONE
        HapticFeedback.DoHaptic(HapticFeedback.HapticForce.Light);
    #else
        print("Haptics not supported on this device");
    #endif
    }

    public static void DoHaptics_Medium() {
        if( !IsVibrationOn ) return;

    #if UNITY_ANDROID
        AndroidHapticManagerNew.Vibrate( 50 );
    # elif UNITY_IOS || UNITY_IPHONE
        HapticFeedback.DoHaptic(HapticFeedback.HapticForce.Medium);
    #else
        print("Haptics not supported on this device");
    #endif
    }

    public static void DoHaptics_Heavy() {
        if( !IsVibrationOn ) return;

    #if UNITY_ANDROID
        AndroidHapticManagerNew.Vibrate( 60 );
    # elif UNITY_IOS || UNITY_IPHONE
        HapticFeedback.DoHaptic(HapticFeedback.HapticForce.Heavy);
    #else
        print("Haptics not supported on this device");
    #endif
    }

#endregion

}
