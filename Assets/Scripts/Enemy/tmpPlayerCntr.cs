using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpPlayerCntr : MonoBehaviour
{
    Rigidbody m_rb;
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

        m_rb.AddForce(10*new Vector3(horizontalInput, 0, verticalInput));
    }
}
