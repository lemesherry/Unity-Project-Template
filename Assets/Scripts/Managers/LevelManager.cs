using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UI.Panels;
using UnityEngine.Events;
using Debug = Core.Debug;

namespace Managers {

    public class LevelManager: MonoBehaviour {

        public static LevelManager Instance { get; private set; }

        public static event Action OnLevelFail;
        public static event Action OnLevelInitiate;
        public static event Action OnLevelRestart;
        public static event Action OnLevelComplete;

        [SerializeField] private UnityEvent onLevelFail;
        [SerializeField] private UnityEvent onLevelInitiate;
        [SerializeField] private UnityEvent onLevelRestart;
        [SerializeField] private UnityEvent onLevelComplete;
        
        private void Awake() {
            
            SetInitialSettings();
            ListenEvents( true );
        }
        private void Start() {
            
        }
        private void OnDisable() => ListenEvents( false );
        private void OnDestroy() => ListenEvents( false );

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance.gameObject );
            Instance = this;

        }

        public static void InvokeOnLevelFailed() {
            
            if( Instance != null ) Instance.onLevelFail?.Invoke();
            OnLevelFail?.Invoke();
        }
        public static void InvokeOnLevelInitiate() {
            
            if( Instance != null ) Instance.onLevelInitiate?.Invoke();
            OnLevelInitiate?.Invoke();
        }
        public static void InvokeOnLevelRestart() {
            
            if( Instance != null ) Instance.onLevelRestart?.Invoke();
            OnLevelRestart?.Invoke();
        }
        public static void InvokeOnLevelCompleted() {
            
            if( Instance != null ) Instance.onLevelComplete?.Invoke();
            OnLevelComplete?.Invoke();
        }
        
        private void ListenEvents( bool doListen ) {

            if( doListen ) {

                OnLevelFail += OnOnLevelFail;
                OnLevelInitiate += OnOnLevelInitiate;
                OnLevelRestart += OnOnLevelRestart;
                OnLevelComplete += OnOnLevelComplete;
            } else {

                OnLevelFail -= OnOnLevelFail;
                OnLevelInitiate -= OnOnLevelInitiate;
                OnLevelRestart -= OnOnLevelRestart;
                OnLevelComplete -= OnOnLevelComplete;
            }
        }
        
        private static void OnOnLevelComplete() {
            
            Debug.Log( "Event Raised: Level Complete" );
            
            if( Instance == null ) return;
        }

        private static void OnOnLevelInitiate() {
            
            Debug.Log( "Event Raised: Level Initiate" );
            Panel.GetMainPanelOfType<GameplayPanel>().Enable();
            
            if( Instance == null ) return;
        }
        
        private static void OnOnLevelFail() {

            Debug.Log( "Event Raised: Level Fail" );
            
            if( Instance == null ) return;

            DataManager.gameData.IsTutorialPlayed = true;
            
            HapticPatterns.PlayPreset( HapticPatterns.PresetType.Failure );
            AudioManager.PlaySound( SoundType.LevelFailed );
            
            // UIManager.ShowLosePanel( true );
        }

        private static void OnOnLevelRestart() {

            Debug.Log( "Event Raised: Level Fail" );

            if( Instance == null ) return;

            DataManager.SaveGameData();
            DataManager.SaveSettings();

            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        }






    }

}