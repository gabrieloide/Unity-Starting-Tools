using System.IO;
using UnityEngine;

namespace Code.Scripts.Save
{
    public static class SaveSystem
    {
        private static string GetFilePath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }

        public static void SaveData<T>(string fileName, T data)
        {
            string json = JsonUtility.ToJson(data, true);
            string path = GetFilePath(fileName);

            try
            {
                File.WriteAllText(path, json);
                Debug.Log($"[SaveSystem] Saved data to {path}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SaveSystem] Error saving data: {e.Message}");
            }
        }

        public static T LoadData<T>(string fileName) where T : new()
        {
            string path = GetFilePath(fileName);

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    return JsonUtility.FromJson<T>(json);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[SaveSystem] Error loading data: {e.Message}");
                    return new T();
                }
            }

            Debug.Log($"[SaveSystem] Save file not found at {path}, returning default data.");
            return new T();
        }

        public static void DeleteSave(string fileName)
        {
            string path = GetFilePath(fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[SaveSystem] Deleted save file at {path}");
            }
        }
        
        public static bool SaveExists(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }
    }
}
