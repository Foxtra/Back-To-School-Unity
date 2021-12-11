using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Components
{
    public interface IBullet
    {
        public GameObject gameObject { get; }
        void SetDamage(float damage);
        void Launch(float force);
    }
}