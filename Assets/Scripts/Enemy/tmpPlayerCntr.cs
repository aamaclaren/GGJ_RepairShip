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
<<<<<<< HEAD
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
=======
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

>>>>>>> 560ad42967e57600aae4b92f287daab733ffd0e1

    }

    private void FixedUpdate()
    {
        m_rb.AddForce(10 * (verticalInput * transform.forward));
        m_rb.AddTorque(5 * (horizontalInput * Vector3.up));
    }
}
