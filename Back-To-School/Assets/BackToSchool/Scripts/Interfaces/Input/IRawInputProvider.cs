using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IRawInputProvider
    {
        public void DirectionChangeInvoked(Vector3 direction);
        public void RotateInvoked(Vector3 position);
        public void FireInvoked();
        public void ReloadInvoked();
        public void CancelInvoked();
        public void ScrollInvoked(float scrollValue);
    }
}