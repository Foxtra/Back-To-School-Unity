using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IPlayerInput : IRawInputProvider, IInputProvider
    {
        public event Action<Vector3> Moved;
        public event Action<Vector3> Rotated;
        public event Action Stopped;
        public event Action Fired;
        public event Action Reloaded;
        public event Action<bool> WeaponChanged;
    }
}