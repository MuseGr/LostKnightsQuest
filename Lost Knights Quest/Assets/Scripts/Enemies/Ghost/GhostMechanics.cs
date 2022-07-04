using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMechanics : MonoBehaviour
{
    public GameObject skeletonPrefab;
    public Animator animator;

    bool didSpawn = false;

    float timeOfSpawn;
    public float timeToStartSpawning = 1;
    public int numOfSkeletons = 3;

    private void Start()
    {
        timeOfSpawn = Time.time;
    }

    private void Update()
    {
        if(timeOfSpawn + timeToStartSpawning <= Time.time && !didSpawn)
        {
            SpawnSkeletons();
        }
        if (Time.time >= timeOfSpawn + 3)
            animator.SetTrigger("die");
            
    }
    void SpawnSkeletons()
    {
        for(int i = 0; i < numOfSkeletons; i++)
        {
            SpawnSkeleton(i + 1);
        }
        didSpawn = true;
    }
    void SpawnSkeleton(int distance)
    {
        Instantiate(skeletonPrefab, new Vector2(transform.position.x - distance, transform.position.y), Quaternion.identity);
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
