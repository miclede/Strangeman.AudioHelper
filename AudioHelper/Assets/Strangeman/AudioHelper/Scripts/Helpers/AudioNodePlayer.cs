using System.Collections;
using UnityEngine;

namespace AudioHelper.Audio
{
    public abstract class AudioNodePlayer : MonoBehaviour, IPlayAudio
    {
        [field: SerializeField] public bool PlayOnStart { get; set; }

        protected virtual void Start()
        {
            if (PlayOnStart)
            {
                Play();
            }
        }

        public abstract void Play();

        public abstract IEnumerator PlayCoroutine();

        public abstract void Stop();
    }
}
