#if UNITY_EDITOR
using AudioHelper.Core;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AudioHelper.Editor
{
    /// <summary>
    /// Editor window for setting up the Audio Helper in a Unity project.
    /// </summary>
    public class AudioHelperWizard : EditorWindow
    {
        string _audioEmitterPath = "Packages/com.strangeman.audiohelper/AudioHelper/Prefabs/AudioEmitter.prefab";
        string _audioManagerPath = "Packages/com.strangeman.audiohelper/AudioHelper/Prefabs/AudioManager.prefab";
        string _exampleScenePath = "Packages/com.strangeman.audiohelper/AudioHelper/Example/Scene/AudioHelperExampleScene.unity";

        private static string _projectFolderPath = "";

        /// <summary>
        /// Draws the GUI for the editor window.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Select Project Directory", EditorStyles.boldLabel);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            _projectFolderPath = EditorGUILayout.TextField("Folder Path", _projectFolderPath);

            if (GUILayout.Button("Browse"))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    if (selectedPath.StartsWith(Application.dataPath))
                    {
                        _projectFolderPath = "Assets" + selectedPath.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        DisplayInvalidPathDialog();
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            if (GUILayout.Button("Create Assets", GUILayout.Height(position.height * 0.13f)))
            {
                if (AssetDatabase.IsValidFolder(_projectFolderPath))
                {
                    Debug.Log("Project folder path set to: " + _projectFolderPath);
                    InitializeAudioHelper();

                    if (IsSetupCompleted())
                        Close();
                }
                else
                {
                    DisplayInvalidPathDialog();
                }
            }

            GUILayout.Space(5);

            GUILayout.TextArea("This will create mutable assets and setup starter references for the Audio Helper to use. " +
                "While not necessary, doing this will allow for a greater level of customization, should you desire or require it.");

            GUILayout.Space(10);

            GUILayout.TextArea("If you would like access to the ExampleScene, press the below button. It will be placed within the directory/AudioHelper referenced above.");

            GUILayout.Space(5);

            if (GUILayout.Button("Get Example Scene", GUILayout.Height(position.height * 0.13f)))
            {
                CopyExampleScene();

                if (IsSetupCompleted())
                    Close();
            }
        }

        private static bool IsSetupCompleted()
        {
            if (string.IsNullOrEmpty(_projectFolderPath))
                return false;

            if (AssetDatabase.LoadAssetAtPath<AudioHelperConfiguration>($"{_projectFolderPath}/AudioHelper/Resources/{AudioHelperConfiguration.k_audioHelperConfigName}.asset")
                && AssetDatabase.LoadAssetAtPath<SceneAsset>($"{_projectFolderPath}/AudioHelper/Example/AudioHelperExampleScene.unity"))
            {
                EditorUtility.DisplayDialog("Setup Complete", "All assets have been found at their relative paths, should you wish to run this wizard again, please delete created assets.", "OK");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the example scene to the specified project path.
        /// </summary>
        private void CopyExampleScene()
        {
            if (!_projectFolderPath.StartsWith("Assets"))
            {
                DisplayInvalidPathDialog();
                return;
            }

            string audioHelperPath = _projectFolderPath + "/AudioHelper";
            string destinationPath = audioHelperPath + "/Example";

            if (!AssetDatabase.IsValidFolder(audioHelperPath))
            {
                AssetDatabase.CreateFolder(_projectFolderPath, "AudioHelper");
            }

            if (!AssetDatabase.IsValidFolder(destinationPath))
            {
                AssetDatabase.CreateFolder(_projectFolderPath + "/AudioHelper", "Example");
            }

            string destinationScenePath = destinationPath + "/AudioHelperExampleScene.unity";

            bool success = AssetDatabase.CopyAsset(_exampleScenePath, destinationScenePath);

            if (success)
            {
                Debug.Log("Example scene copied successfully to: " + destinationScenePath);
            }
            else
            {
                Debug.LogError("Failed to copy the example scene.");
            }
        }

        /// <summary>
        /// Displays a dialog indicating the path is invalid.
        /// </summary>
        private static void DisplayInvalidPathDialog() => EditorUtility.DisplayDialog("Invalid Path", "The specified path is not a valid folder within the Assets directory or Assets itself.", "OK");

        /// <summary>
        /// Initializes the Audio Helper by creating necessary folders and assets.
        /// </summary>
        private void InitializeAudioHelper()
        {
            string destinationPath = _projectFolderPath + "/AudioHelper";
            string destinationResources = destinationPath + "/Resources";

            if (!AssetDatabase.IsValidFolder(destinationPath))
            {
                AssetDatabase.CreateFolder(_projectFolderPath, "AudioHelper");
            }

            if (!AssetDatabase.IsValidFolder(destinationResources))
            {
                AssetDatabase.CreateFolder(destinationPath, "Resources");
            }

            CopyPrefab(_audioEmitterPath, destinationPath + "/AudioEmitter.prefab");
            CopyPrefab(_audioManagerPath, destinationPath + "/AudioManager.prefab");

            CreateAudioConfigAsset(destinationResources);
            SetupReferences();
        }

        /// <summary>
        /// Sets up references for the Audio Helper.
        /// </summary>
        private void SetupReferences()
        {
            string emitterPrefabPath = _projectFolderPath + "/AudioHelper/AudioEmitter.prefab";
            string audioManagerPrefabPath = _projectFolderPath + "/AudioHelper/AudioManager.prefab";

            var emitterPrefab = AssetDatabase.LoadAssetAtPath<AudioEmitter>(emitterPrefabPath);
            var audioManagerPrefab = AssetDatabase.LoadAssetAtPath<AudioManager>(audioManagerPrefabPath);

            var audioConfig = AudioHelperConfiguration.Asset;

            var audioManager = audioManagerPrefab.GetComponent<AudioManager>();

            audioManager.SetAudioConfigAsset(audioConfig);

            audioConfig.SetAudioEmitterPrefab(emitterPrefab);
            audioConfig.SetManagerPersistencePrefab(audioManagerPrefab.gameObject);
        }

        /// <summary>
        /// Creates the Audio Helper configuration asset.
        /// </summary>
        /// <param name="path">The path to create the asset at.</param>
        private void CreateAudioConfigAsset(string path)
        {
            AudioHelperConfiguration asset = CreateInstance<AudioHelperConfiguration>();

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + AudioHelperConfiguration.k_audioHelperConfigName + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log("AudioHelperConfiguration ScriptableObject created successfully at: " + assetPathAndName);
        }

        /// <summary>
        /// Copies a prefab to a specified destination path.
        /// </summary>
        /// <param name="sourcePath">The source path of the prefab.</param>
        /// <param name="destinationPath">The destination path for the prefab.</param>
        private void CopyPrefab(string sourcePath, string destinationPath)
        {
            if (AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath) != null)
            {
                bool success = AssetDatabase.CopyAsset(sourcePath, destinationPath);

                if (success)
                {
                    Debug.Log("Prefab copied successfully to: " + destinationPath);
                }
                else
                {
                    Debug.LogError("Failed to copy the prefab.");
                }
            }
            else
            {
                Debug.LogError("Source prefab not found at: " + sourcePath);
            }
        }

        /// <summary>
        /// Opens the Audio Helper setup wizard.
        /// </summary>
        [MenuItem("Tools/Strangeman/Setup Audio Helper")]
        private static void OpenInstructionWindow()
        {
            if (IsSetupCompleted())
                return;

            if (AssetDatabase.IsValidFolder("Packages/com.strangeman.audiohelper/"))
            {
                Debug.Log("Package found, proceeding with helper setup.");
            }
            else
            {
                Debug.Log("This does not include a package installation, cannot continue setup.");
                return;
            }

            AudioHelperWizard window = (AudioHelperWizard)GetWindow(typeof(AudioHelperWizard));
            window.titleContent = new GUIContent("Audio Setup Wizard");

            window.minSize = new Vector2(450, 205);
            window.maxSize = new Vector2(451, 206);

            window.ShowUtility();
        }
    }
}
#endif