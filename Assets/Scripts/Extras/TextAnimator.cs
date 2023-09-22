using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextAnimator: MonoBehaviour {

    [SerializeField] private bool looping;
    [SerializeField] private bool doAppend;
    [SerializeField] private AnimationCurve curve;
    [Min( 0 )] public float animationSpeed = 1f;
    [Min( 0 )] public float startingDelay = 1f;
    public string textToAnimate = "Your text goes here";

    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationStart;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimating;
    [FoldoutGroup( "Events" )]
    public UnityEvent onAnimationComplete;

    private TextMeshProUGUI _textField;
    private float _animationTime;
    private bool _hasCompleted;
    private bool _hasStarted;

    private float _currentTime;
    private bool _canUpdateDelayTime = true;
    private int _currentIndex;
    private string _defaultTextString;

    private void Start() {

        _textField = GetComponent<TextMeshProUGUI>();
        _defaultTextString = _textField.text;
    }

    private void OnEnable() {

        _textField = GetComponent<TextMeshProUGUI>();
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
        }

        _animationTime += Time.deltaTime * animationSpeed;

        _animationTime = Mathf.Clamp( _animationTime, curve[0].time, curve[curve.length - 1].time );

        float curveValue = curve.Evaluate( _animationTime );

        int endIndex = Mathf.FloorToInt( textToAnimate.Length * curveValue );
        
        if( endIndex != _currentIndex ) {
            _currentIndex = endIndex;

            if( doAppend ) {

                _textField.text = _defaultTextString + textToAnimate.Substring( 0, _currentIndex );
            } else {

                _textField.text = textToAnimate.Substring( 0, _currentIndex );
            }
        }

        if( _animationTime < curve[curve.length - 1].time ) return;

        if( looping ) {

            _animationTime = 0;
            _currentIndex = 0;

            return;
        }

        _hasCompleted = true;
        enabled = false;
    }

    public void StartAnimator() {
        enabled = true;
    }

    public void ResetAnimator() {

        _animationTime = 0;
        _currentTime = 0;
        _currentIndex = 0;
        _canUpdateDelayTime = true;
        _hasCompleted = false;
        _hasStarted = false;
    }

}
