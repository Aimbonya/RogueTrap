using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    up=0, left=1, dowm=2, right=3

}

public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited= new List<Vector2Int>();

    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.right, Vector2Int.right },
        {Direction.left, Vector2Int.left },
        {Direction.dowm, Vector2Int.down },
        {Direction.up, Vector2Int.up },
    };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>();

        for (int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }
        int Iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for(int i = 0;i<Iterations; i++)
        {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newpos = dungeonCrawler.Move(directionMovementMap);
                positionsVisited.Add(newpos);
            }
        }

        return positionsVisited;
    }
}

