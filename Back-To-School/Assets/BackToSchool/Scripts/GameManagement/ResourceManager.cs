using Assets.BackToSchool.Scripts.Interfaces;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.GameManagement
{
    public class ResourceManager : IResourceManager
    {
        public GameObject GetPrefab(string path)
        {
            var obj = Resources.Load(path) as GameObject;
            return obj;
        }
    }
}