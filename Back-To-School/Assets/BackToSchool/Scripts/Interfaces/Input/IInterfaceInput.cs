using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IInterfaceInput : IRawInputProvider
    {
        event Action PauseTriggered;
        event Action UpgradeMenuTriggered;
        event Action InventoryTriggered;

        event Action MoveUpTriggered;
        event Action MoveDownTriggered;
        event Action MoveLeftTriggered;
        event Action MoveRightTriggered;
    }
}