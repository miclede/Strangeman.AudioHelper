using System.Collections;
using UnityEngine;

namespace AudioHelper.Audio
{
    public abstract class AudioNodePlayer : MonoBehaviour, IPlayAudio
    {
        [field: SerializeField] public bool PlayOnStart { get; set; }
        [SerializeField] protected bool _isFollowingTransform = false;

        protected virtual void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }

            UpdateFollowTransform(_isFollowingTransform);
        }

        public abstract void Play();

        public abstract IEnumerator PlayCoroutine();

        public abstract void Stop();

        public virtual void UpdateFollowTransform(bool value)
        {
            _isFollowingTransform = value;
        }
    }
}
