using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceShredder : MonoBehaviour
{
    private void OnTriggerExit(Collider other) {
        ResourcePoolMember otherRPM = other.GetComponent<ResourcePoolMember>();
        
        if(otherRPM != null){
            otherRPM.Despawn();
        }
    }
}
