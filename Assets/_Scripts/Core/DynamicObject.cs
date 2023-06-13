using UnityEngine;

namespace Core {

    public class DynamicObject: MonoBehaviour {

        public static DynamicObject Instance { get; private set; }
        
        private void Awake() {
            
            if( Instance != null && Instance != this ) Destroy( gameObject );
            else {
                Instance = this;
                DontDestroyOnLoad( gameObject );
            }
        }
        

    }

}
