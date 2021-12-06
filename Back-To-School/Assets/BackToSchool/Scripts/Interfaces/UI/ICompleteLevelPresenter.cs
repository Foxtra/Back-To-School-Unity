using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface ICompleteLevelPresenter : IView
    {
        public event Action Restarted;
        public event Action MenuReturned;
    }
}