using System;
using Assets.BackToSchool.Scripts.Interfaces.Core;
using Assets.BackToSchool.Scripts.Interfaces.UI;


namespace Assets.BackToSchool.Scripts.UI
{
    public class ViewFactory : IViewFactory
    {
        private ISystemResourceManager _resourceManager;
        private IUIRoot _uiRoot;

        public ViewFactory(ISystemResourceManager resourceManager, IUIRoot uiRoot)
        {
            _resourceManager = resourceManager;
            _uiRoot          = uiRoot;
        }

        public T CreateView<T, E>(E item)
            where T : IView
            where E : Enum
        {
            var viewObj = _resourceManager.CreatePrefabInstance<T, E>(item);
            viewObj.SetRoot(_uiRoot.OverlayCanvas);

            return viewObj;
        }
    }
}