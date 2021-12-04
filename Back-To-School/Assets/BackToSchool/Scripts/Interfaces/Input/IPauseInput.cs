using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    internal interface IPauseInput : IInputProvider
    {
        public event Action Cancelled;
    }
}