using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IPausePresenter : IView
    {
        event Action Restarted;
        event Action Continued;
        event Action MenuReturned;
        void TogglePausePanel(bool isPausePanelShowed);
    }
}