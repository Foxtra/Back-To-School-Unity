using Assets.BackToSchool.Scripts.Interfaces;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 4f;

        private Rigidbody _rigidbody;

        private float _bulletDamage;

        public void SetDamage(float damage) { _bulletDamage = damage; }

        public void Launch(float force)
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
            {
                enemy.TakeDamage(_bulletDamage);
                Destroy(gameObject);
            }
        }
    }
}