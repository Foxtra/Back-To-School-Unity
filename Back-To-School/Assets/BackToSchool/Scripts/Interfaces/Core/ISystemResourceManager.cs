using System;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Assets.BackToSchool.Scripts.Interfaces.Core
{
    public interface ISystemResourceManager
    {
        public T GetPrefab<T, E>(E item)
            where T : Object
            where E : Enum;

        public T          CreatePrefabInstance<T, E>(E item) where E : Enum;
        public GameObject CreatePrefabInstance<E>(E item) where E : Enum;
    }
}