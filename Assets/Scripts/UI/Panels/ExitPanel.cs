using System;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace UI.Panels {

    public class ExitPanel: Panel {

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
