using UnityEngine;
using AudioHelper.Core;

namespace AudioHelper.Audio
{
    public enum AudioAnimationTiming
    {
        Enter,
        Point,
        //Interval,
        Condition,
        Exit
    }

    //TODO: Add 'Interval' audio support
    public class AnimationAudio: StateMachineBehaviour
    {
        [Header("Timing & Positioning")]
        [SerializeField, Tooltip("Play on Enter, Play on Target Time, Play on Exit")]
        AudioAnimationTiming _audioTiming;
        [SerializeField, Range(0, 1), Tooltip("Normalized")]
        float _targetTime;

        [SerializeField, Tooltip("Animator Transform + offset = positioned audio.")]
        Vector3 _audioPositionOffset;

        [Header("Data")]
        [SerializeField, Tooltip("Scriptable Object, menuName = Audio/Audio Data")] AudioData _audioData;

        bool _played;
        AudioBuilder _builtAudio;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _played = false;

            if (_builtAudio == null)
            {
                _builtAudio = new AudioBuilder()
                    .WithAudioData(_audioData)
                    .WithPosition(animator.transform.position + _audioPositionOffset);
            }

            if (_audioTiming != AudioAnimationTiming.Enter || _played)
            {
                return;
            }

            _builtAudio.Play();
            _played = true;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //TODO: Interval

            if (_audioTiming != AudioAnimationTiming.Point || _played)
            {
                return;
            }

            float normalizedTime = stateInfo.normalizedTime % 1;

            if (normalizedTime >= _targetTime)
            {
                _builtAudio
                    .WithPosition(animator.transform.position + _audioPositionOffset)
                    .Play();

                _played = true;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_audioTiming != AudioAnimationTiming.Exit || _played)
            {
                return;
            }

            _builtAudio
                .WithPosition(animator.transform.position + _audioPositionOffset)
                .Play();

            _played = true;
        }
    }
}
