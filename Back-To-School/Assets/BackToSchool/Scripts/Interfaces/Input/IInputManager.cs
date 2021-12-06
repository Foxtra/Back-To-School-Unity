namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IInputManager
    {
        public void Subscribe(IRawInputProvider provider);
        public void Unsubscribe(IRawInputProvider provider);
    }
}