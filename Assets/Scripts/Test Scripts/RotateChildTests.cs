using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateChildTests : MonoBehaviour
{
    ConnectionSystem cs;
    // Start is called before the first frame update
    void Start()
    {
        cs = GetComponent<ConnectionSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cs.currState == ConnectionSystem.State.connected)
        {
            //transform.LookAt(GM.gm.player.transform.position);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
    }
}
