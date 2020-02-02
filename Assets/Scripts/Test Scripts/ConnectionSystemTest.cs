using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSystemTest : MonoBehaviour {
	public int speedX = 30;
    public int speedZ = 0;
    public bool takeDamageFlag = false;
    public bool stopFlag = false;

	private Rigidbody rb;
    private ConnectionSystem cs;

	// Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody>();
        cs = gameObject.GetComponent<ConnectionSystem>();
    }

    // Start is called before the first frame update
    void Start() {
    	rb.AddForce(new Vector3(speedX,0,speedZ));
    }

    void Update() {
        if (takeDamageFlag) {
            cs.TakeDamage(1);
            takeDamageFlag = false;
        }
        if (stopFlag) {
            rb.velocity = Vector3.zero;
            stopFlag = false;
        }
    }

    
}
