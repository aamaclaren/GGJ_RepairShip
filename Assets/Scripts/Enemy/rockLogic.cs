using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockLogic : MonoBehaviour
{
	public int contactDamage = 1;

	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ConnectionSystem>() != null) {
            other.gameObject.GetComponent<ConnectionSystem>().TakeDamage(contactDamage);
            // other.gameObject.GetComponent<ConnectionSystem>().TakeDamage(contactDamage * (int) (rb.velocity.magnitude * 0.5f));
        }
    }
}
