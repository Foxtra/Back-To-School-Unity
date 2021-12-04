using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IMainMenuPresenter : IView
    {
        event Action ExitTriggered;
        event Action StartTriggered;
        event Action ContinueTriggered;
        void ShowContinueButton(bool isShown);
    }
}