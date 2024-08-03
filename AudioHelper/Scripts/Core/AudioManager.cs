using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Strangeman.Utils.Service;
using UnityEngine.SceneManagement;
using System.Linq;

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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += StopSoundOnSceneLoad;
        }

        private void StopSoundOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (!_audioConfig.StopAudioOnSceneLoad)
            {
                return;
            }

            if (_audioConfig.AllowsAudioOnLoad.FirstOrDefault(x => x.SceneName == scene.name) == default)
            {
                return;
            }

            StopAllEmitters();
        }

        protected override void Awake()
        {
            if (_audioConfig is null && AudioHelperConfiguration.Asset != null)
            {
                _audioConfig = AudioHelperConfiguration.Asset;
            }
            else if (AudioHelperConfiguration.Asset is null)
            {
                Debug.LogError($"AudioManager.Awake: No Audio Helper Configuration file found, please run: Tools/Strangeman/Initialize Audio Helper");
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

        public void AddOrUpdateEmitterCount(AudioData audioData)
        {
            EmitterCounts[audioData] = EmitterCounts.TryGetValue(audioData, out var countForData) ? countForData + 1 : 0;
        }

        public void StopAllEmitters() => _activeAudioEmitters.ForEach(x => x.Stop());

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

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= StopSoundOnSceneLoad;
        }

#if UNITY_EDITOR
        public void SetAudioConfigAsset(AudioHelperConfiguration configAsset) => _audioConfig = configAsset;
#endif
    }
}
