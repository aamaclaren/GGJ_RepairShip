using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunkSpawner : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject spaceJunkObj;

    [Header("Resource Pool Configs")]
    [SerializeField] int poolSize = 20;
    [SerializeField] int maxInstances = 25;

    [Header("Spawned Object Configs")]
    [SerializeField] float spawnInterval = 1;
    [SerializeField] int initialInstances = 5;
    [SerializeField] float minSpawnRange = 5;
    [SerializeField] float maxSpawnRange = 20;
    [SerializeField] float minSpawnForce = 5;
    [SerializeField] float maxSpawnForce = 25;
    [SerializeField] float minSpawnTorque = 0.5f;
    [SerializeField] float maxSpawnTorque = 2.5f;
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 3;
    [SerializeField] bool isAttachableToPlayer = true;
    [SerializeField] bool isPlayerWeapon = false;


    private float time;

    private ResourcePool pool;
    private int currentInstances = 0;
    private GameObject spaceJunkContainer;
    private string spaceJunkObjParentName = "Space Junk Item Parent";
    // Start is called before the first frame update
    void Start()
    {
        pool = new ResourcePool(spaceJunkObj, poolSize);
        setSpaceJunkContainer();
        spawnInitialInstances();
    }

    void Update() {
        checkForSpawn();
    }

    public void despawnListener(GameObject spawendObj) {
        currentInstances -= 1;
    }

    private void setSpaceJunkContainer() {
        string spaceJunkContainerName = "Space Junk";
        spaceJunkContainer = GameObject.Find(spaceJunkContainerName);

        if(spaceJunkContainer == null)
        {
            spaceJunkContainer = new GameObject(spaceJunkContainerName);
        }
    }

    private void spawnInitialInstances(){
        while(currentInstances < initialInstances && currentInstances < maxInstances){
            spawnAroundPlayer();
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
        Vector3 spawnPosOffset = getRandomSpawnPosOffset();
        GameObject obj = spawnPoolObject(spawnPosOffset);

        setInitialObjState(obj);
        setObjParent(obj);
    }

    private Vector3 getRandomSpawnPosOffset(){
        Vector2 randomCircleOffset = Random.insideUnitCircle.normalized * Random.Range(minSpawnRange, maxSpawnRange);
        
        return new Vector3(randomCircleOffset.x, 0, randomCircleOffset.y);
    }

    private GameObject spawnPoolObject(Vector3 spawnPosOffset) {
        Quaternion objRotation = transform.rotation;
        if(isPlayerWeapon){
            objRotation.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }
        else {
            objRotation = Random.rotation;
        }

        GameObject spawnedPoolObject = pool.Spawn(player.transform.position + spawnPosOffset, objRotation);
        spawnedPoolObject.AddComponent<SpaceJunkSpawnTracker>().spawner = this;
        currentInstances += 1;

        return spawnedPoolObject;
    }

    private void setInitialObjState(GameObject obj) {
        float scaleFactor = Random.Range(minScale, maxScale);
        //obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * Random.Range(minSpawnForce, maxSpawnForce), ForceMode.Impulse);
        obj.GetComponent<Rigidbody>().AddTorque(Random.insideUnitSphere.normalized * Random.Range(minSpawnTorque, maxSpawnTorque));
    }

    private void setObjParent(GameObject obj) {
        obj.transform.parent = spaceJunkContainer.transform;
    }
}
