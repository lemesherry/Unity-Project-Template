using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = Core.Debug;

namespace UI.Panels {

    public enum PanelType {

        Gameplay,
        MainMenu,
        LevelFail,
        LevelComplete,
        Settings,
        Exit,
        AdPanel

    }

    public class Panel: MonoBehaviour {

        public bool IsActive => gameObject.activeSelf;
        public PanelType type;
        public Transform objectToAnimate;

        private static readonly List<Panel> Inheritors = new();
        private static readonly Dictionary<PanelType, Type> PanelTypeToPanelMap = new() {
            {
                PanelType.AdPanel, typeof(AdPanel)
            }, {
                PanelType.Gameplay, typeof(GameplayPanel)
            }, {
                PanelType.Settings, typeof(SettingsPanel)
            }, {
                PanelType.Exit, typeof(ExitPanel)
            }, {
                PanelType.LevelFail, typeof(LevelFailPanel)
            },
        };

        private void OnValidate() {

            if( objectToAnimate == null ) objectToAnimate = transform.GetChild( 1 );
        }

        protected virtual void Awake() {

            Initialize();
        }
        protected virtual void OnDestroy() {

            DeInitialize();
        }

        protected virtual void Initialize() {
            if( !Inheritors.Contains( this ) ) Inheritors.Add( this );
        }
        protected virtual void DeInitialize() {
            if( Inheritors.Contains( this ) ) Inheritors.Remove( this );
        }

        private static List<Panel> GetInheritors() => Inheritors;

        public static T GetPanelOfType<T>() where T : Panel {
            var panelType = GetPanelTypeForType( typeof(T) );

            var panel = GetInheritors().FirstOrDefault( inheritor => inheritor.type == panelType );

            return (T)panel;
        }

        public static Panel GetMainPanelOfType<T>() where T : Panel {
            var panelType = GetPanelTypeForType( typeof(T) );

            return GetInheritors().FirstOrDefault( inheritor => inheritor.type == panelType );
        }

        private static PanelType GetPanelTypeForType( Type panelType ) {
            foreach( KeyValuePair<PanelType, Type> entry in PanelTypeToPanelMap.Where(
                entry => entry.Value == panelType ) ) {
                return entry.Key;
            }

            throw new ArgumentException( $"Panel type {panelType} not found in the mapping." );
        }

        public virtual void Enable( Action onAnimationComplete = null ) {

            gameObject.SetActive( true );
            AudioManager.PlaySound();
            GameManager.isGamePlaying = false;
        }

        public virtual void Disable( Action onAnimationComplete = null ) {

            AudioManager.PlaySound();
            GameManager.isGamePlaying = true;
        }

    }

}
