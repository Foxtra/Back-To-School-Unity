using System;
using Assets.BackToSchool.Scripts.Interfaces.Input;


namespace Assets.BackToSchool.Scripts.Inputs
{
    internal class PauseInputProvider : BaseInput, IPauseInput
    {
        public event Action Cancelled;

        public override void CancelInvoked() { Cancelled?.Invoke(); }
    }
}