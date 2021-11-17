using Assets.BackToSchool.Scripts.Interfaces.Input;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Inputs
{
    public class BaseInput : IRawInputProvider, IInputProvider
    {
        protected bool _isPaused;

        public virtual void DirectionChangeInvoked(Vector3 direction) { }
        public virtual void RotateInvoked(Vector3 position)           { }
        public virtual void FireInvoked()                             { }
        public virtual void ReloadInvoked()                           { }
        public virtual void CancelInvoked()                           { }
        public virtual void ScrollInvoked(float scrollValue)          { }
        public         void TogglePause(bool isPause)                 => _isPaused = isPause;
    }
}