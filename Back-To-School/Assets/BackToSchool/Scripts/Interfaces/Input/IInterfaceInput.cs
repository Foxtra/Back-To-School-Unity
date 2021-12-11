using System;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IInterfaceInput : IRawInputProvider
    {
        public event Action PauseTriggered;
        public event Action UpgradeMenuTriggered;
        public event Action InventoryTriggered;

        public event Action MoveUpTriggered;
        public event Action MoveDownTriggered;
        public event Action MoveLeftTriggered;
        public event Action MoveRightTriggered;
    }
}