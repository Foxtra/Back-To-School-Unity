using System;


namespace Assets.BackToSchool.Scripts.Interfaces.UI
{
    public interface IViewFactory
    {
        public T CreateView<T, E>(E item)
            where T : IView
            where E : Enum;
    }
}