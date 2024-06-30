using UnityEngine;

namespace AudioHelper.Core
{
    public static class AudioHelperRuntimeBootstrapper
    {
        const string k_bootstrapperPrefabName = "AudioManagerPersistence";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void BootstrapAudioManager()
        {
            if (AudioHelperConfiguration.Asset.ManagerPersistence == AudioManagerPersistence.RuntimeBootstrap)
            {
                Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load(k_bootstrapperPrefabName)));

                Debug.Log("AudioHelperRuntimeBootstrapper.BootstrapAudioManager: Initialized Persistence.");
            }
        }
    }
}
