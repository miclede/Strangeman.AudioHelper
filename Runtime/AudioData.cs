using UnityEngine;
using UnityEngine.Audio;
using Strangeman.Utils.Attributes;

namespace AudioHelper.Core
{
    [CreateAssetMenu(fileName = "$NEWAudioData", menuName = "Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        // Audio Clip and Mixer Group
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioMixerGroup _mixerGroup;

        // Playback settings
        [SerializeField] private bool _loop = false;
        [SerializeField] private bool _playOnAwake = false;
        [SerializeField] private bool _bypassEffects = false;
        [SerializeField] private bool _bypassListenerEffects = false;
        [SerializeField] private bool _bypassReverbZones = false;
        [SerializeField] private bool _frequentAudio = false; // If true, will use Min Max range on sliders for sound variance

        [SerializeField, Range(0, 256)] private int _priority = 128;

        // Adjustable Ranges
        [SerializeField, MinMaxSlider(0, 1)] private MinMaxSliderValue _volume = 1f;
        [SerializeField, MinMaxSlider(-3, 3)] private MinMaxSliderValue _pitch = 1f;
        [SerializeField, MinMaxSlider(-1, 1)] private MinMaxSliderValue _stereoPan = 0;
        [SerializeField, MinMaxSlider(0, 1)] private MinMaxSliderValue _spatialBlend = 0;
        [SerializeField, MinMaxSlider(0, 1.1f)] private MinMaxSliderValue _reverbZoneMix = 1f;

        // Spatial settings and Rolloff
        [SerializeField, MinMaxSlider(0, 5)] private MinMaxSliderValue _dopplerLevel = 1;
        [SerializeField, Range(0, 360)] private int _spread = 0;
        [SerializeField] private AudioRolloffMode _volumeRollOffMode;
        [SerializeField, Conditional(true, nameof(IsCustomRolloffSelected))] private AnimationCurve _customCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(10, 0));
        [SerializeField, Conditional(false, nameof(IsCustomRolloffSelected))] private int _minDistance = 1;
        [SerializeField] private int _maxDistance = 500;

        [SerializeField] private bool _ignoreAudioListenerPause;

        #region Public Accessors
        public AudioClip Clip => _clip;
        public AudioMixerGroup MixerGroup => _mixerGroup;

        public bool Loop => _loop;
        public bool PlayOnAwake => _playOnAwake;
        public bool BypassEffects => _bypassEffects;
        public bool BypassListenerEffects => _bypassListenerEffects;
        public bool BypassReverbZones => _bypassReverbZones;
        public bool FrequentAudio => _frequentAudio;

        public int Priority => _priority;

        public float Volume => SliderReturnValue(_volume);
        public float Pitch => SliderReturnValue(_pitch);
        public float StereoPan => SliderReturnValue(_stereoPan);
        public float SpatialBlend => SliderReturnValue(_spatialBlend);
        public float ReverbZoneMix => SliderReturnValue(_reverbZoneMix);

        public float DopplerLevel => SliderReturnValue(_dopplerLevel);
        public int Spread => _spread;
        public AudioRolloffMode VolumeRollOffMode => _volumeRollOffMode;
        public AnimationCurve CustomCurve => _customCurve;
        public int MinDistance => _minDistance;
        public int MaxDistance => _maxDistance;

        public bool IgnoreAudioListenerPause => _ignoreAudioListenerPause;
        #endregion

        private float SliderReturnValue(MinMaxSliderValue sliderValue) => _frequentAudio ? Random.Range(sliderValue.MinSliderValue, sliderValue.MaxSliderValue) : sliderValue;

        private bool IsCustomRolloffSelected() => _volumeRollOffMode is AudioRolloffMode.Custom;
    }
}
