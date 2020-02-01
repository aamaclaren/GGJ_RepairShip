using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolSpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 20;
    [SerializeField] float spawnInterval = 1;

    [SerializeField] float minSpawRange = 5;
    [SerializeField] float maxSpawnRange = 20;

    private float time;
    private IEnumerator spawnCoroutine;

    private ResourcePool pool;
    // Start is called before the first frame update
    void Start()
    {
        pool = new ResourcePool(prefab, poolSize);
    }

    void Update() {
        checkForSpawn();
    }

    private void checkForSpawn() {
        time += Time.deltaTime;

        if (time >= spawnInterval) {
            time = 0;
            spawnAroundPlayer();
        }
    }

    private void spawnAroundPlayer(){
        Vector3 spawnPosOffset = Random.insideUnitSphere * Random.Range(minSpawRange, maxSpawnRange);
        pool.Spawn(player.transform.position + spawnPosOffset, Quaternion.identity);
    }
}
