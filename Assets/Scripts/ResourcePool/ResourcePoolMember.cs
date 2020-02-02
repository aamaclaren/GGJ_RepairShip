using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolMember : MonoBehaviour
{
    public ResourcePool pool;

    public void Despawn(){
        pool.Despawn(gameObject);
    }
}
