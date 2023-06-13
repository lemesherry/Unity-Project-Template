using System;
using DG.Tweening;
using Extras;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core {

    public class UIManager: MonoBehaviour {

        public static UIManager Instance { get; private set; }

        public GameObject confetti;
        public Buttons buttons;
        public Panels panels;
        public Texts texts;
        public Renderers renderers;
        public Sprites sprites;

        private void Awake() => SetInstance();

        private void Start() => InitializeLevelAndSetSettings();

        private void SetInstance() {

            if( Instance != null ) Destroy( Instance );
            Instance = this;
        }

        private void InitializeLevelAndSetSettings() {

            SetActiveObject( panels.settings, false );
            SetActiveObject( buttons.nextLevel.gameObject, false );

            _Button_Play();

            // ChangeSettingsPanel();
            // SetSettings();
        }

        private void ChangeSettingsPanel() {

            renderers.music.sprite = DataManager.settingsData.MusicOn? sprites.musicOn : sprites.musicOff;
            renderers.sound.sprite = DataManager.settingsData.SoundOn? sprites.soundOn : sprites.soundOff;
            renderers.haptics.sprite = DataManager.settingsData.HapticsOn? sprites.hapticsOn : sprites.hapticsOff;
        }

        internal void SetSettings() {

            AudioManager.EnableBackgroundAudio( DataManager.settingsData.MusicOn );
            AudioManager.EnableGamePlayAudio( DataManager.settingsData.SoundOn );
            HapticsManager.instance.enabled = DataManager.settingsData.HapticsOn;

            SetButtonInteractable( buttons.removeAdsSettings, !DataManager.settingsData.RemovedAds );
            SetActiveObject( buttons.removeAdsSettings, !DataManager.settingsData.RemovedAds );
            SetButtonInteractable( buttons.restorePurchases, !DataManager.settingsData.RestoredPurchases );
        }

        internal void EnableConfetti( bool enable = true ) { confetti.SetActive( enable ); }

        internal void ShowNextLevelButton( bool value ) {

            if( value ) {

                SetActiveGameplayPanelButtons( false );
                buttons.nextLevel.transform.AnimateScale( Vector3.zero, Vector3.one, 0.3f, Ease.InQuad, false, () => {

                    SetActiveObject( buttons.nextLevel.gameObject, true );
                    SetButtonInteractable( buttons.nextLevel, false );
                }, () => SetButtonInteractable( buttons.nextLevel, true ) );
                SetActiveObject( buttons.nextLevel.gameObject, false );
            } else {

                SetActiveGameplayPanelButtons( true );
                buttons.nextLevel.transform.AnimateScale( Vector3.one, Vector3.zero, 0.3f, Ease.InQuad, false, () => SetButtonInteractable( buttons.nextLevel, false ), () => SetActiveObject( buttons.nextLevel.gameObject, false ) );
            }
        }

        #region ★彡[ Buttons ]彡★

        public void _Button_Hint() {

            var currentLevel = GameManager.isTutorialLevel? $"Tutorial_{DataManager.gameData.TutorialLevel}" : 
            $"Level_{DataManager.gameData.CurrentLevel}";

            // ★彡[ Replace this debug log with analytics event (design) ]彡★
            Debug.Log( $"Monetization:Hint_Button_OnLevel_{currentLevel}" );

            LevelManager.Instance.EnableHintObjects();

            // if( AdsManager.Instance != null ) AdsManager.Instance.ShowRewardedAd( callback => {
            //
            // #if UNITY_EDITOR
            //     callback = true;
            // #endif
            //
            //     if( callback ) {
            //
            //         LevelManager.Instance.EnableHintObjects();
            //         SetHintButtonInteractable( true );
            //         
            //     } else {
            //         SetHintButtonInteractable( true );
            //     }
            // } );
        }

        public void _Button_SkipLevel() {

            SetGameplayPanelButtonsInteractive( false );

            GameManager.isGamePlaying = false;
            DataManager.gameData.IsTutorialPlayed = true;

            // ★彡[ Replace this debug log with analytics event (design skip level) ]彡★
            Debug.Log( GameManager.isTutorialLevel? $"Tutorial_{DataManager.gameData.TutorialLevel}" : $"Level_{DataManager.gameData.CurrentLevel}" );

            if( GameManager.isTutorialLevel ) DataManager.gameData.TutorialLevel++;
            else DataManager.gameData.CurrentLevel++;

            if( HapticsManager.instance != null && DataManager.settingsData.HapticsOn ) HapticsManager.DoHaptics_Short();

            GameManager.ResetSettings();
            GameManager.InitializeLevelAndSetSettings( () => { panels.gameplay.transform.AnimateScale( Vector3.zero, Vector3.one, 0.2f, Ease.InQuad, false, null, () => { SetGameplayPanelButtonsInteractive( true ); } ); } );
        }

        public void _Button_NextLevel() {

            SetButtonInteractable( buttons.nextLevel, false );
            DataManager.gameData.IsTutorialPlayed = true;

            // ★彡[ Replace this debug log with analytics event (progression complete) ]彡★
            Debug.Log( GameManager.isTutorialLevel? $"Tutorial_{DataManager.gameData.TutorialLevel}" : $"Level_{DataManager.gameData.CurrentLevel}" );

            // if( AdsManager.Instance != null ) AdsManager.Instance.ShowInterstitial( callback => {} );

            if( GameManager.isTutorialLevel ) DataManager.gameData.TutorialLevel++;
            else DataManager.gameData.CurrentLevel++;

            if( HapticsManager.instance != null && DataManager.settingsData.HapticsOn ) HapticsManager.DoHaptics_Medium();

            GameManager.ResetSettings();
            GameManager.InitializeLevelAndSetSettings( () => {

                ShowNextLevelButton( false );

                // SetActiveObject( buttons.nextLevel, false );
                // SetActiveObject( buttons.hint );
            } );
        }

        public void _Button_Restart() {

            // SetButtonInteractable( buttons.restart, false );
            GameManager.isGamePlaying = false;

            // ★彡[ Replace this debug log with analytics event (progression failed) ]彡★
            Debug.Log( GameManager.isTutorialLevel? $"Tutorial_{DataManager.gameData.TutorialLevel}" : $"Level_{DataManager.gameData.CurrentLevel}" );

            SceneManager.LoadScene( SceneManager.GetActiveScene().name );
        }

        public void _Button_Settings() {

            var settingsBg = panels.settings.transform.GetChild( 1 );

            if( panels.settings.activeInHierarchy ) {
                
                GameManager.isGamePlaying = true;
                
                if( settingsBg != null ) {
                    
                    settingsBg.AnimateScale( Vector3.one, Vector3.zero, 0.4f, Ease.InQuad, false, () => {

                        SetSettingsButtonInteractable( false );
                    }, () => {
                    SetActiveObject( panels.settings, false );
                    SetGameplayPanelButtonsInteractive( true );
                } ); }
            } else {

                SetActiveObject( panels.settings, true );
                GameManager.isGamePlaying = false;
                
                if( settingsBg != null ) settingsBg.AnimateScale( Vector3.zero, Vector3.one, 0.4f, Ease.InQuad, false, () => {
                    
                    SetGameplayPanelButtonsInteractive( false );
                    SetSettingsButtonInteractable( false );
                }, () => SetSettingsButtonInteractable( true ) );

            }
        }

        public void _Button_CloseAndBack() {

            if( panels.settings.activeInHierarchy ) {
                
                GameManager.isGamePlaying = true;

                var settingsBg = panels.settings.transform.GetChild( 1 );

                if( settingsBg != null ) { settingsBg.AnimateScale( Vector3.one, Vector3.zero, 0.4f, Ease.InQuad, false, null, () => {
                    
                    SetActiveObject( panels.settings, false );
                    SetGameplayPanelButtonsInteractive( true );
                    SetSettingsButtonInteractable( false );
                } ); }
            }

            // if( panels.gameplay.activeInHierarchy && GameManager.isGamePlaying ) {
            //
            //     GameManager.isGamePlaying = false;
            //     SetActiveObject( panels.mainMenu );
            //     SetActiveObject( panels.gameplay, false );
            // }
        }

        public void _Button_Play() {
            
            GameManager.InitializeLevelAndSetSettings( () => {

                // SetActiveObject( panels.mainMenu, false );
                // SetActiveObject( panels.gameplay );
            } );
        }

        public void _Button_Music() {

            DataManager.settingsData.MusicOn = !DataManager.settingsData.MusicOn;
            SetSettings();
            ChangeSettingsPanel();
        }

        public void _Button_Sound() {

            DataManager.settingsData.SoundOn = !DataManager.settingsData.SoundOn;
            SetSettings();
            ChangeSettingsPanel();
        }

        public void _Button_Vibration() {

            DataManager.settingsData.HapticsOn = !DataManager.settingsData.HapticsOn;
            SetSettings();
            ChangeSettingsPanel();
        }

        public void _Button_RemoveAds() {

            // IAPWrapper.GiveReward( "removeAds" );
        }

        public void _Button_RestorePurchases() {

            // IAPWrapper.RestorePurchases();
        }

        public void _Button_ContactUs() {

            const string EMAIL = "info@xrevstudio.com";
            Application.OpenURL( $"mailto:{EMAIL}?subject=&body=" );
        }

        #endregion

        #region ★彡[ Buttons Functionality ]彡★

        internal void ChangeRemoveAdsButtons( bool hasAdsRemoved ) {

            SetButtonInteractable( buttons.removeAdsSettings, !hasAdsRemoved );
            SetActiveObject( buttons.removeAdsGameplay, !hasAdsRemoved );
            SetButtonInteractable( buttons.restorePurchases, hasAdsRemoved );
        }

        internal void SetGameplayPanelButtonsInteractive( bool value ) {

            SetButtonInteractable( buttons.settings, value );
            SetButtonInteractable( buttons.hint, value );
            SetButtonInteractable( buttons.skipLevel, value );
            if( !DataManager.settingsData.RemovedAds ) SetButtonInteractable( buttons.removeAdsGameplay, value );
        }

        internal void SetSettingsButtonInteractable( bool value ) {
            
            SetButtonInteractable( buttons.music, value );
            SetButtonInteractable( buttons.sound, value );
            SetButtonInteractable( buttons.vibration, value );
            SetButtonInteractable( buttons.contactUs, value );
            if( DataManager.settingsData.RemovedAds ) SetButtonInteractable( buttons.restorePurchases, value );
            if( !DataManager.settingsData.RemovedAds ) SetButtonInteractable( buttons.removeAdsSettings, value );
        }

        internal void SetActiveGameplayPanelButtons( bool value ) {

            SetActiveObject( buttons.settings.gameObject, value );
            SetActiveObject( buttons.hint.gameObject, value );
            SetActiveObject( buttons.skipLevel.gameObject, value );
            if( !DataManager.settingsData.RemovedAds ) SetActiveObject( buttons.removeAdsGameplay.gameObject, value );
        }

        internal static void SetActiveObject( Component obj, bool enable ) => obj.gameObject.SetActive( enable );

        internal static void SetActiveObject( GameObject obj, bool enable ) => obj.SetActive( enable );

        internal static void SetButtonInteractable( Button button, bool enable ) => button.interactable = enable;

        #endregion

    }

    [ Serializable ]
    public struct Buttons {

        public Button hint;
        public Button skipLevel;
        public Button nextLevel;
        public Button settings;
        public Button close;
        public Button music;
        public Button sound;
        public Button vibration;
        public Button removeAdsSettings;
        public Button removeAdsGameplay;
        public Button restorePurchases;
        public Button contactUs;

    }

    [ Serializable ]
    public struct Texts {

        public TextMeshProUGUI level;

    }

    [ Serializable ]
    public struct Renderers {

        public Image music;
        public Image sound;
        public Image haptics;

    }

    [ Serializable ]
    public struct Sprites {

        public Sprite musicOn;
        public Sprite musicOff;
        public Sprite soundOn;
        public Sprite soundOff;
        public Sprite hapticsOn;
        public Sprite hapticsOff;

    }

    [ Serializable ]
    public struct Panels {

        public GameObject gameplay;
        public GameObject mainMenu;
        public GameObject levelComplete;
        public GameObject settings;
        public GameObject fade;

    }

}
