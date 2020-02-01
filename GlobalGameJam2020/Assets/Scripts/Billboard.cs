using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Vector3 pos;

    void Start() {
        pos = Camera.main.transform.position;
    }
    void LateUpdate() {
        transform.rotation = Quaternion.LookRotation(transform.position - pos);
    }
}
