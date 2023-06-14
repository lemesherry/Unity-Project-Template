using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core {

    public static class DataManager {
        
        public static SettingsData settingsData;
        public static GameData gameData;
        
        private const string SettingsFileName = "savedSettings.sherry";
        private const string GameDataFileName = "saveGames.sherry";
        
        private static readonly string SavedGameFilesPath = Path.Combine( Application.persistentDataPath, GameDataFileName );
        private static readonly string SavedSettingsFilesPath = Path.Combine( Application.persistentDataPath, SettingsFileName );

        /// <summary>This function is used to initialize data everytime game starts.</summary>
        public static void InitializeData() {

            gameData = LoadData();
            settingsData = LoadSettings();
        }

        /// <summary>This function is used to save game data to a file.</summary>
        public static void SaveData() {
            
            var formatter = new BinaryFormatter();

            if( File.Exists( SavedGameFilesPath ) ) {
                
                using var fileStream = File.Open( SavedGameFilesPath, FileMode.Create );
                formatter.Serialize( fileStream, gameData );
                fileStream.Close();
                Debugger.Log( $"File already exists: {SavedGameFilesPath}" );
                Debugger.Log( $"Saved data to: {SavedGameFilesPath}" );

            } else {
                
                using var fileStream = File.Create( SavedGameFilesPath );
                formatter.Serialize( fileStream, gameData );
                fileStream.Close();
                Debugger.Log( $"Created file: {SavedGameFilesPath}" );
                Debugger.Log( $"Saved data to: {SavedGameFilesPath}" );
            }
        }

        /// <summary>This function is used to load game data from the saved file</summary>
        public static GameData LoadData() {

            if( !File.Exists( SavedGameFilesPath ) ) {

                Debugger.Log( $"No file found with name: {SavedGameFilesPath}", LogSeverity.High );
                return GameData.CreateDefault();
            }
            
            var formatter = new BinaryFormatter();
            
            using var fileStream = File.Open( SavedGameFilesPath, FileMode.Open );
            var loadedObject = (GameData)formatter.Deserialize( fileStream );
            fileStream.Close();

            Debugger.Log( $"Loaded data from: {SavedGameFilesPath}" );
            gameData = loadedObject;
            return loadedObject;
        }

        /// <summary>This function is used to delete game data file</summary>
        public static void DeleteGameData() {
            
            File.Delete( SavedGameFilesPath );
            Debugger.Log( $"Deleted File: {SavedGameFilesPath}", LogSeverity.Critical );
        }

        /// <summary>This function is used to save settings data to a file</summary>
        public static void SaveSettings() {
            
            var formatter = new BinaryFormatter();
            
            if( File.Exists( SavedSettingsFilesPath ) ) {
                
                using var fileStream = File.Open( SavedSettingsFilesPath, FileMode.Create );
                formatter.Serialize( fileStream, settingsData );
                fileStream.Close();
                Debugger.Log( $"File already exists: {SavedSettingsFilesPath}" );
                Debugger.Log( $"Saved data to: {SavedSettingsFilesPath}" );

            } else {
                
                using var fileStream = File.Create( SavedSettingsFilesPath );
                formatter.Serialize( fileStream, settingsData );
                fileStream.Close();
                Debugger.Log( $"Created file: {SavedSettingsFilesPath}" );
                Debugger.Log( $"Saved data to: {SavedSettingsFilesPath}" );
            }
        }
        
        /// <summary>This function is used to load settings data from the saved file</summary>
        public static SettingsData LoadSettings() {
            
            if( !File.Exists( SavedSettingsFilesPath ) ) {

                Debugger.Log( $"No file found with name: {SavedSettingsFilesPath}", LogSeverity.High );
                return SettingsData.CreateDefault();
            }
            
            var formatter = new BinaryFormatter();
            
            using var fileStream = File.Open( SavedSettingsFilesPath, FileMode.Open );
            var loadedObject = (SettingsData)formatter.Deserialize( fileStream );
            fileStream.Close();

            Debugger.Log( $"Loaded data from: {SavedSettingsFilesPath}" );
            settingsData = loadedObject;
            return loadedObject;
        }

        /// <summary>This function is used to delete settings data file</summary>
        public static void DeleteSettings() {
            
            File.Delete( SavedSettingsFilesPath );
            Debugger.Log( $"Deleted File: {SavedSettingsFilesPath}", LogSeverity.Critical );
        }

    }

    /// <summary>GameData class used for saving/loading game data</summary>
    /// <warning>To this struct with default values do not create new object, instead use <see cref="CreateDefault"/> function.</warning>
    [Serializable]
    public struct GameData {

        [field: SerializeField] public int CurrentLevel { get; set; }
        [field: SerializeField] public int TutorialLevel { get; set; }
        [field: SerializeField] public bool IsTutorialPlayed { get; set; }

        /// <returns>A new struct with default values.</returns>
        public static GameData CreateDefault() => new( true );

        private GameData( bool resetValues ) {

            CurrentLevel = 1;
            TutorialLevel = 1;
            IsTutorialPlayed = false;
        }
    }

    /// <summary>SettingsData struct used for saving/loading settings data</summary>
    /// <warning>To this struct with default values do not create new object, instead use <see cref="CreateDefault"/> function.</warning>
    [Serializable]
    public struct SettingsData {

        [field: SerializeField] public bool SoundOn { get; set; }
        [field: SerializeField] public bool MusicOn { get; set; }
        [field: SerializeField] public bool HapticsOn { get; set; }
        [field: SerializeField] public bool VisualEffectsOn { get; set; }
        [field: SerializeField] public bool RestoredPurchases { get; set; }
        [field: SerializeField] public bool RemovedAds { get; set; }
        
        /// <returns>A new struct with default values.</returns>
        public static SettingsData CreateDefault() => new( true );
        
        private SettingsData( bool resetValues ) {

            SoundOn = true;
            MusicOn = true;
            HapticsOn = true;
            VisualEffectsOn = true;
            RestoredPurchases = false;
            RemovedAds = false;
        }

    }

}
