using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface IAudioManager
    {
        public void Play(SoundNames soundName);
        public void Stop(SoundNames soundName);
    }
}