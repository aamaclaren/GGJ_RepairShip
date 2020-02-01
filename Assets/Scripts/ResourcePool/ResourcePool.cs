using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool
{
    [SerializeField] private GameObject prefab;
    private int poolSize;
    private Stack<GameObject> inactivePool;

    public ResourcePool(GameObject prefab, int poolSize){
        this.prefab = prefab;
        this.poolSize = poolSize;

        inactivePool = new Stack<GameObject>(poolSize);
    }


    public GameObject Spawn(Vector3 pos, Quaternion rot) {
        GameObject obj;
        
        if(inactivePool.Count == 0) {
            obj = (GameObject)GameObject.Instantiate(prefab, pos, rot);

            //reference to this Resource Pool for despawn
            obj.AddComponent<ResourcePoolMember>().pool = this;
        }
        else {
            obj = inactivePool.Pop();

            if(obj == null) {
                return Spawn(pos, rot);
            }
        }

        obj.transform.position = pos;
		obj.transform.rotation = rot;
		obj.SetActive(true);
		
        return obj;
    }

    public void Despawn(GameObject obj){
        if(inactivePool.Count < poolSize) {
            obj.SetActive(false);
            inactivePool.Push(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
}
