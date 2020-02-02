using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emit : MonoBehaviour
{
    ParticleSystem system;
    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        system.Play();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
}
