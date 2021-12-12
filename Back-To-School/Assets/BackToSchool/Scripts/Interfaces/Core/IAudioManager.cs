using System;
using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IAudioManager : IDisposable
    {
        public void PlayEffect(ESounds sound);
        public void PlayMusic(ESounds sound, bool isLoop = true);
        public void StopMusic(ESounds sound);
    }
}