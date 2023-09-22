using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
using Debug = Core.Debug;

namespace Managers {

    public class PerformanceManager: MonoBehaviour {

        public static PerformanceManager Instance { get; private set; }

        public bool showInfo;

        [BoxGroup( "Time Interval", false ), InfoBox( "Wipe data interval is calculated in seconds",  nameof(showInfo))]
        public float wipeDataInterval = 2;
    
        private float _frameRate;

        private void Update() {

            _frameRate += Time.deltaTime;

            if( _frameRate > wipeDataInterval ) {

                Debug.Log( "Wiping data" );
                _frameRate = 0;
                Resources.UnloadUnusedAssets();
                GarbageCollector.CollectIncremental( 1000000 );
            }
        }
    }

}
