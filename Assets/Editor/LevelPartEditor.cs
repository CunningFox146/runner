using Runner.Environment;
using UnityEditor;
using UnityEngine;

namespace Runner.Editor
{
    [CustomEditor(typeof(LevelPart))]
    public class LevelPieceEditor : UnityEditor.Editor
    {
        private LevelPart _target;

        public void OnEnable()
        {
            _target = target as LevelPart;
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
            foreach (Vector3 pos in _target.GetTilesPos())
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
}