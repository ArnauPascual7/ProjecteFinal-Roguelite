using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Roguelite.DungeonGeneration
{
    public class DungeonGenerator : MonoBehaviour
    {
        [Header("Generation Settings")]
        [SerializeField] private Vector2Int _startingPosition;
        [SerializeField] private RoomData _startingRoom;
        [SerializeField] private int _roomCount;
        [SerializeField] private RoomData[] _rooms;
        [SerializeField] private RoomData[] _roomEnds;

        [Header("Paint Settings")]
        [SerializeField] private Tilemap _floorTilemap;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private Tilemap _noclipWallTilemap;
        [SerializeField] private Tilemap _decoTilemap;

        private List<RoomData> _doorUpRooms = new List<RoomData>();
        private List<RoomData> _doorDownRooms = new List<RoomData>();
        private List<RoomData> _doorLeftRooms = new List<RoomData>();
        private List<RoomData> _doorRightRooms = new List<RoomData>();

        private List<RoomData> _endUpRooms = new List<RoomData>();
        private List<RoomData> _endDownRooms = new List<RoomData>();
        private List<RoomData> _endLeftRooms = new List<RoomData>();
        private List<RoomData> _endRightRooms = new List<RoomData>();

        private Dictionary<Vector2Int, RealRoomData> _placedRooms = new();
        private Queue<(DoorData door, Vector2Int sourceGridPos)> _openDoors = new();

        private void Awake()
        {
            OrganizeRooms();
        }

        private void OrganizeRooms()
        {
            foreach (RoomData room in _rooms)
            {
                foreach (DoorData door in room.doors)
                {
                    if (door.direction == Direction.Up) _doorUpRooms.Add(room);
                    else if (door.direction == Direction.Down) _doorDownRooms.Add(room);
                    else if (door.direction == Direction.Left) _doorLeftRooms.Add(room);
                    else if (door.direction == Direction.Right) _doorRightRooms.Add(room);
                }
            }

            foreach (RoomData room in _roomEnds)
            {
                foreach (DoorData door in room.doors)
                {
                    if (door.direction == Direction.Up) _endUpRooms.Add(room);
                    else if (door.direction == Direction.Down) _endDownRooms.Add(room);
                    else if (door.direction == Direction.Left) _endLeftRooms.Add(room);
                    else if (door.direction == Direction.Right) _endRightRooms.Add(room);
                }
            }
        }

        private void Start()
        {
            _startingRoom.doors.ForEach(d => _openDoors.Enqueue((d, _startingPosition)));

            BuildDungeon();
            RenderDungeon();
        }

        private void BuildDungeon()
        {
            PlaceRoom(_startingPosition, _startingRoom, Vector2Int.zero);

            int count = 1;
            while (_openDoors.Count > 0 && count < _roomCount)
            {
                (DoorData door, Vector2Int sourceGridPos) = _openDoors.Dequeue();

                Direction incomingDir = Direction2DUtils.GetOposite(door.direction);
                Vector2Int newGridPos = sourceGridPos + Direction2DUtils.ToVector2Int(door.direction);

                RoomData newRoom = GetCompatibleRoom(newGridPos, incomingDir, isEndRoom: false);
                if (newRoom == null) continue;

                (_, Vector2Int newTileOffset) = CalculateNewRoomPlacement(sourceGridPos, door, newRoom);

                if (_placedRooms.ContainsKey(newGridPos) || OverlapsExistingRoom(newRoom, newTileOffset)) continue;

                PlaceRoom(newGridPos, newRoom, newTileOffset);
                count++;
            }

            CloseOpenDoors();
        }

        private void CloseOpenDoors()
        {
            while (_openDoors.Count > 0)
            {
                (DoorData door, Vector2Int sourceGridPos) = _openDoors.Dequeue();

                Direction incomingDir = Direction2DUtils.GetOposite(door.direction);
                Vector2Int newGridPos = sourceGridPos + Direction2DUtils.ToVector2Int(door.direction);

                if (_placedRooms.ContainsKey(newGridPos)) continue;

                RoomData endRoom = GetCompatibleRoom(newGridPos, incomingDir, isEndRoom: true);
                if (endRoom == null)
                {
                    Debug.LogWarning($"DUNGEON GENERATOR: No compatible end room found for direction {door.direction}");
                    continue;
                }

                (_, Vector2Int newTileOffset) = CalculateNewRoomPlacement(sourceGridPos, door, endRoom);

                if (OverlapsExistingRoom(endRoom, newTileOffset)) continue;

                PlaceRoom(newGridPos, endRoom, newTileOffset);
            }
        }

        private RoomData GetCompatibleRoom(Vector2Int newGridPos, Direction incomingDir, bool isEndRoom)
        {
            List<RoomData> pool = isEndRoom
                ? new List<RoomData>(_roomEnds)
                : new List<RoomData>(_rooms);

            List<RoomData> candidates = pool.FindAll(r =>
                r.doors.Exists(d => d.direction == incomingDir) &&
                IsRoomCompatibleWithNeighbours(r, newGridPos, incomingDir));

            if (candidates.Count == 0) return null;
            return candidates[Random.Range(0, candidates.Count)];
        }

        private bool IsRoomCompatibleWithNeighbours(RoomData candidate, Vector2Int newGridPos, Direction incomingDir)
        {
            foreach (Direction dir in System.Enum.GetValues(typeof(Direction)))
            {
                if (dir == incomingDir) continue;

                Vector2Int neighbourGridPos = newGridPos + Direction2DUtils.ToVector2Int(dir);
                if (!_placedRooms.ContainsKey(neighbourGridPos)) continue;

                RealRoomData neighbour = _placedRooms[neighbourGridPos];
                Direction oppositeDir = Direction2DUtils.GetOposite(dir);

                bool candidateHasDoorToNeighbour = candidate.doors.Exists(d => d.direction == dir);
                bool neighbourHasDoorToCandidate = neighbour.room.doors.Exists(d => d.direction == oppositeDir);

                if (candidateHasDoorToNeighbour && !neighbourHasDoorToCandidate) return false;

                if (neighbourHasDoorToCandidate && !candidateHasDoorToNeighbour) return false;
            }

            return true;
        }

        private void PlaceRoom(Vector2Int gridPos, RoomData room, Vector2Int tileOffset)
        {
            RealRoomData realRoom = new RealRoomData(gridPos, tileOffset, room);
            _placedRooms[gridPos] = realRoom;

            foreach (DoorData door in room.doors)
            {
                _openDoors.Enqueue((door, gridPos));
            }
        }

        private (Vector2Int gridPos, Vector2Int tileOffset) CalculateNewRoomPlacement(Vector2Int sourceGridPos, DoorData sourceDoor, RoomData newRoom)
        {
            RealRoomData sourceReal = _placedRooms[sourceGridPos];

            Vector2Int sourceDoorCenter = GetDoorCenter(sourceDoor);
            Direction connectingDir = Direction2DUtils.GetOposite(sourceDoor.direction);
            DoorData connectingDoor = newRoom.doors.Find(d => d.direction == connectingDir);
            Vector2Int newDoorCenter = GetDoorCenter(connectingDoor);

            Vector2Int sourceDoorWorldPos = sourceReal.tileOffset + sourceDoorCenter;
            Vector2Int dirVec = Direction2DUtils.ToVector2Int(sourceDoor.direction);
            Vector2Int newTileOffset = sourceDoorWorldPos + dirVec - newDoorCenter;

            Vector2Int newGridPos = sourceGridPos + dirVec;

            return (newGridPos, newTileOffset);
        }

        private bool OverlapsExistingRoom(RoomData newRoom, Vector2Int newTileOffset)
        {
            RectInt newBounds = GetRoomBounds(newRoom, newTileOffset);

            foreach (RealRoomData placed in _placedRooms.Values)
            {
                RectInt placedBounds = GetRoomBounds(placed.room, placed.tileOffset);

                if (newBounds.Overlaps(placedBounds))
                    return true;
            }

            return false;
        }

        private RectInt GetRoomBounds(RoomData room, Vector2Int tileOffset)
        {
            return new RectInt(tileOffset.x, tileOffset.y, room.size.x, room.size.y);
        }

        private Vector2Int GetDoorCenter(DoorData door)
        {
            Vector2Int sum = Vector2Int.zero;
            foreach (var pos in door.doorPositions) sum += pos;
            return sum / door.doorPositions.Count;
        }

        private RoomData GetRandomRoomByDirection(Direction dir)
        {
            if (dir == Direction.Up) return _doorUpRooms[Random.Range(0, _doorUpRooms.Count)];
            if (dir == Direction.Down) return _doorDownRooms[Random.Range(0, _doorDownRooms.Count)];
            if (dir == Direction.Left) return _doorLeftRooms[Random.Range(0, _doorLeftRooms.Count)];
            if (dir == Direction.Right) return _doorRightRooms[Random.Range(0, _doorRightRooms.Count)];

            Debug.LogError("DUNGEON GENERATOR: Invalid direction: " + dir);
            return default;
        }

        private void RenderDungeon()
        {
            foreach (RealRoomData realRoom in _placedRooms.Values)
            {
                RenderRoom(realRoom);
            }
        }

        private void RenderRoom(RealRoomData realRoom)
        {
            Vector2Int offset = realRoom.tileOffset;
            RoomData data = realRoom.room;

            PaintTiles(_floorTilemap, data.floorTiles, offset);
            PaintTiles(_wallTilemap, data.wallTiles, offset);
            PaintTiles(_noclipWallTilemap, data.noclipWallTiles, offset);
            if (data.decoTiles != null) PaintTiles(_decoTilemap, data.decoTiles, offset);
        }

        private void PaintTiles(Tilemap tilemap, List<TileData> tiles, Vector2Int offset)
        {
            foreach (TileData td in tiles)
            {
                Vector3Int worldPos = new Vector3Int(
                    td.position.x + offset.x,
                    td.position.y + offset.y,
                    0);
                tilemap.SetTile(worldPos, td.tile);
            }
        }
    }

    public struct RealRoomData
    {
        public Vector2Int gridPosition;
        public Vector2Int tileOffset;
        public RoomData room;
        public Dictionary<Direction, RoomData> neighbourRooms;

        public RealRoomData(Vector2Int gridPos, Vector2Int tileOff, RoomData room)
        {
            gridPosition = gridPos;
            tileOffset = tileOff;
            this.room = room;
            neighbourRooms = new();
        }
    }
}