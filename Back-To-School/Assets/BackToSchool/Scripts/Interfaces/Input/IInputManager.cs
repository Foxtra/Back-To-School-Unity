namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IInputManager
    {
        void Subscribe(IInputProvider provider);
        void Unsubscribe(IInputProvider provider);
    }
}