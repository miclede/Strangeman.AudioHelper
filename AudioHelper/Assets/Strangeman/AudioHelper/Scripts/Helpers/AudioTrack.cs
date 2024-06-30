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
            base.Start();

            _audioNode.Init();
        }

        public override void Play()
        {
            if (_audioNode.HasRepetitions() || _audioNode.PlayDelay > 0)
                StartCoroutine(PlayCoroutine());

            else _audioNode
                    .With(transform.position)
                    .Play();
        }

        public override IEnumerator PlayCoroutine()
        {
            int repCount;

            yield return _audioNode.PlayDelayWaiter;

            for (repCount = 0; repCount < _audioNode.RepetitionCount; repCount++)
            {
                if (repCount != 0)
                {
                    yield return _audioNode.ClipLengthWaiter;
                }

                _audioNode
                    .With(transform.position)
                    .Play();
            }
        }

        public override void Stop() => _audioNode.Stop();
    }
}
