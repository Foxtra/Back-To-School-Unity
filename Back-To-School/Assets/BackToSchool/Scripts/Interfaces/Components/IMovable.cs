using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IMovable
    {
        void Move(Vector3 direction);
        void Stop();
        void Rotate(Vector3 pointToRotate);
    }
}