using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Extras {

    public class AlphaAnimation: MonoBehaviour {

        public UnityEvent onStart;
        public UnityEvent onComplete;

        [ Space( 5 ) ]
        public AlphaAnimationSettings otherSettings;

        private Animator _animator;
        private Image _image;
        private SpriteRenderer _spriteRenderer;

        private int _pointIndex;

        private Color _startColor;
        private Color _endColor;

        private bool _animateImage;
        private bool _animateSpriteRenderer;

        private void OnEnable() {

            var anim = GetComponent<Animator>();
            if( anim != null ) _animator = anim;

            if( _animator != null ) {

                StartCoroutine( WaitForAnimator() );

                return;
            }

            var tempImage = GetComponent<Image>();
            var tempSpriteRenderer = GetComponent<SpriteRenderer>();

            if( tempImage != null ) {
                
                _image = tempImage;
                
                _startColor = new Color( _image.color.r, _image.color.g, _image.color.b, _image.color.a ) {
                    a = otherSettings.startAlpha
                };
                _endColor = new Color( _image.color.r, _image.color.g, _image.color.b, _image.color.a ) {
                    a = otherSettings.endAlpha
                };
            }

            if( tempSpriteRenderer != null ) {
                
                _spriteRenderer = tempSpriteRenderer;
                
                var _color = _spriteRenderer.color;
                
                _startColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
                    a = otherSettings.startAlpha
                };
                _endColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
                    a = otherSettings.endAlpha
                };
            }

            if( otherSettings.animateChildrenOnly ) StartCoroutine( AnimateChildrenAlpha() );
            else AnimateAlpha( _image, _spriteRenderer );
        }

        public void AnimateAlpha( Image image = null, SpriteRenderer spriteRenderer = null ) {

            // if( otherSettings.pingPong ) {
            //
            //     if( image != null ) {
            //
            //         image.color = _startColor;
            //
            //         image.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
            //
            //             ( _endColor, _startColor ) = ( _startColor, _endColor );
            //             AnimateAlpha( image, spriteRenderer );
            //         } );
            //     }
            //     if( spriteRenderer != null ) {
            //         
            //         spriteRenderer.color = _startColor;
            //
            //         spriteRenderer.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
            //
            //             ( _endColor, _startColor ) = ( _startColor, _endColor );
            //             AnimateAlpha( image, spriteRenderer );
            //         } );
            //     }
            // } else {
            //
            //     if( image != null ) {
            //
            //         image.color = _startColor;
            //
            //         image.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => onComplete?.Invoke() );
            //     }
            //
            //     if( spriteRenderer != null ) {
            //
            //         spriteRenderer.color = _startColor;
            //
            //         spriteRenderer.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => onComplete?.Invoke() );
            //     }
            // }
        }

        private void OnDisable() {

            if( otherSettings.animateChildrenOnly ) {

                var childImages = GetComponentsInChildren<Image>();
                var childSpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

                foreach( var _t in childImages ) _t.color = _startColor;
                foreach( var _t in childSpriteRenderers ) _t.color = _startColor;
                
                return;
            }

            if( _image != null ) {
        
                _image.color = _startColor;
            }

            if( _spriteRenderer != null ) {
                
                _spriteRenderer.color = _startColor;
            }
        }

        private IEnumerator AnimateChildrenAlpha() {

            // var childImages = GetComponentsInChildren<Image>();
            // var childSpritesRenderers = GetComponentsInChildren<SpriteRenderer>();
            //
            // if( childImages.Length > 0 ) {
            //     
            //     foreach( var image in childImages ) {
            //
            //         if( otherSettings.pingPong ) {
            //
            //             var _color = image.color;
            //             
            //             _startColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.startAlpha
            //             };
            //             _endColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.endAlpha
            //             };
            //             
            //             image.color = _startColor;
            //
            //             var tween = image.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
            //
            //                 ( _endColor, _startColor ) = ( _startColor, _endColor );
            //                 AnimateAlpha();
            //             } );
            //
            //             yield return tween.WaitForCompletion();
            //
            //         } else {
            //
            //             var _color = image.color;
            //             
            //             _startColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.startAlpha
            //             };
            //             _endColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.endAlpha
            //             };
            //             
            //             image.color = _startColor;
            //
            //             var tween = image.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => onComplete?.Invoke() );
            //
            //             yield return tween.WaitForCompletion();
            //         }
            //     }
            // }
            //
            // if( childSpritesRenderers.Length > 0 ) {
            //     
            //     foreach( var _renderer in childSpritesRenderers ) {
            //
            //         if( otherSettings.pingPong ) {
            //
            //             var _color = _renderer.color;
            //             
            //             _startColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.startAlpha
            //             };
            //             _endColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.endAlpha
            //             };
            //             
            //             _renderer.color = _startColor;
            //
            //             var tween = _renderer.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
            //
            //                 ( _endColor, _startColor ) = ( _startColor, _endColor );
            //                 AnimateAlpha();
            //             } );
            //
            //             yield return tween.WaitForCompletion();
            //
            //         } else {
            //
            //             var _color = _renderer.color;
            //             
            //             _startColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.startAlpha
            //             };
            //             _endColor = new Color( _color.r, _color.g, _color.b, _color.a ) {
            //                 a = otherSettings.endAlpha
            //             };
            //             
            //             _renderer.color = _startColor;
            //
            //             var tween = _renderer.DOColor( _endColor, otherSettings.animateDuration ).SetEase( otherSettings.easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => onComplete?.Invoke() );
            //
            //             yield return tween.WaitForCompletion();
            //         }
            //     }
            // }
            
            // ★彡[ Remove below code while un-commenting above code ]彡★
            yield return null;
        }

        private IEnumerator WaitForAnimator() {

            onStart?.Invoke();

            yield return new WaitForSeconds( _animator.GetCurrentAnimatorClipInfo( 0 ).Length );
            onComplete?.Invoke();
        }

    }

    [ Serializable ]
    public struct AlphaAnimationSettings {

        public bool animateChildrenOnly;
        public bool pingPong;
        public float startAlpha;
        public float endAlpha;
        [ Range( 0f, 100f ) ] public float animateDuration;
        public Ease easeMode;

    }

}
