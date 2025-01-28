using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData generationData;

    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(generationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable <Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0,0);
        RoomController.instance.LoadRoom("Shop", 1, 0);
        foreach (var room in rooms)
        {
            RoomController.instance.LoadRoom("Empty", room.x, room.y);          
        }

    }
}

