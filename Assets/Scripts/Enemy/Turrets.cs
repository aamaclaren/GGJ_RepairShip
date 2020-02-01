using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Turrets : MonoBehaviour
{
    private bool notYetFiring = true;
    public float firingInterval = .5f;
    LineRenderer line;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.widthMultiplier = .2f;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetButton("Fire1"))
        {
            Debug.Log("fired");
            if (time > firingInterval)
            {
                line.enabled = true;
                Ray ray = new Ray(transform.position, transform.forward);
                line.SetPosition(0, ray.origin);
                line.SetPosition(1, ray.GetPoint(100));
                time = 0;
            }
            else
            {
                line.enabled = false;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            line.enabled = false;
        }
    }
}
