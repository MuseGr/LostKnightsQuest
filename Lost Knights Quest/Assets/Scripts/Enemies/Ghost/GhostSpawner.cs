using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    public GameObject Ghost;
    public Transform playerPos;
    float timeOfLastSpawn;
    public float minSpawnTime = 15;
    public float maxSpawnTime = 20;

    public List<Transform> spawnPoints;

    void Start()
    {
        timeOfLastSpawn = Time.time;
    }
    void Update()
    {
        float randNum = Random.Range(minSpawnTime, maxSpawnTime);
        if(Time.time >= timeOfLastSpawn + randNum)
        {
            Vector2 pos = new Vector2(playerPos.position.x + 9, playerPos.position.y + 1);
            Instantiate(Ghost, pos, Quaternion.identity);

            timeOfLastSpawn = Time.time;
        }

        foreach(Transform point in spawnPoints)
        {
            if(Mathf.Abs(playerPos.position.x - point.position.x) <= 8)
            {
                Vector2 pos = new Vector2(playerPos.position.x + 9, playerPos.position.y + 1);
                Instantiate(Ghost, pos, Quaternion.identity);

                spawnPoints.Remove(point);
            }
        }
    }
}
