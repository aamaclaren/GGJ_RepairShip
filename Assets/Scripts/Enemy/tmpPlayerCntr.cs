using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpPlayerCntr : MonoBehaviour
{
    Rigidbody m_rb;
    float horizontalInput;
    float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

    }

    private void FixedUpdate()
    {
        m_rb.AddForce(10 * (verticalInput * transform.forward));
        m_rb.AddTorque(5 * (horizontalInput * Vector3.up));
    }
}
