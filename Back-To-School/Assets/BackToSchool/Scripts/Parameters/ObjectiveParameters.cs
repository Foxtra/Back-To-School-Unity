using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public class ObjectiveParameters
    {
        public ObjectiveParameters(EGameModes mode)
        {
            GameMode = mode;
            switch (GameMode)
            {
                case EGameModes.SurviveTime:
                    TimeToSurvive = Constants.TimeToSurvive;
                    break;
                case EGameModes.KillEnemies:
                    WarriorEnemiesToKill = Constants.WarriorEnemiesToKill;
                    ShamanEnemiesToKill  = Constants.ShamanEnemiesToKill;
                    break;
            }
        }

        public EGameModes GameMode { get; set; }
        public float TimeToSurvive { get; set; }
        public float SurvivedTime { get; set; }
        public int WarriorEnemiesToKill { get; set; }
        public int WarriorEnemiesKilled { get; set; }
        public int ShamanEnemiesToKill { get; set; }
        public int ShamanEnemiesKilled { get; set; }
    }
}