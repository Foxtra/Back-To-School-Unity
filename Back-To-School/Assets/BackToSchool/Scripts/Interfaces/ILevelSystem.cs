using System;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface ILevelSystem
    {
        event Action<float> ExperienceChanged;
        event Action<int> LevelChanged;
        event Action ProgressChanged;

        void  Initialize(int level, int experience);
        void  AddExperience(int amount);
        void  SetLevelNumber(int newLevel);
        void  SetExperience(int experience);
        int   GetLevelNumber();
        float GetExperienceNormalized();
        int   GetExperience();
        int   GetExperienceToNextLevel(int level);
        bool  IsMaxLevel();
        bool  IsMaxLevel(int level);
    }
}