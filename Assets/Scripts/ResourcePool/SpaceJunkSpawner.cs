using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject spaceJunkObj;
    [SerializeField] int poolSize = 20;
    [SerializeField] int maxInstances = 25;
    [SerializeField] float spawnInterval = 1;

    [SerializeField] float minSpawRange = 5;
    [SerializeField] float maxSpawnRange = 20;

    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 3;

    private float time;

    private ResourcePool pool;
    private int currentInstances = 0;
    private GameObject spaceJunkContainer;
    // Start is called before the first frame update
    void Start()
    {
        pool = new ResourcePool(spaceJunkObj, poolSize);
        setSpaceJunkContainer();
    }

    void Update() {
        checkForSpawn();
    }

    private void setSpaceJunkContainer() {
        string spaceJunkContainerName = "Space Junk";
        spaceJunkContainer = GameObject.Find(spaceJunkContainerName);

        if(spaceJunkContainer == null)
        {
            spaceJunkContainer = new GameObject(spaceJunkContainerName);
        }
    }

    private void checkForSpawn() {
        time += Time.deltaTime;

        if (time >= spawnInterval && currentInstances < maxInstances) {
            time = 0;
            spawnAroundPlayer();
        }
    }

    private void spawnAroundPlayer(){
        //Get random Vector2 offset to use in spawn pos offset. Only X,Z needed - spawned objects need to use Y value of player object.
        //Y value will be used for Z in the V3 offset.
        Vector2 randomOffset = Random.insideUnitCircle * Random.Range(minSpawRange, maxSpawnRange);
        Vector3 spawnPosOffset = new Vector3(randomOffset.x, 0, randomOffset.y);

        GameObject spawnedPoolObject = pool.Spawn(player.transform.position + spawnPosOffset, Quaternion.identity);
        spawnedPoolObject.AddComponent<SpaceJunkSpawnTracker>().spawner = this;
        currentInstances += 1;

        float scaleFactor = Random.Range(minScale, maxScale);
        spawnedPoolObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        spawnedPoolObject.transform.rotation = Random.rotation;
        spawnedPoolObject.GetComponent<Rigidbody>().AddForce(spawnedPoolObject.transform.forward * 20, ForceMode.Impulse);
        spawnedPoolObject.GetComponent<Rigidbody>().AddTorque(spawnPosOffset * 1.5f);

        spawnedPoolObject.transform.parent = spaceJunkContainer.transform;
    }

    public void despawnListener(GameObject spawendObj) {
        currentInstances -= 1;
    }
}
