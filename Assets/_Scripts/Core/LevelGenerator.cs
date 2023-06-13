using System;
using Object = UnityEngine.Object;

namespace Core {

    public static class LevelGenerator {

        public static void LoadNewLevel( Action<bool> onComplete ) {

            try {

                DestroyCurrentLevel();
                
                var levelNum = DataManager.gameData.CurrentLevel;
                
                if( levelNum >= GameManager.Instance.levelPrefabs.Length ) {

                    if( GameManager.Instance.testing.sequentialLoop ) {
                        
                        levelNum %= GameManager.Instance.levelPrefabs.Length;
                    } else {

                        levelNum = UnityEngine.Random.Range( 0, GameManager.Instance.levelPrefabs.Length );
                    }
                }

                GameManager.currentLevel = Object.Instantiate( GameManager.Instance.levelPrefabs[levelNum], GameManager.Instance.levelSpawner.transform );

                onComplete( true );
                
            } catch( Exception e ) {
                Console.WriteLine( e );
                onComplete( false );
            }
        }

        private static void DestroyCurrentLevel() {
            
            Object.Destroy( GameManager.currentLevel );
            GC.Collect();
            GameManager.currentLevel = null;
        }
    }

}
