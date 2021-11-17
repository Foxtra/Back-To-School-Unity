using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    internal interface IPauseInput : IInputProvider
    {
        event Action Cancelled;
    }
}