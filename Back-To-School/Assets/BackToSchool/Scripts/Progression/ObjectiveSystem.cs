using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class ObjectiveSystem : IObjectiveSystem
    {
        public event Action ObjectivesCompleted;
        public event Action<float> TimeSurvivedChanged;
        public event Action<int, int> EnemiesKilled;

        private ObjectiveParameters _currentObjectives;

        private float _timePassed;
        private float _lastTimeInvoked;
        private float _timerInvokeInterval = 1f;
        private int _warriorEnemiesKilled;
        private int _shamanEnemiesKilled;
        private bool _isObjectivesCompleted;

        public void Initialize(ObjectiveParameters parameters) => _currentObjectives = parameters;

        public ObjectiveParameters GetObjectivesProgress()
        {
            _currentObjectives.ShamanEnemiesKilled  = _shamanEnemiesKilled;
            _currentObjectives.WarriorEnemiesKilled = _warriorEnemiesKilled;
            _currentObjectives.SurvivedTime         = _timePassed;
            return _currentObjectives;
        }

        public void CountEnemyDeath(BaseEnemy sender)
        {
            if (_currentObjectives.GameMode != EGameModes.KillEnemies)
                return;

            if (sender is EnemyWarrior)
                _warriorEnemiesKilled++;
            if (sender is EnemyShaman)
                _shamanEnemiesKilled++;
            EnemiesKilled?.Invoke(_currentObjectives.WarriorEnemiesToKill - _warriorEnemiesKilled,
                _currentObjectives.ShamanEnemiesToKill - _shamanEnemiesKilled);

            CheckKilledEnemies();
        }

        public void CountTimePassed(float time)
        {
            if (_currentObjectives.GameMode != EGameModes.SurviveTime)
                return;

            _timePassed += time;
            CheckPassedTime();
        }

        private void CheckPassedTime()
        {
            if (_currentObjectives == null && _isObjectivesCompleted)
                return;

            if (Math.Abs(_lastTimeInvoked - _timePassed) > _timerInvokeInterval)
            {
                TimeSurvivedChanged?.Invoke(_currentObjectives.TimeToSurvive - _timePassed);
                _lastTimeInvoked = _timePassed;
            }

            if (_timePassed < _currentObjectives.TimeToSurvive)
                return;

            _isObjectivesCompleted = true;
            ObjectivesCompleted?.Invoke();
        }

        private void CheckKilledEnemies()
        {
            if (_currentObjectives == null && _isObjectivesCompleted)
                return;

            if (_warriorEnemiesKilled < _currentObjectives.WarriorEnemiesToKill ||
                _shamanEnemiesKilled < _currentObjectives.ShamanEnemiesToKill)
                return;

            _isObjectivesCompleted = true;
            ObjectivesCompleted?.Invoke();
        }
    }
}