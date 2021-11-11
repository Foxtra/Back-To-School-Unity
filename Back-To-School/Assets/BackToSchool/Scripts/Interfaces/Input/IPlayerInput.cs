using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.Input
{
    public interface IPlayerInput
    {
        event Action<Vector3> Moved;
        event Action<Vector3> Rotated;
        event Action Stopped;
        event Action Fired;
        event Action Reloaded;
        event Action<bool> WeaponChanged;
    }
}