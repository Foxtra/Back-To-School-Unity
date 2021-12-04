using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IMainMenuPresenter : IView
    {
        public event Action ExitTriggered;
        public event Action StartTriggered;
        public event Action ContinueTriggered;
        public void ShowContinueButton(bool isShown);
    }
}