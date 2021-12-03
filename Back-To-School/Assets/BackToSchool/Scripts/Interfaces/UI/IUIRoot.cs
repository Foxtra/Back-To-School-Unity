using UnityEngine;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IUIRoot
    {
        public Transform WorldspaceCanvas { get; }

        public RectTransform OverlayCanvas { get; }
    }
}