using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;

        public SpawnerData spawnerData;
    }

    public GridController grid;

    public RandomSpawner[] spawnerData; 

    private void Start()
    {
        //grid = GetComponent<GridController>();
    }

    public void InitializeObjectSpawning()
    {
        foreach(var spawner in spawnerData)
        {
                SpawnObjects(spawner);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        int randomIteration= Random.Range(data.spawnerData.minSpawnAmount, data.spawnerData.maxSpawnAmount+1);

        for(int i = 0;i< randomIteration; i++) 
        {
            int randPos= Random.Range(0, grid.availablePoints.Count-1);
            GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randPos], Quaternion.identity, transform) as GameObject;
            grid.availablePoints.RemoveAt(randPos);
            //Debug.Log("SpawnedObject");
        }

    }
}
