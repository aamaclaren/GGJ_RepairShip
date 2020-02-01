using System.Collections;
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
    float horizontal;
    float vertical;
    public bool spinning = false;
    public float torqueSpeed = 5;
    public int maxAngularSpeed = 3;
    public bool rotateByTorque = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!spinning)
        {
            if (vertical > 0)
            {
                rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
                if (rb.velocity.magnitude > maxspeed)
                {
                    rb.velocity = rb.velocity.normalized * maxspeed;
                }
            }
            if (horizontal != 0)
            {
                if (!rotateByTorque)
                {
                    transform.RotateAround(transform.position, transform.up * horizontal, Time.deltaTime * rotationSpeed);
                }
                else
                {
                    rb.AddTorque(torqueSpeed * (horizontal * Vector3.up));
                    if (rb.angularVelocity.y > maxAngularSpeed)
                    {
                        rb.angularVelocity = Vector3.up * maxAngularSpeed;
                    }
                }
            }
        }
        Debug.Log(rb.angularVelocity);
    }

    public IEnumerator Spinning()
    {
        spinning = true;
        rb.maxAngularVelocity = 7;
        yield return new WaitForSeconds(3f);
        Debug.Log("freeze");
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(.1f);
        if (spinning)
        {
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            rb.maxAngularVelocity = 3;
        }
        spinning = false;
        
    }

    public void StartSpinning()
    {
        StartCoroutine(Spinning());
    }
}
