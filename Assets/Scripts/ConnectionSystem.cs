using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionSystem : MonoBehaviour {

	// add this to both the player and any attachable objects

	public enum State {
		loose,
		connected
	};

    public bool isAttached = false;
	public State currState = State.loose;
	public int defaultHealth = 10;
	public int mass = 1;
	// the force at which objects are pushed away from the ship when they're disconnected
	public int disconnectForce = 150;
	public bool isConnectable = true;
	public bool isShipCore = false;

	private int health;
	private Rigidbody rb;
    public bool sleep;
    Vector3 scale;
    Quaternion localRot;
    public Rigidbody randomObj;

    //Music variables
    public AudioSource click;
    public AudioSource explosion;
    public AudioSource thud;
    public AudioSource metalThud;

    // Use this for initialization
    void Awake() {

        rb = GetComponent<Rigidbody>();
        health = defaultHealth;
        scale = transform.localScale;
    }

    private void Update()
    {
        if (sleep) {
            //randomObj.Sleep();
        }
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
                if (other.tag == "Player")
                    {
                        Debug.Log("parent transform");
                        transform.SetParent(other.transform);
                        GM.gm.SetMass(GM.gm.GetMassWithChildren());
                        GM.gm.RefreshLargestRadius();
                        isAttached = true;
                        currState = ConnectionSystem.State.connected;
                        gameObject.layer = 8;
                        tag = "connected";
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                        
                        if (click != null)
                        {
                            click.Play();
                        }
                    }
                else 
                {
                    /*if (otherCS.isAttached) 
                    {
                        Debug.Log("other cs");
                        transform.SetParent(other.transform);
                        currState = ConnectionSystem.State.connected;
                                    gameObject.layer = 8;
                                    rb.velocity = Vector3.zero;
                                    rb.angularVelocity = Vector3.zero;
                                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;

                    }
                    else 
                    {
                        if (isAttached) 
                        {
                            //V1
                            Debug.Log("current cs");
                            other.transform.SetParent(transform);
                            currState = ConnectionSystem.State.connected;
                                    gameObject.layer = 8;
                                    rb.velocity = Vector3.zero;
                                    rb.angularVelocity = Vector3.zero;
                                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                            
                            //V2
                            transform.parent.SetParent(other.transform.parent);
                            GM.gm.SetMass(GM.gm.GetMassWithChildren());
                            GM.gm.RefreshLargestRadius();
                            if (click != null)
                            {
                                click.Play();
                            }
                        }
                    }
                            /*if (otherCS.isAttached) 
                            {
                                    Debug.Log("firing off " + other.gameObject);

                                    
                                    other.transform.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                                    other.transform.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                                    other.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                                    transform.SetParent(other.transform);
                                    GM.gm.SetMass(GM.gm.GetMassWithChildren());
                                    GM.gm.RefreshLargestRadius();

                                    sleep = true;
                                    randomObj = other.transform.gameObject.GetComponent<Rigidbody>();
                                    currState = ConnectionSystem.State.connected;
                                    gameObject.layer = 8;
                                    rb.velocity = Vector3.zero;
                                    rb.angularVelocity = Vector3.zero;
                                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                                
                                
                            }*/
                    if (other.tag == "connected") {
                        transform.SetParent(other.transform);
                        isAttached = true;
                        currState = ConnectionSystem.State.connected;
                        gameObject.layer = 8;
                        other.tag = "connected";
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                    }
                  
                }
                currState = ConnectionSystem.State.connected;
	        	gameObject.layer = 8;
	        	rb.velocity = Vector3.zero;
	        	rb.angularVelocity = Vector3.zero;
	        	rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                if(other.tag=="Player")
                    other.gameObject.GetComponent<WeaponSystem>().addnewWeapon(GetComponent<WeaponLogic>());
                else {

                }
                    //other.gameObject.GetComponentInParent<WeaponSystem>().addnewWeapon(GetComponent<WeaponLogic>());
            }
	    }
    }

    // call this on a connected part, not the ship itself (if the part in question happens to be the core, this will still take care of it)
    public void TakeDamage(int damage) {
    	if (!GM.gm.PlayerIsInvincible()) {
	    	health -= damage;
	    	if (health <= 0) {
	    		Disconnect();
                if (explosion != null)
                {
                    explosion.Play();
                }
            }
	    	if (isShipCore) {
                Debug.Log("hittt");
	    		GM.gm.DamagePlayer(damage);
                if (metalThud != null)
                {
                    metalThud.Play();
                }
            } else {
	    		GM.gm.DamagePlayer(0);
                if (thud != null)
                {
                    thud.Play();
                }
            }
	    }
    }

    // this disconnects the current part (along with its children) from the ship
    private void Disconnect() {
        if (!isShipCore)
        {
            Debug.Log(transform.parent);
            transform.parent.SetParent(null);
        }
    	GM.gm.SetMass(GM.gm.GetMassWithChildren());
    	GM.gm.RefreshLargestRadius();
    	currState = ConnectionSystem.State.loose;
    	gameObject.layer = 9;
    	rb.constraints = RigidbodyConstraints.None;
    	foreach (ConnectionSystem child in gameObject.GetComponentsInChildren<ConnectionSystem>()) {
    		child.gameObject.GetComponent<ConnectionSystem>().currState = ConnectionSystem.State.loose;
    		child.gameObject.layer = 9;
    	}
    	rb.AddForce((transform.position - GM.gm.player.transform.position).normalized * disconnectForce);
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
