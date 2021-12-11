using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IGameOverPresenter : IView
    {
        public event Action Restarted;
    }
}