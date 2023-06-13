using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Core {

    public class Test : MonoBehaviour {

        [Button]
        public void SaveSettings() {

            DataManager.SaveSettings();
        }

        [Button]
        public void LoadSettings() {

            DataManager.LoadSettings();
        }

        [Button]
        public void DeleteSettings() {
            
            DataManager.DeleteSettings();
        }
    }

}
