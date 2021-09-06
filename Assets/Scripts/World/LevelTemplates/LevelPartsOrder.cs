using UnityEngine;

namespace Runner.World.LevelTemplates
{

    public abstract class LevelPartsOrder : ScriptableObject
    {
        public abstract GameObject GetNextLevel();
        public abstract GameObject GetAndRemoveNextLevel();
        public abstract GameObject GetLevel(int idx);
        public abstract void RemoveLevel(int idx);
    }
}