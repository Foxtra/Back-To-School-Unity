using Assets.BackToSchool.Scripts.Interfaces.UI;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.UI
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField] private Transform _worldspaceCanvasTransform;
        [SerializeField] private RectTransform _overlayCanvasTransform;

        public Transform WorldspaceCanvas => _worldspaceCanvasTransform;
        public RectTransform OverlayCanvas => _overlayCanvasTransform;

        public void Initialize(Camera mainCamera)
        {
            var worldCanvas = WorldspaceCanvas.GetComponent<Canvas>();
            worldCanvas.worldCamera = mainCamera;
        }
    }
}