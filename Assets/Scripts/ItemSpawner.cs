using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]

    public struct Spawnable
    {
        public GameObject gameObject;

        public float weight;
    }


    public List<Spawnable> items = new List<Spawnable>();

    float totalWegiht;

    void Awake()
    {
        totalWegiht = 0;
        foreach(var spawnable in items)
        {
            totalWegiht += spawnable.weight;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        float pick = Random.value * totalWegiht;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;

        while(pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }

        GameObject i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
