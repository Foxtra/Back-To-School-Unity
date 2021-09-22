using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;

        private Vector3 _velocity = Vector3.zero;

        private void FixedUpdate()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target.position + _offset, ref _velocity, _smoothSpeed);
        }
    }
}