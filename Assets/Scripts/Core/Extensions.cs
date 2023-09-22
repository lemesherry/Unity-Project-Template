using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Core {

    public static class Extensions {

        public static Camera MainCamera => Camera.main;

        private static bool PlayingInUnityEditor {

            get {
            #if UNITY_EDITOR
                return true;
            #else
                    return false;
            #endif
            }
        }

        public static bool IsPointerOverUIElement {

            get => CheckPointerOverUIElement();
        }

        private static bool CheckPointerOverUIElement() {

            var eventSystemRaycastResults = GetEventSystemRaycastResults();

            return eventSystemRaycastResults.Any( curRaycastResult => curRaycastResult.gameObject.layer == 5 );
        }

        private static IEnumerable<RaycastResult> GetEventSystemRaycastResults() {

            var eventData = new PointerEventData( EventSystem.current ) {
                position = MousePosition
            };
            var _raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll( eventData, _raycastResults );

            return _raycastResults;
        }

        public static bool IsMouseDown {
            get {
                if( PlayingInUnityEditor ) return Input.GetMouseButtonDown( 0 );

                if( Input.touchCount <= 0 ) return false;

                return Input.GetTouch( 0 ).phase == TouchPhase.Began;
            }
        }

        public static bool IsMouseMoving {
            get {
                if( PlayingInUnityEditor ) return Input.GetMouseButton( 0 );

                if( Input.touchCount <= 0 ) return false;

                return Input.GetTouch( 0 ).phase == TouchPhase.Moved;
            }
        }

        public static bool IsMouseUp {
            get {
                if( PlayingInUnityEditor ) return Input.GetMouseButtonUp( 0 );

                if( Input.touchCount <= 0 ) return false;

                return Input.GetTouch( 0 ).phase == TouchPhase.Ended;
            }
        }

        private static Vector3 _mouseLastPositionWorldSpace;
        private static Vector3 _currentMousePositionWorldSpace;
        private static Vector3 _mouseLastPosition;
        private static Vector3 _currentMousePosition;

        public static Vector3 MousePosition {

            get {
                if( PlayingInUnityEditor ) return Input.mousePosition;

                return Input.GetTouch( 0 ).position;
            }
        }

        public static Vector3 MouseDelta {

            get {
                if( IsMouseDown ) _mouseLastPosition = MousePosition;

                if( !IsMouseMoving ) return Vector3.zero;

                _currentMousePosition = MousePosition;
                var deltaMousePosition = _currentMousePosition - _mouseLastPosition;
                _mouseLastPosition = _currentMousePosition;

                return deltaMousePosition;
            }
        }

        public static Vector3 MousePositionWorldSpace {

            get {
                if( PlayingInUnityEditor )
                    return MainCamera.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y,
                        MainCamera.transform.position.magnitude ) );

                return MainCamera.ScreenToWorldPoint( new Vector3( Input.GetTouch( 0 ).position.x,
                    Input.GetTouch( 0 ).position.y, MainCamera.transform.position.magnitude ) );

            }
        }

        public static Vector3 MouseDeltaWorldSpace {

            get {
                if( IsMouseDown ) _mouseLastPositionWorldSpace = MousePositionWorldSpace;

                if( !IsMouseMoving ) return Vector3.zero;

                _currentMousePositionWorldSpace = MousePositionWorldSpace;

                var deltaMousePosition = _currentMousePositionWorldSpace - _mouseLastPositionWorldSpace;

                _mouseLastPositionWorldSpace = _currentMousePositionWorldSpace;

                return deltaMousePosition;
            }
        }

        public static int GetActiveChildIndexFromLast( this Transform transform ) {

            for( var i = transform.childCount - 1; i >= 0; i-- ) {

                if( !transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return i;
            }

            return 0;
        }

        public static int GetActiveChildIndexFromFirst( this Transform transform ) {

            for( var i = 0; i < transform.childCount; i++ ) {

                if( !transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return i;
            }

            return 0;
        }

        public static int GetInActiveChildIndexFromLast( this Transform transform ) {

            for( var i = transform.childCount - 1; i >= 0; i-- ) {

                if( transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return i;
            }

            return 0;
        }

        public static int GetInActiveChildIndexFromFirst( this Transform transform ) {

            for( var i = 0; i < transform.childCount; i++ ) {

                if( transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return i;
            }

            return 0;
        }

        public static Transform GetActiveChildFromLast( this Transform transform ) {

            for( var i = transform.childCount - 1; i >= 0; i-- ) {

                if( !transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return transform.GetChild( i );
            }

            return null;
        }

        public static Transform GetActiveChildFromFirst( this Transform transform ) {

            for( var i = 0; i < transform.childCount; i++ ) {

                if( !transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return transform.GetChild( i );
            }

            return null;
        }

        public static Transform GetInActiveChildFromLast( this Transform transform ) {

            for( var i = transform.childCount - 1; i >= 0; i-- ) {

                if( transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return transform.GetChild( i );
            }

            return null;
        }

        public static Transform GetInActiveChildFromFirst( this Transform transform ) {

            for( var i = 0; i < transform.childCount; i++ ) {

                if( transform.GetChild( i ).gameObject.activeInHierarchy ) continue;

                return transform.GetChild( i );
            }

            return null;
        }

        public static List<T> ShuffleElements<T>( this List<T> listToShuffle ) {

            var random = new System.Random();
            var num = listToShuffle.Count;

            while( num > 1 ) {
                num--;
                var randomNum = random.Next( num + 1 );
                (listToShuffle[randomNum], listToShuffle[num]) = (listToShuffle[num], listToShuffle[randomNum]);
            }

            return listToShuffle;
        }

        public static void AddEmptyElements<T>( this List<T> listToAddElement, int elementsToAdd = 1 ) where T : new() {

            for( var i = 0; i < elementsToAdd; i++ ) {
                listToAddElement.Add( new T() );
            }
        }

        public static void AddMultipleSameElements<T>(
            this List<T> listToAddElement, T element, int elementsToAdd = 1
        ) {

            for( var i = 0; i < elementsToAdd; i++ ) {
                listToAddElement.Add( element );
            }
        }

        public static void AddComponentToList<T>( this List<T> listToAddElement, Component objectToAdd )
            where T : Component {

            if( objectToAdd.TryGetComponent( out T component ) ) {

                listToAddElement.Add( component );
            }
        }

        public static void RemoveComponentFromList<T>( this List<T> listToAddElement, Component objectToAdd )
            where T : Component {

            if( objectToAdd.TryGetComponent( out T component ) ) {

                listToAddElement.Remove( component );
            }
        }

        public static void GenerateRandomNumberOrInfiniteLoop(
            this ref int levelNum, int levelsLength, bool haveSequentialLoop
        ) {

            if( levelNum <= 0 ) levelNum = 0;

            if( levelNum < levelsLength ) return;

            if( haveSequentialLoop ) {

                levelNum %= levelsLength;
            } else {

                levelNum = Random.Range( 0, levelsLength );
            }
        }

        public static Vector3 ToVector3( this int value ) {

            return new Vector3( value, value, value );
        }

        public static Vector3 ToVector3( this float value ) {

            return new Vector3( value, value, value );
        }

        public static Vector3 ToVector3( this double value ) {

            return new Vector3( (float)value, (float)value, (float)value );
        }

        public static Vector3 ToVector3( this long value ) {

            return new Vector3( value, value, value );
        }

        public static Vector3 ToVector3( this byte value ) {

            return new Vector3( value, value, value );
        }

        public static T Find<T>( this T[] array, Predicate<T> match ) {
            
            if( array == null ) {
                throw new ArgumentNullException( nameof(array) );
            }
            if( match == null ) {
                throw new ArgumentNullException( nameof(match) );
            }

            foreach( T t in array ) {
                if( match( t ) ) {
                    return t;
                }
            }

            return default(T);
        }

    }

}
