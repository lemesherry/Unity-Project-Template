using UnityEngine;
using Core;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UI.Panels;

namespace Managers {

    public class UIManager: MonoBehaviour {

        private static UIManager Instance { get; set; }
        private static Panel[] _panels;

        private void Awake() => SetInitialSettings();

        private void Start() => InitializeLevelAndSetSettings();

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance.gameObject );
            Instance = this;
        }

        private void InitializeLevelAndSetSettings() {

            _panels = GetComponentsInChildren<Panel>(true);

            foreach( Panel t in _panels ) {
                t.gameObject.SetActive( true ); // Setting it true so that its Awake function gets called
                t.gameObject.SetActive( false );
            }
            
            Button_Play();
            // SetPowerUpIcons( DataManager.gameData.PowerUps );
        }

    #region ★彡[ Buttons ]彡★

        public void Button_RestartLevel() {

            GameManager.isGamePlaying = false;

            // if( GameManager.Testing.enableInterstitialAds ) InterstitialAd.ShowAd();

            AudioManager.PlaySound();

            HapticPatterns.PlayPreset( HapticPatterns.PresetType.Selection );

            LevelManager.InvokeOnLevelRestart();
        }

        public void Button_Settings() {

            // _panels.Find( p => p.type == PanelType.Settings ).Enable();
            Panel.GetMainPanelOfType<SettingsPanel>().Enable();
        }
        
        public void Button_LevelFailed() {

            LevelManager.InvokeOnLevelFailed();
        }

        public void Button_Play() {

            AudioManager.PlaySound();
            LevelManager.InvokeOnLevelInitiate();
        }

        public void Button_Exit() {

            Panel.GetMainPanelOfType<ExitPanel>().Enable();
        }

    #endregion
        
        [Button]
        private void TestEnableSettingsPanel() {

            Panel.GetMainPanelOfType<SettingsPanel>().Enable();
        }
        
        [Button]
        private void TestEnableExitPanel() {

            Panel.GetMainPanelOfType<ExitPanel>().Enable();
        }
        
        [Button]
        private void TestEnableGameplayPanel() {

            Panel.GetMainPanelOfType<GameplayPanel>().Enable();
        }        
        
        [Button]
        private void TestLevelFail() {

            LevelManager.InvokeOnLevelFailed();
        }
    }

    public enum PowerUpType {

        None,
        PowerUp1,
        PowerUp2,
        PowerUp3,
        PowerUp4,
        PowerUp5

    }

}
