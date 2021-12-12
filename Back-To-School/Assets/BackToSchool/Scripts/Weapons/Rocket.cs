using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class Rocket : Bullet
    {
        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool targetHit;

        // --- Audio ---
        public AudioSource inFlightAudioSource;

        // --- VFX ---
        public ParticleSystem disableOnHit;

        [SerializeField] private float _radius;

        private IResourceManager _resourceManager;

        public void Initialize(IResourceManager resourceManager) => _resourceManager = resourceManager;

        public override void Launch(float force)
        {
            Destroy(gameObject, Constants.WeaponStats.BulletLifeTime);
            _rigidbody = GetComponentInChildren<Rigidbody>();

            var impulse = transform.forward * _rigidbody.mass * force;
            _rigidbody.AddForce(impulse, ForceMode.Impulse);
        }

        /// <summary>
        ///     Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            // --- return if not enabled because OnCollision is still called if component is disabled ---
            if (!enabled) return;

            // --- Explode when hitting an object and disable the projectile mesh ---
            Explode();
            projectileMesh.enabled = false;
            targetHit              = true;
            inFlightAudioSource.Stop();
            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            disableOnHit.Stop();

            DamageEnemiesInArea(collision.transform.position, _radius, _bulletDamage);

            // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
            Destroy(gameObject, 5f);
        }

        /// <summary>
        ///     Instantiates an explode object.
        /// </summary>
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            var newExplosion = _resourceManager.CreateExplosion(transform);
        }

        private void DamageEnemiesInArea(Vector3 location, float radius, float damage)
        {
            var objectsInRange = Physics.OverlapSphere(location, radius);
            foreach (var col in objectsInRange)
            {
                var enemy = col.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    // linear falloff of effect
                    //float proximity = (location - enemy.transform.position).magnitude;
                    //float effect = 1 - (proximity / radius);

                    enemy.TakeDamage(damage);
                }
            }
        }
    }
}