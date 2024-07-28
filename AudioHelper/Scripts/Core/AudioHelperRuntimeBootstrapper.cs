using UnityEngine;

namespace AudioHelper.Core
{
    public static class AudioHelperRuntimeBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void BootstrapAudioManager()
        {
            AudioHelperConfiguration config = AudioHelperConfiguration.Asset;

            if (config is null)
            {
                Debug.LogError("AudioHelperRuntimeBootstrapper.BootstrapAudioManager: No Audio Helper Configuration file found, please run: Tools/Strangeman/Initialize Audio Helper");
                return;
            }

            if (config.ManagerPersistence is AudioManagerPersistence.RuntimeBootstrap)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(config.AudioManagerPersistencePrefab));

                Debug.Log("AudioHelperRuntimeBootstrapper.BootstrapAudioManager: Initialized Persistence.");
            }
        }
    }
}
