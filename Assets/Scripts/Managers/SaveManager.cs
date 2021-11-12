using Runner.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Runner.Managers
{
    public static class SaveManager
    {

        public static GameSave CurrentSave;

        private static string FilePath = $"{Application.persistentDataPath}/GameSave.bytes";

        public static void LoadSave()
        {
            if (!File.Exists(FilePath))
            {
                Debug.Log($"Save file does not exsist: {FilePath}");
                CurrentSave = new GameSave();
                return;
            }

            GameSave data = null;
            FileStream fs = new FileStream(FilePath, FileMode.Open);
            fs.Position = 0;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                data = formatter.Deserialize(fs) as GameSave;
            }
            catch (SerializationException e)
            {
                Debug.LogError($"Failed to deserialize save file: {e.Message}");
            }
            finally
            {
                fs.Close();
            }

            CurrentSave = data ?? new GameSave();

            Debug.Log($"[SaveManager]: Loadded save: {CurrentSave.ToString()}; From: {FilePath}");
        }

        public static void SaveCurrent()
        {
            FileStream fs = new FileStream(FilePath, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {
                formatter.Serialize(fs, CurrentSave);
            }
            catch (SerializationException e)
            {
                Debug.LogError($"Failed to serialize save file: {e.Message}");
            }
            finally
            {
                fs.Close();
            }
        }

    }

    [Serializable]
    public class GameSave
    {
        public int coins = 0;
        public int highScore = 0;

        // Strings are names, so they have to be unique
        public List<string> boughtItems = new List<string>();
        public string selectedItem;
    }
}