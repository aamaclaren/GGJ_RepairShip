using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSystemTest : MonoBehaviour {
	public int speed = 70;

	private Rigidbody rb;

	// Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
    	rb.AddForce(new Vector3(speed,0,0));
    }
}
