using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chanepos : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = GM.gm.player.transform.position;
    }
}
