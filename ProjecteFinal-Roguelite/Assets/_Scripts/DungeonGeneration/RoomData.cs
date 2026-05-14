using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Roguelite.DungeonGeneration
{
    public class RoomData : ScriptableObject
    {
        public Vector2Int size;

        public List<TileData> floorTiles = new();
        public List<TileData> wallTiles = new();
        public List<TileData> noclipWallTiles = new();
        public List<TileData> decoTiles = new();
    }

    [Serializable]
    public struct TileData
    {
        public Vector2Int position;
        public TileBase tile;

        public TileData(Vector2Int pos, TileBase tile)
        {
            this.position = pos;
            this.tile = tile;
        }
    }
}
