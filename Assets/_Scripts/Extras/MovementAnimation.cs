using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Extras {

    public class MovementAnimation: MonoBehaviour {
        
        public UnityEvent onStart;
        public UnityEvent onComplete;
        
        [ Space( 5 ) ]
        public MovementAnimationSettings otherSettings;

        private Animator _animator;

        private int _pointIndex;

        private void OnEnable() {

            var anim = GetComponent<Animator>();
            if( anim != null ) _animator = anim;

            if( _animator != null ) {
                
                StartCoroutine( WaitForAnimator() );
                return;
            }

            if( otherSettings.pingPong ) PingPongMove( transform, otherSettings.easeMode );
            else Move( transform, otherSettings.easeMode );
        }

        private void OnDisable() {

            if( otherSettings.points.Length <= 0 ) return;
            
            if( otherSettings.localMove ) transform.localPosition = otherSettings.points[otherSettings.points.Length - 1];
            else transform.position = otherSettings.points[otherSettings.points.Length - 1];
        }

        private void PingPongMove( Transform trans, Ease easeMode ) {
            
            if( otherSettings.localMove ) {
                
                trans.DOLocalMove( otherSettings.points[_pointIndex], otherSettings.animationDuration ).SetEase( easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
                    _pointIndex++;

                    InverseXScale();
                    if( _pointIndex >= otherSettings.points.Length ) _pointIndex = 0;
                    PingPongMove( trans, easeMode );
                } );
            } else {
                
                trans.DOMove( otherSettings.points[_pointIndex], otherSettings.animationDuration ).SetEase( easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
                    _pointIndex++;
                    
                    InverseXScale();
                    if( _pointIndex >= otherSettings.points.Length ) _pointIndex = 0;
                    PingPongMove( trans, easeMode );
                } );
            }
        }

        private void Move( Transform trans, Ease easeMode ) {
            
            if( otherSettings.localMove ) {
                
                trans.DOLocalMove( otherSettings.points[_pointIndex], otherSettings.animationDuration ).SetEase( easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
                    _pointIndex++;

                    InverseXScale();
                    if( _pointIndex >= otherSettings.points.Length ) {
                        
                        onComplete.Invoke();
                        return;
                    }
                    PingPongMove( trans, easeMode );
                } );
            } else {
                
                trans.DOMove( otherSettings.points[_pointIndex], otherSettings.animationDuration ).SetEase( easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
                    _pointIndex++;

                    InverseXScale();
                    if( _pointIndex >= otherSettings.points.Length ) {

                        onComplete.Invoke();
                        return;
                    }
                    PingPongMove( trans, easeMode );
                } );
            }
        }

        public void InverseXScale() {

            if( !otherSettings.inverseXScaleOnComplete ) return;

            var _transform = transform;
            var _localScale = _transform.localScale;
            
            _localScale = new Vector3( _localScale.x * -1, _localScale.y, _localScale.z );
            _transform.localScale = _localScale;
        }
        
        private IEnumerator WaitForAnimator() {

            onStart?.Invoke();

            yield return new WaitForSeconds( _animator.GetCurrentAnimatorClipInfo( 0 ).Length );
            onComplete?.Invoke();
        }
    }

    [ Serializable ]
    public struct MovementAnimationSettings {

        public bool localMove;
        public bool pingPong;
        public bool inverseXScaleOnComplete;
        [ Range( 0f, 100f ) ] public float animationDuration;
        public Ease easeMode;
        public Vector3[] points;
    }

}