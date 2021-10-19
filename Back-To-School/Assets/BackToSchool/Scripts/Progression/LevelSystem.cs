using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class LevelSystem
    {
        public event Action<float> ExperienceChanged;
        public event Action<int> LevelChanged;
        public event Action ProgressChanged;

        private static readonly int[] experiencePerLevel = { 100, 120, 140, 160, 180, 200, 220, 250, 300, 400 };

        private int level;
        private int experience;

        public LevelSystem()
        {
            level      = 0;
            experience = 0;
        }

        public void AddExperience(int amount)
        {
            if (!IsMaxLevel())
            {
                experience += amount;
                while (!IsMaxLevel() && experience >= GetExperienceToNextLevel(level))
                {
                    experience -= GetExperienceToNextLevel(level);
                    level++;
                    LevelChanged?.Invoke(level);
                }

                ExperienceChanged?.Invoke(GetExperienceNormalized());
                ProgressChanged?.Invoke();
            }
        }

        public void SetLevelNumber(int newLevel)
        {
            level = newLevel;
            LevelChanged?.Invoke(level);
        }

        public int GetLevelNumber() => level;

        public float GetExperienceNormalized()
        {
            if (IsMaxLevel())
                return 1f;
            return (float)experience / GetExperienceToNextLevel(level);
        }

        public int GetExperience() => experience;

        public int GetExperienceToNextLevel(int level)
        {
            if (level < experiencePerLevel.Length)
                return experiencePerLevel[level];

            Debug.LogError("Level invalid: " + level);
            return 100;
        }

        public bool IsMaxLevel() => IsMaxLevel(level);

        public bool IsMaxLevel(int level) => level == experiencePerLevel.Length - 1;
    }
}