using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Weapons
{
    public class FireBall : Bullet
    {
        public override void Launch(float force)
        {
            Destroy(gameObject, Constants.BulletLifeTime);
            _rigidbody = GetComponentInChildren<Rigidbody>();

            var impulse = transform.forward * _rigidbody.mass * force;
            _rigidbody.AddForce(impulse, ForceMode.Impulse);
        }
    }
}