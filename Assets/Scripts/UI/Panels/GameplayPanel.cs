using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = Core.Debug;

namespace UI.Panels {

    public class GameplayPanel: Panel {

        [SerializeField] private GameplayPanelSettings settings;
        internal static PowerUpType currentPowerUp = PowerUpType.None;

        public override void Enable( Action onAnimationComplete = null ) {

            base.Enable( onAnimationComplete );

            objectToAnimate.localScale = Vector3.zero;
            objectToAnimate.DOScale( Vector3.one, 0.2f ).OnComplete( () => {

                onAnimationComplete?.Invoke();
            } );
        }

        public override void Disable( Action onAnimationComplete = null ) {

            base.Disable( onAnimationComplete );

            objectToAnimate.DOScale( Vector3.zero, 0.2f ).OnComplete( () => {

                onAnimationComplete?.Invoke();
                gameObject.SetActive( false );
            } );
        }
        
        public void ClosePanel() => Disable();
        
        public void PowerUp1() {

            var gameDataPowerUps = DataManager.gameData.PowerUps;

            if( gameDataPowerUps.powerUp1Count <= 0 ) {

                var adPanel = GetPanelOfType<AdPanel>();
                if( adPanel == null ) return;

                adPanel.OpenAdPanel( PowerUpType.PowerUp1 );
            } else {

                AudioManager.PlaySound();

                gameDataPowerUps.powerUp1Count--;
                SetPowerUpIcons( gameDataPowerUps );

                // EventsManager.SendPowerUpUsedEvent( PowerUpType.SkipLevel );
            }
        }

        public void PowerUp2() {

            var gameDataPowerUps = DataManager.gameData.PowerUps;

            if( gameDataPowerUps.powerUp2Count <= 0 ) {

                var adPanel = GetPanelOfType<AdPanel>();
                if( adPanel == null ) return;

                adPanel.OpenAdPanel( PowerUpType.PowerUp2 );
            } else {

                AudioManager.PlaySound();

                gameDataPowerUps.powerUp2Count--;
                SetPowerUpIcons( gameDataPowerUps );

                // EventsManager.SendPowerUpUsedEvent( PowerUpType.Hint );
            }
        }
        
        internal void GivePowerUp() {

            var gameDataPowerUps = DataManager.gameData.PowerUps;

            switch( currentPowerUp ) {

            case PowerUpType.PowerUp1:
                gameDataPowerUps.powerUp1Count += settings.powerUps[0].countToAdd;

                // EventsManager.SendPowerUpAddedEvent( currentPowerUp, Instance.powerUpsToAdd.skipLevelCount );

                break;
            case PowerUpType.PowerUp2:
                gameDataPowerUps.powerUp2Count += settings.powerUps[1].countToAdd;

                // EventsManager.SendPowerUpAddedEvent( currentPowerUp, Instance.powerUpsToAdd.hintCount );

                break;
            case PowerUpType.None:
            default:
                break;
            }

            SetPowerUpIcons( gameDataPowerUps );
        }
        
        private void SetPowerUpIcons( PowerUps powerUps ) {

            if( powerUps.powerUp1Count <= 0 ) {
                settings.powerUps[0].adIcon.SetActive( true );
                settings.powerUps[0].countText.gameObject.SetActive( false );
            } else {
                settings.powerUps[0].adIcon.SetActive( false );
                settings.powerUps[0].countText.gameObject.SetActive( true );
                settings.powerUps[0].countText.text = $"{powerUps.powerUp1Count}";
            }

            if( powerUps.powerUp1Count <= 0 ) {
                settings.powerUps[1].adIcon.SetActive( true );
                settings.powerUps[1].countText.gameObject.SetActive( false );
            } else {
                settings.powerUps[1].adIcon.SetActive( false );
                settings.powerUps[1].countText.gameObject.SetActive( true );
                settings.powerUps[1].countText.text = $"{powerUps.powerUp1Count}";
            }

            DataManager.gameData.PowerUps = powerUps;
        }
        
        private async void AnimatePowerUps() {

            var animationDuration = 0.2f;
            var delayTime = (int)(1000 * animationDuration / 2);

            var skipLevel = settings.bomb.transform.parent;
            var hint = settings.mystery.transform.parent;

            skipLevel.localScale = new Vector3( 0.8f, 0.8f, 0.8f );
            hint.localScale = new Vector3( 0.8f, 0.8f, 0.8f );

            skipLevel.DOScale( 1.2f.ToVector3(), animationDuration ).OnComplete( () => {

                skipLevel.DOScale( Vector3.one, animationDuration );
            } );

            await Task.Delay( delayTime );

            hint.DOScale( 1.2f.ToVector3(), animationDuration ).OnComplete( () => {

                hint.DOScale( Vector3.one, animationDuration );
            } );

            await Task.Delay( delayTime );
        }

    }

    [Serializable]
    public struct GameplayPanelSettings {

        public Button bomb;
        public Button mystery;
        public Button removeAds;

        public List<PowerUpIcons> powerUps;
    }
    
    
    [Serializable]
    public struct PowerUpIcons {

        public int countToAdd;
        public GameObject adIcon;
        public TextMeshProUGUI countText;
    }

}
