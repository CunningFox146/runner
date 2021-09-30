using UnityEngine;

namespace Runner.SoundSystem
{
    public static class SoundLoader
    {
        private static string Path = "SoundsData/";

        public static SoundData Get(string name)
        {
            //var sw = System.Diagnostics.Stopwatch.StartNew();
            SoundData sound = Resources.Load<SoundData>(Path + name);
            //sw.Stop();
            //Debug.Log($"Sound {Path} was loaded in {sw.Elapsed}");
            return sound;
        }

        public static void Unload(string name)
        {
            Resources.UnloadAsset(Get(name));
        }

        public static void UnloadUnused()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}
