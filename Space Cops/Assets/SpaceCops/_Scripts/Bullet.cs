using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float bulletSpeed = 10f;
    private Rigidbody rigid;

	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rigid.velocity = Vector3.forward * bulletSpeed;
	}
}
