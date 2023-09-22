using System;
using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = Core.Debug;

namespace Managers {

    public enum SoundType {

        ButtonClick,
        LevelComplete,
        LevelFailed,
        PowerUp1,
        PowerUp2,
        PowerUp3,
        PowerUp4,
        PowerUp5
    }
    
    public class AudioManager: MonoBehaviour {

        public AudioSource musicAudioSource;
        [Space, BoxGroup("Sources")]
        public List<AudioSource> soundSources;
        [Space, BoxGroup("Clips")]
        public List<Clip> audioClips;

        private static AudioManager Instance { get; set; }
        
        private static AudioSource AvailAbleAudioSource {

            get {
                if( Instance == null ) return null;
                return Instance.soundSources.Find( x => !x.isPlaying );
            }
        }

        private static AudioSource MusicAudioSource => Instance.musicAudioSource;

        private void Awake() {
            
            SetInitialSettings();
        }

        private void SetInitialSettings() {
            if( Instance != null ) Destroy( Instance.gameObject );
            Instance = this;
        }

        public static void EnableSound( bool enable ) {
            if( Instance == null ) return;

            try {
                foreach( AudioSource t in Instance.soundSources ) {
                    t.enabled = enable;
                }
            } catch( Exception exception ) {
                Debug.Log( $"can't enable/disable sound \n exception:{exception}", LogSeverity.Medium );
            }
        }

        public static void EnableMusic( bool enable ) {

            if( Instance == null ) return;

            try {
                MusicAudioSource.enabled = enable;
            } catch( Exception exception ) {
                Debug.Log( $"can't enable/disable music \n exception:{exception}", LogSeverity.Medium );
            }
        }

        public static void PlaySound( SoundType type = SoundType.ButtonClick ) {

            if( Instance == null ) return;
            
            try {

                var clip = Instance.audioClips.Find( x => x.type == type ).clip;

                if( clip == null ) return;
                ChangeSoundClip( clip );
                
            } catch( Exception exception ) {

                Debug.Log( $"can't play music of type: {type} \n exception:{exception}", LogSeverity.Medium );
            }
        }

        private static void ChangeSoundClip( AudioClip clip ) {

            var source = AvailAbleAudioSource;
            if( source == null ) {

                var newObject = new GameObject( "spawnedSoundSource" );
                newObject.transform.parent = Instance.transform;
                var audioSource = newObject.AddComponent<AudioSource>();
                
                Instance.soundSources.Add(audioSource);
                source = audioSource;
            }
            
            if( source == null || !source.enabled ) return;

            source.clip = clip;
            source.Play();
        }

    }
    
    [Serializable]
    public struct Clip {

        public SoundType type;
        public AudioClip clip;

    }

}
