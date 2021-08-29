using UnityEngine;

namespace Runner.Util
{
    public static class RandomUtil
    {
        /// <summary>
        /// Returns start + random * variance
        /// </summary>
        /// <param name="baseVal">Minimum value</param>
        /// <param name="randomVal">Maximum delta</param>
        /// <returns></returns>
        public static float Variance(float baseVal, float randomVal)
        {
            return baseVal + (Random.Range(0f, 1f) * randomVal);
        }

        public static bool RandomBool(float trueChance)
        {
            return Random.Range(0f, 1f) <= trueChance;
        }

        public static bool RandomBool() => RandomBool(0.5f);
    }
}