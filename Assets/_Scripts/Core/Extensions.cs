using UnityEngine;

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
                if( PlayingInUnityEditor ) return new Vector3( Input.mousePosition.x, Input.mousePosition.y, MainCamera.transform.position.magnitude );
                
                return new Vector3( Input.GetTouch( 0 ).position.x, Input.GetTouch( 0 ).position.y, MainCamera.transform.position.magnitude );
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
                if( PlayingInUnityEditor ) return MainCamera.ScreenToWorldPoint( new Vector3( Input.mousePosition.x, Input.mousePosition.y, MainCamera.transform.position.magnitude ) );
                
                return MainCamera.ScreenToWorldPoint( new Vector3( Input.GetTouch( 0 ).position.x, Input.GetTouch( 0 ).position.y, MainCamera.transform.position.magnitude ) );

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



    }

}
