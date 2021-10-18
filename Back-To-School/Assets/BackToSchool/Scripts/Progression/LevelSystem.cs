using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class LevelSystem
    {
        public event EventHandler OnExperienceChanged;

        private static readonly int[] experiencePerLevel = { 100, 120, 140, 160, 180, 200, 220, 250, 300, 400 };
        public Action<int> OnLevelChanged;

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
                    OnLevelChanged?.Invoke(level);
                }

                if (OnExperienceChanged != null) OnExperienceChanged(this, EventArgs.Empty);
            }
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