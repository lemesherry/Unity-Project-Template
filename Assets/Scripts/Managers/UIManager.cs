using UnityEngine;
using Core;
using Lofelt.NiceVibrations;
using Sirenix.OdinInspector;
using UI.Panels;

namespace Managers {

    public class UIManager: MonoBehaviour {
        private static UIManager Instance { get; set; }
        private static PanelBase[] _panels;

        private void Awake() => SetInitialSettings();

        private void Start() => InitializeLevelAndSetSettings();

        private void SetInitialSettings() {

            if( Instance != null ) Destroy( Instance.gameObject );
            Instance = this;
        }

        private void InitializeLevelAndSetSettings() {

            _panels = GetComponentsInChildren<PanelBase>( true );

            foreach( PanelBase t in _panels ) {
                t.gameObject.SetActive( true ); // Setting it true so that its Awake function gets called
                t.gameObject.SetActive( false );
            }

            PlayGame();

            // SetPowerUpIcons( DataManager.gameData.PowerUps );
        }

        public static void ShowLevelFailedPanel() {

            LevelManager.InvokeOnLevelFailed();

            PanelBase.GetPanelOfType<LevelFailPanel>().Enable( 0.3f );
        }
        public static void ShowLevelCompletePanel() {

            LevelManager.InvokeOnLevelCompleted();

            PanelBase.GetPanelOfType<LevelCompletePanel>().Enable( 0.3f );
        }
        public static void ShowExitPanel() {

            PanelBase.GetPanelOfType<ExitPanel>().Enable();
        }

        public static void ShowSettingsPanel() {

            // _panels.Find( p => p.type == PanelType.Settings ).Enable();
            PanelBase.GetPanelOfType<SettingsPanel>().Enable();
        }

        public static void PlayGame() {

            AudioManager.PlaySound();
            LevelManager.InvokeOnLevelInitiate();

            PanelBase.GetPanelOfType<GameplayPanel>().Enable();
        }
    }

}
