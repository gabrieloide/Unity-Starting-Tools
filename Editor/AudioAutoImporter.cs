using UnityEditor;
using UnityEngine;
using Code.Scripts.Audio;
using System.IO;
using System.Linq;

namespace Code.Editor
{
    public class AudioAutoImporter : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            bool audioChanged = false;

            foreach (string str in importedAssets)
            {
                if (str.Contains("Resources/Audio/") && (str.EndsWith(".wav") || str.EndsWith(".mp3") || str.EndsWith(".ogg")))
                {
                    audioChanged = true;
                }
            }

            foreach (string str in deletedAssets)
            {
                if (str.Contains("Resources/Audio/")) audioChanged = true;
            }

            if (audioChanged)
            {
                AudioDatabase database = GetDatabase();
                if (database != null)
                {
                    SyncAudios(database);
                }
            }
        }

        private static AudioDatabase GetDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:AudioDatabase");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                return AssetDatabase.LoadAssetAtPath<AudioDatabase>(path);
            }
            return null;
        }

        public static void SyncAudios(AudioDatabase database)
        {
            if (database == null) return;

            string audioFolderPath = "Assets/Resources/Audio";
            if (!AssetDatabase.IsValidFolder(audioFolderPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Audio");
            }

            string dataFolderPath = "Assets/Resources/Audio/Data";
            if (!AssetDatabase.IsValidFolder(dataFolderPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Audio", "Data");
            }

            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { audioFolderPath });
            var currentClipPaths = guids.Select(AssetDatabase.GUIDToAssetPath).ToList();

            // Add new clips
            foreach (string path in currentClipPaths)
            {
                AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
                if (clip != null && !database.audioDataList.Any(d => d != null && d.clip == clip))
                {
                    AudioData newData = ScriptableObject.CreateInstance<AudioData>();
                    newData.id = clip.name;
                    newData.clip = clip;

                    string dataPath = $"{dataFolderPath}/{clip.name}_Data.asset";
                    AssetDatabase.CreateAsset(newData, dataPath);
                    database.audioDataList.Add(newData);
                    Debug.Log($"[AudioImporter] Added new audio: {clip.name}");
                }
            }

            // Remove null or missing clips
            for (int i = database.audioDataList.Count - 1; i >= 0; i--)
            {
                if (database.audioDataList[i] == null || database.audioDataList[i].clip == null)
                {
                    if (database.audioDataList[i] != null)
                    {
                         string path = AssetDatabase.GetAssetPath(database.audioDataList[i]);
                         AssetDatabase.DeleteAsset(path);
                    }
                    database.audioDataList.RemoveAt(i);
                }
            }

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }
    }
}
