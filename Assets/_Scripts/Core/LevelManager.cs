using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core {

    public class LevelManager: MonoBehaviour {

        public static LevelManager Instance { get; private set; }

        public Transform linesSpawner;
        public GameObject hintObject;
        // [Space(10)]
        // public LevelSettings levelSettings;

        internal bool isLevelComplete;
        private static readonly int TUTORIAL_1 = Animator.StringToHash( "tutorial_1" );
        private static readonly int TUTORIAL_2 = Animator.StringToHash( "tutorial_2" );

        private void Awake() => SetInitialSettings();
        private void Start() => OnStart();
        private void OnDisable() => UnListenLevelCompleteEvent();

        private void OnDestroy() => UnListenLevelCompleteEvent();

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance );
            Instance = this;

            GameManager.isTutorialLevel = !DataManager.gameData.IsTutorialPlayed;

            // ★彡[ Replace this debug log with analytics event (progression start) ]彡★
            Debug.Log( GameManager.isTutorialLevel? $"Tutorial_{DataManager.gameData.TutorialLevel}" : $"Level_{DataManager.gameData.CurrentLevel}" );
        }

        private void OnStart() {

            ListenLevelCompleteEvent();

            if( GameManager.isTutorialLevel ) {
                
                EnableHintObjects();
            } else {
                
                EnableHintObjects( false );
            }

            var level = GameManager.isTutorialLevel? $"Tutorial {DataManager.gameData.TutorialLevel}" : $"Level {DataManager.gameData.CurrentLevel}";
            UIManager.Instance.texts.level.text = level;
            
            UIManager.Instance.ShowNextLevelButton( false );
            GameManager.isGamePlaying = true;
        }

        private void ListenLevelCompleteEvent() => GameManager.OnLevelComplete += OnLevelCompleteEvent;
        private void UnListenLevelCompleteEvent() => GameManager.OnLevelComplete -= OnLevelCompleteEvent;

        internal void EnableHintObjects( bool enable = true ) {
            
            if( hintObject != null ) hintObject.SetActive( enable );
        }

        private void OnLevelCompleteEvent() {
            
            DataManager.gameData.IsTutorialPlayed = true;
            PlayLevelCompletionSounds();
            UIManager.Instance.ShowNextLevelButton( true );
        }

        public void LevelCompleted() {

            isLevelComplete = true;
            GameManager.isGamePlaying = false;
            StartCoroutine( InvokeLevelCompletePanel( 0.5f ) );
        }

        internal IEnumerator InvokeLevelCompletePanel( float delay ) {
            
            print( $"-----------------LEVEL COMPLETED-----------------" );
            yield return new WaitForSeconds( delay );
            GameManager.InvokeOnLevelComplete();
        }
        
        private void PlayLevelCompletionSounds() {
            
            AudioManager.ChangeSoundClip( AudioManager.LevelCompletionClip );
        }

    }

    // [ Serializable ]
    // public struct LevelSettings {
    //     
    // }

}
