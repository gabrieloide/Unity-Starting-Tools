using UnityEditor;
using UnityEngine;
using Code.Scripts.Audio;

namespace Code.Editor
{
    public class AudioEditorWindow : EditorWindow
    {
        private AudioDatabase _database;
        private Vector2 _scrollPosition;

        [MenuItem("Starter Tools/Audio Manager")]
        public static void ShowWindow()
        {
            GetWindow<AudioEditorWindow>("Audio Manager");
        }

        private void OnEnable()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:AudioDatabase");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _database = AssetDatabase.LoadAssetAtPath<AudioDatabase>(path);
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Audio Database Manager", EditorStyles.boldLabel);

            if (_database == null)
            {
                EditorGUILayout.HelpBox("No AudioDatabase found in the project. Please create one.", MessageType.Warning);
                if (GUILayout.Button("Create Audio Database"))
                {
                    CreateDatabase();
                }
                return;
            }

            if (GUILayout.Button("Sync Audios from Resources/Audio"))
            {
                AudioAutoImporter.SyncAudios(_database);
            }

            EditorGUILayout.Space();

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            int indexToRemove = -1;
            for (int i = 0; i < _database.audioDataList.Count; i++)
            {
                AudioData data = _database.audioDataList[i];
                if (data == null) continue;

                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.BeginHorizontal();
                data.id = EditorGUILayout.TextField("ID (Name)", data.id);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    indexToRemove = i;
                }
                EditorGUILayout.EndHorizontal();

                data.clip = (AudioClip)EditorGUILayout.ObjectField("Clip", data.clip, typeof(AudioClip), false);
                data.volume = EditorGUILayout.Slider("Volume", data.volume, 0f, 1f);
                data.pitch = EditorGUILayout.Slider("Pitch", data.pitch, 0.1f, 3f);
                data.loop = EditorGUILayout.Toggle("Loop", data.loop);
                data.audioMixerGroup = (UnityEngine.Audio.AudioMixerGroup)EditorGUILayout.ObjectField("Mixer Group", data.audioMixerGroup, typeof(UnityEngine.Audio.AudioMixerGroup), false);

                EditorGUILayout.EndVertical();
            }

            if (indexToRemove != -1)
            {
                _database.audioDataList.RemoveAt(indexToRemove);
                EditorUtility.SetDirty(_database);
            }

            EditorGUILayout.EndScrollView();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_database);
                AssetDatabase.SaveAssets();
            }
        }

        private void CreateDatabase()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Audio"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "Audio");
            }

            _database = ScriptableObject.CreateInstance<AudioDatabase>();
            AssetDatabase.CreateAsset(_database, "Assets/Resources/Audio/AudioDatabase.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
