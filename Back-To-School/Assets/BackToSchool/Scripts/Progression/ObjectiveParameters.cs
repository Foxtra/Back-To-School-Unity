using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Progression
{
    public class ObjectiveParameters
    {
        public ObjectiveParameters(GameModes mode)
        {
            GameMode = mode;
            switch (GameMode)
            {
                case GameModes.SurviveTime:
                    TimeToSurvive = Constants.TimeToSurvive;
                    break;
                case GameModes.KillEnemies:
                    WarriorEnemiesToKill = Constants.WarriorEnemiesToKill;
                    ShamanEnemiesToKill  = Constants.ShamanEnemiesToKill;
                    break;
            }
        }

        public GameModes GameMode { get; set; }
        public float TimeToSurvive { get; set; }
        public float SurvivedTime { get; set; }
        public int WarriorEnemiesToKill { get; set; }
        public int WarriorEnemiesKilled { get; set; }
        public int ShamanEnemiesToKill { get; set; }
        public int ShamanEnemiesKilled { get; set; }
    }
}