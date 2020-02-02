using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //how fast it goes
    public float speed = 10;
    //how many angles it should rotate per second
    public float rotationSpeed = 90f;
    //max speed of object
    public float maxspeed = 100;
    public ConnectionSystem cs;
    // public Material hurtMaterial;
    public bool playerIsInvincible = false;
    public float spinningTime = 1f;
    public int spinningSpeed = 500;
    public float invincibilityTime = 1.5f;
    public int numFlashes = 5;
    Rigidbody rb;
    float horizontal;
    float vertical;
    public bool spinning = false;
    public float torqueSpeed = 5;
    public int maxAngularSpeed = 3;
    public bool rotateByTorque = true;
    public float height;
    float actualRotationSpeed;

    //Music variables
    public AudioSource spin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cs = GetComponent<ConnectionSystem>();
        height = transform.position.y;
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        actualRotationSpeed = Mathf.Lerp(actualRotationSpeed, horizontal * rotationSpeed, Time.deltaTime * 4);
        if (transform.position.y > height)
        {
            transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!spinning)
        {
            if (vertical > 0)
            {
                rb.AddForce(transform.forward * speed, ForceMode.Acceleration);
                if (rb.velocity.magnitude > maxspeed)
                {
                    rb.velocity = rb.velocity.normalized * maxspeed;
                }
            }
            // if (horizontal != 0)
            // {
                if (!rotateByTorque)
                {
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * actualRotationSpeed);
                }
                else if (horizontal != 0)
                {
                    rb.AddRelativeTorque(torqueSpeed * (horizontal * Vector3.up));
                    if (rb.angularVelocity.y > maxAngularSpeed)
                    {
                        rb.angularVelocity = Vector3.up * maxAngularSpeed;
                    }
                }
            // }
        }
        //Debug.Log(rb.angularVelocity);
    }

    public IEnumerator Spinning()
    {
        spinning = true;
        Debug.Log("crap");
        // rb.maxAngularVelocity = 7;
        float timer = 0f;
        while (timer < spinningTime) {
            Debug.Log("spinning time");
            transform.RotateAround(transform.position, transform.up, Time.deltaTime * spinningSpeed);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
        // yield return new WaitForSeconds(3f);
        // Debug.Log("freeze");
        //rb.constraints = RigidbodyConstraints.FreezeRotation;
        // yield return new WaitForSeconds(.1f);
         if (spinning)
         {
             rb.angularVelocity = Vector3.zero;
         }
         else
         {
             rb.constraints = RigidbodyConstraints.None;
             rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
         }
        spinning = false;
        
    }

    public void StartSpinning()
    {
        if (spin != null)
        {
            spin.Play();
        }
        StartCoroutine(Spinning());
    }

    public IEnumerator Flashing() {
        playerIsInvincible = true;
        cs.isConnectable = false;
        float iTimePerFlash = invincibilityTime / (numFlashes * 2 - 1);
        List<GameObject> meshChildren = new List<GameObject>();
        Color meshColor = GetComponent<Renderer>().material.color;
        List<Color> meshChildrenColors = new List<Color>();
        foreach (Transform child in GetComponentsInChildren<Transform>()) {
            if (child.gameObject.GetComponent<MeshRenderer>() != null) { // || child.gameObject.GetComponent<Renderer>() != null
                meshChildren.Add(child.gameObject);
                meshChildrenColors.Add(child.gameObject.GetComponent<Renderer>().material.color);
            }
        }
        for (int i = 0; i < numFlashes; i++)
        {
            // GetComponent<Renderer>().material = hurtMaterial;
            // yield return new WaitForSeconds(iTimePerFlash);
            // GetComponent<Renderer>().material = defaultPlayerMaterial;
            // if (i < numFlashes - 1) {
            //     yield return new WaitForSeconds(iTimePerFlash);
            // }
            GetComponent<Renderer>().material.color = Color.red;
            for (int j = 0; j < meshChildren.Count; j++) {
                meshChildren[j].GetComponent<Renderer>().material.color = Color.red;
            }
            yield return new WaitForSeconds(iTimePerFlash);
            GetComponent<Renderer>().material.color = meshColor;
            for (int j = 0; j < meshChildren.Count; j++) {
                meshChildren[j].GetComponent<Renderer>().material.color = meshChildrenColors[j];
            }
            if (i < numFlashes - 1) {
                yield return new WaitForSeconds(iTimePerFlash);
            }
        }
        playerIsInvincible = false;
        cs.isConnectable = true;
    }

    public void StartFlashing()
    {
        StartCoroutine(Flashing());
    }
}
