using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Core {

    public static class DataManager {

        /// <summary>
        /// Just change these two variables data below according to your game's preference and everything else will be done for you automatically.
        /// </summary>
        public static SettingsData settingsData;
        public static GameData gameData;

        private const string SettingsFileName = "savedSettings.sherry";
        private const string GameDataFileName = "saveGames.sherry";

        private static readonly string SavedGameFilesPath =
            Path.Combine( Application.persistentDataPath, GameDataFileName );
        private static readonly string SavedSettingsFilesPath =
            Path.Combine( Application.persistentDataPath, SettingsFileName );

        /// <summary>This function is used to initialize data everytime game starts.</summary>
        public static void InitializeData() {

            gameData = LoadGameData();
            settingsData = LoadSettings();
            gameData.LogsSessionsCount++;
            SaveGameData();
        }

        /// <summary>This function is used to save game data to a file.</summary>
        public static void SaveGameData() {

            var formatter = new BinaryFormatter();

            if( File.Exists( SavedGameFilesPath ) ) {

                using var fileStream = File.Open( SavedGameFilesPath, FileMode.Create );
                formatter.Serialize( fileStream, gameData );
                fileStream.Close();
                Debug.Log( $"File already exists, Saved data to: {SavedGameFilesPath}" );

            } else {

                using var fileStream = File.Create( SavedGameFilesPath );
                formatter.Serialize( fileStream, gameData );
                fileStream.Close();
                Debug.Log( $"Created file & Saved data to: {SavedGameFilesPath}" );
            }
        }

        /// <summary>This function is used to load game data from the saved file</summary>
        public static GameData LoadGameData() {

            if( !File.Exists( SavedGameFilesPath ) ) {

                Debug.Log( $"No file found with name: {SavedGameFilesPath}", LogSeverity.High );
                var newData = GameData.CreateDefault();
                SaveGameData();

                return newData;
            }

            var formatter = new BinaryFormatter();

            using var fileStream = File.Open( SavedGameFilesPath, FileMode.Open );
            var loadedObject = (GameData)formatter.Deserialize( fileStream );
            fileStream.Close();

            Debug.Log( $"Loaded data from: {SavedGameFilesPath}" );
            gameData = loadedObject;

            return loadedObject;
        }

        /// <summary>This function is used to delete game data file</summary>
        public static void DeleteGameData() {

            File.Delete( SavedGameFilesPath );
            Debug.Log( $"Deleted File: {SavedGameFilesPath}", LogSeverity.Critical );
        }

        /// <summary>This function is used to save settings data to a file</summary>
        public static void SaveSettings() {

            var formatter = new BinaryFormatter();

            if( File.Exists( SavedSettingsFilesPath ) ) {

                using var fileStream = File.Open( SavedSettingsFilesPath, FileMode.Create );
                formatter.Serialize( fileStream, settingsData );
                fileStream.Close();
                Debug.Log( $"File already exists, Saved data to: {SavedSettingsFilesPath}" );

            } else {

                using var fileStream = File.Create( SavedSettingsFilesPath );
                formatter.Serialize( fileStream, settingsData );
                fileStream.Close();
                Debug.Log( $"Created file & Saved data to: {SavedSettingsFilesPath}" );
            }
        }

        /// <summary>This function is used to load settings data from the saved file</summary>
        public static SettingsData LoadSettings() {

            if( !File.Exists( SavedSettingsFilesPath ) ) {

                Debug.Log( $"No file found with name: {SavedSettingsFilesPath}", LogSeverity.High );
                var newData = SettingsData.CreateDefault();
                SaveSettings();

                return newData;
            }

            var formatter = new BinaryFormatter();

            using var fileStream = File.Open( SavedSettingsFilesPath, FileMode.Open );
            var loadedObject = (SettingsData)formatter.Deserialize( fileStream );
            fileStream.Close();

            Debug.Log( $"Loaded data from: {SavedSettingsFilesPath}" );
            settingsData = loadedObject;

            return loadedObject;
        }

        /// <summary>This function is used to delete settings data file</summary>
        public static void DeleteSettings() {

            File.Delete( SavedSettingsFilesPath );
            Debug.Log( $"Deleted File: {SavedSettingsFilesPath}", LogSeverity.Critical );
        }

    }

    /// <summary>GameData class used for saving/loading game data</summary>
    /// <warning>To this struct with default values do not create new object, instead use <see cref="CreateDefault"/> function.</warning>
    [Serializable]
    public struct GameData {

        [field: SerializeField] public int CurrentLevel { get; set; }
        [field: SerializeField] public int TutorialLevel { get; set; }
        [field: SerializeField] public bool IsTutorialPlayed { get; set; }
        [field: SerializeField] public int LogsSessionsCount { get; set; }
        [field: SerializeField] public long HighScore { get; set; }
        [field: SerializeField] public PowerUps PowerUps { get; set; }

        /// <returns>A new struct with default values.</returns>
        public static GameData CreateDefault() => new( true );

        private GameData( bool resetValues ) {

            CurrentLevel = 1;
            TutorialLevel = 1;
            IsTutorialPlayed = false;
            LogsSessionsCount = 0;
            HighScore = 0;
            PowerUps = new PowerUps();
        }

    }

    [Serializable]
    public struct PowerUps {

        public int powerUp1Count;
        public int powerUp2Count;
        public int powerUp3Count;
        public int powerUp4Count;
        public int powerUp5Count;

    }

    /// <summary>SettingsData struct used for saving/loading settings data</summary>
    /// <warning>To this struct with default values do not create new object, instead use <see cref="CreateDefault"/> function.</warning>
    [Serializable]
    public struct SettingsData {

        [field: SerializeField] public bool SoundOn { get; set; }
        [field: SerializeField] public bool MusicOn { get; set; }
        [field: SerializeField] public bool VibrationOn { get; set; }
        [field: SerializeField] public bool VisualEffectsOn { get; set; }
        [field: SerializeField] public bool RestoredPurchases { get; set; }
        [field: SerializeField] public bool RemovedAds { get; set; }

        /// <returns>A new struct with default values.</returns>
        public static SettingsData CreateDefault() => new( true );

        private SettingsData( bool resetValues ) {

            SoundOn = true;
            MusicOn = true;
            VibrationOn = true;
            VisualEffectsOn = true;
            RestoredPurchases = false;
            RemovedAds = false;
        }

    }

}
