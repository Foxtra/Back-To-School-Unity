using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IRawInputProvider
    {
        void DirectionChangeInvoked(Vector3 direction);
        void RotateInvoked(Vector3 position);
        void FireInvoked();
        void ReloadInvoked();
        void CancelInvoked();
        void ScrollInvoked(float scrollValue);
    }
}