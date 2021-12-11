using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IView
    {
        public void SetRoot(RectTransform canvas);
        public void Enable();
        public void Disable();
        public void Show();
        public void Hide();
    }
}