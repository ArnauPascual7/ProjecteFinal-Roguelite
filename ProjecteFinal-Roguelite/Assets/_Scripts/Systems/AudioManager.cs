using System;
using UnityEngine;

namespace Roguelite.Systems
{
    public enum SoundType
    {
        a,
        b,
        None
    }
    public enum MusicType
    {
        a,
        b
    }

    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Clips")]
        [SerializeField] private MusicList[] _musics = new MusicList[0];
        [SerializeField] private SoundList[] _sounds = new SoundList[0];

        [Header("Source Pool Settings")]
        [SerializeField] private int _poolSize = 10;

        private AudioSource _musicSource;
        private AudioSource _soundSource;
        private AudioSource[] _sourcePool;

        private float _masterVolume;
        private float _musicVolume;
        private float _sfxVolume;
        private int _poolIndex = 0;

        private void OnEnable()
        {
            OptionsMenuManager.OnOptionsChange += UpdateVolumes;

#if UNITY_EDITOR
            string[] soundNames = Enum.GetNames(typeof(SoundType));
            string[] musicNames = Enum.GetNames(typeof(MusicType));

            Array.Resize(ref _sounds, soundNames.Length);
            Array.Resize(ref _musics, musicNames.Length);

            for (int i = 0; i < _sounds.Length; i++)
            {
                _sounds[i].name = soundNames[i];
            }
            for (int i = 0; i < _musics.Length; i++)
            {
                _musics[i].name = musicNames[i];
            }
#endif
        }


        private void OnDisable()
        {
            OptionsMenuManager.OnOptionsChange -= UpdateVolumes;
        }

        private void Reset()
        {
            int sourcesCount = GetComponents<AudioSource>().Length;
            for (int i = 0; i < 2 - sourcesCount; i++)
            {
                gameObject.AddComponent<AudioSource>();
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _musicSource = GetComponents<AudioSource>()[0];
            _soundSource = GetComponents<AudioSource>()[1];

            _musicSource.loop = true;

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            _sourcePool = new AudioSource[_poolSize];
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject obj = new GameObject($"PosAudioSource_{i}");
                obj.transform.SetParent(transform);

                AudioSource src = obj.AddComponent<AudioSource>();
                src.playOnAwake = false;
                src.spatialBlend = 1f;
                src.rolloffMode = AudioRolloffMode.Logarithmic;
                src.minDistance = 2f;
                src.maxDistance = 30f;

                _sourcePool[i] = src;
            }
        }

        private void Start()
        {
            UpdateVolumes();
        }

        private void UpdateVolumes()
        {
            _masterVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MasterVolumeKey, OptionSettingsUtils.DefaultMasterVolume);
            _musicVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MusicVolumeKey, OptionSettingsUtils.DefaultMusicVolume);
            _sfxVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.SFXVolumeKey, OptionSettingsUtils.DefaultSFXVolume);

            _musicSource.volume = _musicVolume * _masterVolume;
            _soundSource.volume = _sfxVolume * _masterVolume;
        }

        public void PlaySound(SoundType sound, float volume = 1f)
        {
            if (sound == SoundType.None) return;

            AudioClip[] clips = _sounds[(int)sound].SoundClips;
            AudioClip rclip = clips[UnityEngine.Random.Range(0, clips.Length)];
            _soundSource.PlayOneShot(rclip, _sfxVolume * _masterVolume * volume);
        }

        public void PlaySoundAtPoint(SoundType sound, Vector3 position, float volume = 1f, float minDist = 2f, float maxDist = 30f)
        {
            if (_sourcePool == null) return;
            if (sound == SoundType.None) return;

            AudioClip[] clips = _sounds[(int)sound].SoundClips;
            AudioClip rclip = clips[UnityEngine.Random.Range(0, clips.Length)];

            AudioSource source = _sourcePool[_poolIndex];
            _poolIndex = (_poolIndex + 1) % _poolSize;

            source.transform.position = position;
            source.minDistance = minDist;
            source.maxDistance = maxDist;
            source.clip = rclip;
            source.volume = _sfxVolume * _masterVolume * volume;
            source.Play();
        }

        public AudioClip GetSound(SoundType sound)
        {
            AudioClip[] clips = _sounds[(int)sound].SoundClips;
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }

        public void PlayMusic(MusicType music, float volume = 1f)
        {
            _musicSource.clip = _musics[(int)music].MusicClip;
            _musicSource.volume = _musicVolume * _masterVolume * volume;
            _musicSource.Play();
        }

    }
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] _soundClips;

        public AudioClip[] SoundClips => _soundClips;
    }

    [Serializable]
    public struct MusicList
    {
        [HideInInspector] public string name;
        [SerializeField] private AudioClip _musicClip;

        public AudioClip MusicClip => _musicClip;
    }
}