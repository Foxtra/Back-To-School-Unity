using UnityEngine;


namespace Assets.BackToSchool.Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 4f;

        private Rigidbody _rigidbody;

        public void Launch(float force)
        {
            Destroy(gameObject, _lifeTime);
            _rigidbody = GetComponentInChildren<Rigidbody>();

            var impulse = transform.up * _rigidbody.mass * force;
            _rigidbody.AddForce(impulse, ForceMode.Impulse);
        }

        public void OnTriggerEnter(Collider collider)
        {
            var enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetDamage();
                Destroy(gameObject);
            }
        }
    }
}