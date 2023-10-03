using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Panels {

    public enum PanelType {

        None,
        Gameplay,
        MainMenu,
        LevelFail,
        LevelComplete,
        Settings,
        Exit,
        AdPanel

    }

    public class PanelBase: MonoBehaviour {

        public bool IsActive => gameObject.activeSelf;
        [ReadOnly] public PanelType type;
        public Transform objectToAnimate;

        private static readonly List<PanelBase> Inheritors = new();
        private static readonly Dictionary<PanelType, Type> TypeMap = new() {
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
            }, {
                PanelType.LevelComplete, typeof(LevelCompletePanel)
            }, {
                PanelType.MainMenu, typeof(MainMenuPanel)
            },
        };

        private void OnValidate() {

            if( objectToAnimate == null ) objectToAnimate = transform.GetChild( 1 );
            type = type == PanelType.None ? GetType( GetType() ) : type;
        }

        protected virtual void Awake() {

            Initialize();
        }
        protected virtual void OnDestroy() {

            DeInitialize();
        }

        private void Initialize() {
            if( !Inheritors.Contains( this ) ) Inheritors.Add( this );
            if( type is PanelType.None ) type = GetType( GetType() );
        }
        private void DeInitialize() {
            if( Inheritors.Contains( this ) ) Inheritors.Remove( this );
            if( type is not PanelType.None ) type = PanelType.None;
        }

        private static List<PanelBase> GetInheritors() => Inheritors;

        public static T GetPanelOfType<T>() where T : PanelBase {
            
            var _t = typeof(T);
        
            var objectOfType = Inheritors.FirstOrDefault( inheritor => inheritor.GetType() == _t );
        
            return (T)objectOfType;
        }

        private static PanelType GetType( Type panelType ) {
            foreach( KeyValuePair<PanelType, Type> entry in TypeMap.Where( entry =>
                entry.Value == panelType ) ) {
                return entry.Key;
            }

            throw new ArgumentException( $"Panel type {panelType} not found in the mapping." );
        }

        public virtual void Enable( float delay = 0, Action onAnimationComplete = null ) {

            gameObject.SetActive( true );
            AudioManager.PlaySound();
            GameManager.isGamePlaying = false;
        }

        public virtual void Disable( float delay = 0, Action onAnimationComplete = null ) {

            AudioManager.PlaySound();
            GameManager.isGamePlaying = true;
        }

    }

}
