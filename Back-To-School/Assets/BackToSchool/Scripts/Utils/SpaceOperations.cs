using System;
using UnityEngine;


namespace Assets.BackToSchool.Scripts.Utils
{
    public static class SpaceOperations
    {
        /// <summary>Check if two object are close.</summary>
        /// <param name="objOnePos">Position of first object.</param>
        /// <param name="objTwoPos">Position of second object</param>
        /// <param name="maxRange">Max distance between them.</param>
        /// <returns>
        ///     Return true if distance between <paramref name="objOnePos" /> and <paramref name="objTwoPos" />
        ///     less than <paramref name="maxRange" />.
        /// </returns>
        public static bool CheckIfTwoObjectsClose(Vector3 objOnePos, Vector3 objTwoPos, float maxRange)
        {
            var heading = objOnePos - objTwoPos;

            return heading.sqrMagnitude < Math.Pow(maxRange, 2);
        }
    }
}