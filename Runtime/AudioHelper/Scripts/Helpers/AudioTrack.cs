using System.Collections;
using UnityEngine;

namespace AudioHelper.Audio
{
    [AddComponentMenu("Audio/Audio Track")]
    public class AudioTrack : AudioNodePlayer
    {
        [SerializeField] AudioNode _audioNode;

        protected override void Start()
        {
            _audioNode.Init();
            base.Start();
        }

        public override void Play()
        {
            if (_audioNode.HasRepetitions() || _audioNode.PlayDelay > 0)
                StartCoroutine(PlayCoroutine());

            else if (_isFollowingTransform)
            {
                _audioNode
                    .With(transform)
                    .Play();
            }
            else
            {
                _audioNode
                    .With(transform.position)
                    .Play();
            }
        }

        public override IEnumerator PlayCoroutine()
        {
            int repCount;

            yield return _audioNode.WaitPlayDelay;

            if (_isFollowingTransform)
            {
                _audioNode.With(transform);
            }
            else
            {
                _audioNode.With(transform.position);
            }

            for (repCount = 0; repCount < _audioNode.RepetitionCount; repCount++)
            {
                if (repCount != 0)
                {
                    yield return _audioNode.WaitClipLength;
                }

                _audioNode.Play();
            }
        }

        public override void Stop() => _audioNode.Stop();

        public override void UpdateFollowTransform(bool value)
        {
            base.UpdateFollowTransform(value);
            _audioNode.With(_isFollowingTransform ? Core.AudioPositioningStyle.FollowTransform : Core.AudioPositioningStyle.ManualPosition);
        }
    }
}
