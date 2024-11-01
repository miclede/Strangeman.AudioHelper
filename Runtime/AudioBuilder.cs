using Strangeman.Utils.Service;
using System;
using UnityEngine;

namespace AudioHelper.Core
{
    public enum AudioPositioningStyle { FollowTransform, ManualPosition }

    public class AudioBuilder
    {
        readonly AudioManager _audioManager;
        AudioData _audioData;
        Transform _followTransform;
        Vector3 _manualPosition = Vector3.zero;
        Vector3 _offsetPosition = Vector3.zero;
        float _playDelay;
        int _repetetionCount;
        AudioEmitter _currentEmitter;
        AudioPositioningStyle _positioningStyle;

        public AudioBuilder()
        {
            ServiceLocator.Global.GetMonoService(out _audioManager);
        }

        #region Fluid Builder Self-Referentials
        public AudioBuilder WithRepetition(int repCount)
        {
            int count = repCount < 0 ? 0 : repCount;
            _repetetionCount = count;
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
            if (audioData is null)
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

        public AudioBuilder WithOffset(Vector3 offset)
        {
            _offsetPosition = offset;
            return this;
        }

        public AudioBuilder WithPositioningStyle(AudioPositioningStyle audioPositioningStyle)
        {
            _positioningStyle = audioPositioningStyle;
            return this;
        }
        #endregion

        // Build Equivalent
        public void Play()
        {
            if (_audioData is null)
            {
                Debug.LogError("AudioBuilder.Play: No AudioData has been assigned to this builder.");
                return;
            }

            try { if (!_audioManager.CanPlayAudio(_audioData)) return; }
            catch
            {
                Debug.LogError("AudioBuilder.Play: Inaccessible AudioManager");
            }
            _currentEmitter = _audioManager.Get();
            _currentEmitter.Initialize(_audioData);

            _currentEmitter.transform.parent = (_positioningStyle is AudioPositioningStyle.FollowTransform && _followTransform != null) 
                ? _followTransform 
                : _audioManager.transform;

            _currentEmitter.transform.position = (_positioningStyle is AudioPositioningStyle.ManualPosition || _followTransform is null)
                ? _manualPosition + _offsetPosition
                : _followTransform.transform.position + _offsetPosition;

            _audioManager.AddOrUpdateEmitterCount(_audioData);
            _currentEmitter.Play();
        }

        // Helper that is accessible should you want to stop the audio related to this specific build
        public void Stop()
        {
            if (_currentEmitter.isActiveAndEnabled)
                _currentEmitter.Stop();
        }

        //Noop TODO: Repetitions on builder level with async and cancelation on stop
        private bool NeedCoroutine() => (_playDelay > 0 || _repetetionCount > 0);
    }
}