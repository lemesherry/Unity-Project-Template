
using UnityEngine;

namespace Core {

    public static class AudioManager {
        
        private static GameManager GameManager => GameManager.Instance;
        public static AudioSource BackgroundAudioSource => GameManager.audioSettings.backgroundAudioSource;
        public static AudioSource GameplayAudioSource => GameManager.audioSettings.gameplayAudioSource;
        public static AudioClip LevelCompletionClip => GameManager.audioSettings.levelCompletionClip; 
        
        public static void EnableGamePlayAudio( bool enable ) => GameplayAudioSource.enabled = enable;
        public static void EnableBackgroundAudio( bool enable ) => BackgroundAudioSource.enabled = enable;
        public static void ChangeSoundClip( AudioClip clip ) {

            GameplayAudioSource.clip = clip;
            GameplayAudioSource.Play();
        }
    }

}
