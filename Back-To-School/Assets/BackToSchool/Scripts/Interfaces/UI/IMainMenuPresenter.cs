using System;
using Assets.BackToSchool.Scripts.Enums;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IMainMenuPresenter : IView
    {
        public event Action ExitTriggered;
        public event Action ContinueTriggered;
        public event Action<EGameModes> KillEnemiesModeTriggered;
        public event Action<EGameModes> SurviveModeTriggered;
        public void ShowContinueButton(bool isShown);
    }
}