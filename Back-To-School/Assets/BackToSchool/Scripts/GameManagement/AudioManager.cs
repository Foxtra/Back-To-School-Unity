using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Sounds;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        public Sound[] Sounds;
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void Start() => Play(SoundNames.BackGround1);

        public void Play(SoundNames soundName)
        {
            if (!Sounds[0].IsSourceExists())
            {
                foreach (var s in Sounds)
                    s.Initialize();
            }

            var sound = Array.Find(Sounds, s => s.Name == soundName);
            sound.PlaySource();
        }

        public void Stop(SoundNames soundName)
        {
            var sound = Array.Find(Sounds, s => s.Name == soundName);
            sound.StopPlayingSource();
        }
    }
}