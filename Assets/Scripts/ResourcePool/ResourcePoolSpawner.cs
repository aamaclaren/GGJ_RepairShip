using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int poolSize = 20;
    [SerializeField] float spawnInterval = 3;

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
            pool.Spawn(new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
