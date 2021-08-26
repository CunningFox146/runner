using UnityEngine;

namespace Runner.Util
{
    public static class ArrayUtil
    {
        /// <summary>
        /// Returns random item from array. If array contains only one item, it will be returned.
        /// </summary>
        /// <typeparam name="T">Type of array items</typeparam>
        /// <param name="array">Array of items</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(T[] array)
        {
            int length = array.Length;
            if (length == 1) return array[0];

            int idx = Random.Range(0, length - 1);
            return array[idx];
        }

        /// <summary>
        /// Returns float between array[0] and array[1]. Returns array[0] is Length is 1.
        /// </summary>
        /// <param name="array">Array of numbers</param>
        /// <returns></returns>
        public static float GetRandomRangeFromArray(float[] array)
        {
            if (array.Length == 1) return array[0];
            return Random.Range(array[0], array[1]);
        }
    }
}