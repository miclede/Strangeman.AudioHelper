using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioHelper.Audio
{
    [AddComponentMenu("Audio/Audio Track List")]
    public class AudioTrackList : AudioNodePlayer
    {
        [SerializeField] List<AudioNode> _playList = new();

        protected override void Start()
        {
            base.Start();
            _playList.ForEach(n => n.Init());
        }

        public override void Play()
        {
            if (_playList.Count == 0)
                return;

            if (_playList.Count > 0)
                StartCoroutine(PlayCoroutine());

            else _playList[0]
                    .With(transform.position)
                    .Play();
        }

        public override IEnumerator PlayCoroutine()
        {
            foreach (var node in _playList)
            {
                int repCount;

                yield return node.PlayDelayWaiter;

                for (repCount = 0; repCount < node.RepetitionCount; repCount++)
                {
                    if (repCount != 0)
                    {
                        yield return node.ClipLengthWaiter;
                    }

                    node.With(transform.position)
                        .Play();
                }
            }
        }

        public override void Stop()
        {
            if (_playList.Count == 0)
                return;

            _playList.ForEach(s => s.Stop());
        }
    }
}
