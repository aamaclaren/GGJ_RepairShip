﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Turrets : MonoBehaviour {

    public float firingInterval = .5f;
    LineRenderer line;
    float time;
    RaycastHit hit;
    public float damage = 1;
    AudioSource source;

    public Transform firingColumn;
    public float colSmooth = 4;
    Vector3 colInitialPos;
    public Vector3 colPosOffet;

    // Start is called before the first frame update
    void Start() {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.widthMultiplier = .2f;
        line.enabled = false;
        source = GetComponent<AudioSource>();
        if (firingColumn) colInitialPos = firingColumn.localPosition;
    }

    // Update is called once per frame
    void Update() {
        if (firingColumn) {
            firingColumn.localPosition = Vector3.Lerp(firingColumn.localPosition, colInitialPos, Time.deltaTime * colSmooth);
        }
        time += Time.deltaTime;
        if (Input.GetButton("Fire1")) {
            Debug.Log("fired");
            if (time > firingInterval) {
                Fire();
            } else {
                line.enabled = false;
            }
        }
        if (Input.GetButtonUp("Fire1")) {
            line.enabled = false;
        }
    }

    public void Fire() {
        line.enabled = true;
        firingColumn.localPosition = firingColumn.localPosition + colPosOffet;
        if (source != null) {
            source.Play();
        }
        Ray ray = new Ray(transform.position, transform.forward);
        line.SetPosition(0, ray.origin);
        if (Physics.Raycast(ray, out hit)) {
            Debug.Log(hit.collider.gameObject);
            line.SetPosition(1, hit.collider.gameObject.transform.position);
            //Damage health here

        } else {
            line.SetPosition(1, ray.GetPoint(100));
        }
        time = 0;
    }
}