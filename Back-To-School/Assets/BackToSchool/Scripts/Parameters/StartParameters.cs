using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public class StartParameters
    {
        public StartParameters(bool isNew, EGameModes mode) : this(isNew, EScenes.FirstLevel, mode,
            Constants.Level.InitialLevel) { }

        public StartParameters(bool isNew, EScenes scene, EGameModes mode, int levelNumber, bool isLoaded = true)
        {
            IsNewGame   = isNew;
            Scene       = scene;
            GameMode    = mode;
            LevelNumber = levelNumber;

            IsLevelLoadedFromSave = isLoaded;
        }

        public bool IsNewGame { get; private set; }
        public bool IsLevelLoadedFromSave { get; }
        public int LevelNumber { get; private set; }
        public EScenes Scene { get; private set; }
        public EGameModes GameMode { get; private set; }

        public void SetNextScene(EScenes scene)  => Scene = scene;
        public void SetIsNewGame(bool isNew)     => IsNewGame = isNew;
        public void SetGameMode(EGameModes mode) => GameMode = mode;
        public void SetLevelNumber(int number)   => LevelNumber = number;
    }
}