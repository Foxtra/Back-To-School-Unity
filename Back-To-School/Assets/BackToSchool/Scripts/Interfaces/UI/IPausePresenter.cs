using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IPausePresenter : IView
    {
        public event Action Restarted;
        public event Action Continued;
        public event Action MenuReturned;
        public void TogglePausePanel(bool isPausePanelShowed);
    }
}