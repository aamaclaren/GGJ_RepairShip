﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //how fast it goes
    public float speed = 10;
    //how many angles it should rotate per second
    public float rotationSpeed = 90f;
    //max speed of object
    public float maxspeed = 100;
    Rigidbody rb;

    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
            if (rb.velocity.magnitude > maxspeed)
            {
                rb.velocity = rb.velocity.normalized * maxspeed;
            }
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            //Quaternion a = new Quaternion(transform.rotation.x, transform.rotation.y + .1f, transform.rotation.z, transform.rotation.w);
            //transform.rotation = Quaternion.Slerp(transform.rotation, a, Time.deltaTime * rotationSpeed);
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            //Quaternion a = new Quaternion(transform.rotation.x, transform.rotation.y - .1f, transform.rotation.z, transform.rotation.w);
            //transform.rotation = Quaternion.Slerp(transform.rotation, a, Time.deltaTime * rotationSpeed
            transform.RotateAround(transform.position, -transform.up, Time.deltaTime * rotationSpeed);
        }
    }
}
