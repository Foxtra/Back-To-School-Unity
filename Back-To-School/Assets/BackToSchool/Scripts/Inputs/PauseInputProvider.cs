using System;
using Assets.BackToSchool.Scripts.Interfaces.Input;


namespace Assets.BackToSchool.Scripts.Inputs
{
    internal class PauseInputProvider : Input, IPauseInput
    {
        public event Action Cancelled;

        public override void CancelInvoked() => Cancelled?.Invoke();
    }
}