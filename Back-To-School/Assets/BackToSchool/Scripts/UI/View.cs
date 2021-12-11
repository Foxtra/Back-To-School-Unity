using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.UI
{
    public class View : MonoBehaviour, IView
    {
        public void SetRoot(RectTransform canvas) => transform.SetParent(canvas, false);

        public void Enable()  => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);

        public async void Show() { }
        public async void Hide() { }
    }
}