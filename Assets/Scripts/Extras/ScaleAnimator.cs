using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ScaleAnimator: MonoBehaviour {

    [SerializeField] private AnimationCurve curve;
    [Min( 0 ), SerializeField] private float animationSpeed;
    [Min( 0 ), SerializeField] private float startingDelay;

    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationStart;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimating;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationComplete;

    private float _animationTime;
    private bool _hasCompleted;
    private bool _hasStarted;

    private float _currentTime;
    private bool _canUpdateDelayTime = true;

    private void OnEnable() {

        ResetAnimator();
    }

    private void Update() {

        if( _canUpdateDelayTime ) {

            _currentTime += Time.deltaTime;
            if( _currentTime < startingDelay ) return;
        }

        _canUpdateDelayTime = false;

        if( _hasCompleted || curve.length <= 0 ) return;

        if( !_hasStarted ) {

            _hasStarted = true;
            onAnimationStart?.Invoke();
        }

        _animationTime += Time.deltaTime * animationSpeed;

        _animationTime = Mathf.Clamp( _animationTime, curve[0].time, curve[curve.length - 1].time );

        float curveValue = curve.Evaluate( _animationTime );

        var newScale = new Vector3( curveValue, curveValue, curveValue );
        transform.localScale = newScale;

        onAnimating?.Invoke();

        if( _animationTime < curve[curve.length - 1].time ) return;

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

        _animationTime = 0;
        _currentTime = 0;
        _canUpdateDelayTime = true;
        _hasCompleted = false;
        _hasStarted = false;
    }

}
