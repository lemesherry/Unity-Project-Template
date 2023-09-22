using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FillAnimator: MonoBehaviour {

    [Min( 0 )] public float animationSpeed = 0.1f;

    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationStart;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimating;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationComplete;

    private bool _isUsingSlider;
    private bool _hasCompleted;
    private bool _hasStarted;

    private float _lerpValue;

    private Image _image;
    private Slider _slider;

    private void OnEnable() {

        _isUsingSlider = !TryGetComponent( out _image );
        _slider = GetComponent<Slider>();
        
        ResetAnimator();
    }

    private void Update() {

        if( _hasCompleted ) return;

        if( !_hasStarted ) {

            _hasStarted = true;

            if( _isUsingSlider ) {

                _slider.value = 0;
            } else {

                _image.fillAmount = 0;
            }
            onAnimationStart?.Invoke();
        }

        _lerpValue += Mathf.Lerp( 0, 1, animationSpeed * 0.01f );

        if( _isUsingSlider ) {

            _slider.value = _lerpValue;
        } else {

            _image.fillAmount = _lerpValue;
        }

        onAnimating?.Invoke();

        if( _lerpValue < 1 ) return;

        _hasCompleted = true;
        onAnimationComplete?.Invoke();
        enabled = false;
    }

    private void OnDisable() {

        ResetAnimator();
    }

    public void StartAnimator() {

        enabled = true;
    }

    public void ResetAnimator() {

        _lerpValue = 0;
        _hasCompleted = false;
        _hasStarted = false;
    }

}
