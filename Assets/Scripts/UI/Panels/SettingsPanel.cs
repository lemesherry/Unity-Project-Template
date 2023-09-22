using System;
using Core;
using DG.Tweening;
using Lofelt.NiceVibrations;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Debug = Core.Debug;

namespace UI.Panels {

    public class SettingsPanel: Panel {

        [SerializeField] private SettingPanelSettings settings;

        private void OnEnable() {
            
            ChangeObjectVisuals();
            SetSettings();
        }

        private void ChangeObjectVisuals() {

            if( settings.music != null ) settings.music.isOn = DataManager.settingsData.MusicOn;
            if( settings.sound != null ) settings.sound.isOn = DataManager.settingsData.SoundOn;
            if( settings.vibration != null ) settings.vibration.isOn = DataManager.settingsData.VibrationOn;
        }

        private void SetSettings() {

            AudioManager.EnableMusic( DataManager.settingsData.MusicOn );
            AudioManager.EnableSound( DataManager.settingsData.SoundOn );

            HapticController.hapticsEnabled = DataManager.settingsData.VibrationOn;

            if( settings.removeAds != null ) settings.removeAds.interactable = !DataManager.settingsData.RemovedAds;
            if( settings.removeAds != null ) settings.removeAds.gameObject.SetActive( !DataManager.settingsData.RemovedAds );
            if( settings.restorePurchases != null ) settings.restorePurchases.interactable = !DataManager.settingsData.RestoredPurchases;
        }

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
        
        public void ToggleMusic() {

            DataManager.settingsData.MusicOn = settings.music.isOn;
            AudioManager.PlaySound();
            SetSettings();
        }

        public void ToggleSound() {

            DataManager.settingsData.SoundOn = settings.sound.isOn;
            AudioManager.PlaySound();
            SetSettings();
        }

        public void ToggleVibration() {

            DataManager.settingsData.VibrationOn = settings.vibration.isOn;
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
    
    [Serializable]
    public struct SettingPanelSettings {

        public Toggle music;
        public Toggle sound;
        public Toggle vibration;
        public Button removeAds;
        public Button restorePurchases;
        public Button contactUs;

    }

}
