namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IInputManager
    {
        void Subscribe(IRawInputProvider provider);
        void Unsubscribe(IRawInputProvider provider);
    }
}