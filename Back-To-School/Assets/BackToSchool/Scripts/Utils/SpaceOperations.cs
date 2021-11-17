using System;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.BackToSchool.Scripts.Utils
{
    public static class SpaceOperations
    {
        /// <summary>Check if two object are close.</summary>
        /// <param name="objOnePos">Position of first object.</param>
        /// <param name="objTwoPos">Position of second object</param>
        /// <param name="maxRange">Max distance between them.</param>
        /// <returns>
        ///     Returns true if distance between <paramref name="objOnePos" /> and <paramref name="objTwoPos" />
        ///     less than <paramref name="maxRange" />.
        /// </returns>
        public static bool CheckIfTwoObjectsClose(Vector3 objOnePos, Vector3 objTwoPos, float maxRange)
        {
            var heading = objOnePos - objTwoPos;

            return heading.sqrMagnitude < Math.Pow(maxRange, 2);
        }

        /// <summary>Generates position on game field.</summary>
        /// <param name="minXpos">Minimum value for X coordinate.</param>
        /// <param name="maxXpos">Maximum value for X coordinate.</param>
        /// <param name="minZpos">Minimum value for Z coordinate.</param>
        /// <param name="maxZpos">Maximum value for Z coordinate.</param>
        /// <returns>
        ///     Returns new Vector3 with random generated X and Z coordinates./>.
        /// </returns>
        public static Vector3 GeneratePositionOnField(float minXpos, float maxXpos, float minZpos, float maxZpos)
        {
            var xPos = Random.Range(minXpos, maxXpos);
            var zPos = Random.Range(minZpos, maxZpos);
            var yPos = 0f;

            return new Vector3(xPos, yPos, zPos);
        }
    }
}