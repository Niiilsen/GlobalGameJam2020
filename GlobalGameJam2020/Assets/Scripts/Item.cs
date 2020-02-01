using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour {

    public ScrapType scrapType;

    private Transform tr;
    private Rigidbody rb;

    public void Init() {
        rb = GetComponent<Rigidbody>();
        tr = transform;
    }

    public void PickedUp(Transform t) {
        tr.parent = t;

        rb.useGravity = false;
        rb.isKinematic = true;

        SetColliders(false);
    }

    private void Dropped() {
        tr.parent = null;

        rb.useGravity = true;
        rb.isKinematic = false;
        SetColliders(true);
    }

    public void PutDown(Vector3 position) {
        tr.position = position;
        Dropped();
    }

    public void Thrown(Vector3 force) {
        Dropped();

        rb.AddForce(force, ForceMode.Impulse);
    }

    private void SetColliders(bool state) {
        foreach(Collider c in GetComponentsInChildren<Collider>()) {
            c.enabled = state;
        }
    }
}
