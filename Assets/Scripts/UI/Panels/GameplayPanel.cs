using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DG.Tweening;
using Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = Core.Debug;

namespace UI.Panels {

    public class GameplayPanel: PanelBase {

        [FoldoutGroup("Settings"), SerializeField] public Button bomb;
        [FoldoutGroup("Settings"), SerializeField] public Button mystery;
        [FoldoutGroup("Settings"), SerializeField] public Button removeAds;

        [FoldoutGroup("Settings"), SerializeField] public List<PowerUpIcons> powerUps;
        
        public override void Enable( float delay = 0, Action onAnimationComplete = null ) {

            base.Enable( delay, onAnimationComplete );

            objectToAnimate.localScale = Vector3.zero;
            objectToAnimate.DOScale( Vector3.one, 0.2f ).SetDelay( delay ).OnComplete( () => {

                onAnimationComplete?.Invoke();
            } );
        }

        public override void Disable( float delay = 0, Action onAnimationComplete = null ) {

            base.Disable( delay, onAnimationComplete );

            objectToAnimate.DOScale( Vector3.zero, 0.2f ).SetDelay( delay ).OnComplete( () => {

                onAnimationComplete?.Invoke();
                gameObject.SetActive( false );
            } );
        }
        
        public void ClosePanel() => Disable();
        
        public void PowerUp1() {

            var gameDataPowerUps = DataManager.gameData.PowerUpCounts;

            if( gameDataPowerUps.powerUp1 <= 0 ) {

                var adPanel = GetPanelOfType<AdPanel>();
                if( adPanel == null ) return;

                // adPanel.OpenAdPanel( PowerUpType.PowerUp1 );
            } else {

                AudioManager.PlaySound();

                gameDataPowerUps.powerUp1--;
                SetPowerUpIcons( gameDataPowerUps );

                // EventsManager.SendPowerUpUsedEvent( PowerUpType.SkipLevel );
            }
        }

        public void PowerUp2() {

            var gameDataPowerUps = DataManager.gameData.PowerUpCounts;

            if( gameDataPowerUps.powerUp2 <= 0 ) {

                var adPanel = GetPanelOfType<AdPanel>();
                if( adPanel == null ) return;

                // adPanel.OpenAdPanel( PowerUpType.PowerUp2 );
            } else {

                AudioManager.PlaySound();

                gameDataPowerUps.powerUp2--;
                SetPowerUpIcons( gameDataPowerUps );

                // EventsManager.SendPowerUpUsedEvent( PowerUpType.Hint );
            }
        }
        
        internal void GivePowerUp() {

            var gameDataPowerUps = DataManager.gameData.PowerUpCounts;

            // switch( currentPowerUp ) {
            //
            // case PowerUpType.PowerUp1:
            //     gameDataPowerUps.powerUp1Count += powerUps[0].countToAdd;
            //
            //     // EventsManager.SendPowerUpAddedEvent( currentPowerUp, Instance.powerUpsToAdd.skipLevelCount );
            //
            //     break;
            // case PowerUpType.PowerUp2:
            //     gameDataPowerUps.powerUp2Count += powerUps[1].countToAdd;
            //
            //     // EventsManager.SendPowerUpAddedEvent( currentPowerUp, Instance.powerUpsToAdd.hintCount );
            //
            //     break;
            // case PowerUpType.None:
            // default:
            //     break;
            // }

            SetPowerUpIcons( gameDataPowerUps );
        }
        
        private void SetPowerUpIcons( PowerUpCounts powerUpCount ) {

            if( powerUpCount.powerUp1 <= 0 ) {
                powerUps[0].adIcon.SetActive( true );
                powerUps[0].countText.gameObject.SetActive( false );
            } else {
                powerUps[0].adIcon.SetActive( false );
                powerUps[0].countText.gameObject.SetActive( true );
                powerUps[0].countText.text = $"{powerUpCount.powerUp1}";
            }

            if( powerUpCount.powerUp1 <= 0 ) {
                powerUps[1].adIcon.SetActive( true );
                powerUps[1].countText.gameObject.SetActive( false );
            } else {
                powerUps[1].adIcon.SetActive( false );
                powerUps[1].countText.gameObject.SetActive( true );
                powerUps[1].countText.text = $"{powerUpCount.powerUp1}";
            }

            DataManager.gameData.PowerUpCounts = powerUpCount;
        }
        
        private async void AnimatePowerUps() {

            var animationDuration = 0.2f;
            var delayTime = (int)(1000 * animationDuration / 2);

            var skipLevel = bomb.transform.parent;
            var hint = mystery.transform.parent;

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
    public struct PowerUpIcons {

        public int countToAdd;
        public GameObject adIcon;
        public TextMeshProUGUI countText;
    }

}
