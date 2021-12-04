using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;


namespace Assets.BackToSchool.Scripts.Parameters
{
    public class StartParameters
    {
        public StartParameters(bool isNew) : this(isNew, EScenes.MainScene.ToStringCached()) { }

        public StartParameters(bool isNew, string scene)
        {
            IsNewGame = isNew;
            NextScene = scene;
        }

        public string NextScene { get; private set; }
        public bool IsNewGame { get; private set; }

        public void SetNextScene(EScenes scene) => NextScene = scene.ToStringCached();
        public void SetIsNewGame(bool isNew)    => IsNewGame = isNew;
    }
}