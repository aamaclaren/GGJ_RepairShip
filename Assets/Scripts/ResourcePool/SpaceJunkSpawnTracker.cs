using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunkSpawnTracker : MonoBehaviour
{
    public SpaceJunkSpawner spawner;
    private void OnDestroy() {
        OnDespawn();
    }

    private void OnDisable() {
        OnDespawn();
    }

    private void OnDespawn(){
        if(spawner){
            spawner.despawnListener(gameObject);
        }
    }
}
