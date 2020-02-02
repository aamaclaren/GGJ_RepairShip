using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    Vector3 offset;
    bool off = false;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) { off = true; }
        if (off)
        {
            transform.position = offset - GM.gm.player.transform.position;
        }
    }
}