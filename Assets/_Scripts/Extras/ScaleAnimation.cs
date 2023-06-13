using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Extras {

    public class ScaleAnimation : MonoBehaviour {

        [ Range( 0f, 10f ) ] public float duration = 0.3f;

        public UnityEvent onStart;
        public UnityEvent onComplete;
        [ Space( 5 ) ]
        public ScaleSettings otherSettings;

        private Animator _animator;

        private void OnEnable() {

            var anim = GetComponent<Animator>();
            if( anim != null ) _animator = anim;

            if( _animator != null ) {
                
                StartCoroutine( WaitForAnimator() );
                return;
            }
            
            if( otherSettings.pingPongScale ) transform.AnimateScale( otherSettings.scaleFrom, otherSettings.scaleTo, duration, Ease.Linear, true, () => onStart.Invoke(), () => onComplete.Invoke() );
            
            else transform.AnimateScale( otherSettings.scaleFrom, otherSettings.scaleTo, duration );
        }

        private void OnDisable() {
            
            transform.AnimateScale( otherSettings.scaleTo, otherSettings.scaleFrom, duration );
        }

        private IEnumerator WaitForAnimator() {
            
            onStart?.Invoke();

            yield return new WaitForSeconds( _animator.GetCurrentAnimatorClipInfo( 0 ).Length );
            onComplete?.Invoke();
        }
    }

    [ Serializable ]
    public struct ScaleSettings {

        public bool pingPongScale;
        public Vector3 scaleFrom;
        public Vector3 scaleTo;
    }

}
