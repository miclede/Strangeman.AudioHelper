using AudioHelper.Core;
using UnityEngine;

namespace AudioHelper.Audio
{
    [System.Serializable]
    public class AudioNode
    {
        [SerializeField] private AudioData _audioData;
        [SerializeField, Min(0)] private float _playDelay = 0;
        [SerializeField] private bool _repeats = false;
        [SerializeField] private int _repetitionCount = 0;

        public float PlayDelay => _playDelay;
        public int RepetitionCount => _repetitionCount;

        private AudioBuilder _builtAudio;

        public WaitForSeconds WaitPlayDelay { get; private set; }
        public WaitForSeconds WaitClipLength { get; private set; }

        bool _initialized;

        public AudioNode()
        {
            _initialized = false;
        }

        public AudioNode Init()
        {
            if (_audioData is null)
            {
                Debug.LogError("AudioNode.Init: Attempted to initialize node with no Audio Data.");
                return this;
            }

            WaitPlayDelay = new WaitForSeconds(_playDelay);
            WaitClipLength = new WaitForSeconds(_audioData.Clip.length);

            _builtAudio = new AudioBuilder()
                .WithAudioData(_audioData);

            _initialized = true;
            return this;
        }

        public AudioNode With(Vector3 position)
        {
            _builtAudio.WithPosition(position);
            return this;
        }

        public AudioNode With(Transform transform)
        {
            _builtAudio.WithTransform(transform);
            return this;
        }

        public AudioNode With(AudioPositioningStyle audioPositioningStyle)
        {
            _builtAudio.WithPositioningStyle(audioPositioningStyle);
            return this;
        }

        public void Play()
        {
            if (!_initialized)
                return;

            _builtAudio.Play();
        }

        public void Stop()
        {
            if (!_initialized)
                return;

            _builtAudio.Stop();
        }

        public bool HasRepetitions() => _repeats;
    }
}
