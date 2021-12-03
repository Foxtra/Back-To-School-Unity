using Assets.BackToSchool.Scripts.Interfaces.Components;
using Assets.BackToSchool.Scripts.Weapons;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Enemies
{
    public class EnemyShaman : BaseEnemy, IShootable
    {
        [SerializeField] private Transform _shootingPosition;
        [SerializeField] private FireBall _bulletPrefab;
        [SerializeField] private float _bulletForce;
        [SerializeField] private float _attackInterval = 2f;

        private FireBall _bullet;
        private float _timer;

        public void Fire()
        {
            _bullet                    = Instantiate(_bulletPrefab);
            _bullet.transform.position = _shootingPosition.position;
            _bullet.transform.rotation = _shootingPosition.rotation;
            _bullet.transform.parent   = null;
            _bullet.SetDamage(_enemyDamage);
            _bullet.Launch(_bulletForce);
        }

        protected override void Attack()
        {
            if (_timer > _attackInterval)
            {
                base.Attack();
                Fire();
                _timer = 0f;
            }
            else
                EnableEnemy();
        }

        private void FixedUpdate() { _timer += Time.fixedDeltaTime; }
    }
}