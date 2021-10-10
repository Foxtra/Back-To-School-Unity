using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private float _rayCastLenght = 100f;

        private Camera _camera;
        private Vector3 _direction = Vector3.zero;
        private RaycastHit _previousHit;
        private RaycastHit _hit;
        private Ray _ray;
        public event Action<Vector3> Moved;
        public event Action<RaycastHit> Rotated;
        public event Action Stopped;
        public event Action Fired;
        public event Action Reloaded;
        public event Action Canceled;

        public void SetCamera(Camera camera) => _camera = camera;

        private void Update()
        {
            CheckDirection();
            CheckRay();
            CheckFire();
            CheckReload();
            CheckCancel();
        }

        private void CheckCancel()
        {
            if (Input.GetButtonDown("Cancel")) Canceled?.Invoke();
        }

        private void CheckReload()
        {
            if (Input.GetButtonDown("Reload")) Reloaded?.Invoke();
        }

        private void CheckFire()
        {
            if (Input.GetButtonDown("Fire1")) Fired?.Invoke();
        }

        private void CheckRay()
        {
            _ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(_ray, out _hit, _rayCastLenght, _layerMask)) return;
            if (_previousHit.Equals(_hit)) return;

            _previousHit = _hit;
            Rotated?.Invoke(_hit);
        }

        private void CheckDirection()
        {
            _direction.x = Input.GetAxisRaw("Horizontal");
            _direction.z = Input.GetAxisRaw("Vertical");

            if (_direction != Vector3.zero) { Moved?.Invoke(_direction); }
            else { Stopped?.Invoke(); }
        }
    }
}