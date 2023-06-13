using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Extras {

    public static class AnimationsManager {

        public static void AnimateScale( this Transform trans, Vector3 from, Vector3 to, float duration, Ease easeMode = Ease.Linear, bool pingPong = false, Action onStart = null, Action onComplete = null ) {

            trans.localScale = from;
            
            if( pingPong ) {

                PingPongScale( trans, from, to, duration, easeMode );
            } else {
                
                trans.DOScale( to, duration ).SetEase( easeMode ).OnStart( () => onStart?.Invoke() ).OnComplete( () => onComplete?.Invoke() );
            }
        }

        private static void PingPongScale( Transform trans, Vector3 from, Vector3 to, float duration, Ease easeMode ) {

            var sequence = DOTween.Sequence();
            
            sequence.Append( trans.DOScale( to, duration ).SetEase( easeMode ) ).Append( trans.DOScale( from, duration ).SetEase( easeMode ).OnComplete( () => PingPongScale( trans, from, to, duration, easeMode ) ) );
        }

        public static void AnimateAlpha( this GameObject gameObj, float from, float to, float duration, Action onStart = null, Action onComplete = null ) {

            var image = gameObj.GetComponent<Image>();

            if( image == null ) return;

            var _temporaryColor = new Color( image.color.r, image.color.g, image.color.b, image.color.a ) {
                a = from
            };
            image.color = _temporaryColor;
            
            image.DOFade( to, duration ).OnStart( () => onStart?.Invoke() ).OnComplete( () => {
                
                onComplete?.Invoke();
            } );
        }
    }

}
