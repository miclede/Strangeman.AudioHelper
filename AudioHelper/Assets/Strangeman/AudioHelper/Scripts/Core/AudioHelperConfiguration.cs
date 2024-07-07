using UnityEngine;

namespace AudioHelper.Core
{
    public enum AudioManagerPersistence
    {
        NoPersistence,
        ScenePersistence,
        RuntimeBootstrap
    }
    
    [CreateAssetMenu(menuName = "Audio/Audio Helper Config", fileName = k_audioHelperConfigName)]
    public class AudioHelperConfiguration : ScriptableObject
    {
        [SerializeField] private AudioManagerPersistence _managerPersistence;
        [SerializeField] private AudioEmitter _audioEmitterPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 100;
        [SerializeField] private int _maxAudioInstances = 30;

        public AudioManagerPersistence ManagerPersistence => _managerPersistence;
        public AudioEmitter AudioEmitterPrefab => _audioEmitterPrefab;
        public bool CollectionCheck => _collectionCheck;
        public int DefaultCapacity => _defaultCapacity;
        public int MaxPoolSize => _maxPoolSize;
        public int MaxAudioInstances => _maxAudioInstances;

        private const string k_audioHelperConfigName = "AudioHelperConfig";

        private static AudioHelperConfiguration _asset;
        public static AudioHelperConfiguration Asset
        {
            get
            {
                if (_asset is null)
                {
                    _asset = Resources.Load<AudioHelperConfiguration>(k_audioHelperConfigName);
                }
                return _asset;
            }
        }
    }
}
