using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IAudioManager
    {
        public void Play(SoundNames soundName);
        public void Stop(SoundNames soundName);
    }
}