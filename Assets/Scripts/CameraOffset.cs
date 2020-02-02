using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    public float zoomSpeed = 2f;
    public float intensity = 0.35f;

    private float offset;
    // Vector3 offset;
    // bool off = false;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.L)) { off = true; }
        if (off)
        {
            transform.position = offset + GM.gm.player.transform.position;
        }*/
        // transform.position = offset + GM.gm.player.transform.position;
        MoveCamera(GM.gm.GetLargestRadius() * intensity + offset + GM.gm.player.transform.position);
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