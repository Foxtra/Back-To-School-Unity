using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface ILevelSystem
    {
        public event Action<float> ExperienceChanged;
        public event Action<int> LevelChanged;
        public event Action ProgressChanged;

        public void  Initialize(int level, int experience);
        public void  AddExperience(int amount);
        public void  SetLevelNumber(int newLevel);
        public void  SetExperience(int experience);
        public int   GetLevelNumber();
        public float GetExperienceNormalized();
        public int   GetExperience();
        public int   GetExperienceToNextLevel(int level);
        public bool  IsMaxLevel();
        public bool  IsMaxLevel(int level);
    }
}