using UnityEngine;

namespace Managers {

    public class DynamicObjectManager: MonoBehaviour {

        public static DynamicObjectManager Instance { get; private set; }
        
        private void Awake() {
            
            if( Instance != null && Instance != this ) Destroy( gameObject );
            else {
                Instance = this;
                DontDestroyOnLoad( gameObject );
            }
        }
        

    }

}
