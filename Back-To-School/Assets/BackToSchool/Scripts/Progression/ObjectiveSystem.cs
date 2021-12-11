using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Parameters;
using Cysharp.Threading.Tasks;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class ObjectiveSystem : IObjectiveSystem
    {
        public event Action ObjectivesCompleted;
        public event Action<float> TimeSurvivedChanged;
        public event Action<int, int> EnemiesKilled;

        private ObjectiveParameters _currentObjectives;

        private float _timePassed;
        private int _timerInvokeInterval = Constants.Time.TimerInvokeInterval;
        private int _warriorEnemiesKilled;
        private int _shamanEnemiesKilled;
        private bool _isObjectivesCompleted;

        public void Initialize(ObjectiveParameters parameters)
        {
            _currentObjectives = parameters;
            CountTimePassed();
        }

        public ObjectiveParameters GetObjectivesProgress()
        {
            _currentObjectives.ShamanEnemiesKilled  = _shamanEnemiesKilled;
            _currentObjectives.WarriorEnemiesKilled = _warriorEnemiesKilled;
            _currentObjectives.SurvivedTime         = _timePassed;
            return _currentObjectives;
        }

        public void CountEnemyDeath(Enemy sender)
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

        private async void CountTimePassed()
        {
            if (_currentObjectives.GameMode != EGameModes.SurviveTime)
                return;

            await UniTask.Delay(_timerInvokeInterval);
            _timePassed += 1f;
            CheckPassedTime();
        }

        private void CheckPassedTime()
        {
            if (_currentObjectives == null && _isObjectivesCompleted)
                return;

            TimeSurvivedChanged?.Invoke(_currentObjectives.TimeToSurvive - _timePassed);

            if (_timePassed < _currentObjectives.TimeToSurvive)
            {
                CountTimePassed();
                return;
            }

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