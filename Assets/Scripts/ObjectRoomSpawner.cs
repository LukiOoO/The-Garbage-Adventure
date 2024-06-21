using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]

   public struct RandomSpawner
    {
        public string name;

        public SpawnerData SpawnerData;
    }

    public GridController grid;

    public RandomSpawner[] spawnerData;

    void Start()
    {
        grid = GetComponentInChildren<GridController>();
    }

    public void InitialiseObjectSpawning()
    {
        foreach(RandomSpawner rs in spawnerData)
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        int randomIteration = Random.Range(data.SpawnerData.minSpawn, data.SpawnerData.maxSpawn + 1);

        for (int i = 0; i<randomIteration; i++)
        {
            int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
            GameObject go = Instantiate(data.SpawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
            grid.availablePoints.RemoveAt(randomPos);
            Debug.Log("Spawned Object!");
        }

        
    }
}
