using UnityEngine;


namespace Assets.BackToSchool.Scripts.Player
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform Target;
        public Vector3 Offset;
        public float SmoothSpeed = 0.125f;

        private Vector3 _velocity = Vector3.zero;

        private void FixedUpdate()
        {
            var desiredPosition = Target.position + Offset;
            var smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, SmoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}