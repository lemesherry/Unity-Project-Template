using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace UI.Panels {

    public class ExitPanel: PanelBase {

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
        
        public void QuitGame() {

            AudioManager.PlaySound();

        #if UNITY_EDITOR

            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        }

        public void ClosePanel() => Disable();

    }

}
