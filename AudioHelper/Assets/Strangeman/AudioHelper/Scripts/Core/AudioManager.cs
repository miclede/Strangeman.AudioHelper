using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Strangeman.Utils.Service;

namespace AudioHelper.Core
{
    [DefaultExecutionOrder(-1)]
    public class AudioManager : GlobalMonoService<AudioManager>
    {
        IObjectPool<AudioEmitter> _audioEmitterPool;
        readonly List<AudioEmitter> _activeAudioEmitters = new();
        public readonly Dictionary<AudioData, int> EmitterCounts = new();

        [SerializeField] AudioHelperConfiguration _audioConfig;

        const string k_audioManagerName = "Audio Manager [Service]";

        protected override void Awake()
        {
            if (_audioConfig == null)
            {
                _audioConfig = AudioHelperConfiguration.Asset;
            }

            if (_audioConfig.ManagerPersistence == AudioManagerPersistence.ScenePersistence)
            {
                Debug.Log("AudioManager.Awake: Initialized Persistence.");

                DontDestroyOnLoad(this.gameObject);
            }

            base.Awake();
            gameObject.name = k_audioManagerName;
            InitializePool();
        }

        public bool CanPlayAudio(AudioData data)
        {
            if (EmitterCounts.TryGetValue(data, out var count))
            {
                if (count >= _audioConfig.MaxAudioInstances)
                {
                    return false;
                }
            }
            return true;
        }

        public AudioEmitter Get() => _audioEmitterPool.Get();
        public void ReturnToPool(AudioEmitter emitter) => _audioEmitterPool.Release(emitter);

        private void InitializePool()
        {
            _audioEmitterPool = new ObjectPool<AudioEmitter>(
                CreateAudioEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _audioConfig.CollectionCheck,
                _audioConfig.DefaultCapacity,
                _audioConfig.MaxPoolSize);
        }

        #region Emitter ObjectPool Delegates
        private AudioEmitter CreateAudioEmitter()
        {
            GameObject audioEmitter;

            if (_audioConfig.AudioEmitterPrefab != null)
            {
                audioEmitter = Instantiate(_audioConfig.AudioEmitterPrefab).gameObject;
            }
            else
            {
                audioEmitter = new GameObject("Audio Emitter", typeof(AudioEmitter));
            }

            audioEmitter.gameObject.SetActive(false);
            return audioEmitter.GetComponent<AudioEmitter>();
        }

        private void OnTakeFromPool(AudioEmitter emitter)
        {
            emitter.gameObject.SetActive(true);
            _activeAudioEmitters.Add(emitter);
        }

        private void OnReturnedToPool(AudioEmitter emitter)
        {
            if (EmitterCounts.TryGetValue(emitter.AudioData, out var count))
            {
                EmitterCounts[emitter.AudioData] -= count > 0 ? 1 : 0;
            }

            emitter.gameObject.SetActive(false);
            _activeAudioEmitters.Remove(emitter);
        }

        private void OnDestroyPoolObject(AudioEmitter emitter)
        {
            Destroy(emitter.gameObject);
        }
        #endregion
    }
}
