using Runner.Environment;
using UnityEditor;
using UnityEngine;

namespace Runner.Editor
{
    [CustomEditor(typeof(SetPiece))]
    public class SetPieceEditor : UnityEditor.Editor
    {
        private SetPiece _target;

        public void OnEnable()
        {
            _target = target as SetPiece;
        }

        private void DrawCube(Vector3 pos, float size, float thickness = 2f)
        {
            size *= 0.5f;

            Handles.DrawLine(pos + new Vector3(-size, 0f, -size), pos + new Vector3(size, 0f, -size), thickness);
            Handles.DrawLine(pos + new Vector3(size, 0f, -size), pos + new Vector3(size, 0f, size), thickness);
            Handles.DrawLine(pos + new Vector3(size, 0f, size), pos + new Vector3(-size, 0f, size), thickness);
            Handles.DrawLine(pos + new Vector3(-size, 0f, size), pos + new Vector3(-size, 0f, -size), thickness);
        }

        public void OnSceneGUI()
        {
            _target.UpdateSetLength();
            if (_target.length <= 0f) return;

            GUIStyle styleMissing = new GUIStyle();
            styleMissing.normal.textColor = Color.red;
            GUIStyle styleFound = new GUIStyle();
            styleFound.normal.textColor = Color.cyan;

            Handles.color = Color.red;
            foreach (Vector3 pos in _target.CheckTiles())
            {
                DrawCube(pos, 2f);
                Handles.Label(pos, $"{pos.x}; {pos.z}", styleMissing);
            }
            Handles.color = Color.yellow;
            foreach (Vector3 pos in _target.GetCachedTiles())
            {
                DrawCube(pos, 2f, 5f);
                Handles.Label(pos, $"{pos.x}; {pos.z}", styleFound);
            }
            
        }
    }
}