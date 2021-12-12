using System.Collections.Generic;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Parameters;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class AudioManager : IAudioManager
    {
        private Dictionary<ESounds, AudioSource> _effectDictionary = new Dictionary<ESounds, AudioSource>();
        private Dictionary<ESounds, AudioSource> _musicDictionary = new Dictionary<ESounds, AudioSource>();
        private ISystemResourceManager _resourceManager;

        public AudioManager(ISystemResourceManager resourceManager) => _resourceManager = resourceManager;

        public void PlayEffect(ESounds sound)
        {
            AudioSource effect;
            if (_effectDictionary.ContainsKey(sound))
                effect = _effectDictionary[sound];
            else
            {
                effect = _resourceManager.CreatePrefabInstance<AudioSource, ESounds>(sound);
                _effectDictionary.Add(sound, effect);
            }

            effect.Play();
        }

        public void PlayMusic(ESounds sound, bool isLoop = true)
        {
            var alreadyPlaying = _musicDictionary.ContainsKey(sound);

            if (alreadyPlaying)
            {
                _musicDictionary[sound].time = 0f;
                return;
            }

            var music = _resourceManager.CreatePrefabInstance<AudioSource, ESounds>(sound);
            music.loop = isLoop;
            music.Play();
            _musicDictionary.Add(sound, music);
        }

        public void StopMusic(ESounds sound)
        {
            var music = _musicDictionary[sound];
            music.Stop();
            _musicDictionary.Remove(sound);
        }

        private async void PlayAndDestroy(AudioSource source)
        {
            var length = source.clip.length;
            source.Play();
            await UniTask.Delay(Mathf.RoundToInt(length * Constants.Time.MillisecondsMultiplier));
            Object.Destroy(source);
        }

        public void Dispose()
        {
            _musicDictionary.Clear();
            _effectDictionary.Clear();
        }
    }
}