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

	private Rigidbody rb;

	// Use this for initialization
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other) {
        ConnectionSystem cs = other.gameObject.GetComponent<ConnectionSystem>();
        if (currState == ConnectionSystem.State.loose && cs != null && cs.currState == ConnectionSystem.State.connected) {
             // rb.constraints = RigidbodyConstraints.FreezeRotation;
        	// transform.SetParent(other.gameObject.transform, false);
        	transform.SetParent(GM.gm.player.transform, false);
        	currState = ConnectionSystem.State.connected;
        	rb.velocity = Vector3.zero;
        	rb.angularVelocity = Vector3.zero;
        	rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }
    }
}
