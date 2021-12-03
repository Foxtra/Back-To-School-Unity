using System;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class LevelSystem : ILevelSystem
    {
        public event Action<float> ExperienceChanged;
        public event Action<int> LevelChanged;
        public event Action ProgressChanged;

        private static readonly int[] _experiencePerLevel = { 100, 120, 140, 160, 180, 200, 220, 250, 300, 400 };

        private int _level;
        private int _experience;

        public void Initialize(int level, int experience)
        {
            SetLevelNumber(level);
            SetExperience(experience);
        }

        public void AddExperience(int amount)
        {
            if (IsMaxLevel())
                return;

            _experience += amount;
            while (!IsMaxLevel() && _experience >= GetExperienceToNextLevel(_level))
            {
                _experience -= GetExperienceToNextLevel(_level);
                _level++;
                LevelChanged?.Invoke(_level);
            }

            ExperienceChanged?.Invoke(GetExperienceNormalized());
            ProgressChanged?.Invoke();
        }

        public void SetLevelNumber(int newLevel)
        {
            _level = newLevel;
            LevelChanged?.Invoke(_level);
        }

        public void SetExperience(int experience)
        {
            _experience = experience;
            ExperienceChanged?.Invoke(GetExperienceNormalized());
        }

        public int GetLevelNumber() => _level;

        public float GetExperienceNormalized()
        {
            if (IsMaxLevel())
                return 1f;
            return (float)_experience / GetExperienceToNextLevel(_level);
        }

        public int GetExperience() => _experience;

        public int GetExperienceToNextLevel(int level)
        {
            if (level < _experiencePerLevel.Length)
                return _experiencePerLevel[level];

            Debug.LogError("Level invalid: " + level);
            return 100;
        }

        public bool IsMaxLevel() => IsMaxLevel(_level);

        public bool IsMaxLevel(int level) => level == _experiencePerLevel.Length - 1;
    }
}