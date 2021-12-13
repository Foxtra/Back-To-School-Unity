using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public class LevelParameters
    {
        public LevelParameters(EScenes scene, EGameModes mode, int levelNumber)
        {
            Scene = scene;
            Mode  = mode;

            LevelNumber = levelNumber;
        }

        public EScenes Scene { get; set; }
        public EGameModes Mode { get; set; }
        public int LevelNumber { get; set; }
    }
}