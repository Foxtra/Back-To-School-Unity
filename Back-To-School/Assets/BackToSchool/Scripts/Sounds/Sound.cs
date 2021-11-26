using System;
using Assets.BackToSchool.Scripts.Enums;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Sounds
{
    [Serializable]
    public class Sound
    {
        public SoundNames Name;

        [SerializeField] private AudioClip _clip;
        [Range(0f, 1f)] [SerializeField] private float _volume;
        [SerializeField] private bool _loop;

        private AudioSource _source;

        public void Initialize()
        {
            _source        = new GameObject().AddComponent<AudioSource>();
            _source.volume = _volume;
            _source.clip   = _clip;
            _source.loop   = _loop;
        }

        public void PlaySource()        => _source.Play();
        public void StopPlayingSource() => _source.Stop();
        public bool IsSourcePlaying()   => _source.isPlaying;
        public bool IsSourceExists()    => _source != null;
    }
}