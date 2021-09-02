using Runner.World.LevelTemplates;
using System.Collections;
using UnityEngine;

namespace Runner.World
{
    public abstract class LevelItem : MonoBehaviour
    {
        public Transform pointStart;
        public Transform pointEnd;
    }
}