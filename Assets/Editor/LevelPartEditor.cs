using Runner.World;
using System.Collections.Generic;
using Runner.World.LevelTemplates;
using UnityEditor;
using UnityEngine;

namespace Runner.Editor
{
    public class LevelItemEditor : UnityEditor.Editor
    {
        private LevelItem _target;

        public void OnEnable()
        {
            _target = target as LevelItem;
        }

        public List<Vector3> GetTilesPos()
        {
            float tileSize = LevelGenerator.TileSize;
            List<Vector3> list = new List<Vector3>();

            for (float x = -tileSize; x <= tileSize; x += tileSize)
            {
                for (float z = _target.pointStart.position.z + tileSize * 0.5f; z < _target.pointEnd.position.z; z += tileSize)
                {
                    list.Add(new Vector3(x, 0f, z));
                }
            }

            return list;
        }

        private void DrawCube(Vector3 pos, float size, float thickness = 2f)
        {
            size *= 0.5f;

            Handles.DrawLine(pos + new Vector3(-size, 0f, -size), pos + new Vector3(size, 0f, -size), thickness);
            Handles.DrawLine(pos + new Vector3(size, 0f, -size), pos + new Vector3(size, 0f, size), thickness);
            Handles.DrawLine(pos + new Vector3(size, 0f, size), pos + new Vector3(-size, 0f, size), thickness);
            Handles.DrawLine(pos + new Vector3(-size, 0f, size), pos + new Vector3(-size, 0f, -size), thickness);
            Handles.DrawLine(pos + new Vector3(-size, 0f, -size), pos + new Vector3(size, 0f, size), thickness);
            Handles.DrawLine(pos + new Vector3(size, 0f, -size), pos + new Vector3(-size, 0f, size), thickness);
        }

        private string FormatPos(Vector3 pos) => $"{pos.x:0.0}; {pos.z:0.0}";

        public void OnSceneGUI()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.black;

            Handles.color = Color.red;
            foreach (Vector3 pos in GetTilesPos())
            {
                DrawCube(pos, 2f, 3f);
                Handles.Label(pos, FormatPos(pos), style);
            }

            //Handles.color = Color.yellow;
            //foreach (Transform tile in _target.GetCachedTiles())
            //{
            //    var pos = tile.position;
            //    DrawCube(pos, 2f, 1f);
            //    Handles.Label(pos, FormatPos(pos), style);
            //}
        }
    }

    [CustomEditor(typeof(LevelTransfer))]
    public class LevelTransferEditor : LevelItemEditor { }
    [CustomEditor(typeof(LevelPart))]
    public class LevelPartEditor : LevelItemEditor { }
}