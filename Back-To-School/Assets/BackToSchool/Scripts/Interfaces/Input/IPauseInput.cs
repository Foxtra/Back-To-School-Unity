using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    internal interface IPauseInput
    {
        event Action Cancelled;
    }
}