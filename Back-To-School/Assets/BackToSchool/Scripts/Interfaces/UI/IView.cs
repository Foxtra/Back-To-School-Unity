using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IView
    {
        public void SetRoot(RectTransform canvas);
        public void ShowView();
        public void HideView();
    }
}