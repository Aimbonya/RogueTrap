using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{

    public Vector2Int position {  get; set; }


   
    public DungeonCrawler(Vector2Int startPos)
    {
         position = startPos;
    }

    public Vector2Int Move( Dictionary<Direction, Vector2Int> directionMovMap)
    {
        Direction toMove = (Direction)Random.Range(0, directionMovMap.Count);
        position += directionMovMap[toMove];
        return position;
    }
}
