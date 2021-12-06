using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IMovable
    {
        public void Move(Vector3 direction);
        public void Stop();
        public void Rotate(Vector3 pointToRotate);
    }
}