using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces
{
    public interface IMovable
    {
        void Move(Vector3 direction);
        void Stop();
        void Rotate(RaycastHit rayCastHit);
    }
}