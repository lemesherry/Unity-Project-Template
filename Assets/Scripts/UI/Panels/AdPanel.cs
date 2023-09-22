using System;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;

namespace UI.Panels {

    public class AdPanel: Panel {

        [SerializeField] private AdPanelSettings settings;
        
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
        
        internal void OpenAdPanel( PowerUpType powerUpType ) {

            AudioManager.PlaySound();
            GameManager.isGamePlaying = false;

            GameplayPanel.currentPowerUp = powerUpType;

            switch( powerUpType ) {
            case PowerUpType.PowerUp1:
                settings.adPanelText.text = "Watch an add to get \"Skip\" powerup?";

                break;
            case PowerUpType.PowerUp2:
                settings.adPanelText.text = "Watch an add to get \"Hint\" powerup?";

                break;
            case PowerUpType.None:
            default:
                break;
            }

            Enable();
        }

        public void ShowAd( bool isAdPanel ) {

            AudioManager.PlaySound();
            var gameplayPanel = GetPanelOfType<GameplayPanel>();

            if( GameManager.Testing.enableRewardedAds ) {

                // RewardedAd.ShowAd( hasRewarded => {
                //
                //     if( hasRewarded ) {
                //
                //         if( gameplayPanel != null ) gameplayPanel.GivePowerUp();
                //     }
                //     else {
                //
                //         GameplayPanel.currentPowerUp = PowerUpType.None;
                //     }
                //
                //     Debugger.Log( hasRewarded? "Rewarded" : "Not Rewarded", LogSeverity.High );
                // } );
            } else {

                if( gameplayPanel != null ) gameplayPanel.GivePowerUp();
            }

            GameManager.isGamePlaying = true;
            Disable();
        }
        
        public void ClosePanel() => Disable();

    }

    [Serializable]
    public struct AdPanelSettings {

        public TextMeshProUGUI adPanelText;

    }

}
