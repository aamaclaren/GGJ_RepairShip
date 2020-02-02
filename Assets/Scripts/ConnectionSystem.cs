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
	public bool isShipCore = false;

	private int health;
	private Rigidbody rb;

    Vector3 scale;
    Quaternion localRot;
    // Use this for initialization
    void Awake() {

        rb = GetComponent<Rigidbody>();
        health = defaultHealth;
        scale = transform.localScale;
    }

    private void Update()
    {
    }
    // if the object touches the ship (or its connected parts), connect to it
    private void OnTriggerEnter(Collider other) {
    	if (isConnectable && currState == ConnectionSystem.State.loose) {
	        ConnectionSystem otherCS = other.gameObject.GetComponent<ConnectionSystem>();
            //Debug.Log(otherCS); 
	        if (otherCS != null && otherCS.currState == ConnectionSystem.State.connected && GM.gm.player.GetComponent<ConnectionSystem>().isConnectable) {
                //transform.SetParent(other.gameObject.transform);
                localRot = transform.localRotation;
                Debug.Log(localRot);
                if (transform.parent != null)
                {
                    if (other.tag == "Player")
                    {
                        transform.parent.SetParent(other.transform);
                    }
                    else
                    {
                        if (other.transform.parent != null)
                        {
                            transform.parent.SetParent(other.transform.parent);
                        }
                        //transform.parent.SetParent(other.gameObject.transform);
                    }
                }
                currState = ConnectionSystem.State.connected;
	        	gameObject.layer = 8;
	        	rb.velocity = Vector3.zero;
	        	rb.angularVelocity = Vector3.zero;
	        	rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                if(other.tag=="Player")
                    other.gameObject.GetComponent<WeaponSystem>().addnewWeapon(GetComponent<WeaponLogic>());
                else
                    other.gameObject.GetComponentInParent<WeaponSystem>().addnewWeapon(GetComponent<WeaponLogic>());
            }
	    }
    }

    // call this on a connected part, not the ship itself (if the part in question happens to be the core, this will still take care of it)
    public void TakeDamage(int damage = 100) {
    	health -= damage;
    	if (health <= 0) {
    		Disconnect();
    	}
    	if (isShipCore) {
    		GM.gm.DamagePlayer(damage);
    	} else if (!GM.gm.playerIsInvincible()) {
    		GM.gm.DamagePlayer(0);
    	}
    }

    // this disconnects the current part (along with its children) from the ship
    private void Disconnect() {
    	transform.SetParent(null);
    	currState = ConnectionSystem.State.loose;
    	gameObject.layer = 9;
    	rb.constraints = RigidbodyConstraints.None;
    	foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
    		child.gameObject.GetComponent<ConnectionSystem>().currState = ConnectionSystem.State.loose;
    		child.gameObject.layer = 9;
    	}
    	rb.AddForce((transform.position - GM.gm.player.transform.position).normalized * 50);
    	health = defaultHealth;
    }
    private void LateUpdate()
    {
        rb.angularVelocity = Vector3.zero;
        if (gameObject.tag != "Player")
        {
            transform.localRotation = localRot;
        }
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
