using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;

        private Transform _target;
        private Vector3 _velocity = Vector3.zero;

        public void SetTarget(Transform target) => _target = target;

        private void FixedUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, _smoothSpeed);
        }
    }
}