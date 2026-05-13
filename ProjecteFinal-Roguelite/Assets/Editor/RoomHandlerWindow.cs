using Roguelite.LevelGeneration;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Roguelite
{
    public class RoomHandlerWindow : EditorWindow
    {
        private Tilemap _floorTilemap;
        private Tilemap _wallTilemap;
        private Tilemap _noclipWallTilemap;
        private Tilemap _decoTilemap;

        private string _newRoomName = "NewRoom";

        private RoomData _room;

        [MenuItem("Dungeon/Room Handler")]
        public static void ShowWindow()
        {
            GetWindow<RoomHandlerWindow>("Room Handler");
        }

        private void OnGUI()
        {
            GUILayout.Label("Save Room", EditorStyles.whiteLargeLabel);

            GUILayout.Space(10);

            GUILayout.Label("Grid Tilemaps", EditorStyles.boldLabel);
            _floorTilemap = (Tilemap)EditorGUILayout.ObjectField("Floor Tilemap", _floorTilemap, typeof(Tilemap), true);
            _wallTilemap = (Tilemap)EditorGUILayout.ObjectField("Wall Tilemap", _wallTilemap, typeof(Tilemap), true);
            _noclipWallTilemap = (Tilemap)EditorGUILayout.ObjectField("NoclipWall Tilemap", _noclipWallTilemap, typeof(Tilemap), true);
            _decoTilemap = (Tilemap)EditorGUILayout.ObjectField("Decoration Tilemap", _decoTilemap, typeof(Tilemap), true);

            GUILayout.Space(10);

            GUILayout.Label("New Room Name", EditorStyles.boldLabel);
            _newRoomName = EditorGUILayout.TextField("Room Name", _newRoomName);

            GUILayout.Space(10);

            if (GUILayout.Button("Save"))
            {
                SaveRoom();
            }

            GUILayout.Space(50);

            GUILayout.Label("Load Room", EditorStyles.whiteLargeLabel);

            GUILayout.Space(10);

            _room = (RoomData)EditorGUILayout.ObjectField("Room to Load", _room, typeof(RoomData), true);

            GUILayout.Space(10);

            if (GUILayout.Button("Load"))
            {
                LoadRoom();
            }
        }

        private void SaveRoom()
        {

        }

        private void LoadRoom()
        {

        }
    }
}
