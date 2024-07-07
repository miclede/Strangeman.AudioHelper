using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioHelper.Audio
{
    [AddComponentMenu("Audio/Audio Track List")]
    public class AudioTrackList : AudioNodePlayer
    {
        [SerializeField] bool _overridePlayDelayWithClipDuration;
        [SerializeField] List<AudioNode> _playList = new();

        protected override void Start()
        {
            _playList.ForEach(n => n.Init());
            base.Start();
        }

        public override void Play()
        {
            if (_playList.Count == 0)
                return;

            if (_playList.Count > 0)
                StartCoroutine(PlayCoroutine());

            else if (_isFollowingTransform)
            {
                _playList[0]
                    .With(transform)
                    .Play();
            }
            else
            {
                _playList[0]
                    .With(transform.position)
                    .Play();
            }

        }

        public override IEnumerator PlayCoroutine()
        {
            WaitForSeconds WaitPreviousNodeClipLength = new WaitForSeconds(0);

            foreach (var node in _playList)
            {
                int repCount;

                if (_overridePlayDelayWithClipDuration)
                {
                    yield return WaitPreviousNodeClipLength;
                }
                else
                {
                    yield return node.WaitPlayDelay;
                }

                WaitPreviousNodeClipLength = node.WaitClipLength;

                for (repCount = 0; repCount <= node.RepetitionCount; repCount++)
                {
                    if (repCount != 0)
                    {
                        yield return node.WaitClipLength;
                    }

                    if (_isFollowingTransform)
                    {
                        node.With(transform)
                            .Play();
                    }
                    else
                    {
                        node.With(transform.position)
                        .Play();
                    }
                }
            }
        }

        public override void Stop()
        {
            if (_playList.Count == 0)
                return;

            _playList.ForEach(s => s.Stop());
        }

        public override void UpdateFollowTransform(bool value)
        {
            base.UpdateFollowTransform(value);
            _playList.ForEach(n => n.With(_isFollowingTransform ? Core.AudioPositioningStyle.FollowTransform : Core.AudioPositioningStyle.ManualPosition));
        }
    }
}
