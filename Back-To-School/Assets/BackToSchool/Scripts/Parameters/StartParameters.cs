using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public class StartParameters
    {
        public StartParameters(bool isNew) : this(isNew, EScenes.MainScene.ToStringCached()) { }
        public StartParameters(bool isNew, EGameModes mode) : this(isNew, EScenes.MainScene.ToStringCached(), mode) { }

        public StartParameters(bool isNew, string scene)
        {
            IsNewGame = isNew;
            NextScene = scene;
        }

        public StartParameters(bool isNew, string scene, EGameModes mode)
        {
            IsNewGame = isNew;
            NextScene = scene;
            GameMode  = mode;
        }

        public string NextScene { get; private set; }
        public bool IsNewGame { get; private set; }
        public EGameModes GameMode { get; private set; }

        public void SetNextScene(EScenes scene)  => NextScene = scene.ToStringCached();
        public void SetIsNewGame(bool isNew)     => IsNewGame = isNew;
        public void SetGameMode(EGameModes mode) => GameMode = mode;
    }
}