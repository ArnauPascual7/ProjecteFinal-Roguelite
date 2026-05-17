using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = Roguelite.DungeonGeneration.TileData;

namespace Roguelite.DungeonGeneration
{
    public class RoomHandlerWindow : EditorWindow
    {
        private const string SAVE_PATH = "Assets/Resources/Rooms/";

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

            GUILayout.Space(50);

            GUILayout.Label("Clear Tilemaps", EditorStyles.whiteLargeLabel);

            GUILayout.Space(10);

            if (GUILayout.Button("Clear"))
            {
                ClearAllTilemaps();
            }
        }

        private void SaveRoom()
        {
            if (_floorTilemap == null || _wallTilemap == null || _noclipWallTilemap == null)
            {
                Debug.LogError("ROOM HANDLER WINDOW: One or more of the Tilemaps are Null");
                return;
            }

            _floorTilemap.CompressBounds();
            _wallTilemap.CompressBounds();
            _noclipWallTilemap.CompressBounds();

            if (_decoTilemap != null) _decoTilemap.CompressBounds();

            BoundsInt roomBounds = _wallTilemap.cellBounds;

            int width = roomBounds.size.x;
            int height = roomBounds.size.y;

            RoomData newRoom = ScriptableObject.CreateInstance<RoomData>();
            newRoom.size = new Vector2Int(width, height);
            newRoom.floorTiles = GetTilesData(_floorTilemap);
            newRoom.wallTiles = GetTilesData(_wallTilemap);
            newRoom.noclipWallTiles = GetTilesData(_noclipWallTilemap);
            if (_decoTilemap != null) newRoom.decoTiles = GetTilesData(_decoTilemap);
            newRoom.doors = GetDoorsData(_floorTilemap, _wallTilemap);

            AssetDatabase.CreateAsset(newRoom, SAVE_PATH + _newRoomName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ROOM HANDLER WINDOW: Room saved successfully at " + SAVE_PATH + _newRoomName + ".asset");
        }

        private List<TileData> GetTilesData(Tilemap tilemap)
        {
            List<TileData> tiles = new();

            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    TileBase tile = tilemap.GetTile(cellPos);

                    if (tile != null)
                    {
                        tiles.Add(new TileData(new Vector2Int(x, y), tile));
                    }
                }
            }
            return tiles;
        }

        private List<DoorData> GetDoorsData(Tilemap floor, Tilemap walls)
        {
            List<DoorData> doors = new();

            BoundsInt bounds = walls.cellBounds;

            List<Vector2Int> upDoorPositions = new List<Vector2Int>();
            List<Vector2Int> downDoorPositions = new List<Vector2Int>();
            List<Vector2Int> leftDoorPositions = new List<Vector2Int>();
            List<Vector2Int> rightDoorPositions = new List<Vector2Int>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);

                    TileBase wallTile = walls.GetTile(cellPos);
                    TileBase floorTile = floor.GetTile(cellPos);

                    if (x == bounds.xMin && (wallTile == null && floorTile != null))
                    {
                        leftDoorPositions.Add((Vector2Int)cellPos);
                    }

                    if (y == bounds.yMin && (wallTile == null && floorTile != null))
                    {
                        downDoorPositions.Add((Vector2Int)cellPos);
                    }

                    if (x == bounds.xMax - 1 && (wallTile == null && floorTile != null))
                    {
                        rightDoorPositions.Add((Vector2Int)cellPos);
                    }

                    if (x == bounds.yMax - 1 && (wallTile == null && floorTile != null))
                    {
                        upDoorPositions.Add((Vector2Int)cellPos);
                    }
                }
            }

            if (upDoorPositions.Count > 0) doors.Add(new DoorData(Direction.Up, upDoorPositions));
            if (downDoorPositions.Count > 0) doors.Add(new DoorData(Direction.Down, downDoorPositions));
            if (leftDoorPositions.Count > 0) doors.Add(new DoorData(Direction.Left, leftDoorPositions));
            if (rightDoorPositions.Count > 0) doors.Add(new DoorData(Direction.Right, rightDoorPositions));

            return doors;
        }

        private void LoadRoom()
        {
            ClearAllTilemaps();

            foreach (TileData tileData in _room.floorTiles)
            {
                _floorTilemap.SetTile(new Vector3Int(tileData.position.x, tileData.position.y, 0), tileData.tile);
            }

            foreach (TileData tileData in _room.wallTiles)
            {
                _wallTilemap.SetTile(new Vector3Int(tileData.position.x, tileData.position.y, 0), tileData.tile);
            }

            foreach (TileData tileData in _room.noclipWallTiles)
            {
                _noclipWallTilemap.SetTile(new Vector3Int(tileData.position.x, tileData.position.y, 0), tileData.tile);
            }

            Debug.Log("ROOM HANDLER WINDOW: Room loaded successfully from " + AssetDatabase.GetAssetPath(_room));
        }

        private void ClearAllTilemaps()
        {
            if (_floorTilemap != null) _floorTilemap.ClearAllTiles();
            if (_wallTilemap != null) _wallTilemap.ClearAllTiles();
            if (_noclipWallTilemap != null) _noclipWallTilemap.ClearAllTiles();
            if (_decoTilemap != null) _decoTilemap.ClearAllTiles();
        }
    }
}
