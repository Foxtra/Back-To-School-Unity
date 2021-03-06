using System;
using Assets.BackToSchool.Scripts.Enums;
using Assets.BackToSchool.Scripts.Extensions;
using Assets.BackToSchool.Scripts.Interfaces.Input;
using Assets.BackToSchool.Scripts.Parameters;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Inputs
{
    public class PlayerInputProvider : Input, IPlayerInput
    {
        public event Action<Vector3> Moved;
        public event Action<Vector3> Rotated;
        public event Action Stopped;
        public event Action Fired;
        public event Action Reloaded;
        public event Action<bool> WeaponChanged;

        private readonly Camera _mainCamera;
        private RaycastHit _previousHit;
        private RaycastHit _hit;
        private Ray _ray;
        private LayerMask _layerMask = LayerMask.GetMask(ELayers.Ground.ToStringCached());

        private float _rayCastLength = Constants.Camera.RayCastLength;

        public PlayerInputProvider(Camera mainCamera) => _mainCamera = mainCamera;

        public override void FireInvoked() => Fired?.Invoke();

        public override void DirectionChangeInvoked(Vector3 direction)
        {
            if (_isPaused)
                return;

            if (direction != Vector3.zero)
                Moved?.Invoke(direction);
            else
                Stopped?.Invoke();
        }

        public override void ScrollInvoked(float scrollValue)
        {
            if (_isPaused)
                return;

            if (scrollValue > 0)
                WeaponChanged?.Invoke(true);
            else if (scrollValue < 0)
                WeaponChanged?.Invoke(false);
        }

        public override void ReloadInvoked() => Reloaded?.Invoke();

        public override void RotateInvoked(Vector3 mousePosition)
        {
            if (_isPaused)
                return;

            _ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(_ray, out _hit, _rayCastLength, _layerMask)) return;
            if (_previousHit.Equals(_hit)) return;

            _previousHit = _hit;
            Rotated?.Invoke(_hit.point);
        }
    }
}