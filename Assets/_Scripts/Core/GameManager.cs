using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core {

    public class GameManager: MonoBehaviour {

        public static GameManager Instance { get; private set; }
        
        public GameObject levelSpawner;
        public GameObject tutorialGameObject;

        [Space( 10 )]
        public GameObject[] levelPrefabs;
        public Testing testing;
        public LineSettings lineSettings;
        public AudioSettings audioSettings;
        
        private float _frameTime;

        public static bool isGamePlaying;
        public static bool isTutorialLevel;
        public static GameObject currentLevel;
        public static event Action OnLevelComplete;
        
        private void Awake() => SetInitialSettings();

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance );
            Instance = this;

            // AdsManager.interGap = 60;
            ResetSettings();
        }

        public static void InitializeLevelAndSetSettings( Action onLevelInitialized ) {

            if( Instance.testing.isTestingLevels ) {
                
                isGamePlaying = true;
                return;
            }
            
            LevelGenerator.LoadNewLevel( loadingCompleted => {

                if( loadingCompleted ) onLevelInitialized();
            });
        }
        
        public static void ResetSettings() {
            
            isGamePlaying = false;
            isTutorialLevel = false;
            OnLevelComplete = null;
            DataManager.InitializeData();
        }
        
        public static bool IsPointerOverUIElement() { return IsPointerOverUIElement( GetEventSystemRaycastResults() ); }

        private static bool IsPointerOverUIElement( IEnumerable<RaycastResult> eventSystemRaycastResults ) { return eventSystemRaycastResults.Any( curRaycastResult => curRaycastResult.gameObject.layer == 5 ); }

        private static IEnumerable<RaycastResult> GetEventSystemRaycastResults() {

            var eventData = new PointerEventData( EventSystem.current ) {
                position = Input.mousePosition
            };
            var _raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll( eventData, _raycastResults );

            return _raycastResults;
        }
        
        public static void InvokeOnLevelComplete() => OnLevelComplete?.Invoke();
        
        public static void EnableTutorialAnimator( bool enable ) => Instance.tutorialGameObject.SetActive( enable );
    }

    [ Serializable ]
    public struct Testing {

        public bool isTestingLevels;
        public bool sequentialLoop;
    }
    
    [ Serializable ]
    public struct AudioSettings {

        public AudioSource backgroundAudioSource;
        public AudioSource gameplayAudioSource;
        public AudioClip levelCompletionClip;
    }

    [Serializable]
    public struct LineSettings {

        public Material normalMaterial;
        public Material onCollisionMaterial;

    }

}
