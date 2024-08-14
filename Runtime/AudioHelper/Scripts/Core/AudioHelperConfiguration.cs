using Strangeman.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AudioHelper.Core
{
    public enum AudioManagerPersistence
    {
        NoPersistence,
        ScenePersistence,
        RuntimeBootstrap
    }
    
    //Create from Tools/Strangeman/Audio Helper Wizard
    //[CreateAssetMenu(menuName = "Audio/Audio Helper Config", fileName = k_audioHelperConfigName)]
    public class AudioHelperConfiguration : ScriptableObject
    {
        [SerializeField] private AudioManagerPersistence _managerPersistence;
        [SerializeField] private GameObject _audioManagerPersistencePrefab;
        [SerializeField] private AudioEmitter _audioEmitterPrefab;
        [SerializeField] private bool _collectionCheck = true;
        [SerializeField] private int _defaultCapacity = 10;
        [SerializeField] private int _maxPoolSize = 100;
        [SerializeField] private int _maxAudioInstances = 30;
        [SerializeField] private bool _stopAudioOnSceneLoad = true;
        [SerializeField] private List<SceneField> _allowsAudioOnLoad = new();

        public AudioManagerPersistence ManagerPersistence => _managerPersistence;
        public GameObject AudioManagerPersistencePrefab => _audioManagerPersistencePrefab;
        public AudioEmitter AudioEmitterPrefab => _audioEmitterPrefab;
        public bool CollectionCheck => _collectionCheck;
        public int DefaultCapacity => _defaultCapacity;
        public int MaxPoolSize => _maxPoolSize;
        public int MaxAudioInstances => _maxAudioInstances;
        public bool StopAudioOnSceneLoad => _stopAudioOnSceneLoad;
        public List<SceneField> AllowsAudioOnLoad => _allowsAudioOnLoad;

        public const string k_audioHelperConfigName = "AudioHelperConfig";

        private static AudioHelperConfiguration _asset;
        public static AudioHelperConfiguration Asset
        {
            get
            {
                if (_asset is null)
                {
                    var assetAtPath = Resources.Load<AudioHelperConfiguration>(k_audioHelperConfigName);

                    _asset = assetAtPath != null ? assetAtPath : throw new NullReferenceException("AudioHelperConfiguration.Asset: no asset in Resources folder, please create.");
                }
                return _asset;
            }
        }

#if UNITY_EDITOR
        public void SetManagerPersistencePrefab(GameObject persistencePrefab) => _audioManagerPersistencePrefab = persistencePrefab;
        public void SetAudioEmitterPrefab(AudioEmitter emitterPrefab) => _audioEmitterPrefab = emitterPrefab;
#endif
    }
}
