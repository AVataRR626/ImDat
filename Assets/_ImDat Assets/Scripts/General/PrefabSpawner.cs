using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public Transform spawnPoint;
    public int spawnIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public void SpawnObject()
    {
        SpawnObject(spawnIndex);
    }

    public void SpawnObject(int i)
    {
        GameObject newSpawn = Instantiate(spawnObjects[i], spawnPoint.position, spawnPoint.rotation);
    }
}
