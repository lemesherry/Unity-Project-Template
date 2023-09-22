using Sirenix.OdinInspector;
using UnityEngine;

namespace Core {

    public class Test: MonoBehaviour {

        [Button, FoldoutGroup( "Settings" )]
        public void SaveSettings() => DataManager.SaveSettings();

        [Button, FoldoutGroup( "Settings" )]
        public void LoadSettings() => DataManager.LoadSettings();

        [Button, FoldoutGroup( "Settings" )]
        public void DeleteSettings() => DataManager.DeleteSettings();

        [Button, FoldoutGroup( "Game Data" )]
        public void SaveGameData() => DataManager.SaveGameData();

        [Button, FoldoutGroup( "Game Data" )]
        public void LoadGameData() => DataManager.LoadGameData();

        [Button, FoldoutGroup( "Game Data" )]
        public void DeleteGameData() => DataManager.DeleteGameData();

        [Button, FoldoutGroup( "All" )]
        public void SaveAll() {

            DataManager.SaveSettings();
            DataManager.SaveGameData();
        }

        [Button, FoldoutGroup( "All" )]
        public void LoadAll() {

            DataManager.LoadGameData();
            DataManager.LoadSettings();
        }

        [Button, FoldoutGroup( "All" )]
        public void DeleteAll() {

            DataManager.DeleteGameData();
            DataManager.DeleteSettings();
        }

    }

}
