using Assets.BackToSchool.Scripts.Interfaces.Components;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class Rocket : MonoBehaviour
    {
        // --- Config ---
        public float speed = 100;
        public LayerMask collisionLayerMask;

        // --- Explosion VFX ---
        public GameObject rocketExplosion;

        // --- Projectile Mesh ---
        public MeshRenderer projectileMesh;

        // --- Script Variables ---
        private bool targetHit;

        // --- Audio ---
        public AudioSource inFlightAudioSource;

        // --- VFX ---
        public ParticleSystem disableOnHit;

        [SerializeField] private float _radius;

        private float _rocketDamage;

        public void SetDamage(float damage) => _rocketDamage = damage;

        private void Update()
        {
            // --- Check to see if the target has been hit. We don't want to update the position if the target was hit ---
            if (targetHit) return;

            // --- moves the game object in the forward direction at the defined speed ---
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        /// <summary>
        ///     Explodes on contact.
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            // --- return if not enabled because OnCollision is still called if compoenent is disabled ---
            if (!enabled) return;

            // --- Explode when hitting an object and disable the projectile mesh ---
            Explode();
            projectileMesh.enabled = false;
            targetHit              = true;
            inFlightAudioSource.Stop();
            foreach (var col in GetComponents<Collider>())
                col.enabled = false;

            disableOnHit.Stop();

            DamageEnemiesInArea(collision.transform.position, _radius, _rocketDamage);

            // --- Destroy this object after 2 seconds. Using a delay because the particle system needs to finish ---
            Destroy(gameObject, 5f);
        }

        /// <summary>
        ///     Instantiates an explode object.
        /// </summary>
        private void Explode()
        {
            // --- Instantiate new explosion option. I would recommend using an object pool ---
            var newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);
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