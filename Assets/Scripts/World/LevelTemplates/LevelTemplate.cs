using UnityEngine;

namespace Runner.World.LevelTemplates
{
    [CreateAssetMenu(fileName = "Template", menuName = "Scriptable Objects/LevelTemplate")]
    public class LevelTemplate : ScriptableObject
    {
        public LevelBiomes biome;
        public GameObject tilePrefab;
        public Color skyColor;
        [Space]
        [Header("Decor")]
        public GameObject decorPrefab;
        public float decorChance = 0.5f;
        public float spacing = 0.8f;
    }
}