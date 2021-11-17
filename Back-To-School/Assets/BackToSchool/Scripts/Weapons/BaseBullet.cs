using Assets.BackToSchool.Scripts.Interfaces;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public abstract class BaseBullet : MonoBehaviour
    {
        [SerializeField] protected float _lifeTime = 4f;

        protected Rigidbody _rigidbody;

        protected float _bulletDamage;

        public void SetDamage(float damage) { _bulletDamage = damage; }

        public virtual void Launch(float force)
        {
            Destroy(gameObject, _lifeTime);
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