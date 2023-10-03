using System;
using Core;
using DG.Tweening;
using Lofelt.NiceVibrations;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {

    public class SettingsPanel: PanelBase {

        [FoldoutGroup("Settings"), SerializeField] public Toggle music;
        [FoldoutGroup("Settings"), SerializeField] public Toggle sound;
        [FoldoutGroup("Settings"), SerializeField] public Toggle vibration;
        [FoldoutGroup("Settings"), SerializeField] public Button removeAds;
        [FoldoutGroup("Settings"), SerializeField] public Button restorePurchases;
        [FoldoutGroup("Settings"), SerializeField] public Button contactUs;
        
        private void OnEnable() {
            
            ChangeObjectVisuals();
            SetSettings();
        }

        private void ChangeObjectVisuals() {

            if( music != null ) music.isOn = DataManager.settingsData.MusicOn;
            if( sound != null ) sound.isOn = DataManager.settingsData.SoundOn;
            if( vibration != null ) vibration.isOn = DataManager.settingsData.VibrationOn;
        }

        private void SetSettings() {

            AudioManager.EnableMusic( DataManager.settingsData.MusicOn );
            AudioManager.EnableSound( DataManager.settingsData.SoundOn );

            HapticController.hapticsEnabled = DataManager.settingsData.VibrationOn;

            if( removeAds != null ) removeAds.interactable = !DataManager.settingsData.RemovedAds;
            if( removeAds != null ) removeAds.gameObject.SetActive( !DataManager.settingsData.RemovedAds );
            if( restorePurchases != null ) restorePurchases.interactable = !DataManager.settingsData.RestoredPurchases;
        }

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
        
        public void ToggleMusic() {

            DataManager.settingsData.MusicOn = music.isOn;
            AudioManager.PlaySound();
            SetSettings();
        }

        public void ToggleSound() {

            DataManager.settingsData.SoundOn = sound.isOn;
            AudioManager.PlaySound();
            SetSettings();
        }

        public void ToggleVibration() {

            DataManager.settingsData.VibrationOn = vibration.isOn;
            AudioManager.PlaySound();
            SetSettings();
        }

        public void RemoveAds() {

            // IAPWrapper.GiveReward( "removeAds" );
        }

        public void RestorePurchases() {

            // IAPWrapper.RestorePurchases();
        }

        public void OpenContactUsURL() {

            // const string Email = "hello@xrevstudio.com";
            // var mailTo = $"mailto:{Email}?subject='{Application.productName}'&body=";

            var rateUsSite = $"https://play.google.com/store/apps/details?id={Application.identifier}";

            Application.OpenURL( rateUsSite );
        }


    }
}
