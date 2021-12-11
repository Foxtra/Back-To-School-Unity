using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class Bullet : MonoBehaviour, IBullet
    {
        protected Rigidbody _rigidbody;

        protected float _bulletDamage;

        public void SetDamage(float damage) => _bulletDamage = damage;

        public virtual void Launch(float force)
        {
            Destroy(gameObject, Constants.WeaponStats.BulletLifeTime);
            _rigidbody = GetComponentInChildren<Rigidbody>();

            var impulse = transform.up * _rigidbody.mass * force;
            _rigidbody.AddForce(impulse, ForceMode.Impulse);
        }

        public void OnTriggerEnter(Collider collider)
        {
            var enemy = collider.GetComponent<IDamageable>();
            if (enemy != null)
                enemy.TakeDamage(_bulletDamage);

            Destroy(gameObject);
        }
    }
}