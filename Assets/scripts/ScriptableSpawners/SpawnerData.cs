using UnityEngine;


[CreateAssetMenu(fileName ="spawner.asset", menuName = "Spawner/Spawner")]
public class SpawnerData : ScriptableObject
{

    public GameObject itemToSpawn;

    public int minSpawnAmount;

    public int maxSpawnAmount;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
