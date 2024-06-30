using Strangeman.Utils.Extensions;
using Strangeman.Utils.Service;
using System;
using UnityEngine;

namespace AudioHelper.Core
{
    public class AudioBuilder
    {
        readonly AudioManager _audioManager;
        AudioData _audioData;
        Transform _followTransform;
        Vector3 _manualPosition = Vector3.zero;
        float _playDelay;
        int _repetitionCount;
        AudioEmitter _currentEmitter;

        public enum PositioningStyle { FollowTransform, ManualPosition }

        public AudioBuilder()
        {
            ServiceLocator.Global.GetMonoService(out _audioManager);
        }

        public AudioBuilder WithRepetition(int repCount)
        {
            int count = repCount < 0 ? 0 : repCount;
            _repetitionCount = count;
            return this;
        }

        public AudioBuilder WithDelay(float playDelay)
        {
            float delay = playDelay < 0 ? 0 : playDelay;
            _playDelay = delay;
            return this;
        }

        public AudioBuilder WithAudioData(AudioData audioData)
        {
            if (audioData == null)
            {
                throw new ArgumentException("AudioBuilder.WithAudioData: AudioData cannot be null.");
            }

            _audioData = audioData;
            return this;
        }

        public AudioBuilder WithPosition(Vector3 targetPosition)
        {
            _manualPosition = targetPosition;
            return this;
        }

        public AudioBuilder WithTransform(Transform targetTransform)
        {
            _followTransform = targetTransform;
            return this;
        }

        public void Play(PositioningStyle positioningStyle = PositioningStyle.FollowTransform)
        {
            try { if (!_audioManager.CanPlayAudio(_audioData)) return; }
            catch
            {
                Debug.LogError("AudioBuilder.Play: Inaccessible AudioManager");
            }

            Vector3 audioPosition = _followTransform.OrNull()?.position ?? _manualPosition;

            _currentEmitter = _audioManager.Get();
            _currentEmitter.Initialize(_audioData);

            _currentEmitter.transform.position = positioningStyle is PositioningStyle.ManualPosition ? _manualPosition : audioPosition;

            _currentEmitter.transform.parent = _audioManager.transform;

            _audioManager.EmitterCounts[_audioData] = _audioManager.EmitterCounts.TryGetValue(_audioData, out var count) ? count + 1 : 1;
            _currentEmitter.Play();
        }

        public void Stop()
        {
            if (_currentEmitter.isActiveAndEnabled)
                _currentEmitter.Stop();
        }

        private bool NeedCoroutine() => (_playDelay > 0 || _repetitionCount > 0);
    }
}