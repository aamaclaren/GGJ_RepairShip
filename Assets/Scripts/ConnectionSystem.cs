using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSystem : MonoBehaviour {

	// add this to both the player and any attachable objects

	public enum State {
		loose,
		connected
	};
	public State currState = State.loose;
	public int defaultHealth = 10;
	public bool isConnectable = true;

	private int health;
	private Rigidbody rb;

	public bool takeDamageFlag = false;

	// Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody>();
        health = defaultHealth;
    }

    void Update() {
    	if (takeDamageFlag) {
    		TakeDamage();
    		takeDamageFlag = false;
    	}
    }

    // if the object touches the ship (or its connected parts), connect to it
    private void OnCollisionEnter(Collision other) {
    	if (isConnectable && currState == ConnectionSystem.State.loose) {
	        ConnectionSystem cs = other.gameObject.GetComponent<ConnectionSystem>();
	        if (cs != null && cs.currState == ConnectionSystem.State.connected) {
	             // rb.constraints = RigidbodyConstraints.FreezeRotation;
	        	transform.SetParent(other.gameObject.transform);
	        	// transform.SetParent(GM.gm.player.transform);
	        	currState = ConnectionSystem.State.connected;
	        	gameObject.layer = 8;
	        	rb.velocity = Vector3.zero;
	        	rb.angularVelocity = Vector3.zero;
	        	rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
	        }
	    }
    }

    public void TakeDamage(int damage = 100) {
    	health -= damage;
    	if (health <= 0) {
    		Disconnect();
    	}
    }

    // this disconnects the current part (along with its children) from the ship
    private void Disconnect() {
    	transform.SetParent(null);
    	currState = ConnectionSystem.State.loose;
    	gameObject.layer = 9;
    	rb.constraints = RigidbodyConstraints.None;
    	foreach (Transform child in transform) {
    		child.gameObject.GetComponent<ConnectionSystem>().currState = ConnectionSystem.State.loose;
    		child.gameObject.layer = 9;
    		// child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    	}
    	rb.AddForce((transform.position - GM.gm.player.transform.position).normalized * 50);
    	// foreach (Transform child in transform) {
    	// 	child.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    	// 	child.gameObject.GetComponent<ConnectionSystem>().isConnectable = false;
    	// }
    	isConnectable = false;
    	health = defaultHealth;
    }
}
