using System.Collections;
using UnityEngine;
using Strangeman.Utils.Service;
using Strangeman.Utils.Extensions;

namespace AudioHelper.Core
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AudioEmitter : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;

        Coroutine _playingCoroutine;

        AudioManager _audioManager;
        public AudioData AudioData { get; private set; }

        private void Awake()
        {
            _audioSource.GetOrAdd(gameObject, out _audioSource);
        }

        private void Start()
        {
            ServiceLocator.For(this, LocatorConfiguration.Global).GetMonoService(out _audioManager);
        }

        public void Play()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }

            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForAudiotoEnd());
        }

        private IEnumerator WaitForAudiotoEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            _audioManager.ReturnToPool(this);
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }

            _audioSource.Stop();
            _audioManager.ReturnToPool(this);
        }

        public void Initialize(AudioData data)
        {
            this.AudioData = data;
            _audioSource.clip = data.Clip;
            _audioSource.outputAudioMixerGroup = data.MixerGroup;

            _audioSource.loop = data.Loop;
            _audioSource.playOnAwake = data.PlayOnAwake;
            _audioSource.bypassEffects = data.BypassEffects;
            _audioSource.bypassListenerEffects = data.BypassListenerEffects;
            _audioSource.bypassReverbZones = data.BypassReverbZones;

            _audioSource.priority = data.Priority;

            _audioSource.volume = data.Volume;
            _audioSource.pitch = data.Pitch;
            _audioSource.panStereo = data.StereoPan;
            _audioSource.spatialBlend = data.SpatialBlend;
            _audioSource.reverbZoneMix = data.ReverbZoneMix;

            //3D settings
            _audioSource.dopplerLevel = data.DopplerLevel;
            _audioSource.spread = data.Spread;

            _audioSource.minDistance = data.MinDistance;
            _audioSource.maxDistance = data.MaxDistance;
            _audioSource.rolloffMode = data.VolumeRollOffMode;

            if (data.VolumeRollOffMode == AudioRolloffMode.Custom)
                _audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, data.CustomCurve);

            _audioSource.ignoreListenerPause = data.IgnoreAudioListenerPause;
        }
    }
}
