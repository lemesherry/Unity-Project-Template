using System;
using UnityEngine;
using Debug = Core.Debug;
using Core;

namespace Managers {

    public class GameManager: MonoBehaviour {

        public static GameManager Instance { get; private set; }

        [Space( 10 )]
        [SerializeField] private Testing testing;

        public static bool isGamePlaying;
        public static Camera mainCamera;

        public static Testing Testing => Instance == null? new Testing() : Instance.testing;

        private void Awake() => SetInitialSettings();

        private void OnEnable() => Debug.ListenLogEvents();
        private void OnDisable() {
            
            Debug.UnListenLogEvents();
            ListenEvents( false );
        }
        private void OnDestroy() {
            
            Debug.UnListenLogEvents();
            ListenEvents( false );
        }

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance.gameObject );
            Instance = this;

            ListenEvents( true );
            SetDefaultSettings();
            InitializeVariables();
        }

        private void OnApplicationQuit() {

            DataManager.SaveGameData();
            DataManager.SaveSettings();
        }

        private void OnApplicationPause( bool pauseStatus ) {

            if( pauseStatus ) {

                DataManager.SaveGameData();
                DataManager.SaveSettings();
            }
        }

        internal static void SetDefaultSettings() {

            isGamePlaying = true;
        }

        private static void InitializeVariables() {
            
            mainCamera = Camera.main;
            DataManager.InitializeData();
        }
        
        private void ListenEvents( bool doListen ) {

            if( doListen ) {

                LevelManager.OnLevelInitiate += LevelManagerOnOnLevelInitiate;
                LevelManager.OnLevelFail += LevelManagerOnOnLevelFail;
            } else {

                LevelManager.OnLevelInitiate -= LevelManagerOnOnLevelInitiate;
                LevelManager.OnLevelFail -= LevelManagerOnOnLevelFail;
            }
        }
        private void LevelManagerOnOnLevelFail() {

            isGamePlaying = false;
        }
        
        private void LevelManagerOnOnLevelInitiate() {

            SetDefaultSettings();
            Debug.Log( "On level initiated" );
        }

    }

    [Serializable]
    public struct Testing {

        public bool isTestingLevels;
        public bool enableBannerAds;
        public bool enableRewardedAds;
        public bool enableInterstitialAds;

    }

}
