using System;
using Assets.BackToSchool.Scripts.Enemies;
using Assets.BackToSchool.Scripts.Parameters;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IObjectiveSystem
    {
        public event Action ObjectivesCompleted;
        public event Action<float> TimeSurvivedChanged;
        public event Action<int, int> EnemiesKilled;

        public void Initialize(ObjectiveParameters parameters);

        public ObjectiveParameters GetObjectivesProgress();

        public void CountEnemyDeath(BaseEnemy sender);

        public void CountTimePassed(float time);
    }
}