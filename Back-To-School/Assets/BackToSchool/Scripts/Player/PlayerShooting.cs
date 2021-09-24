using Assets.BackToSchool.Scripts.Constants;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private GameObject _shootingPosition;
        [SerializeField] private float _reloadTime = 2.0f;
        [SerializeField] private float _bulletForce = 15.0f;
        [SerializeField] private int _maxAmmo = 10;

        private Bullet _bullet;
        private Animator _animator;

        private int _currentAmmo;
        private bool _isReloading;

        private void Awake()
        {
            _currentAmmo = _maxAmmo;
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            Fire();
        }

        private void Fire()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!_isReloading && _currentAmmo != 0)
                {
                    _bullet = Instantiate(_bulletPrefab);
                    _bullet.transform.position = _shootingPosition.transform.position;
                    _bullet.transform.rotation = _shootingPosition.transform.rotation;
                    _bullet.Launch(_bulletForce);
                    _currentAmmo--;
                }
            }

            if (Input.GetButtonDown("Reload"))
            {
                _isReloading = true;
                _currentAmmo = _maxAmmo;
                _animator.SetTrigger(AnimationStates.Reload);
                Invoke(nameof(Reload), _reloadTime);
            }
        }

        private void Reload()
        {
            _isReloading = false;
        }
    }
}