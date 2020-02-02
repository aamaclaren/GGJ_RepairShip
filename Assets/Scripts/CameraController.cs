using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	public float zoomSpeed = 2f;
	public float intensity = 0.35f;
	public float offset = 20;

    // Update is called once per frame
    void Update()
    {
        //MoveCamera(GM.gm.GetLargestRadius() * intensity + offset);
    }

    private void MoveCamera(float newPos = 10) {
    	if (Mathf.Abs(transform.position.y - newPos) > 0.01) {
    		int mult = 1;
    		if (transform.position.y > newPos) {
    			mult = -1;
    		}
    		transform.position = new Vector3(transform.position.x, transform.position.y + zoomSpeed * mult * Time.deltaTime, transform.position.z);
    	}
    }
}
